namespace wowfishbot;

public sealed partial class KeyCombinationCaptureForm : Form
{
    private readonly bool allowModifiers;
    private Keys capturedCombination;
    private Keys pendingCombination;

    public KeyCombinationCaptureForm()
        : this("Record key", "Press the key to record.", allowModifiers: false)
    {
    }

    public KeyCombinationCaptureForm(string title, string instruction, bool allowModifiers, Keys? initialCombination = null)
    {
        this.allowModifiers = allowModifiers;
        capturedCombination = initialCombination ?? Keys.None;
        InitializeComponent();
        Text = title;
        instructionLabel.Text = instruction;
        UpdateDisplay();
    }

    public Keys CapturedCombination => capturedCombination;

    internal void ClearFromGlobalEscape()
    {
        ClearCapturedCombination();
        DialogResult = DialogResult.OK;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if ((keyData & Keys.KeyCode) == Keys.Escape)
        {
            ClearCapturedCombination();
            DialogResult = DialogResult.OK;
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
        if ((keyData & Keys.KeyCode) == Keys.Escape)
        {
            ClearFromGlobalEscape();
            return true;
        }

        return base.ProcessDialogKey(keyData);
    }

    private void KeyCombinationCaptureForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            ClearCapturedCombination();
            DialogResult = DialogResult.OK;
            e.SuppressKeyPress = true;
            e.Handled = true;
            return;
        }

        if (e.KeyCode is Keys.ControlKey or Keys.ShiftKey or Keys.Menu or Keys.LWin or Keys.RWin)
        {
            return;
        }

        var modifiers = allowModifiers ? e.Modifiers & (Keys.Control | Keys.Alt | Keys.Shift) : Keys.None;
        pendingCombination = modifiers | e.KeyCode;
        capturedCombination = pendingCombination;
        UpdateDisplay();
        e.SuppressKeyPress = true;
        e.Handled = true;
    }

    private void KeyCombinationCaptureForm_KeyUp(object? sender, KeyEventArgs e)
    {
        if (pendingCombination == Keys.None || (pendingCombination & Keys.KeyCode) != e.KeyCode)
        {
            return;
        }

        pendingCombination = Keys.None;
        DialogResult = DialogResult.OK;
        e.SuppressKeyPress = true;
        e.Handled = true;
    }

    private void UpdateDisplay()
    {
        combinationLabel.Text = capturedCombination == Keys.None
            ? "No key recorded"
            : Services.InputService.FormatKeyCombination(capturedCombination);
        confirmButton.Enabled = capturedCombination != Keys.None;
    }

    private void clearButton_Click(object? sender, EventArgs e)
    {
        ClearCapturedCombination();
        Focus();
    }

    private void ClearCapturedCombination()
    {
        capturedCombination = Keys.None;
        pendingCombination = Keys.None;
        UpdateDisplay();
    }

    private void confirmButton_Click(object? sender, EventArgs e)
    {
        if (capturedCombination != Keys.None)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
