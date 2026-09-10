namespace wowfishbot.Services.Audio;

public sealed class AudioSampleMatcher
{
    private readonly float[] sample;
    private readonly double sampleMean;
    private readonly double sampleNorm;
    private readonly double[] sampleEnvelope;
    private readonly double sampleEnvelopeMean;
    private readonly double sampleEnvelopeNorm;
    private readonly double sampleRms;
    private readonly int featureFrameSamples;
    private readonly int alignmentSamples;

    public AudioSampleMatcher(PcmWaveData recordedSample, int liveSampleRate, double threshold)
    {
        ArgumentNullException.ThrowIfNull(recordedSample);
        if (recordedSample.Samples.Length == 0)
        {
            throw new ArgumentException("The catch sound sample is empty.", nameof(recordedSample));
        }

        sample = recordedSample.SampleRate == liveSampleRate
            ? recordedSample.Samples
            : Resample(recordedSample.Samples, recordedSample.SampleRate, liveSampleRate);
        SampleRate = liveSampleRate;
        Threshold = Math.Clamp(threshold, 0.1, 0.99);
        featureFrameSamples = Math.Max(32, liveSampleRate / 100);
        alignmentSamples = Math.Max(featureFrameSamples * 4, liveSampleRate / 4);
        sampleMean = sample.Average();
        sampleNorm = CalculateNorm(sample, sampleMean);
        sampleRms = CalculateRms((ReadOnlySpan<float>)sample);
        sampleEnvelope = BuildEnvelope(sample, featureFrameSamples);
        sampleEnvelopeMean = sampleEnvelope.Average();
        sampleEnvelopeNorm = CalculateNorm(sampleEnvelope, sampleEnvelopeMean);

        if (sampleNorm <= double.Epsilon || sampleRms <= 0.0001 || sampleEnvelopeNorm <= double.Epsilon)
        {
            throw new ArgumentException("The catch sound sample does not contain enough audio variation.", nameof(recordedSample));
        }
    }

    public int SampleLength => sample.Length;

    public int WindowLength => checked(sample.Length + alignmentSamples);

    public TimeSpan SampleDuration => TimeSpan.FromSeconds(sample.Length / (double)SampleRate);

    public int SampleRate { get; }

    public double Threshold { get; }

    public bool TryMatch(ReadOnlySpan<float> liveWindow, out AudioMatchResult result)
    {
        result = AudioMatchResult.Empty;
        if (liveWindow.Length < sample.Length)
        {
            return false;
        }

        var liveEnvelope = BuildEnvelope(liveWindow, featureFrameSamples);
        if (liveEnvelope.Length < sampleEnvelope.Length)
        {
            return false;
        }

        var best = AudioMatchResult.Empty;
        var maxOffset = liveEnvelope.Length - sampleEnvelope.Length;
        for (var envelopeOffset = 0; envelopeOffset <= maxOffset; envelopeOffset++)
        {
            var envelopeScore = CalculateCorrelation(sampleEnvelope, sampleEnvelopeMean, sampleEnvelopeNorm, liveEnvelope, envelopeOffset);
            var offsetSamples = envelopeOffset * featureFrameSamples;
            if (offsetSamples + sample.Length > liveWindow.Length)
            {
                continue;
            }

            var waveformScore = CalculateCorrelation(sample, sampleMean, sampleNorm, liveWindow, offsetSamples);
            var energySimilarity = CalculateEnergySimilarity(liveWindow.Slice(offsetSamples, sample.Length));
            var normalizedEnvelopeScore = Math.Max(0, envelopeScore);
            var normalizedWaveformScore = Math.Max(0, waveformScore);
            var score = normalizedEnvelopeScore * 0.82 + normalizedWaveformScore * 0.10 + energySimilarity * 0.08;
            var candidate = new AudioMatchResult(score, normalizedEnvelopeScore, normalizedWaveformScore, energySimilarity, offsetSamples);
            if (candidate.Score > best.Score)
            {
                best = candidate;
            }
        }

        result = best;
        return best.Score >= Threshold;
    }

    private double CalculateEnergySimilarity(ReadOnlySpan<float> liveSamples)
    {
        var liveRms = CalculateRms(liveSamples);
        if (liveRms <= 0.0001 || sampleRms <= 0.0001)
        {
            return 0;
        }

        var ratio = Math.Max(liveRms, sampleRms) / Math.Min(liveRms, sampleRms);
        return Math.Clamp(1 - Math.Log10(ratio) / 2.0, 0, 1);
    }

    private static double CalculateCorrelation(
        IReadOnlyList<double> expected,
        double expectedMean,
        double expectedNorm,
        IReadOnlyList<double> actual,
        int actualOffset)
    {
        if (actualOffset < 0 || actualOffset + expected.Count > actual.Count)
        {
            return 0;
        }

        var actualMean = 0d;
        for (var index = 0; index < expected.Count; index++)
        {
            actualMean += actual[actualOffset + index];
        }

        actualMean /= expected.Count;
        var actualNorm = 0d;
        var dot = 0d;
        for (var index = 0; index < expected.Count; index++)
        {
            var actualValue = actual[actualOffset + index] - actualMean;
            actualNorm += actualValue * actualValue;
            dot += (expected[index] - expectedMean) * actualValue;
        }

        return actualNorm <= double.Epsilon
            ? 0
            : dot / (expectedNorm * Math.Sqrt(actualNorm));
    }

    private static double CalculateCorrelation(
        IReadOnlyList<float> expected,
        double expectedMean,
        double expectedNorm,
        ReadOnlySpan<float> actual,
        int actualOffset)
    {
        if (actualOffset < 0 || actualOffset + expected.Count > actual.Length)
        {
            return 0;
        }

        var actualMean = 0d;
        for (var index = 0; index < expected.Count; index++)
        {
            actualMean += actual[actualOffset + index];
        }

        actualMean /= expected.Count;
        var actualNorm = 0d;
        var dot = 0d;
        for (var index = 0; index < expected.Count; index++)
        {
            var actualValue = actual[actualOffset + index] - actualMean;
            actualNorm += actualValue * actualValue;
            dot += (expected[index] - expectedMean) * actualValue;
        }

        return actualNorm <= double.Epsilon
            ? 0
            : dot / (expectedNorm * Math.Sqrt(actualNorm));
    }

    private static double CalculateRms(IReadOnlyList<float> values)
    {
        var total = 0d;
        for (var index = 0; index < values.Count; index++)
        {
            total += values[index] * values[index];
        }

        return values.Count == 0 ? 0 : Math.Sqrt(total / values.Count);
    }

    private static double CalculateRms(ReadOnlySpan<float> values)
    {
        var total = 0d;
        for (var index = 0; index < values.Length; index++)
        {
            total += values[index] * values[index];
        }

        return values.Length == 0 ? 0 : Math.Sqrt(total / values.Length);
    }

    private static double CalculateNorm(IReadOnlyList<float> values, double mean)
    {
        var norm = 0d;
        for (var index = 0; index < values.Count; index++)
        {
            var value = values[index] - mean;
            norm += value * value;
        }

        return Math.Sqrt(norm);
    }

    private static double CalculateNorm(IReadOnlyList<double> values, double mean)
    {
        var norm = 0d;
        for (var index = 0; index < values.Count; index++)
        {
            var value = values[index] - mean;
            norm += value * value;
        }

        return Math.Sqrt(norm);
    }

    private static double[] BuildEnvelope(ReadOnlySpan<float> values, int frameSamples)
    {
        var frameCount = Math.Max(1, (values.Length + frameSamples - 1) / frameSamples);
        var envelope = new double[frameCount];
        for (var frame = 0; frame < frameCount; frame++)
        {
            var start = frame * frameSamples;
            var length = Math.Min(frameSamples, values.Length - start);
            if (length <= 0)
            {
                break;
            }

            var total = 0d;
            for (var index = 0; index < length; index++)
            {
                var value = values[start + index];
                total += value * value;
            }

            envelope[frame] = Math.Sqrt(total / length);
        }

        return envelope;
    }

    private static float[] Resample(IReadOnlyList<float> source, int sourceRate, int targetRate)
    {
        var targetLength = Math.Max(1, (int)Math.Round(source.Count * (double)targetRate / sourceRate));
        var result = new float[targetLength];
        for (var index = 0; index < targetLength; index++)
        {
            var sourcePosition = index * (double)sourceRate / targetRate;
            var left = Math.Min(source.Count - 1, (int)sourcePosition);
            var right = Math.Min(source.Count - 1, left + 1);
            var fraction = sourcePosition - left;
            result[index] = (float)(source[left] + (source[right] - source[left]) * fraction);
        }

        return result;
    }
}
