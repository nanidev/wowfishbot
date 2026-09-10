#nullable enable

namespace wowfishbot;

public sealed partial class KeyCombinationCaptureForm
{
    private System.ComponentModel.IContainer? components;
    private Label instructionLabel = null!;
    private Label combinationLabel = null!;
    private Button clearButton = null!;
    private Button confirmButton = null!;
    private Button cancelButton = null!;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        instructionLabel = new Label();
        combinationLabel = new Label();
        clearButton = new Button();
        confirmButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // 
        // instructionLabel
        // 
        instructionLabel.Dock = DockStyle.Top;
        instructionLabel.Location = new Point(0, 0);
        instructionLabel.Name = "instructionLabel";
        instructionLabel.Padding = new Padding(16, 14, 16, 8);
        instructionLabel.Size = new Size(520, 64);
        instructionLabel.TabIndex = 0;
        instructionLabel.Text = "Press a key combination.";
        instructionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // combinationLabel
        // 
        combinationLabel.BackColor = Color.FromArgb(226, 232, 240);
        combinationLabel.Dock = DockStyle.Fill;
        combinationLabel.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
        combinationLabel.Location = new Point(0, 64);
        combinationLabel.Name = "combinationLabel";
        combinationLabel.Size = new Size(520, 104);
        combinationLabel.TabIndex = 1;
        combinationLabel.Text = "No key recorded";
        combinationLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // clearButton
        // 
        clearButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        clearButton.FlatStyle = FlatStyle.Flat;
        clearButton.Location = new Point(214, 181);
        clearButton.Name = "clearButton";
        clearButton.Size = new Size(90, 34);
        clearButton.TabIndex = 2;
        clearButton.Text = "Clear";
        clearButton.UseVisualStyleBackColor = true;
        clearButton.Click += clearButton_Click;
        // 
        // confirmButton
        // 
        confirmButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        confirmButton.BackColor = Color.FromArgb(22, 163, 74);
        confirmButton.Enabled = false;
        confirmButton.FlatStyle = FlatStyle.Flat;
        confirmButton.ForeColor = Color.White;
        confirmButton.Location = new Point(309, 181);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new Size(100, 34);
        confirmButton.TabIndex = 3;
        confirmButton.Text = "Save key";
        confirmButton.UseVisualStyleBackColor = false;
        confirmButton.Click += confirmButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.Location = new Point(414, 181);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(90, 34);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // KeyCombinationCaptureForm
        // 
        AcceptButton = confirmButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(520, 230);
        Controls.Add(combinationLabel);
        Controls.Add(cancelButton);
        Controls.Add(confirmButton);
        Controls.Add(clearButton);
        Controls.Add(instructionLabel);
        KeyPreview = true;
        MinimumSize = new Size(520, 230);
        Name = "KeyCombinationCaptureForm";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        KeyDown += KeyCombinationCaptureForm_KeyDown;
        KeyUp += KeyCombinationCaptureForm_KeyUp;
        ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }
}
