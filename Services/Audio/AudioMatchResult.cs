namespace wowfishbot.Services.Audio;

public sealed record AudioMatchResult(
    double Score,
    double EnvelopeScore,
    double WaveformScore,
    double EnergySimilarity,
    int OffsetSamples)
{
    public static AudioMatchResult Empty { get; } = new(0, 0, 0, 0, 0);
}
