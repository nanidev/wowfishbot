#nullable enable

namespace wowfishbot;

public sealed partial class AreaSelectionForm
{
    private System.ComponentModel.IContainer? components;
    private Label instructionLabel = null!;
    private Panel imagePanel = null!;
    private PictureBox imageBox = null!;
    private Panel footerPanel = null!;
    private Label selectionLabel = null!;
    private Button clearButton = null!;
    private Button confirmButton = null!;
    private Button cancelButton = null!;

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        instructionLabel = new Label();
        imagePanel = new Panel();
        imageBox = new PictureBox();
        footerPanel = new Panel();
        selectionLabel = new Label();
        clearButton = new Button();
        confirmButton = new Button();
        cancelButton = new Button();
        ((System.ComponentModel.ISupportInitialize)imageBox).BeginInit();
        imagePanel.SuspendLayout();
        footerPanel.SuspendLayout();
        SuspendLayout();
        // 
        // instructionLabel
        // 
        instructionLabel.BackColor = Color.FromArgb(17, 24, 39);
        instructionLabel.Dock = DockStyle.Top;
        instructionLabel.ForeColor = Color.White;
        instructionLabel.Location = new Point(0, 0);
        instructionLabel.Name = "instructionLabel";
        instructionLabel.Padding = new Padding(14, 10, 14, 4);
        instructionLabel.Size = new Size(900, 58);
        instructionLabel.TabIndex = 0;
        instructionLabel.Text = "Drag over the area of the WoW client where the bobber can appear. The rectangle is stored relative to the client window.";
        instructionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // imagePanel
        // 
        imagePanel.AutoScroll = true;
        imagePanel.BackColor = Color.FromArgb(15, 23, 42);
        imagePanel.Controls.Add(imageBox);
        imagePanel.Dock = DockStyle.Fill;
        imagePanel.Location = new Point(0, 58);
        imagePanel.Name = "imagePanel";
        imagePanel.Padding = new Padding(12);
        imagePanel.TabIndex = 1;
        // 
        // imageBox
        // 
        imageBox.BackColor = Color.Black;
        imageBox.Cursor = Cursors.Cross;
        imageBox.Location = new Point(12, 12);
        imageBox.Name = "imageBox";
        imageBox.Size = new Size(1, 1);
        imageBox.SizeMode = PictureBoxSizeMode.Normal;
        imageBox.TabIndex = 0;
        imageBox.TabStop = false;
        imageBox.MouseDown += imageBox_MouseDown;
        imageBox.MouseMove += imageBox_MouseMove;
        imageBox.MouseUp += imageBox_MouseUp;
        imageBox.Paint += imageBox_Paint;
        // 
        // footerPanel
        // 
        footerPanel.BackColor = Color.FromArgb(226, 232, 240);
        footerPanel.Controls.Add(cancelButton);
        footerPanel.Controls.Add(confirmButton);
        footerPanel.Controls.Add(clearButton);
        footerPanel.Controls.Add(selectionLabel);
        footerPanel.Dock = DockStyle.Bottom;
        footerPanel.Location = new Point(0, 628);
        footerPanel.Name = "footerPanel";
        footerPanel.Padding = new Padding(12, 10, 12, 10);
        footerPanel.Size = new Size(900, 72);
        footerPanel.TabIndex = 2;
        // 
        // selectionLabel
        // 
        selectionLabel.Dock = DockStyle.Left;
        selectionLabel.ForeColor = Color.FromArgb(51, 65, 85);
        selectionLabel.Location = new Point(12, 10);
        selectionLabel.Name = "selectionLabel";
        selectionLabel.Size = new Size(540, 52);
        selectionLabel.TabIndex = 0;
        selectionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // clearButton
        // 
        clearButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        clearButton.FlatStyle = FlatStyle.Flat;
        clearButton.Location = new Point(562, 12);
        clearButton.Name = "clearButton";
        clearButton.Size = new Size(90, 38);
        clearButton.TabIndex = 1;
        clearButton.Text = "Clear area";
        clearButton.UseVisualStyleBackColor = true;
        clearButton.Click += clearButton_Click;
        // 
        // confirmButton
        // 
        confirmButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        confirmButton.BackColor = Color.FromArgb(22, 163, 74);
        confirmButton.Enabled = false;
        confirmButton.FlatStyle = FlatStyle.Flat;
        confirmButton.ForeColor = Color.White;
        confirmButton.Location = new Point(657, 12);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new Size(110, 38);
        confirmButton.TabIndex = 2;
        confirmButton.Text = "Save area";
        confirmButton.UseVisualStyleBackColor = false;
        confirmButton.Click += confirmButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.Location = new Point(772, 12);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(100, 38);
        cancelButton.TabIndex = 3;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // AreaSelectionForm
        // 
        AcceptButton = confirmButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(241, 245, 249);
        CancelButton = cancelButton;
        ClientSize = new Size(900, 700);
        Controls.Add(imagePanel);
        Controls.Add(footerPanel);
        Controls.Add(instructionLabel);
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(700, 500);
        Name = "AreaSelectionForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Select bobber search area";
        ((System.ComponentModel.ISupportInitialize)imageBox).EndInit();
        imagePanel.ResumeLayout(false);
        footerPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
