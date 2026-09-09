using System.Runtime.InteropServices;

namespace wowfishbot.Services.Audio;

public sealed class WasapiLoopbackCapture : IDisposable
{
    private readonly IAudioClient audioClient;
    private readonly IAudioCaptureClient captureClient;
    private readonly AudioFormatInfo format;
    private bool disposed;

    public WasapiLoopbackCapture()
    {
        IMMDevice? device = null;
        object? audioClientObject = null;
        try
        {
            var enumerator = (IMMDeviceEnumerator)new MMDeviceEnumeratorComObject();
            ThrowIfFailed(enumerator.GetDefaultAudioEndpoint(AudioDataFlow.Render, 0, out device));
            var audioClientId = WasapiConstants.AudioClientGuid;
            ThrowIfFailed(device.Activate(ref audioClientId, WasapiConstants.ClsCtxAll, IntPtr.Zero, out audioClientObject));
            audioClient = (IAudioClient)audioClientObject;

            ThrowIfFailed(audioClient.GetMixFormat(out var formatPointer));
            try
            {
                format = ReadFormat(formatPointer);
                ThrowIfFailed(audioClient.Initialize(
                    WasapiConstants.ShareModeShared,
                    WasapiConstants.StreamFlagsLoopback,
                    10_000_000,
                    0,
                    formatPointer,
                    IntPtr.Zero));
            }
            finally
            {
                Marshal.FreeCoTaskMem(formatPointer);
            }

            var captureClientId = WasapiConstants.AudioCaptureClientGuid;
            ThrowIfFailed(audioClient.GetService(ref captureClientId, out var captureClientObject));
            captureClient = (IAudioCaptureClient)captureClientObject;
        }
        catch
        {
            if (audioClientObject is not null && Marshal.IsComObject(audioClientObject))
            {
                Marshal.ReleaseComObject(audioClientObject);
            }

            if (device is not null && Marshal.IsComObject(device))
            {
                Marshal.ReleaseComObject(device);
            }

            throw;
        }
        finally
        {
            if (device is not null && Marshal.IsComObject(device))
            {
                Marshal.ReleaseComObject(device);
            }
        }
    }

    public int SampleRate => checked((int)format.SamplesPerSecond);

    public float[] Capture(TimeSpan duration, CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        if (duration <= TimeSpan.Zero)
        {
            return Array.Empty<float>();
        }

        var samples = new List<float>(checked((int)(duration.TotalSeconds * SampleRate)));
        var deadline = DateTime.UtcNow + duration;
        ThrowIfFailed(audioClient.Start());
        try
        {
            while (DateTime.UtcNow < deadline)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ThrowIfFailed(captureClient.GetNextPacketSize(out var frames));
                if (frames == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                ThrowIfFailed(captureClient.GetBuffer(out var data, out frames, out var flags, out _, out _));
                try
                {
                    AppendFrames(samples, data, frames, flags);
                }
                finally
                {
                    ThrowIfFailed(captureClient.ReleaseBuffer(frames));
                }
            }
        }
        finally
        {
            _ = audioClient.Stop();
        }

        return samples.ToArray();
    }

    public bool ListenForMatch(AudioSampleMatcher matcher, TimeSpan timeout, CancellationToken cancellationToken, out AudioMatchResult bestMatch)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        ArgumentNullException.ThrowIfNull(matcher);
        bestMatch = AudioMatchResult.Empty;

        var window = new Queue<float>(matcher.WindowLength);
        var evaluationStep = Math.Max(1, SampleRate / 20);
        var samplesSinceEvaluation = evaluationStep;
        var deadline = DateTime.UtcNow + timeout;
        ThrowIfFailed(audioClient.Start());
        try
        {
            while (DateTime.UtcNow < deadline)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ThrowIfFailed(captureClient.GetNextPacketSize(out var frames));
                if (frames == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                ThrowIfFailed(captureClient.GetBuffer(out var data, out frames, out var flags, out _, out _));
                try
                {
                    var packet = new List<float>(checked((int)frames));
                    AppendFrames(packet, data, frames, flags);
                    foreach (var sample in packet)
                    {
                        window.Enqueue(sample);
                        samplesSinceEvaluation++;
                    }

                    while (window.Count > matcher.WindowLength)
                    {
                        window.Dequeue();
                    }

                    if (window.Count >= matcher.SampleLength && samplesSinceEvaluation >= evaluationStep)
                    {
                        var candidate = window.ToArray();
                        if (matcher.TryMatch(candidate, out var match))
                        {
                            bestMatch = match;
                            return true;
                        }

                        if (match.Score > bestMatch.Score)
                        {
                            bestMatch = match;
                        }

                        samplesSinceEvaluation = 0;
                    }
                }
                finally
                {
                    ThrowIfFailed(captureClient.ReleaseBuffer(frames));
                }
            }
        }
        finally
        {
            _ = audioClient.Stop();
        }

        return false;
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        if (Marshal.IsComObject(captureClient))
        {
            Marshal.ReleaseComObject(captureClient);
        }

        if (Marshal.IsComObject(audioClient))
        {
            Marshal.ReleaseComObject(audioClient);
        }

        GC.SuppressFinalize(this);
    }

    private void AppendFrames(List<float> destination, IntPtr data, uint frames, uint flags)
    {
        if ((flags & WasapiConstants.BufferFlagSilent) != 0)
        {
            destination.AddRange(new float[checked((int)frames)]);
            return;
        }

        var sampleCount = checked((int)(frames * format.Channels));
        var byteCount = checked((int)(frames * format.BlockAlign));
        var bytes = new byte[byteCount];
        Marshal.Copy(data, bytes, 0, bytes.Length);

        for (var frame = 0; frame < frames; frame++)
        {
            var sum = 0d;
            var frameOffset = checked((int)(frame * format.BlockAlign));
            for (var channel = 0; channel < format.Channels; channel++)
            {
                var offset = frameOffset + channel * format.BytesPerSample;
                sum += ReadSample(bytes, offset);
            }

            destination.Add((float)(sum / format.Channels));
        }

        _ = sampleCount;
    }

    private double ReadSample(byte[] data, int offset)
    {
        if (format.IsFloat && format.BitsPerSample == 32)
        {
            return BitConverter.ToSingle(data, offset);
        }

        return format.BitsPerSample switch
        {
            8 => (data[offset] - 128) / 128d,
            16 => BitConverter.ToInt16(data, offset) / (double)short.MaxValue,
            24 => Read24Bit(data, offset) / 8_388_608d,
            32 => BitConverter.ToInt32(data, offset) / (double)int.MaxValue,
            _ => throw new NotSupportedException($"The audio format uses {format.BitsPerSample} bits per sample.")
        };
    }

    private static int Read24Bit(byte[] data, int offset)
    {
        var value = data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16;
        return (value & 0x800000) != 0 ? value | unchecked((int)0xFF000000) : value;
    }

    private static AudioFormatInfo ReadFormat(IntPtr pointer)
    {
        var waveFormat = Marshal.PtrToStructure<WaveFormatEx>(pointer);
        var isFloat = waveFormat.FormatTag == 3;
        if (waveFormat.FormatTag == 0xFFFE && waveFormat.ExtraSize >= 22)
        {
            var subFormatBytes = new byte[16];
            Marshal.Copy(IntPtr.Add(pointer, 24), subFormatBytes, 0, subFormatBytes.Length);
            isFloat = new Guid(subFormatBytes) == new Guid("00000003-0000-0010-8000-00AA00389B71");
        }

        if (waveFormat.Channels == 0 || waveFormat.SamplesPerSecond == 0 || waveFormat.BitsPerSample == 0)
        {
            throw new InvalidDataException("The default audio device returned an invalid format.");
        }

        return new AudioFormatInfo(
            waveFormat.Channels,
            waveFormat.SamplesPerSecond,
            waveFormat.BitsPerSample,
            waveFormat.BlockAlign,
            isFloat);
    }

    private static void ThrowIfFailed(int result)
    {
        if (result < 0)
        {
            Marshal.ThrowExceptionForHR(result);
        }
    }
}
