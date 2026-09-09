using System.Text.Json;
using wowfishbot.Models;

namespace wowfishbot.Services;

public sealed class SettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public SettingsStore()
    {
        Directory.CreateDirectory(DataDirectory);
    }

    public string DataDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WowFishBot");

    public string SettingsPath => Path.Combine(DataDirectory, "settings.json");

    public BotSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath))
            {
                return new BotSettings();
            }

            var json = File.ReadAllText(SettingsPath);
            var settings = JsonSerializer.Deserialize<BotSettings>(json, JsonOptions) ?? new BotSettings();
            if (string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination))
            {
                using var document = JsonDocument.Parse(json);
                if (document.RootElement.TryGetProperty("StartStopKeyCombination", out var legacyKey) &&
                    legacyKey.ValueKind == JsonValueKind.String)
                {
                    settings.HideShowUiKeyCombination = legacyKey.GetString() ?? string.Empty;
                }
            }

            return settings;
        }
        catch (JsonException)
        {
            return new BotSettings();
        }
        catch (IOException)
        {
            return new BotSettings();
        }
    }

    public void Save(BotSettings settings)
    {
        Directory.CreateDirectory(DataDirectory);
        var temporaryPath = SettingsPath + ".tmp";
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        File.WriteAllText(temporaryPath, json);
        File.Move(temporaryPath, SettingsPath, true);
    }
}
