using System.Text.Json.Serialization;

namespace wowfishbot.Models;

public enum MouseButton
{
    Left,
    Right,
    Middle
}

public sealed class BotSettings
{
    public string WindowTitle { get; set; } = "World of Warcraft";
    public string ProcessName { get; set; } = "Wow.exe";
    public string CastKey { get; set; } = "1";
    public string HideShowUiKeyCombination { get; set; } = string.Empty;
    public MouseButton ClickButton { get; set; } = MouseButton.Right;
    public int CastDelayMilliseconds { get; set; } = 750;
    public int DetectionTimeoutSeconds { get; set; } = 60;
    public int ClickDelayMilliseconds { get; set; } = 75;
    public double DetectionThreshold { get; set; } = 0.72;
    public int BobberX { get; set; }
    public int BobberY { get; set; }
    public bool HasBobberPosition { get; set; }
    [Obsolete("Use BobberPixelSamples for bobber calibration.")]
    [JsonIgnore]
    public bool HasBobberReference { get; set; }

    [Obsolete("Use BobberPixelSamples for bobber calibration.")]
    [JsonIgnore]
    public int BobberReferenceRed { get; set; }

    [Obsolete("Use BobberPixelSamples for bobber calibration.")]
    [JsonIgnore]
    public int BobberReferenceGreen { get; set; }

    [Obsolete("Use BobberPixelSamples for bobber calibration.")]
    [JsonIgnore]
    public int BobberReferenceBlue { get; set; }
    public List<BobberPixelSample> BobberPixelSamples { get; set; } = [];
    public int BobberClickOffsetX { get; set; }
    public int BobberClickOffsetY { get; set; }
    public int BobberSearchTimeoutSeconds { get; set; } = 6;
    public bool ValidateBobberPixel { get; set; } = true;
    public int BobberColorTolerance { get; set; } = 135;
    public int BobberNeighborhoodRadius { get; set; } = 3;
    public int BobberMinimumMatchScorePercent { get; set; } = 55;
    public BobberSearchArea? BobberSearchArea { get; set; }
    public string? CatchSoundPath { get; set; }
    public long WindowHandle { get; set; }

    [JsonIgnore]
    public bool HasCatchSound => !string.IsNullOrWhiteSpace(CatchSoundPath) && File.Exists(CatchSoundPath);

    [JsonIgnore]
    public bool HasBobberPixelPattern => BobberPixelSamples is { Count: >= 2 };
}
