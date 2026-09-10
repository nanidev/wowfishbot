using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using wowfishbot.Models;
using wowfishbot.Services;
using wowfishbot.Services.Audio;

namespace wowfishbot;

public partial class Form1 : Form
{
    private const int CaptureHotKeyId = 0x4F31;
    private const int WmHotKey = 0x0312;
    private const int WhKeyboardLl = 13;
    private const int WmKeyDown = 0x0100;
    private const int WmSysKeyDown = 0x0104;
    private const int VirtualKeyEscape = 0x1B;

    private readonly SettingsStore settingsStore = new();
    private readonly WowWindowService windowService = new();
    private readonly InputService inputService = new();
    private readonly BobberLocator bobberLocator;
    private BotSettings settings;
    private CancellationTokenSource? botCancellation;
    private CancellationTokenSource? recordCancellation;
    private Task? botTask;
    private bool captureArmed;
    private bool isClosing;
    private bool stopRequested;
    private bool hideShowUiKeyNeedsRestore;
    private WowWindowInfo? runningWindow;
    private KeyCombinationCaptureForm? activeKeyCaptureDialog;
    private readonly LowLevelKeyboardProc keyboardHookProc;
    private IntPtr keyboardHook;
    private bool globalEscapeActionPending;

    public Form1()
    {
        keyboardHookProc = KeyboardHookCallback;
        InitializeComponent();
        ConfigureRuntimeControls();
        bobberLocator = new BobberLocator(windowService);
        settings = settingsStore.Load();

        cmbClickButton.DataSource = Enum.GetValues<MouseButton>();
        ApplySettingsToUi();
        KeyPreview = true;
        KeyDown += Form1_KeyDown;
        Shown += Form1_Shown;
        FormClosing += Form1_FormClosing;
    }

    private void ConfigureRuntimeControls()
    {
        ConfigureNumber(numCastDelay, 0, 5000, 750, 50);
        ConfigureNumber(numTimeout, 5, 600, 60, 5);
        ConfigureNumber(numClickDelay, 0, 1000, 75, 5);
        ConfigureNumber(numPerformanceDelay, 50, 1000, 150, 25);
        ConfigureNumber(numThreshold, 0.10M, 0.99M, 0.72M, 0.01M);
        ConfigureNumber(numPixelColorTolerance, 10, 442, 135, 1);
        ConfigureNumber(numPixelNeighborhoodRadius, 0, 10, 3, 1);
        ConfigureNumber(numPixelMatchScore, 1, 100, 55, 1);
        ConfigureNumber(numRecordSeconds, 2, 20, 5, 1);
        chkInteractMode.CheckedChanged += chkInteractMode_CheckedChanged;

        toolTip.SetToolTip(numTimeout, "How long to listen for a catch after each bobber is found. This is independent of recording duration.");
        toolTip.SetToolTip(numThreshold, "Normalized match threshold from 0.10 to 0.99. Start around 0.55 to 0.75 and use the log diagnostics to tune it.");
        toolTip.SetToolTip(btnCalibrateBobberPattern, "Capture a bobber screenshot and select the red feather first, the blue feather second, and optionally a third stable pixel. The original cursor pixel is excluded because hovering changes its color.");
        toolTip.SetToolTip(lblBobberPatternStatus, "The detector matches the selected pixels by relative position and searches nearby locations and small scale variations to tolerate bobber jiggle.");
        toolTip.SetToolTip(chkValidateBobber, "Rechecks the saved pixel pattern near the detected bobber before clicking.");
        toolTip.SetToolTip(chkInteractMode, "For newer WoW versions: use the configured cast key as the Interact With Target key. The key is pressed to cast and again after the catch sound to reel in; bobber scanning and mouse clicking are skipped.");
        toolTip.SetToolTip(btnStop, "Stop the bot. Press ESC at any time while the bot is running as an emergency kill switch.");
        toolTip.SetToolTip(btnStart, "Start the bot after calibrating the pixel pattern and loading a catch sound. Press ESC to stop it.");
        toolTip.SetToolTip(cmbWindows, "Select the WoW window where the calibrated bobber pattern will be detected.");
        toolTip.SetToolTip(txtProcessName, "Process name used to find the WoW window, normally Wow.exe.");
        toolTip.SetToolTip(txtWindowTitle, "Optional title text used to filter the detected WoW windows.");
        toolTip.SetToolTip(numCastDelay, "Delay after casting before scanning for the bobber.");
        toolTip.SetToolTip(numClickDelay, "Delay after the catch sound before clicking the detected bobber.");
        toolTip.SetToolTip(numPerformanceDelay, "Pause between bobber scans and after reeling in. Higher values reduce CPU and screen-capture load; lower values make the bot react faster.");
        toolTip.SetToolTip(btnRecordCastKey, "Record the single key used to cast.");
        toolTip.SetToolTip(btnRecordHideShowUiKey, "Optionally record a modifier and key combination, such as Alt+Z, to hide the UI when the bot starts and show it again when the bot stops.");
        toolTip.SetToolTip(numPixelColorTolerance, "Maximum RGB distance accepted for a calibrated bobber pixel.");
        toolTip.SetToolTip(numPixelNeighborhoodRadius, "Pixels around each calibrated offset to search when the bobber jiggles.");
        toolTip.SetToolTip(numPixelMatchScore, "Minimum percentage of calibrated pixels that must match.");
        toolTip.SetToolTip(btnSelectBobberSearchArea, "Limit bobber detection to a rectangle selected from the WoW client.");
        toolTip.SetToolTip(txtSoundPath, "WAV recording of the catch sound used by the audio matcher.");
        toolTip.SetToolTip(numRecordSeconds, "Length of the catch-sound recording. Cast once during recording.");
    }

    private void btnRecordCastKey_Click(object? sender, EventArgs e)
    {
        using var dialog = new KeyCombinationCaptureForm(
            "Record cast key",
            "Press the single key that the bot should use to cast. Press ESC to clear it.",
            allowModifiers: false,
            inputService.TryParseKey(settings.CastKey, out var currentKey) ? currentKey : null);

        activeKeyCaptureDialog = dialog;
        DialogResult result;
        try
        {
            result = dialog.ShowDialog(this);
        }
        finally
        {
            activeKeyCaptureDialog = null;
        }

        if (result != DialogResult.OK)
        {
            return;
        }

        settings.CastKey = InputService.FormatKeyCombination(dialog.CapturedCombination);
        lblCastKeyValue.Text = string.IsNullOrWhiteSpace(settings.CastKey) ? "Not configured" : settings.CastKey;
        SaveCurrentSettings(false);
        Log(string.IsNullOrWhiteSpace(settings.CastKey)
            ? "Cast key cleared."
            : $"Cast key recorded: {settings.CastKey}.");
    }

    private void btnRecordHideShowUiKey_Click(object? sender, EventArgs e)
    {
        var initialCombination = inputService.TryParseKeyCombination(settings.HideShowUiKeyCombination, out var parsedCombination)
            ? parsedCombination
            : (Keys?)null;
        using var dialog = new KeyCombinationCaptureForm(
            "Record hide/show UI key",
            "Press the optional modifier and key combination to hide the UI when the bot starts and show it when the bot stops. Press ESC to clear it.",
            allowModifiers: true,
            initialCombination);

        activeKeyCaptureDialog = dialog;
        DialogResult result;
        try
        {
            result = dialog.ShowDialog(this);
        }
        finally
        {
            activeKeyCaptureDialog = null;
        }

        if (result != DialogResult.OK)
        {
            return;
        }

        settings.HideShowUiKeyCombination = InputService.FormatKeyCombination(dialog.CapturedCombination);
        lblHideShowUiKeyValue.Text = string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination) ? "Not configured" : settings.HideShowUiKeyCombination;
        SaveCurrentSettings(false);
        Log(string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination)
            ? "Hide/show UI key cleared."
            : $"Hide/show UI key combination recorded: {settings.HideShowUiKeyCombination}.");
    }

    private void btnSelectBobberSearchArea_Click(object? sender, EventArgs e)
    {
        var window = GetSelectedWindow();
        if (window is null)
        {
            ShowValidation("Select a detected WoW window before selecting a search area.");
            return;
        }

        if (!bobberLocator.TryCaptureClientImage(window.Handle, out var screenshot, out _, out var reason))
        {
            ShowValidation(reason);
            return;
        }

        using (screenshot)
        using (var dialog = new AreaSelectionForm(screenshot))
        {
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            settings.BobberSearchArea = dialog.SelectedArea;
        }

        lblBobberSearchArea.Text = settings.BobberSearchArea is { IsValid: true } area
            ? $"Search area: ({area.X}, {area.Y}) {area.Width} x {area.Height}"
            : "Search area: full client";
        SaveCurrentSettings(false);
        Log(settings.BobberSearchArea is null
            ? "Bobber search area cleared; scanning the full client."
            : $"Bobber search area saved: ({settings.BobberSearchArea.X}, {settings.BobberSearchArea.Y}) {settings.BobberSearchArea.Width} x {settings.BobberSearchArea.Height}.");
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (!RegisterHotKey(Handle, CaptureHotKeyId, 0, (uint)Keys.F8))
        {
            Log("F8 is already registered by another application; use the capture button while this window is focused.");
        }

        if (keyboardHook == IntPtr.Zero)
        {
            keyboardHook = SetWindowsHookEx(WhKeyboardLl, keyboardHookProc, GetModuleHandle(null), 0);
            if (keyboardHook == IntPtr.Zero)
            {
                Log("ESC could not be registered globally; keep this window focused to use the kill switch.");
            }
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        UnregisterHotKey(Handle, CaptureHotKeyId);
        if (keyboardHook != IntPtr.Zero)
        {
            UnhookWindowsHookEx(keyboardHook);
            keyboardHook = IntPtr.Zero;
        }

        base.OnHandleDestroyed(e);
    }

    protected override void WndProc(ref Message message)
    {
        if (message.Msg == WmHotKey)
        {
            switch (message.WParam.ToInt32())
            {
                case CaptureHotKeyId:
                    CalibrateBobberPattern();
                    break;
            }
        }

        base.WndProc(ref message);
    }

    private IntPtr KeyboardHookCallback(int code, IntPtr message, IntPtr data)
    {
        if (code >= 0 &&
            (message == (IntPtr)WmKeyDown || message == (IntPtr)WmSysKeyDown) &&
            Marshal.ReadInt32(data) == VirtualKeyEscape)
        {
            if (activeKeyCaptureDialog is { IsDisposed: false } dialog)
            {
                try
                {
                    BeginInvoke(() =>
                    {
                        if (!dialog.IsDisposed)
                        {
                            dialog.ClearFromGlobalEscape();
                        }
                    });
                    return (IntPtr)1;
                }
                catch (InvalidOperationException)
                {
                }
            }
            else if (botCancellation is not null && !globalEscapeActionPending)
            {
                globalEscapeActionPending = true;
                try
                {
                    BeginInvoke(HandleGlobalEscape);
                    return (IntPtr)1;
                }
                catch (InvalidOperationException)
                {
                    globalEscapeActionPending = false;
                }
            }
        }

        return CallNextHookEx(keyboardHook, code, message, data);
    }

    private async void HandleGlobalEscape()
    {
        try
        {
            await StopBotAsync(killSwitch: true);
        }
        catch (Exception exception)
        {
            Log($"ESC could not stop the bot: {exception.Message}");
        }
        finally
        {
            globalEscapeActionPending = false;
        }
    }

    private void Form1_Shown(object? sender, EventArgs e)
    {
        RefreshWindows();
        UpdateSoundStatus();
        Log(settings.UseInteractMode
            ? "Ready. Interact mode is enabled; configure the cast key and catch sound for the selected WoW version."
            : "Ready. Select the Classic client, record the catch sound, and capture the bobber red and blue feather pixels.");
    }

    private void chkInteractMode_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateInteractModeUi();
    }

    private void UpdateInteractModeUi()
    {
        var canEditBobberOptions = botTask is null && !chkInteractMode.Checked;
        btnCalibrateBobberPattern.Enabled = canEditBobberOptions;
        btnSelectBobberSearchArea.Enabled = canEditBobberOptions;
        numPixelColorTolerance.Enabled = canEditBobberOptions;
        numPixelNeighborhoodRadius.Enabled = canEditBobberOptions;
        numPixelMatchScore.Enabled = canEditBobberOptions;
        chkValidateBobber.Enabled = canEditBobberOptions;
    }

    private void btnRefreshWindows_Click(object? sender, EventArgs e) => RefreshWindows();

    private void RefreshWindows()
    {
        var windows = windowService.FindWindows(txtProcessName.Text, txtWindowTitle.Text).ToList();
        cmbWindows.DataSource = null;
        cmbWindows.DataSource = windows;

        var savedWindow = windows.FirstOrDefault(window => window.Handle.ToInt64() == settings.WindowHandle);
        if (savedWindow is not null)
        {
            cmbWindows.SelectedItem = savedWindow;
        }
        else if (windows.Count > 0)
        {
            cmbWindows.SelectedIndex = 0;
        }

        if (windows.Count == 0)
        {
            SetStatus("WoW not found", warning: true);
            Log("No matching window found. Start WoW or adjust the process/title filters.");
        }
        else
        {
            SetStatus($"{windows.Count} window{(windows.Count == 1 ? string.Empty : "s")} found");
            Log($"Detected {windows.Count} matching WoW window{(windows.Count == 1 ? string.Empty : "s")}.");
        }
    }

    private void btnCalibrateBobberPattern_Click(object? sender, EventArgs e)
    {
        if (GetSelectedWindow() is null)
        {
            ShowValidation("Select a detected WoW window before calibrating the bobber pixels.");
            return;
        }

        captureArmed = true;
        btnCalibrateBobberPattern.Text = "Press F8 over bobber";
        lblBobberPatternInstructions.Text = "Move the cursor over the bobber in WoW, then press F8 to select its red and blue feather pixels.";
        SetStatus("Capture armed");
            Log("Bobber pattern calibration armed. Place the cursor over a clearly visible bobber and press F8.");
    }

    private void CalibrateBobberPattern()
    {
        if (!captureArmed)
        {
            return;
        }

        captureArmed = false;
        var window = GetSelectedWindow();
        var point = Cursor.Position;
        if (window is null || !windowService.TryGetClientBounds(window.Handle, out var bounds) || !bounds.Contains(point))
        {
            btnCalibrateBobberPattern.Text = "Calibrate pixel pattern (F8)";
            lblBobberPatternInstructions.Text = "Calibration cancelled: the cursor must be inside the selected WoW client.";
            ShowValidation("The calibration point was outside the selected WoW client.");
            return;
        }

        if (!bobberLocator.TryCaptureCalibrationImage(window.Handle, point, out var screenshot, out var screenshotOrigin, out var reason))
        {
            btnCalibrateBobberPattern.Text = "Calibrate pixel pattern (F8)";
            lblBobberPatternInstructions.Text = "The bobber screenshot could not be captured. Try again with the cursor centered on the bobber.";
            ShowValidation(reason);
            return;
        }

        using (var calibration = new BobberCalibrationForm(screenshot, screenshotOrigin, point))
        {
            if (calibration.ShowDialog(this) != DialogResult.OK)
            {
                btnCalibrateBobberPattern.Text = "Calibrate pixel pattern (F8)";
                lblBobberPatternInstructions.Text = "Pixel selection cancelled. Move the cursor over the bobber and capture again when ready.";
                SetStatus("Capture cancelled", warning: true);
                return;
            }

            settings.BobberPixelSamples = calibration.SelectedPixels.ToList();
            settings.BobberClickOffsetX = calibration.ClickOffset.X;
            settings.BobberClickOffsetY = calibration.ClickOffset.Y;
        }

        settings.BobberX = point.X;
        settings.BobberY = point.Y;
        settings.HasBobberPosition = true;
        settings.WindowHandle = window.Handle.ToInt64();
        btnCalibrateBobberPattern.Text = "Recalibrate pixel pattern (F8)";
        lblBobberPatternStatus.Text = $"Pattern: {settings.BobberPixelSamples.Count} pixels, click offset ({settings.BobberClickOffsetX}, {settings.BobberClickOffsetY})";
        lblBobberPatternInstructions.Text = "Pixel pattern saved. The bot will tolerate bobber jiggle and small size changes when scanning after each cast.";
        SetStatus("Bobber pixel pattern saved");
        Log($"Saved bobber pattern with {settings.BobberPixelSamples.Count} pixels and click offset ({settings.BobberClickOffsetX}, {settings.BobberClickOffsetY}).");
        SaveCurrentSettings(false);
    }

    private async void btnRecordSound_Click(object? sender, EventArgs e)
    {
        if (botTask is not null)
        {
            return;
        }

        var cancellation = new CancellationTokenSource();
        recordCancellation = cancellation;
        btnRecordSound.Enabled = false;
        btnPlaySound.Enabled = false;
        btnBrowseSound.Enabled = false;
        SetStatus("Recording audio");
        lblAudioStatus.Text = "Listening to the default output device...";
        Log($"Recording {numRecordSeconds.Value:0} seconds. Cast once and wait for the fish catch sound.");

        var outputPath = Path.Combine(settingsStore.DataDirectory, $"catch-{DateTime.Now:yyyyMMdd-HHmmss}.wav");
        try
        {
            var duration = TimeSpan.FromSeconds((double)numRecordSeconds.Value);
            await Task.Run(() =>
            {
                using var capture = new WasapiLoopbackCapture();
                var samples = capture.Capture(duration, cancellation.Token);
                if (samples.Length == 0)
                {
                    throw new InvalidDataException("No audio samples were captured.");
                }

                PcmWaveFile.Write(outputPath, capture.SampleRate, samples);
            }, cancellation.Token);

            txtSoundPath.Text = outputPath;
            settings.CatchSoundPath = outputPath;
            SaveCurrentSettings(false);
            UpdateSoundStatus();
            SetStatus("Catch sound saved");
            Log($"Saved catch sound sample: {outputPath}");
        }
        catch (OperationCanceledException)
        {
            SetStatus("Recording cancelled", warning: true);
            lblAudioStatus.Text = "Recording cancelled";
            Log("Audio recording cancelled.");
        }
        catch (Exception exception)
        {
            SetStatus("Audio error", warning: true);
            lblAudioStatus.Text = "Recording failed";
            Log($"Audio recording failed: {exception.Message}");
        }
        finally
        {
            if (ReferenceEquals(recordCancellation, cancellation))
            {
                recordCancellation = null;
            }

            cancellation.Dispose();
            btnRecordSound.Enabled = true;
            btnPlaySound.Enabled = true;
            btnBrowseSound.Enabled = true;
        }
    }

    private void btnBrowseSound_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "WAV audio files (*.wav)|*.wav|All files (*.*)|*.*",
            Title = "Choose a fish catch sound sample",
            CheckFileExists = true,
            Multiselect = false
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            _ = PcmWaveFile.Read(dialog.FileName);
            txtSoundPath.Text = dialog.FileName;
            settings.CatchSoundPath = dialog.FileName;
            SaveCurrentSettings(false);
            UpdateSoundStatus();
            Log($"Loaded catch sound sample: {dialog.FileName}");
        }
        catch (Exception exception)
        {
            ShowValidation($"That audio file cannot be used: {exception.Message}");
        }
    }

    private void btnPlaySound_Click(object? sender, EventArgs e)
    {
        var path = txtSoundPath.Text.Trim();
        if (!File.Exists(path))
        {
            ShowValidation("Record or select a WAV catch sound first.");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
        }
        catch (Exception exception)
        {
            ShowValidation($"The sample could not be played: {exception.Message}");
        }
    }

    private void btnSaveSettings_Click(object? sender, EventArgs e)
    {
        if (TryReadSettingsFromUi(requireRuntimeSetup: false, out _))
        {
            SaveCurrentSettings(true);
            SetStatus("Settings saved");
            Log($"Settings saved to {settingsStore.SettingsPath}");
        }
    }

    private void btnStart_Click(object? sender, EventArgs e)
    {
        if (botTask is not null)
        {
            return;
        }

        if (!TryReadSettingsFromUi(requireRuntimeSetup: true, out var window) || window is null)
        {
            return;
        }

        SaveCurrentSettings(false);
        if (!inputService.TryParseKey(settings.CastKey, out var castKey))
        {
            ShowValidation("The cast key is not valid.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination) &&
            !inputService.TryParseKeyCombination(settings.HideShowUiKeyCombination, out _))
        {
            ShowValidation("Record a valid hide/show UI key combination or clear it before starting.");
            return;
        }

        var cancellation = new CancellationTokenSource();
        botCancellation = cancellation;
        runningWindow = window;
        SetRunningUi(true);
        Log("Bot started. Stop the bot at any time to cancel the current listen operation.");
        botTask = Task.Run(() => RunBotAsync(window, castKey, cancellation.Token), cancellation.Token);
        _ = ObserveBotAsync(botTask, cancellation);
    }

    private async void btnStop_Click(object? sender, EventArgs e)
    {
        await StopBotAsync();
    }

    private async void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Escape || botCancellation is null)
        {
            return;
        }

        e.Handled = true;
        e.SuppressKeyPress = true;
        await StopBotAsync(killSwitch: true);
    }

    private async Task StopBotAsync(bool killSwitch = false)
    {
        if (botCancellation is null || stopRequested)
        {
            return;
        }

        stopRequested = true;
        SetStatus("Stopping", warning: true);
        Log(killSwitch
            ? "ESC kill switch pressed. Stopping the bot..."
            : "Stop requested. Finishing the current audio operation...");
        TryRestoreUiAfterFocusingWindow();

        botCancellation.Cancel();
        if (botTask is not null)
        {
            try
            {
                await botTask;
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Log($"Bot stopped with an error: {exception.Message}");
            }
        }
    }

    private bool PressConfiguredHideShowUiKey(string action)
    {
        if (string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination))
        {
            return true;
        }

        if (!inputService.TryParseKeyCombination(settings.HideShowUiKeyCombination, out var combination))
        {
            Log($"Configured hide/show UI key '{settings.HideShowUiKeyCombination}' is invalid; the bot will not {action}.");
            if (action == "start")
            {
                ShowValidation("Record a valid hide/show UI key combination or clear it before starting.");
            }

            return action != "start";
        }

        if (!inputService.PressKeyCombination(combination))
        {
            Log($"Windows rejected the configured hide/show UI key while attempting to {action} the bot.");
            return action != "start";
        }

        Log($"Pressed {InputService.FormatKeyCombination(combination)} to {action} the UI.");
        return true;
    }

    private bool TryRestoreUiAfterFocusingWindow()
    {
        if (!hideShowUiKeyNeedsRestore)
        {
            return true;
        }

        if (runningWindow is null || !windowService.TryFocus(runningWindow))
        {
            Log("Could not focus WoW before sending the hide/show UI key; restoration will be retried.");
            return false;
        }

        if (!WowWindowService.IsForeground(runningWindow.Handle) || !PressConfiguredHideShowUiKey("stop"))
        {
            Log("The hide/show UI key was not sent while WoW was focused; restoration will be retried.");
            return false;
        }

        hideShowUiKeyNeedsRestore = false;
        return true;
    }

    private async Task ObserveBotAsync(Task task, CancellationTokenSource cancellation)
    {
        try
        {
            await task;
        }
        catch (OperationCanceledException)
        {
            if (!isClosing)
            {
                Log("Bot worker cancelled.");
            }
        }
        catch (Exception exception)
        {
            if (!isClosing)
            {
                Log($"Bot worker stopped: {exception.Message}");
                SetStatus("Bot error", warning: true);
            }
        }
        finally
        {
            TryRestoreUiAfterFocusingWindow();

            if (ReferenceEquals(botCancellation, cancellation))
            {
                botCancellation = null;
                botTask = null;
                runningWindow = null;
                stopRequested = false;
                cancellation.Dispose();
                if (!isClosing)
                {
                    SetRunningUi(false);
                    SetStatus("Ready");
                }
            }
        }
    }

    private async Task RunBotAsync(WowWindowInfo window, Keys castKey, CancellationToken cancellationToken)
    {
        using var capture = new WasapiLoopbackCapture();
        var sample = PcmWaveFile.Read(settings.CatchSoundPath!);
        var matcher = new AudioSampleMatcher(sample, capture.SampleRate, settings.DetectionThreshold);
        var bobberPixels = settings.BobberPixelSamples.ToArray();
        var bobberClickOffset = new Point(settings.BobberClickOffsetX, settings.BobberClickOffsetY);
        Log($"Audio listener ready at {capture.SampleRate:N0} Hz. Sample: {matcher.SampleDuration.TotalSeconds:0.00}s. Match threshold: {settings.DetectionThreshold:0.00}.");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!windowService.TryFocus(window))
            {
                Log("Could not focus the selected WoW window; retrying in one second.");
                await Task.Delay(1000, cancellationToken);
                continue;
            }

            if (!hideShowUiKeyNeedsRestore)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!PressConfiguredHideShowUiKey("start"))
                {
                    throw new InvalidOperationException("The hide/show UI key could not be sent after focusing WoW.");
                }

                hideShowUiKeyNeedsRestore = !string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination);
            }

            if (!WowWindowService.IsForeground(window.Handle))
            {
                Log("WoW is not focused; casting was skipped.");
                await Task.Delay(500, cancellationToken);
                continue;
            }

            if (!inputService.PressKey(castKey))
            {
                throw new InvalidOperationException("Windows rejected the cast key input.");
            }

            Log(settings.UseInteractMode
                ? "Interact key pressed to cast. Waiting for the catch sound..."
                : "Cast key pressed. Searching the WoW client for the newly landed bobber...");
            if (settings.UseInteractMode)
            {
                Log("Interact mode listening immediately after casting to avoid missing a fast catch sound.");
            }
            else
            {
                await Task.Delay(settings.CastDelayMilliseconds, cancellationToken);
            }

            var bobberPoint = Point.Empty;
            var bobberScore = 0d;
            var bobberReason = "The bobber was not detected before the search timeout.";
            if (!settings.UseInteractMode)
            {
                var searchDeadline = DateTime.UtcNow.AddSeconds(settings.BobberSearchTimeoutSeconds);
                var bobberFound = false;
                var bobberAttempts = 0;
                while (DateTime.UtcNow < searchDeadline)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    bobberAttempts++;
                    if (bobberLocator.TryFindBobber(
                            window.Handle,
                            bobberPixels,
                            bobberClickOffset,
                            settings.ValidateBobberPixel,
                            settings.BobberColorTolerance,
                            settings.BobberNeighborhoodRadius,
                            settings.BobberMinimumMatchScorePercent,
                            settings.BobberSearchArea,
                            out bobberPoint,
                            out bobberScore,
                            out bobberReason))
                    {
                        bobberFound = true;
                        Log($"Bobber found on scan {bobberAttempts} at ({bobberPoint.X}, {bobberPoint.Y}) with score {bobberScore:0.00}. {bobberReason}");
                        break;
                    }

                    if (bobberAttempts == 1 || bobberAttempts % 5 == 0)
                    {
                        Log($"Bobber not found on scan {bobberAttempts}: {bobberReason}");
                    }

                    await Task.Delay(settings.PerformanceDelayMilliseconds, cancellationToken);
                }

                if (!bobberFound)
                {
                    Log($"Bobber not found after {bobberAttempts} scan(s): {bobberReason}");
                    continue;
                }

                SaveDetectedBobberPosition(bobberPoint, bobberScore);
                if (!WowWindowService.IsForeground(window.Handle))
                {
                    Log("WoW lost focus after bobber detection; this cast was ignored.");
                    continue;
                }
            }

            Log($"Listening for catch sound for {settings.DetectionTimeoutSeconds} seconds...");
            var detected = capture.ListenForMatch(
                matcher,
                TimeSpan.FromSeconds(settings.DetectionTimeoutSeconds),
                cancellationToken,
                out var audioMatch);
            if (!detected)
            {
                Log($"Catch sound not detected before timeout. Best score: {audioMatch.Score:0.00} (envelope {audioMatch.EnvelopeScore:0.00}, waveform {audioMatch.WaveformScore:0.00}, energy {audioMatch.EnergySimilarity:0.00}).");
                continue;
            }

            if (settings.UseInteractMode)
            {
                if (!windowService.TryFocus(window) || !WowWindowService.IsForeground(window.Handle))
                {
                    Log("Catch detected, but WoW could not be focused; interact key was not sent.");
                    continue;
                }

                await Task.Delay(settings.ClickDelayMilliseconds, cancellationToken);
                if (!inputService.PressKey(castKey))
                {
                    throw new InvalidOperationException("Windows rejected the interact key input.");
                }

                Log($"Catch sound detected with score {audioMatch.Score:0.00}. Interact key pressed to reel in.");
                await Task.Delay(settings.PerformanceDelayMilliseconds, cancellationToken);
                continue;
            }

            if (!WowWindowService.IsForeground(window.Handle))
            {
                Log("Catch detected, but WoW no longer has focus. Click skipped.");
                continue;
            }

            if (!bobberLocator.TryValidateAt(
                    window.Handle,
                    bobberPoint,
                    bobberPixels,
                    bobberClickOffset,
                    settings.ValidateBobberPixel,
                    28,
                    settings.BobberColorTolerance,
                    settings.BobberNeighborhoodRadius,
                    settings.BobberMinimumMatchScorePercent,
                    settings.BobberSearchArea,
                    out var clickPoint,
                    out bobberReason))
            {
                Log($"Catch detected, but bobber validation failed: {bobberReason}");
                continue;
            }

            Log($"Bobber validation succeeded at ({clickPoint.X}, {clickPoint.Y}).");

            SaveDetectedBobberPosition(clickPoint, bobberScore);
            inputService.MoveCursor(clickPoint);
            await Task.Delay(settings.ClickDelayMilliseconds, cancellationToken);
            if (!inputService.Click(settings.ClickButton))
            {
                throw new InvalidOperationException("Windows rejected the bobber click input.");
            }

            Log($"Catch sound detected with score {audioMatch.Score:0.00}. {settings.ClickButton} click sent at ({clickPoint.X}, {clickPoint.Y}).");
            await Task.Delay(settings.PerformanceDelayMilliseconds, cancellationToken);
        }
    }

    private void SaveDetectedBobberPosition(Point point, double score)
    {
        settings.BobberX = point.X;
        settings.BobberY = point.Y;
        settings.HasBobberPosition = true;
        try
        {
            settingsStore.Save(settings);
        }
        catch (IOException exception)
        {
            Log($"Bobber position detected but could not be persisted: {exception.Message}");
        }

        UpdateDetectedBobberUi(point, score);
    }

    private void UpdateDetectedBobberUi(Point point, double score)
    {
        if (isClosing || IsDisposed)
        {
            return;
        }

        if (InvokeRequired)
        {
            try
            {
                BeginInvoke(() => UpdateDetectedBobberUi(point, score));
            }
            catch (InvalidOperationException)
            {
            }

            return;
        }

        lblBobberPatternStatus.Text = $"Last detection: ({point.X}, {point.Y})  •  score {score:0.00}";
    }

    private bool TryReadSettingsFromUi(bool requireRuntimeSetup, out WowWindowInfo? window)
    {
        window = GetSelectedWindow();
        if (window is null)
        {
            ShowValidation("Select a detected WoW window first.");
            return false;
        }

        if (!inputService.TryParseKey(settings.CastKey, out _))
        {
            ShowValidation("Record a valid cast key, such as 1, F5, Space, or NumPad1.");
            return false;
        }

        settings.WindowTitle = txtWindowTitle.Text.Trim();
        settings.ProcessName = txtProcessName.Text.Trim();
        settings.CastKey = settings.CastKey.Trim();
        settings.UseInteractMode = chkInteractMode.Checked;
        settings.HideShowUiKeyCombination = settings.HideShowUiKeyCombination.Trim();
        settings.ClickButton = cmbClickButton.SelectedItem is MouseButton button ? button : MouseButton.Right;
        settings.CastDelayMilliseconds = (int)numCastDelay.Value;
        settings.DetectionTimeoutSeconds = (int)numTimeout.Value;
        settings.ClickDelayMilliseconds = (int)numClickDelay.Value;
        settings.PerformanceDelayMilliseconds = (int)numPerformanceDelay.Value;
        settings.DetectionThreshold = (double)numThreshold.Value;
        settings.ValidateBobberPixel = chkValidateBobber.Checked;
        settings.BobberColorTolerance = (int)numPixelColorTolerance.Value;
        settings.BobberNeighborhoodRadius = (int)numPixelNeighborhoodRadius.Value;
        settings.BobberMinimumMatchScorePercent = (int)numPixelMatchScore.Value;
        settings.WindowHandle = window.Handle.ToInt64();
        settings.CatchSoundPath = txtSoundPath.Text.Trim();

        if (requireRuntimeSetup)
        {
            if (!settings.UseInteractMode && !settings.HasBobberPixelPattern)
            {
                ShowValidation("Calibrate the bobber pixel pattern by selecting the red and blue feather pixels before starting the bot.");
                return false;
            }

            if (!settings.HasCatchSound)
            {
                ShowValidation("Record or select a valid WAV catch sound before starting the bot.");
                return false;
            }
        }

        return true;
    }

    private WowWindowInfo? GetSelectedWindow() => cmbWindows.SelectedItem as WowWindowInfo;

    private void ApplySettingsToUi()
    {
        txtProcessName.Text = settings.ProcessName;
        txtWindowTitle.Text = settings.WindowTitle;
        lblCastKeyValue.Text = string.IsNullOrWhiteSpace(settings.CastKey) ? "Not configured" : settings.CastKey;
        chkInteractMode.Checked = settings.UseInteractMode;
        lblHideShowUiKeyValue.Text = string.IsNullOrWhiteSpace(settings.HideShowUiKeyCombination) ? "Not configured" : settings.HideShowUiKeyCombination;
        cmbClickButton.SelectedItem = settings.ClickButton;
        numCastDelay.Value = Math.Clamp(settings.CastDelayMilliseconds, (int)numCastDelay.Minimum, (int)numCastDelay.Maximum);
        numTimeout.Value = Math.Clamp(settings.DetectionTimeoutSeconds, (int)numTimeout.Minimum, (int)numTimeout.Maximum);
        numClickDelay.Value = Math.Clamp(settings.ClickDelayMilliseconds, (int)numClickDelay.Minimum, (int)numClickDelay.Maximum);
        numPerformanceDelay.Value = Math.Clamp(settings.PerformanceDelayMilliseconds, (int)numPerformanceDelay.Minimum, (int)numPerformanceDelay.Maximum);
        numThreshold.Value = Math.Clamp((decimal)settings.DetectionThreshold, numThreshold.Minimum, numThreshold.Maximum);
        chkValidateBobber.Checked = settings.ValidateBobberPixel;
        numPixelColorTolerance.Value = Math.Clamp(settings.BobberColorTolerance, (int)numPixelColorTolerance.Minimum, (int)numPixelColorTolerance.Maximum);
        numPixelNeighborhoodRadius.Value = Math.Clamp(settings.BobberNeighborhoodRadius, (int)numPixelNeighborhoodRadius.Minimum, (int)numPixelNeighborhoodRadius.Maximum);
        numPixelMatchScore.Value = Math.Clamp(settings.BobberMinimumMatchScorePercent, (int)numPixelMatchScore.Minimum, (int)numPixelMatchScore.Maximum);
        txtSoundPath.Text = settings.CatchSoundPath ?? string.Empty;
        lblBobberSearchArea.Text = settings.BobberSearchArea is { IsValid: true } area
            ? $"Search area: ({area.X}, {area.Y}) {area.Width} x {area.Height}"
            : "Search area: full client";
        if (settings.HasBobberPixelPattern)
        {
            lblBobberPatternStatus.Text = $"Pattern: {settings.BobberPixelSamples.Count} pixels, click offset ({settings.BobberClickOffsetX}, {settings.BobberClickOffsetY})";
        }
        else if (settings.HasBobberPosition)
        {
            lblBobberPatternStatus.Text = $"Last detection: ({settings.BobberX}, {settings.BobberY})";
        }
    }

    private void UpdateSoundStatus()
    {
        if (File.Exists(txtSoundPath.Text.Trim()))
        {
            try
            {
                var wave = PcmWaveFile.Read(txtSoundPath.Text.Trim());
                lblAudioStatus.Text = $"Loaded  •  {wave.SampleRate:N0} Hz  •  {wave.Samples.Length / (double)wave.SampleRate:0.0}s";
                return;
            }
            catch (InvalidDataException)
            {
            }
        }

        lblAudioStatus.Text = "No valid sample loaded";
    }

    private void SaveCurrentSettings(bool logResult)
    {
        try
        {
            settingsStore.Save(settings);
            if (logResult)
            {
                Log("Settings saved.");
            }
        }
        catch (Exception exception)
        {
            ShowValidation($"Settings could not be saved: {exception.Message}");
        }
    }

    private void SetRunningUi(bool running)
    {
        if (InvokeRequired)
        {
            if (!IsDisposed)
            {
                BeginInvoke(() => SetRunningUi(running));
            }
            return;
        }

        btnStart.Enabled = !running;
        btnStop.Enabled = running;
        btnSaveSettings.Enabled = !running;
        btnRefreshWindows.Enabled = !running;
        chkInteractMode.Enabled = !running;
        btnRecordSound.Enabled = !running;
        btnBrowseSound.Enabled = !running;
        btnPlaySound.Enabled = !running;
        UpdateInteractModeUi();
        lblFooter.Text = running ? "Worker thread active" : "Ready for setup";
        if (running)
        {
            SetStatus("Bot running");
        }
    }

    private void SetStatus(string message, bool warning = false)
    {
        if (isClosing || IsDisposed)
        {
            return;
        }

        if (InvokeRequired)
        {
            BeginInvoke(() => SetStatus(message, warning));
            return;
        }

        lblStatus.Text = message;
        lblStatus.ForeColor = warning ? Color.FromArgb(253, 186, 116) : Color.FromArgb(134, 239, 172);
    }

    private void ShowValidation(string message)
    {
        SetStatus("Needs attention", warning: true);
        Log(message);
        MessageBox.Show(this, message, "WoW Fish Bot", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void Log(string message)
    {
        if (isClosing || IsDisposed)
        {
            return;
        }

        if (InvokeRequired)
        {
            try
            {
                BeginInvoke(() => Log(message));
            }
            catch (InvalidOperationException)
            {
            }
            return;
        }

        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        txtLog.SelectionStart = txtLog.TextLength;
        txtLog.ScrollToCaret();
    }

    private async void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (isClosing)
        {
            return;
        }

        isClosing = true;
        recordCancellation?.Cancel();
        botCancellation?.Cancel();
        if (botTask is not null)
        {
            try
            {
                await botTask;
            }
            catch
            {
            }
        }

        SaveCurrentSettings(false);
    }

    private delegate IntPtr LowLevelKeyboardProc(int code, IntPtr message, IntPtr data);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int hookType, LowLevelKeyboardProc callback, IntPtr moduleHandle, uint threadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hookHandle, int code, IntPtr message, IntPtr data);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string? moduleName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr handle, int id, uint modifiers, uint virtualKey);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr handle, int id);


}
