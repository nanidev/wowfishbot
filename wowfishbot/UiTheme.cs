namespace wowfishbot;

internal static class UiTheme
{
    public static Color ApplicationBackground { get; } = Color.FromArgb(11, 18, 32);
    public static Color PanelBackground { get; } = Color.FromArgb(18, 28, 46);
    public static Color ElevatedPanelBackground { get; } = Color.FromArgb(26, 39, 62);
    public static Color FooterBackground { get; } = Color.FromArgb(7, 12, 23);
    public static Color Border { get; } = Color.FromArgb(61, 79, 108);
    public static Color GoldAccent { get; } = Color.FromArgb(214, 158, 67);
    public static Color BlueAccent { get; } = Color.FromArgb(67, 139, 202);
    public static Color NormalText { get; } = Color.FromArgb(226, 232, 240);
    public static Color MutedText { get; } = Color.FromArgb(156, 171, 194);
    public static Color Success { get; } = Color.FromArgb(91, 191, 126);
    public static Color Warning { get; } = Color.FromArgb(234, 170, 91);
    public static Color Danger { get; } = Color.FromArgb(190, 65, 70);

    public static void Apply(Form1 form)
    {
        form.BackColor = ApplicationBackground;
        ApplyContainer(form, form.Controls);
        ApplyButton(FindControl<Button>(form, "btnStart"), GoldAccent, Color.FromArgb(20, 29, 45));
        ApplyButton(FindControl<Button>(form, "btnStop"), Danger, Color.White);
        ApplyButton(FindControl<Button>(form, "btnSaveSettings"), Color.FromArgb(58, 86, 119), NormalText);
        ApplyButton(FindControl<Button>(form, "btnToggleLog"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "btnMinimize"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "btnMaximize"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "btnClose"), Danger, Color.White);
        ApplyButton(FindControl<Button>(form, "btnDonate"), Color.FromArgb(151, 65, 104), Color.White);
        var footer = FindControl<Panel>(form, "footerPanel");
        footer.BackColor = FooterBackground;
        var status = FindControl<Label>(form, "lblStatus");
        status.BackColor = ElevatedPanelBackground;
        status.ForeColor = Success;
        status.BorderStyle = BorderStyle.FixedSingle;
        status.Padding = new Padding(10, 4, 10, 0);
        var log = FindControl<RichTextBox>(form, "txtLog");
        log.BackColor = Color.FromArgb(8, 14, 25);
        log.ForeColor = NormalText;
        var repositoryLink = FindControl<LinkLabel>(form, "lnkOfficialRepository");
        repositoryLink.LinkColor = BlueAccent;
        repositoryLink.ActiveLinkColor = GoldAccent;
        var footerLabel = FindControl<Label>(form, "lblFooter");
        footerLabel.ForeColor = MutedText;
    }

    public static void ApplyDialog(Form form)
    {
        form.BackColor = ApplicationBackground;
        ApplyContainer(form, form.Controls);
    }

    public static void ApplyAreaSelection(AreaSelectionForm form)
    {
        ApplyDialog(form);
        ApplyDialogHeader(form, "instructionLabel");
        ApplyDialogFooter(form, "footerPanel");
        ApplyButton(FindControl<Button>(form, "clearButton"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "confirmButton"), GoldAccent, Color.FromArgb(20, 29, 45));
        ApplyButton(FindControl<Button>(form, "cancelButton"), ElevatedPanelBackground, NormalText);
        FindControl<Panel>(form, "imagePanel").BackColor = Color.FromArgb(8, 14, 25);
        FindControl<Label>(form, "selectionLabel").ForeColor = MutedText;
    }

    public static void ApplyBobberCalibration(BobberCalibrationForm form)
    {
        ApplyDialog(form);
        ApplyDialogHeader(form, "instructionLabel");
        ApplyDialogFooter(form, "footerPanel");
        ApplyButton(FindControl<Button>(form, "undoButton"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "clearButton"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "confirmButton"), GoldAccent, Color.FromArgb(20, 29, 45));
        ApplyButton(FindControl<Button>(form, "cancelButton"), ElevatedPanelBackground, NormalText);
        FindControl<Panel>(form, "imagePanel").BackColor = Color.FromArgb(8, 14, 25);
        FindControl<Label>(form, "selectionLabel").ForeColor = MutedText;
    }

    public static void ApplyKeyCombinationCapture(KeyCombinationCaptureForm form)
    {
        ApplyDialog(form);
        ApplyDialogHeader(form, "instructionLabel");
        ApplyButton(FindControl<Button>(form, "clearButton"), ElevatedPanelBackground, NormalText);
        ApplyButton(FindControl<Button>(form, "confirmButton"), GoldAccent, Color.FromArgb(20, 29, 45));
        ApplyButton(FindControl<Button>(form, "cancelButton"), ElevatedPanelBackground, NormalText);

        var combinationLabel = FindControl<Label>(form, "combinationLabel");
        combinationLabel.BackColor = ElevatedPanelBackground;
        combinationLabel.ForeColor = NormalText;
        combinationLabel.BorderStyle = BorderStyle.FixedSingle;
    }

    private static void ApplyDialogHeader(Form form, string name)
    {
        var header = FindControl<Label>(form, name);
        header.BackColor = Color.FromArgb(17, 24, 39);
        header.ForeColor = NormalText;
    }

    private static void ApplyDialogFooter(Form form, string name)
    {
        FindControl<Panel>(form, name).BackColor = FooterBackground;
    }

    private static void ApplyContainer(Control parent, Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            switch (control)
            {
                case GroupBox group:
                    group.BackColor = PanelBackground;
                    group.ForeColor = GoldAccent;
                    group.Padding = new Padding(10);
                    group.Margin = new Padding(0, 8, 0, 0);
                    break;
                case Panel panel when panel.Name != "headerPanel" && panel.Name != "footerPanel":
                    panel.BackColor = ApplicationBackground;
                    break;
                case Label label when label.Name is not "lblTitle" and not "lblSubtitle" and not "lblStatus":
                    label.ForeColor = MutedText;
                    break;
                case TextBox textBox:
                    textBox.BackColor = ElevatedPanelBackground;
                    textBox.ForeColor = NormalText;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case ComboBox comboBox:
                    comboBox.BackColor = ElevatedPanelBackground;
                    comboBox.ForeColor = NormalText;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;
                case NumericUpDown numericUpDown:
                    numericUpDown.BackColor = ElevatedPanelBackground;
                    numericUpDown.ForeColor = NormalText;
                    break;
                case CheckBox checkBox:
                    checkBox.ForeColor = NormalText;
                    break;
                case Button button:
                    ApplyButton(button, ElevatedPanelBackground, NormalText);
                    break;
                case RichTextBox richTextBox:
                    richTextBox.BackColor = Color.FromArgb(8, 14, 25);
                    richTextBox.ForeColor = NormalText;
                    break;
            }

            if (control.HasChildren)
            {
                ApplyContainer(control, control.Controls);
            }
        }
    }

    private static void ApplyButton(Button button, Color backColor, Color foreColor)
    {
        button.BackColor = backColor;
        button.ForeColor = foreColor;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = Border;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(
            Math.Min(255, backColor.R + 18),
            Math.Min(255, backColor.G + 18),
            Math.Min(255, backColor.B + 18));
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(
            Math.Max(0, backColor.R - 16),
            Math.Max(0, backColor.G - 16),
            Math.Max(0, backColor.B - 16));
    }

    private static T FindControl<T>(Control parent, string name) where T : Control
    {
        var control = FindControl(parent.Controls, name);
        return control as T ?? throw new InvalidOperationException($"The themed control '{name}' was not found.");
    }

    private static Control? FindControl(Control.ControlCollection controls, string name)
    {
        foreach (Control control in controls)
        {
            if (control.Name == name)
            {
                return control;
            }

            if (control.HasChildren && FindControl(control.Controls, name) is { } nested)
            {
                return nested;
            }
        }

        return null;
    }
}
