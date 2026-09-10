namespace wowfishbot;

partial class BobberCalibrationForm
{
    private System.ComponentModel.IContainer? components;
    private Label instructionLabel = null!;
    private Panel imagePanel = null!;
    private PictureBox imageBox = null!;
    private Panel footerPanel = null!;
    private Label selectionLabel = null!;
    private Button undoButton = null!;
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
        undoButton = new Button();
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
        instructionLabel.Size = new Size(820, 58);
        instructionLabel.TabIndex = 0;
        instructionLabel.Text = "Select the red feather pixel first, the blue feather pixel second, and optionally one more stable pixel. Do not select the highlighted cursor pixel.";
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
        imageBox.MouseClick += ImageBox_MouseClick;
        // 
        // footerPanel
        // 
        footerPanel.BackColor = Color.FromArgb(226, 232, 240);
        footerPanel.Controls.Add(cancelButton);
        footerPanel.Controls.Add(confirmButton);
        footerPanel.Controls.Add(clearButton);
        footerPanel.Controls.Add(undoButton);
        footerPanel.Controls.Add(selectionLabel);
        footerPanel.Dock = DockStyle.Bottom;
        footerPanel.Location = new Point(0, 628);
        footerPanel.Name = "footerPanel";
        footerPanel.Padding = new Padding(12, 10, 12, 10);
        footerPanel.Size = new Size(820, 72);
        footerPanel.TabIndex = 2;
        // 
        // selectionLabel
        // 
        selectionLabel.Dock = DockStyle.Left;
        selectionLabel.ForeColor = Color.FromArgb(51, 65, 85);
        selectionLabel.Location = new Point(12, 10);
        selectionLabel.Name = "selectionLabel";
        selectionLabel.Size = new Size(390, 52);
        selectionLabel.TabIndex = 0;
        selectionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // undoButton
        // 
        undoButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        undoButton.FlatStyle = FlatStyle.Flat;
        undoButton.Location = new Point(412, 12);
        undoButton.Name = "undoButton";
        undoButton.Size = new Size(90, 38);
        undoButton.TabIndex = 1;
        undoButton.Text = "Undo";
        undoButton.UseVisualStyleBackColor = true;
        undoButton.Click += undoButton_Click;
        // 
        // clearButton
        // 
        clearButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        clearButton.FlatStyle = FlatStyle.Flat;
        clearButton.Location = new Point(507, 12);
        clearButton.Name = "clearButton";
        clearButton.Size = new Size(90, 38);
        clearButton.TabIndex = 2;
        clearButton.Text = "Clear";
        clearButton.UseVisualStyleBackColor = true;
        clearButton.Click += clearButton_Click;
        // 
        // confirmButton
        // 
        confirmButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        confirmButton.BackColor = Color.FromArgb(22, 163, 74);
        confirmButton.DialogResult = DialogResult.OK;
        confirmButton.Enabled = false;
        confirmButton.FlatStyle = FlatStyle.Flat;
        confirmButton.ForeColor = Color.White;
        confirmButton.Location = new Point(602, 12);
        confirmButton.Name = "confirmButton";
        confirmButton.Size = new Size(100, 38);
        confirmButton.TabIndex = 3;
        confirmButton.Text = "Confirm pixels";
        confirmButton.UseVisualStyleBackColor = false;
        confirmButton.Click += confirmButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.Location = new Point(707, 12);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(100, 38);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // BobberCalibrationForm
        // 
        AcceptButton = confirmButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(241, 245, 249);
        CancelButton = cancelButton;
        ClientSize = new Size(820, 700);
        Controls.Add(imagePanel);
        Controls.Add(footerPanel);
        Controls.Add(instructionLabel);
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(720, 620);
        Name = "BobberCalibrationForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Calibrate Bobber Pixels";
        ((System.ComponentModel.ISupportInitialize)imageBox).EndInit();
        imagePanel.ResumeLayout(false);
        footerPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            displayBitmap?.Dispose();
            sourceBitmap.Dispose();
            components?.Dispose();
        }

        base.Dispose(disposing);
    }
}
