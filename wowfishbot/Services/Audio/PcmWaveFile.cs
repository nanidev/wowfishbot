using System.Text;

namespace wowfishbot.Services.Audio;

public sealed record PcmWaveData(int SampleRate, float[] Samples);

public static class PcmWaveFile
{
    public static void Write(string path, int sampleRate, IReadOnlyList<float> samples)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");

        using var stream = File.Create(path);
        using var writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: false);
        var dataLength = checked(samples.Count * sizeof(short));

        writer.Write(Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(checked(36 + dataLength));
        writer.Write(Encoding.ASCII.GetBytes("WAVE"));
        writer.Write(Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)1);
        writer.Write(sampleRate);
        writer.Write(sampleRate * sizeof(short));
        writer.Write((short)sizeof(short));
        writer.Write((short)16);
        writer.Write(Encoding.ASCII.GetBytes("data"));
        writer.Write(dataLength);

        foreach (var sample in samples)
        {
            var clamped = Math.Clamp(sample, -1f, 1f);
            writer.Write((short)Math.Round(clamped * short.MaxValue));
        }
    }

    public static PcmWaveData Read(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: false);
        if (ReadText(reader, 4) != "RIFF")
        {
            throw new InvalidDataException("The sound file is not a RIFF wave file.");
        }

        _ = reader.ReadInt32();
        if (ReadText(reader, 4) != "WAVE")
        {
            throw new InvalidDataException("The sound file is not a WAVE file.");
        }

        byte[]? formatData = null;
        byte[]? data = null;
        while (stream.Position + 8 <= stream.Length)
        {
            var chunkId = ReadText(reader, 4);
            var chunkSize = reader.ReadInt32();
            if (chunkSize < 0 || stream.Position + chunkSize > stream.Length)
            {
                throw new InvalidDataException("The sound file contains an invalid chunk.");
            }

            switch (chunkId)
            {
                case "fmt ":
                    formatData = reader.ReadBytes(chunkSize);
                    break;
                case "data":
                    data = reader.ReadBytes(chunkSize);
                    break;
                default:
                    stream.Seek(chunkSize, SeekOrigin.Current);
                    break;
            }

            if ((chunkSize & 1) != 0 && stream.Position < stream.Length)
            {
                stream.Seek(1, SeekOrigin.Current);
            }
        }

        if (formatData is null || data is null)
        {
            throw new InvalidDataException("The sound file is missing a format or data chunk.");
        }

        var format = ParseFormat(formatData);
        if (!format.IsPcm && !format.IsFloat)
        {
            throw new InvalidDataException("Only PCM and IEEE float WAVE files are supported.");
        }

        var bytesPerSample = format.BitsPerSample / 8;
        if (format.Channels < 1 || format.SampleRate <= 0 || bytesPerSample < 1 || format.BlockAlign < format.Channels * bytesPerSample)
        {
            throw new InvalidDataException("The sound file contains an invalid audio format.");
        }

        var frameCount = data.Length / format.BlockAlign;
        var samples = new float[frameCount];
        for (var frame = 0; frame < frameCount; frame++)
        {
            var sum = 0f;
            for (var channel = 0; channel < format.Channels; channel++)
            {
                var offset = frame * format.BlockAlign + channel * bytesPerSample;
                sum += ReadSample(data, offset, format.BitsPerSample, format.IsFloat);
            }

            samples[frame] = sum / format.Channels;
        }

        return new PcmWaveData(format.SampleRate, TrimSilence(samples, format.SampleRate));
    }

    private static WaveFormatInfo ParseFormat(byte[] data)
    {
        if (data.Length < 16)
        {
            throw new InvalidDataException("The sound file format chunk is incomplete.");
        }

        var formatTag = BitConverter.ToUInt16(data, 0);
        var channels = BitConverter.ToUInt16(data, 2);
        var sampleRate = BitConverter.ToInt32(data, 4);
        var blockAlign = BitConverter.ToUInt16(data, 12);
        var bitsPerSample = BitConverter.ToUInt16(data, 14);
        var isPcm = formatTag == 1;
        var isFloat = formatTag == 3;

        if (formatTag == 0xFFFE && data.Length >= 40)
        {
            var subFormatBytes = new byte[16];
            Array.Copy(data, 24, subFormatBytes, 0, subFormatBytes.Length);
            var subFormat = new Guid(subFormatBytes);
            isPcm = subFormat == new Guid("00000001-0000-0010-8000-00AA00389B71");
            isFloat = subFormat == new Guid("00000003-0000-0010-8000-00AA00389B71");
        }

        return new WaveFormatInfo(formatTag, channels, sampleRate, blockAlign, bitsPerSample, isPcm, isFloat);
    }

    private static float ReadSample(byte[] data, int offset, int bitsPerSample, bool isFloat)
    {
        if (isFloat && bitsPerSample == 32)
        {
            return BitConverter.ToSingle(data, offset);
        }

        return bitsPerSample switch
        {
            8 => (data[offset] - 128) / 128f,
            16 => BitConverter.ToInt16(data, offset) / (float)short.MaxValue,
            24 => Read24Bit(data, offset) / 8_388_608f,
            32 => BitConverter.ToInt32(data, offset) / (float)int.MaxValue,
            _ => throw new InvalidDataException($"The sound file uses {bitsPerSample}-bit samples, which are not supported.")
        };
    }

    private static int Read24Bit(byte[] data, int offset)
    {
        var value = data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16;
        return (value & 0x800000) != 0 ? value | unchecked((int)0xFF000000) : value;
    }

    private static float[] TrimSilence(float[] samples, int sampleRate)
    {
        if (samples.Length == 0)
        {
            return samples;
        }

        var peak = samples.Max(Math.Abs);
        var threshold = Math.Max(0.008f, peak * 0.12f);
        var first = Array.FindIndex(samples, sample => Math.Abs(sample) >= threshold);
        var last = Array.FindLastIndex(samples, sample => Math.Abs(sample) >= threshold);
        if (first < 0 || last < first)
        {
            return samples;
        }

        var padding = Math.Max(1, sampleRate / 20);
        first = Math.Max(0, first - padding);
        last = Math.Min(samples.Length - 1, last + padding);
        var trimmed = new float[last - first + 1];
        Array.Copy(samples, first, trimmed, 0, trimmed.Length);
        return trimmed;
    }

    private static string ReadText(BinaryReader reader, int count) => Encoding.ASCII.GetString(reader.ReadBytes(count));

    private sealed record WaveFormatInfo(
        ushort FormatTag,
        ushort Channels,
        int SampleRate,
        ushort BlockAlign,
        ushort BitsPerSample,
        bool IsPcm,
        bool IsFloat);
}
