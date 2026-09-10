namespace wowfishbot
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnDonate;
        private Label lblSubtitle;
        private Label lblStatus;
        private FlowLayoutPanel titleBarControls;
        private Button btnMinimize;
        private Button btnMaximize;
        private Button btnClose;
        private TableLayoutPanel mainLayout;
        private Panel leftScrollPanel;
        private TableLayoutPanel leftLayout;
        private GroupBox grpTarget;
        private TableLayoutPanel targetLayout;
        private Label lblWindow;
        private ComboBox cmbWindows;
        private Button btnRefreshWindows;
        private Label lblProcess;
        private TextBox txtProcessName;
        private Label lblTitleFilter;
        private TextBox txtWindowTitle;
        private Button btnCalibrateBobberPattern;
        private Label lblBobberPatternInstructions;
        private GroupBox grpAudio;
        private TableLayoutPanel audioLayout;
        private Label lblCatchSound;
        private TextBox txtSoundPath;
        private Button btnBrowseSound;
        private Label lblRecordDuration;
        private NumericUpDown numRecordSeconds;
        private Button btnRecordSound;
        private Button btnPlaySound;
        private Label lblAudioStatus;
        private GroupBox grpOptions;
        private TableLayoutPanel optionsLayout;
        private Label lblCastKey;
        private Label lblBobberPatternStatus;
        private Button btnRecordCastKey;
        private Label lblCastKeyValue;
        private Label lblHideShowUiKey;
        private Button btnRecordHideShowUiKey;
        private Label lblHideShowUiKeyValue;
        private Label lblPixelColorTolerance;
        private NumericUpDown numPixelColorTolerance;
        private Label lblPixelNeighborhoodRadius;
        private NumericUpDown numPixelNeighborhoodRadius;
        private Label lblPixelMatchScore;
        private NumericUpDown numPixelMatchScore;
        private Button btnSelectBobberSearchArea;
        private Label lblBobberSearchArea;
        private Label lblClickButton;
        private ComboBox cmbClickButton;
        private Label lblCastDelay;
        private NumericUpDown numCastDelay;
        private Label lblTimeout;
        private NumericUpDown numTimeout;
        private Label lblClickDelay;
        private NumericUpDown numClickDelay;
        private Label lblPerformanceDelay;
        private NumericUpDown numPerformanceDelay;
        private Label lblThreshold;
        private NumericUpDown numThreshold;
        private CheckBox chkValidateBobber;
        private CheckBox chkInteractMode;
        private GroupBox grpLog;
        private RichTextBox txtLog;
        private Panel footerPanel;
        private Label lblFooter;
        private LinkLabel lnkOfficialRepository;
        private Button btnSaveSettings;
        private Button btnToggleLog;
        private Button btnStart;
        private Button btnStop;
        private ToolTip toolTip;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            headerPanel = new Panel();
            titleBarControls = new FlowLayoutPanel();
            lblStatus = new Label();
            btnMinimize = new Button();
            btnMaximize = new Button();
            btnClose = new Button();
            btnDonate = new Button();
            lblSubtitle = new Label();
            lblTitle = new Label();
            mainLayout = new TableLayoutPanel();
            leftScrollPanel = new Panel();
            leftLayout = new TableLayoutPanel();
            grpTarget = new GroupBox();
            targetLayout = new TableLayoutPanel();
            lblWindow = new Label();
            cmbWindows = new ComboBox();
            lblProcess = new Label();
            txtProcessName = new TextBox();
            lblTitleFilter = new Label();
            txtWindowTitle = new TextBox();
            btnRefreshWindows = new Button();
            grpOptions = new GroupBox();
            optionsLayout = new TableLayoutPanel();
            lblBobberPatternInstructions = new Label();
            lblBobberPatternStatus = new Label();
            lblCastKey = new Label();
            lblClickButton = new Label();
            lblCastDelay = new Label();
            lblTimeout = new Label();
            lblClickDelay = new Label();
            lblPerformanceDelay = new Label();
            lblThreshold = new Label();
            btnRecordCastKey = new Button();
            lblCastKeyValue = new Label();
            cmbClickButton = new ComboBox();
            numCastDelay = new NumericUpDown();
            numTimeout = new NumericUpDown();
            numClickDelay = new NumericUpDown();
            numPerformanceDelay = new NumericUpDown();
            numThreshold = new NumericUpDown();
            chkValidateBobber = new CheckBox();
            chkInteractMode = new CheckBox();
            btnRecordHideShowUiKey = new Button();
            btnCalibrateBobberPattern = new Button();
            lblHideShowUiKeyValue = new Label();
            lblHideShowUiKey = new Label();
            numPixelColorTolerance = new NumericUpDown();
            lblPixelColorTolerance = new Label();
            numPixelNeighborhoodRadius = new NumericUpDown();
            lblPixelNeighborhoodRadius = new Label();
            numPixelMatchScore = new NumericUpDown();
            lblPixelMatchScore = new Label();
            btnSelectBobberSearchArea = new Button();
            lblBobberSearchArea = new Label();
            grpAudio = new GroupBox();
            audioLayout = new TableLayoutPanel();
            lblCatchSound = new Label();
            txtSoundPath = new TextBox();
            btnBrowseSound = new Button();
            lblRecordDuration = new Label();
            numRecordSeconds = new NumericUpDown();
            btnRecordSound = new Button();
            lblAudioStatus = new Label();
            btnPlaySound = new Button();
            grpLog = new GroupBox();
            txtLog = new RichTextBox();
            footerPanel = new Panel();
            btnStop = new Button();
            btnStart = new Button();
            btnToggleLog = new Button();
            btnSaveSettings = new Button();
            lnkOfficialRepository = new LinkLabel();
            lblFooter = new Label();
            toolTip = new ToolTip(components);
            headerPanel.SuspendLayout();
            titleBarControls.SuspendLayout();
            mainLayout.SuspendLayout();
            leftScrollPanel.SuspendLayout();
            leftLayout.SuspendLayout();
            grpTarget.SuspendLayout();
            targetLayout.SuspendLayout();
            grpOptions.SuspendLayout();
            optionsLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCastDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numClickDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPerformanceDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPixelColorTolerance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPixelNeighborhoodRadius).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPixelMatchScore).BeginInit();
            grpAudio.SuspendLayout();
            audioLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRecordSeconds).BeginInit();
            grpLog.SuspendLayout();
            footerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(17, 24, 39);
            headerPanel.Controls.Add(titleBarControls);
            headerPanel.Controls.Add(btnDonate);
            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(22, 14, 22, 10);
            headerPanel.Size = new Size(945, 84);
            headerPanel.TabIndex = 2;
            headerPanel.Paint += HeaderPanel_Paint;
            // 
            // titleBarControls
            // 
            titleBarControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            titleBarControls.AutoSize = true;
            titleBarControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            titleBarControls.BackColor = Color.Transparent;
            titleBarControls.Controls.Add(lblStatus);
            titleBarControls.Controls.Add(btnMinimize);
            titleBarControls.Controls.Add(btnMaximize);
            titleBarControls.Controls.Add(btnClose);
            titleBarControls.Location = new Point(591, 16);
            titleBarControls.Margin = new Padding(0);
            titleBarControls.Name = "titleBarControls";
            titleBarControls.Size = new Size(332, 32);
            titleBarControls.TabIndex = 3;
            titleBarControls.WrapContents = false;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStatus.BackColor = Color.FromArgb(30, 41, 59);
            lblStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(134, 239, 172);
            lblStatus.Location = new Point(0, 0);
            lblStatus.Margin = new Padding(0, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(12, 7, 12, 0);
            lblStatus.Size = new Size(220, 32);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnMinimize
            // 
            btnMinimize.AccessibleDescription = "Minimize the application window";
            btnMinimize.AccessibleName = "Minimize";
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Segoe UI", 11F);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Location = new Point(224, 0);
            btnMinimize.Margin = new Padding(0);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(36, 32);
            btnMinimize.TabIndex = 0;
            btnMinimize.Text = "—";
            btnMinimize.UseVisualStyleBackColor = false;
            btnMinimize.Click += btnMinimize_Click;
            // 
            // btnMaximize
            // 
            btnMaximize.AccessibleDescription = "Maximize the application window";
            btnMaximize.AccessibleName = "Maximize";
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.Font = new Font("Segoe UI", 11F);
            btnMaximize.ForeColor = Color.White;
            btnMaximize.Location = new Point(260, 0);
            btnMaximize.Margin = new Padding(0);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(36, 32);
            btnMaximize.TabIndex = 1;
            btnMaximize.Text = "□";
            btnMaximize.UseVisualStyleBackColor = false;
            btnMaximize.Click += btnMaximize_Click;
            // 
            // btnClose
            // 
            btnClose.AccessibleDescription = "Close the application";
            btnClose.AccessibleName = "Close";
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 12F);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(296, 0);
            btnClose.Margin = new Padding(0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(36, 32);
            btnClose.TabIndex = 2;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnDonate
            // 
            btnDonate.AccessibleDescription = "Open the WoW Fish Bot donation page on Ko-fi";
            btnDonate.AccessibleName = "Donate on Ko-fi";
            btnDonate.FlatAppearance.BorderSize = 0;
            btnDonate.FlatStyle = FlatStyle.Flat;
            btnDonate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDonate.Location = new Point(170, 14);
            btnDonate.Name = "btnDonate";
            btnDonate.Size = new Size(94, 28);
            btnDonate.TabIndex = 3;
            btnDonate.Text = "♥ Donate";
            btnDonate.UseVisualStyleBackColor = false;
            btnDonate.Click += btnDonate_Click;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitle.Location = new Point(24, 47);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(188, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "World of Warcraft fishing assistant";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(241, 245, 249);
            lblTitle.Location = new Point(22, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(139, 32);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "wowfishbot";
            // 
            // mainLayout
            // 
            mainLayout.BackColor = Color.FromArgb(241, 245, 249);
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54.10628F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45.8937225F));
            mainLayout.Controls.Add(leftScrollPanel, 0, 0);
            mainLayout.Controls.Add(grpLog, 1, 0);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 84);
            mainLayout.Name = "mainLayout";
            mainLayout.Padding = new Padding(2);
            mainLayout.RowCount = 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(945, 560);
            mainLayout.TabIndex = 0;
            // 
            // leftScrollPanel
            // 
            leftScrollPanel.AutoScroll = true;
            leftScrollPanel.Controls.Add(leftLayout);
            leftScrollPanel.Dock = DockStyle.Fill;
            leftScrollPanel.Location = new Point(5, 5);
            leftScrollPanel.Name = "leftScrollPanel";
            leftScrollPanel.Padding = new Padding(0, 0, 8, 0);
            leftScrollPanel.Size = new Size(503, 550);
            leftScrollPanel.TabIndex = 0;
            // 
            // leftLayout
            // 
            leftLayout.AutoSize = true;
            leftLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            leftLayout.ColumnCount = 1;
            leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftLayout.Controls.Add(grpTarget, 0, 0);
            leftLayout.Controls.Add(grpOptions, 0, 2);
            leftLayout.Controls.Add(grpAudio, 0, 1);
            leftLayout.Dock = DockStyle.Top;
            leftLayout.Location = new Point(0, 0);
            leftLayout.Name = "leftLayout";
            leftLayout.RowCount = 3;
            leftLayout.RowStyles.Add(new RowStyle());
            leftLayout.RowStyles.Add(new RowStyle());
            leftLayout.RowStyles.Add(new RowStyle());
            leftLayout.Size = new Size(478, 709);
            leftLayout.TabIndex = 0;
            // 
            // grpTarget
            // 
            grpTarget.Controls.Add(targetLayout);
            grpTarget.Dock = DockStyle.Fill;
            grpTarget.Location = new Point(3, 3);
            grpTarget.Name = "grpTarget";
            grpTarget.Size = new Size(472, 160);
            grpTarget.TabIndex = 0;
            grpTarget.TabStop = false;
            grpTarget.Text = "WoW target and bobber pattern";
            // 
            // targetLayout
            // 
            targetLayout.ColumnCount = 2;
            targetLayout.ColumnStyles.Add(new ColumnStyle());
            targetLayout.ColumnStyles.Add(new ColumnStyle());
            targetLayout.Controls.Add(lblWindow, 0, 0);
            targetLayout.Controls.Add(cmbWindows, 1, 0);
            targetLayout.Controls.Add(lblProcess, 0, 1);
            targetLayout.Controls.Add(txtProcessName, 1, 1);
            targetLayout.Controls.Add(lblTitleFilter, 0, 2);
            targetLayout.Controls.Add(txtWindowTitle, 1, 2);
            targetLayout.Controls.Add(btnRefreshWindows, 1, 3);
            targetLayout.Dock = DockStyle.Fill;
            targetLayout.Location = new Point(3, 19);
            targetLayout.Name = "targetLayout";
            targetLayout.Padding = new Padding(10, 12, 10, 8);
            targetLayout.RowCount = 4;
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            targetLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            targetLayout.Size = new Size(466, 138);
            targetLayout.TabIndex = 0;
            // 
            // lblWindow
            // 
            lblWindow.Location = new Point(13, 12);
            lblWindow.Name = "lblWindow";
            lblWindow.Size = new Size(100, 23);
            lblWindow.TabIndex = 0;
            lblWindow.Text = "WoW window";
            // 
            // cmbWindows
            // 
            cmbWindows.Dock = DockStyle.Fill;
            cmbWindows.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWindows.FormattingEnabled = true;
            cmbWindows.Location = new Point(119, 14);
            cmbWindows.Margin = new Padding(3, 2, 3, 2);
            cmbWindows.Name = "cmbWindows";
            cmbWindows.Size = new Size(334, 23);
            cmbWindows.TabIndex = 0;
            // 
            // lblProcess
            // 
            lblProcess.Location = new Point(13, 42);
            lblProcess.Name = "lblProcess";
            lblProcess.Size = new Size(100, 23);
            lblProcess.TabIndex = 0;
            lblProcess.Text = "Process";
            // 
            // txtProcessName
            // 
            txtProcessName.Dock = DockStyle.Fill;
            txtProcessName.Location = new Point(119, 44);
            txtProcessName.Margin = new Padding(3, 2, 3, 2);
            txtProcessName.Name = "txtProcessName";
            txtProcessName.Size = new Size(334, 23);
            txtProcessName.TabIndex = 1;
            // 
            // lblTitleFilter
            // 
            lblTitleFilter.Location = new Point(13, 72);
            lblTitleFilter.Name = "lblTitleFilter";
            lblTitleFilter.Size = new Size(100, 23);
            lblTitleFilter.TabIndex = 0;
            lblTitleFilter.Text = "Window title";
            // 
            // txtWindowTitle
            // 
            txtWindowTitle.Dock = DockStyle.Fill;
            txtWindowTitle.Location = new Point(119, 74);
            txtWindowTitle.Margin = new Padding(3, 2, 3, 2);
            txtWindowTitle.Name = "txtWindowTitle";
            txtWindowTitle.Size = new Size(334, 23);
            txtWindowTitle.TabIndex = 2;
            // 
            // btnRefreshWindows
            // 
            btnRefreshWindows.Anchor = AnchorStyles.Left;
            btnRefreshWindows.AutoSize = true;
            btnRefreshWindows.BackColor = Color.FromArgb(226, 232, 240);
            btnRefreshWindows.FlatAppearance.BorderSize = 0;
            btnRefreshWindows.FlatStyle = FlatStyle.Flat;
            btnRefreshWindows.Location = new Point(119, 106);
            btnRefreshWindows.Name = "btnRefreshWindows";
            btnRefreshWindows.Size = new Size(155, 25);
            btnRefreshWindows.TabIndex = 3;
            btnRefreshWindows.Text = "Refresh detected windows";
            btnRefreshWindows.UseVisualStyleBackColor = false;
            btnRefreshWindows.Click += btnRefreshWindows_Click;
            // 
            // grpOptions
            // 
            grpOptions.Controls.Add(optionsLayout);
            grpOptions.Dock = DockStyle.Fill;
            grpOptions.Location = new Point(3, 318);
            grpOptions.MinimumSize = new Size(0, 388);
            grpOptions.Name = "grpOptions";
            grpOptions.Size = new Size(472, 388);
            grpOptions.TabIndex = 2;
            grpOptions.TabStop = false;
            grpOptions.Text = "Bot options";
            // 
            // optionsLayout
            // 
            optionsLayout.ColumnCount = 4;
            optionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            optionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            optionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            optionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            optionsLayout.Controls.Add(lblBobberPatternInstructions, 0, 9);
            optionsLayout.Controls.Add(lblBobberPatternStatus, 0, 8);
            optionsLayout.Controls.Add(lblCastKey, 0, 0);
            optionsLayout.Controls.Add(lblClickButton, 0, 1);
            optionsLayout.Controls.Add(lblCastDelay, 0, 2);
            optionsLayout.Controls.Add(lblTimeout, 2, 1);
            optionsLayout.Controls.Add(lblClickDelay, 0, 3);
            optionsLayout.Controls.Add(lblPerformanceDelay, 2, 3);
            optionsLayout.Controls.Add(lblThreshold, 2, 2);
            optionsLayout.Controls.Add(btnRecordCastKey, 2, 0);
            optionsLayout.Controls.Add(lblCastKeyValue, 1, 0);
            optionsLayout.Controls.Add(cmbClickButton, 1, 1);
            optionsLayout.Controls.Add(numCastDelay, 1, 2);
            optionsLayout.Controls.Add(numTimeout, 3, 1);
            optionsLayout.Controls.Add(numClickDelay, 1, 3);
            optionsLayout.Controls.Add(numPerformanceDelay, 3, 3);
            optionsLayout.Controls.Add(numThreshold, 3, 2);
            optionsLayout.Controls.Add(chkValidateBobber, 0, 10);
            optionsLayout.Controls.Add(chkInteractMode, 2, 10);
            optionsLayout.Controls.Add(btnRecordHideShowUiKey, 1, 6);
            optionsLayout.Controls.Add(btnCalibrateBobberPattern, 1, 8);
            optionsLayout.Controls.Add(lblHideShowUiKeyValue, 2, 6);
            optionsLayout.Controls.Add(lblHideShowUiKey, 0, 6);
            optionsLayout.Controls.Add(numPixelColorTolerance, 1, 4);
            optionsLayout.Controls.Add(lblPixelColorTolerance, 0, 4);
            optionsLayout.Controls.Add(numPixelNeighborhoodRadius, 3, 4);
            optionsLayout.Controls.Add(lblPixelNeighborhoodRadius, 2, 4);
            optionsLayout.Controls.Add(numPixelMatchScore, 1, 5);
            optionsLayout.Controls.Add(lblPixelMatchScore, 0, 5);
            optionsLayout.Controls.Add(btnSelectBobberSearchArea, 1, 7);
            optionsLayout.Controls.Add(lblBobberSearchArea, 0, 7);
            optionsLayout.Dock = DockStyle.Fill;
            optionsLayout.Location = new Point(3, 19);
            optionsLayout.Name = "optionsLayout";
            optionsLayout.Padding = new Padding(10, 12, 10, 32);
            optionsLayout.RowCount = 11;
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.RowStyles.Add(new RowStyle());
            optionsLayout.Size = new Size(466, 366);
            optionsLayout.TabIndex = 0;
            // 
            // lblBobberPatternInstructions
            // 
            optionsLayout.SetColumnSpan(lblBobberPatternInstructions, 4);
            lblBobberPatternInstructions.Dock = DockStyle.Fill;
            lblBobberPatternInstructions.ForeColor = Color.FromArgb(100, 116, 139);
            lblBobberPatternInstructions.Location = new Point(13, 283);
            lblBobberPatternInstructions.Name = "lblBobberPatternInstructions";
            lblBobberPatternInstructions.Size = new Size(440, 29);
            lblBobberPatternInstructions.TabIndex = 14;
            lblBobberPatternInstructions.Text = "Select WoW, place the cursor over the bobber, then select red + blue pixels.";
            lblBobberPatternInstructions.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBobberPatternStatus
            // 
            lblBobberPatternStatus.AutoSize = true;
            optionsLayout.SetColumnSpan(lblBobberPatternStatus, 2);
            lblBobberPatternStatus.Dock = DockStyle.Fill;
            lblBobberPatternStatus.ForeColor = Color.FromArgb(30, 64, 175);
            lblBobberPatternStatus.Location = new Point(13, 254);
            lblBobberPatternStatus.Name = "lblBobberPatternStatus";
            lblBobberPatternStatus.Size = new Size(216, 29);
            lblBobberPatternStatus.TabIndex = 14;
            lblBobberPatternStatus.Text = "Pattern: not calibrated";
            lblBobberPatternStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCastKey
            // 
            lblCastKey.AutoSize = true;
            lblCastKey.Dock = DockStyle.Fill;
            lblCastKey.ForeColor = Color.FromArgb(71, 85, 105);
            lblCastKey.Location = new Point(13, 12);
            lblCastKey.Name = "lblCastKey";
            lblCastKey.Size = new Size(127, 32);
            lblCastKey.TabIndex = 0;
            lblCastKey.Text = "Cast key";
            lblCastKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClickButton
            // 
            lblClickButton.AutoSize = true;
            lblClickButton.Dock = DockStyle.Fill;
            lblClickButton.ForeColor = Color.FromArgb(71, 85, 105);
            lblClickButton.Location = new Point(13, 44);
            lblClickButton.Name = "lblClickButton";
            lblClickButton.Size = new Size(127, 29);
            lblClickButton.TabIndex = 0;
            lblClickButton.Text = "Click button";
            lblClickButton.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCastDelay
            // 
            lblCastDelay.AutoSize = true;
            lblCastDelay.Dock = DockStyle.Fill;
            lblCastDelay.ForeColor = Color.FromArgb(71, 85, 105);
            lblCastDelay.Location = new Point(13, 73);
            lblCastDelay.Name = "lblCastDelay";
            lblCastDelay.Size = new Size(127, 29);
            lblCastDelay.TabIndex = 0;
            lblCastDelay.Text = "Cast delay (ms)";
            lblCastDelay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Dock = DockStyle.Fill;
            lblTimeout.ForeColor = Color.FromArgb(71, 85, 105);
            lblTimeout.Location = new Point(235, 44);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new Size(127, 29);
            lblTimeout.TabIndex = 0;
            lblTimeout.Text = "Catch wait (s)";
            lblTimeout.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClickDelay
            // 
            lblClickDelay.AutoSize = true;
            lblClickDelay.Dock = DockStyle.Fill;
            lblClickDelay.ForeColor = Color.FromArgb(71, 85, 105);
            lblClickDelay.Location = new Point(13, 102);
            lblClickDelay.Name = "lblClickDelay";
            lblClickDelay.Size = new Size(127, 30);
            lblClickDelay.TabIndex = 0;
            lblClickDelay.Text = "Click delay (ms)";
            lblClickDelay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPerformanceDelay
            // 
            lblPerformanceDelay.AutoSize = true;
            lblPerformanceDelay.Dock = DockStyle.Fill;
            lblPerformanceDelay.ForeColor = Color.FromArgb(71, 85, 105);
            lblPerformanceDelay.Location = new Point(235, 102);
            lblPerformanceDelay.Name = "lblPerformanceDelay";
            lblPerformanceDelay.Size = new Size(127, 30);
            lblPerformanceDelay.TabIndex = 0;
            lblPerformanceDelay.Text = "Performance delay (ms)";
            lblPerformanceDelay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblThreshold
            // 
            lblThreshold.AutoSize = true;
            lblThreshold.Dock = DockStyle.Fill;
            lblThreshold.ForeColor = Color.FromArgb(71, 85, 105);
            lblThreshold.Location = new Point(235, 73);
            lblThreshold.Name = "lblThreshold";
            lblThreshold.Size = new Size(127, 29);
            lblThreshold.TabIndex = 0;
            lblThreshold.Text = "Sound sensitivity";
            lblThreshold.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnRecordCastKey
            // 
            optionsLayout.SetColumnSpan(btnRecordCastKey, 2);
            btnRecordCastKey.Dock = DockStyle.Fill;
            btnRecordCastKey.FlatStyle = FlatStyle.Flat;
            btnRecordCastKey.Location = new Point(235, 15);
            btnRecordCastKey.Name = "btnRecordCastKey";
            btnRecordCastKey.Size = new Size(218, 26);
            btnRecordCastKey.TabIndex = 0;
            btnRecordCastKey.Text = "Record cast key";
            btnRecordCastKey.UseVisualStyleBackColor = true;
            btnRecordCastKey.Click += btnRecordCastKey_Click;
            // 
            // lblCastKeyValue
            // 
            lblCastKeyValue.Dock = DockStyle.Fill;
            lblCastKeyValue.ForeColor = Color.FromArgb(30, 64, 175);
            lblCastKeyValue.Location = new Point(146, 12);
            lblCastKeyValue.Name = "lblCastKeyValue";
            lblCastKeyValue.Size = new Size(83, 32);
            lblCastKeyValue.TabIndex = 1;
            lblCastKeyValue.Text = "1";
            lblCastKeyValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbClickButton
            // 
            cmbClickButton.Dock = DockStyle.Fill;
            cmbClickButton.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClickButton.Location = new Point(146, 46);
            cmbClickButton.Margin = new Padding(3, 2, 3, 2);
            cmbClickButton.Name = "cmbClickButton";
            cmbClickButton.Size = new Size(83, 23);
            cmbClickButton.TabIndex = 1;
            // 
            // numCastDelay
            // 
            numCastDelay.Dock = DockStyle.Fill;
            numCastDelay.Location = new Point(146, 76);
            numCastDelay.Name = "numCastDelay";
            numCastDelay.Size = new Size(83, 23);
            numCastDelay.TabIndex = 2;
            // 
            // numTimeout
            // 
            numTimeout.Dock = DockStyle.Fill;
            numTimeout.Location = new Point(368, 47);
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new Size(85, 23);
            numTimeout.TabIndex = 3;
            // 
            // numClickDelay
            // 
            numClickDelay.Dock = DockStyle.Fill;
            numClickDelay.Location = new Point(146, 105);
            numClickDelay.Name = "numClickDelay";
            numClickDelay.Size = new Size(83, 23);
            numClickDelay.TabIndex = 4;
            // 
            // numPerformanceDelay
            // 
            numPerformanceDelay.Dock = DockStyle.Fill;
            numPerformanceDelay.Location = new Point(368, 105);
            numPerformanceDelay.Name = "numPerformanceDelay";
            numPerformanceDelay.Size = new Size(85, 23);
            numPerformanceDelay.TabIndex = 6;
            // 
            // numThreshold
            // 
            numThreshold.Dock = DockStyle.Fill;
            numThreshold.Location = new Point(368, 76);
            numThreshold.Name = "numThreshold";
            numThreshold.Size = new Size(85, 23);
            numThreshold.TabIndex = 6;
            toolTip.SetToolTip(numThreshold, "Higher values reduce false positives but require a cleaner recording.");
            // 
            // chkValidateBobber
            // 
            chkValidateBobber.AutoSize = true;
            chkValidateBobber.Checked = true;
            chkValidateBobber.CheckState = CheckState.Checked;
            optionsLayout.SetColumnSpan(chkValidateBobber, 2);
            chkValidateBobber.Font = new Font("Segoe UI", 10F);
            chkValidateBobber.ForeColor = Color.FromArgb(51, 65, 85);
            chkValidateBobber.Location = new Point(13, 315);
            chkValidateBobber.Name = "chkValidateBobber";
            chkValidateBobber.Size = new Size(216, 23);
            chkValidateBobber.TabIndex = 6;
            chkValidateBobber.Text = "Recheck bobber before clicking";
            toolTip.SetToolTip(chkValidateBobber, "Rechecks the saved red/blue pixel pattern near the detected bobber before audio listening and clicking.");
            // 
            // chkInteractMode
            // 
            chkInteractMode.AutoSize = true;
            optionsLayout.SetColumnSpan(chkInteractMode, 2);
            chkInteractMode.Font = new Font("Segoe UI", 10F);
            chkInteractMode.ForeColor = Color.FromArgb(51, 65, 85);
            chkInteractMode.Location = new Point(235, 315);
            chkInteractMode.Name = "chkInteractMode";
            chkInteractMode.Size = new Size(200, 23);
            chkInteractMode.TabIndex = 15;
            chkInteractMode.Text = "Interact mode (newer WoW)";
            chkInteractMode.UseVisualStyleBackColor = true;
            // 
            // btnRecordHideShowUiKey
            // 
            btnRecordHideShowUiKey.Dock = DockStyle.Fill;
            btnRecordHideShowUiKey.FlatStyle = FlatStyle.Flat;
            btnRecordHideShowUiKey.Location = new Point(146, 193);
            btnRecordHideShowUiKey.Name = "btnRecordHideShowUiKey";
            btnRecordHideShowUiKey.Size = new Size(83, 26);
            btnRecordHideShowUiKey.TabIndex = 7;
            btnRecordHideShowUiKey.Text = "Record hide/show";
            btnRecordHideShowUiKey.UseVisualStyleBackColor = true;
            btnRecordHideShowUiKey.Click += btnRecordHideShowUiKey_Click;
            // 
            // btnCalibrateBobberPattern
            // 
            btnCalibrateBobberPattern.AutoSize = true;
            btnCalibrateBobberPattern.BackColor = Color.FromArgb(219, 234, 254);
            btnCalibrateBobberPattern.FlatAppearance.BorderSize = 0;
            btnCalibrateBobberPattern.FlatStyle = FlatStyle.Flat;
            btnCalibrateBobberPattern.Location = new Point(235, 256);
            btnCalibrateBobberPattern.Margin = new Padding(3, 2, 3, 2);
            btnCalibrateBobberPattern.Name = "btnCalibrateBobberPattern";
            btnCalibrateBobberPattern.Size = new Size(127, 25);
            btnCalibrateBobberPattern.TabIndex = 5;
            btnCalibrateBobberPattern.Text = "Calibrate pixel pattern (F8)";
            btnCalibrateBobberPattern.UseVisualStyleBackColor = false;
            btnCalibrateBobberPattern.Click += btnCalibrateBobberPattern_Click;
            // 
            // lblHideShowUiKeyValue
            // 
            optionsLayout.SetColumnSpan(lblHideShowUiKeyValue, 2);
            lblHideShowUiKeyValue.Dock = DockStyle.Fill;
            lblHideShowUiKeyValue.ForeColor = Color.FromArgb(30, 64, 175);
            lblHideShowUiKeyValue.Location = new Point(235, 190);
            lblHideShowUiKeyValue.Name = "lblHideShowUiKeyValue";
            lblHideShowUiKeyValue.Size = new Size(218, 32);
            lblHideShowUiKeyValue.TabIndex = 8;
            lblHideShowUiKeyValue.Text = "Not configured";
            lblHideShowUiKeyValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHideShowUiKey
            // 
            lblHideShowUiKey.Location = new Point(13, 190);
            lblHideShowUiKey.Name = "lblHideShowUiKey";
            lblHideShowUiKey.Size = new Size(100, 15);
            lblHideShowUiKey.TabIndex = 0;
            lblHideShowUiKey.Text = "Hide/show UI key";
            lblHideShowUiKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numPixelColorTolerance
            // 
            numPixelColorTolerance.Dock = DockStyle.Fill;
            numPixelColorTolerance.Location = new Point(146, 135);
            numPixelColorTolerance.Maximum = new decimal(new int[] { 442, 0, 0, 0 });
            numPixelColorTolerance.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numPixelColorTolerance.Name = "numPixelColorTolerance";
            numPixelColorTolerance.Size = new Size(83, 23);
            numPixelColorTolerance.TabIndex = 9;
            numPixelColorTolerance.Value = new decimal(new int[] { 135, 0, 0, 0 });
            // 
            // lblPixelColorTolerance
            // 
            lblPixelColorTolerance.Location = new Point(13, 132);
            lblPixelColorTolerance.Name = "lblPixelColorTolerance";
            lblPixelColorTolerance.Size = new Size(88, 23);
            lblPixelColorTolerance.TabIndex = 0;
            lblPixelColorTolerance.Text = "Color tolerance";
            lblPixelColorTolerance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numPixelNeighborhoodRadius
            // 
            numPixelNeighborhoodRadius.Dock = DockStyle.Fill;
            numPixelNeighborhoodRadius.Location = new Point(368, 135);
            numPixelNeighborhoodRadius.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numPixelNeighborhoodRadius.Name = "numPixelNeighborhoodRadius";
            numPixelNeighborhoodRadius.Size = new Size(85, 23);
            numPixelNeighborhoodRadius.TabIndex = 10;
            numPixelNeighborhoodRadius.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // lblPixelNeighborhoodRadius
            // 
            lblPixelNeighborhoodRadius.Location = new Point(235, 132);
            lblPixelNeighborhoodRadius.Name = "lblPixelNeighborhoodRadius";
            lblPixelNeighborhoodRadius.Size = new Size(88, 23);
            lblPixelNeighborhoodRadius.TabIndex = 0;
            lblPixelNeighborhoodRadius.Text = "Jiggle radius";
            // 
            // numPixelMatchScore
            // 
            numPixelMatchScore.Dock = DockStyle.Fill;
            numPixelMatchScore.Location = new Point(146, 164);
            numPixelMatchScore.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPixelMatchScore.Name = "numPixelMatchScore";
            numPixelMatchScore.Size = new Size(83, 23);
            numPixelMatchScore.TabIndex = 11;
            numPixelMatchScore.Value = new decimal(new int[] { 55, 0, 0, 0 });
            // 
            // lblPixelMatchScore
            // 
            lblPixelMatchScore.Location = new Point(13, 161);
            lblPixelMatchScore.Name = "lblPixelMatchScore";
            lblPixelMatchScore.Size = new Size(88, 23);
            lblPixelMatchScore.TabIndex = 0;
            lblPixelMatchScore.Text = "Pixel match %";
            lblPixelMatchScore.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnSelectBobberSearchArea
            // 
            btnSelectBobberSearchArea.Dock = DockStyle.Fill;
            btnSelectBobberSearchArea.FlatStyle = FlatStyle.Flat;
            btnSelectBobberSearchArea.Location = new Point(235, 225);
            btnSelectBobberSearchArea.Name = "btnSelectBobberSearchArea";
            btnSelectBobberSearchArea.Size = new Size(127, 26);
            btnSelectBobberSearchArea.TabIndex = 12;
            btnSelectBobberSearchArea.Text = "Select area";
            btnSelectBobberSearchArea.UseVisualStyleBackColor = true;
            btnSelectBobberSearchArea.Click += btnSelectBobberSearchArea_Click;
            // 
            // lblBobberSearchArea
            // 
            optionsLayout.SetColumnSpan(lblBobberSearchArea, 2);
            lblBobberSearchArea.Dock = DockStyle.Fill;
            lblBobberSearchArea.ForeColor = Color.FromArgb(30, 64, 175);
            lblBobberSearchArea.Location = new Point(13, 222);
            lblBobberSearchArea.Name = "lblBobberSearchArea";
            lblBobberSearchArea.Size = new Size(216, 32);
            lblBobberSearchArea.TabIndex = 13;
            lblBobberSearchArea.Text = "Search area: full client";
            lblBobberSearchArea.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpAudio
            // 
            grpAudio.Controls.Add(audioLayout);
            grpAudio.Dock = DockStyle.Fill;
            grpAudio.Location = new Point(3, 169);
            grpAudio.Name = "grpAudio";
            grpAudio.Size = new Size(472, 143);
            grpAudio.TabIndex = 1;
            grpAudio.TabStop = false;
            grpAudio.Text = "Catch sound";
            // 
            // audioLayout
            // 
            audioLayout.ColumnCount = 3;
            audioLayout.ColumnStyles.Add(new ColumnStyle());
            audioLayout.ColumnStyles.Add(new ColumnStyle());
            audioLayout.ColumnStyles.Add(new ColumnStyle());
            audioLayout.Controls.Add(lblCatchSound, 0, 0);
            audioLayout.Controls.Add(txtSoundPath, 1, 0);
            audioLayout.Controls.Add(btnBrowseSound, 2, 0);
            audioLayout.Controls.Add(lblRecordDuration, 0, 1);
            audioLayout.Controls.Add(numRecordSeconds, 1, 1);
            audioLayout.Controls.Add(btnRecordSound, 2, 1);
            audioLayout.Controls.Add(lblAudioStatus, 0, 2);
            audioLayout.Controls.Add(btnPlaySound, 2, 2);
            audioLayout.Dock = DockStyle.Fill;
            audioLayout.Location = new Point(3, 19);
            audioLayout.Name = "audioLayout";
            audioLayout.Padding = new Padding(10, 12, 10, 8);
            audioLayout.RowCount = 3;
            audioLayout.RowStyles.Add(new RowStyle());
            audioLayout.RowStyles.Add(new RowStyle());
            audioLayout.RowStyles.Add(new RowStyle());
            audioLayout.Size = new Size(466, 121);
            audioLayout.TabIndex = 0;
            // 
            // lblCatchSound
            // 
            lblCatchSound.Location = new Point(13, 12);
            lblCatchSound.Name = "lblCatchSound";
            lblCatchSound.Size = new Size(100, 23);
            lblCatchSound.TabIndex = 0;
            lblCatchSound.Text = "Catch WAV";
            // 
            // txtSoundPath
            // 
            txtSoundPath.Dock = DockStyle.Fill;
            txtSoundPath.Location = new Point(119, 14);
            txtSoundPath.Margin = new Padding(3, 2, 3, 2);
            txtSoundPath.Name = "txtSoundPath";
            txtSoundPath.Size = new Size(175, 23);
            txtSoundPath.TabIndex = 0;
            // 
            // btnBrowseSound
            // 
            btnBrowseSound.Dock = DockStyle.Fill;
            btnBrowseSound.FlatStyle = FlatStyle.Flat;
            btnBrowseSound.Location = new Point(300, 15);
            btnBrowseSound.Name = "btnBrowseSound";
            btnBrowseSound.Size = new Size(153, 24);
            btnBrowseSound.TabIndex = 1;
            btnBrowseSound.Text = "Browse";
            btnBrowseSound.Click += btnBrowseSound_Click;
            // 
            // lblRecordDuration
            // 
            lblRecordDuration.Location = new Point(13, 42);
            lblRecordDuration.Name = "lblRecordDuration";
            lblRecordDuration.Size = new Size(100, 23);
            lblRecordDuration.TabIndex = 0;
            lblRecordDuration.Text = "Record seconds";
            // 
            // numRecordSeconds
            // 
            numRecordSeconds.Dock = DockStyle.Left;
            numRecordSeconds.Location = new Point(119, 45);
            numRecordSeconds.Margin = new Padding(3, 3, 3, 2);
            numRecordSeconds.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numRecordSeconds.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numRecordSeconds.Name = "numRecordSeconds";
            numRecordSeconds.Size = new Size(1, 23);
            numRecordSeconds.TabIndex = 2;
            numRecordSeconds.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // btnRecordSound
            // 
            btnRecordSound.BackColor = Color.FromArgb(220, 252, 231);
            btnRecordSound.Dock = DockStyle.Fill;
            btnRecordSound.FlatAppearance.BorderSize = 0;
            btnRecordSound.FlatStyle = FlatStyle.Flat;
            btnRecordSound.Location = new Point(300, 45);
            btnRecordSound.Name = "btnRecordSound";
            btnRecordSound.Size = new Size(153, 28);
            btnRecordSound.TabIndex = 3;
            btnRecordSound.Text = "Record";
            btnRecordSound.UseVisualStyleBackColor = false;
            btnRecordSound.Click += btnRecordSound_Click;
            // 
            // lblAudioStatus
            // 
            lblAudioStatus.AutoSize = true;
            audioLayout.SetColumnSpan(lblAudioStatus, 2);
            lblAudioStatus.Dock = DockStyle.Fill;
            lblAudioStatus.ForeColor = Color.FromArgb(71, 85, 105);
            lblAudioStatus.Location = new Point(13, 76);
            lblAudioStatus.Name = "lblAudioStatus";
            lblAudioStatus.Size = new Size(281, 37);
            lblAudioStatus.TabIndex = 4;
            lblAudioStatus.Text = "No sample loaded";
            lblAudioStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnPlaySound
            // 
            btnPlaySound.Dock = DockStyle.Fill;
            btnPlaySound.FlatStyle = FlatStyle.Flat;
            btnPlaySound.Location = new Point(300, 79);
            btnPlaySound.Name = "btnPlaySound";
            btnPlaySound.Size = new Size(153, 31);
            btnPlaySound.TabIndex = 5;
            btnPlaySound.Text = "Play";
            btnPlaySound.Click += btnPlaySound_Click;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Dock = DockStyle.Fill;
            grpLog.Location = new Point(514, 5);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(426, 550);
            grpLog.TabIndex = 1;
            grpLog.TabStop = false;
            grpLog.Text = "Activity log";
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(15, 23, 42);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Cascadia Mono", 9F);
            txtLog.ForeColor = Color.FromArgb(203, 213, 225);
            txtLog.Location = new Point(3, 19);
            txtLog.Margin = new Padding(8);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtLog.Size = new Size(420, 528);
            txtLog.TabIndex = 0;
            txtLog.Text = "";
            // 
            // footerPanel
            // 
            footerPanel.BackColor = Color.FromArgb(226, 232, 240);
            footerPanel.Controls.Add(btnStop);
            footerPanel.Controls.Add(btnStart);
            footerPanel.Controls.Add(btnToggleLog);
            footerPanel.Controls.Add(btnSaveSettings);
            footerPanel.Controls.Add(lnkOfficialRepository);
            footerPanel.Controls.Add(lblFooter);
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Location = new Point(0, 644);
            footerPanel.Name = "footerPanel";
            footerPanel.Padding = new Padding(18, 10, 18, 10);
            footerPanel.Size = new Size(945, 56);
            footerPanel.TabIndex = 1;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStop.BackColor = Color.FromArgb(220, 38, 38);
            btnStop.Enabled = false;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(821, 9);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(112, 36);
            btnStop.TabIndex = 0;
            btnStop.Text = "Stop bot";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStart.BackColor = Color.FromArgb(22, 163, 74);
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(701, 9);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(112, 36);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start bot";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnToggleLog
            // 
            btnToggleLog.AccessibleDescription = "Hide the activity log and make the window narrower";
            btnToggleLog.AccessibleName = "Hide activity log";
            btnToggleLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnToggleLog.BackColor = Color.FromArgb(30, 41, 59);
            btnToggleLog.FlatAppearance.BorderColor = Color.FromArgb(71, 85, 105);
            btnToggleLog.FlatStyle = FlatStyle.Flat;
            btnToggleLog.ForeColor = Color.FromArgb(226, 232, 240);
            btnToggleLog.Location = new Point(445, 9);
            btnToggleLog.Name = "btnToggleLog";
            btnToggleLog.Size = new Size(120, 36);
            btnToggleLog.TabIndex = 3;
            btnToggleLog.Text = "Hide log";
            btnToggleLog.UseVisualStyleBackColor = false;
            btnToggleLog.Click += btnToggleLog_Click;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveSettings.BackColor = Color.White;
            btnSaveSettings.FlatAppearance.BorderColor = Color.FromArgb(148, 163, 184);
            btnSaveSettings.FlatStyle = FlatStyle.Flat;
            btnSaveSettings.Location = new Point(573, 9);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(120, 36);
            btnSaveSettings.TabIndex = 2;
            btnSaveSettings.Text = "Save settings";
            btnSaveSettings.UseVisualStyleBackColor = false;
            btnSaveSettings.Click += btnSaveSettings_Click;
            // 
            // lnkOfficialRepository
            // 
            lnkOfficialRepository.AccessibleDescription = "Open the official WoW Fish Bot project repository";
            lnkOfficialRepository.AccessibleName = "Official project repository";
            lnkOfficialRepository.AutoSize = true;
            lnkOfficialRepository.LinkColor = Color.FromArgb(37, 99, 235);
            lnkOfficialRepository.Location = new Point(126, 20);
            lnkOfficialRepository.Name = "lnkOfficialRepository";
            lnkOfficialRepository.Size = new Size(267, 15);
            lnkOfficialRepository.TabIndex = 4;
            lnkOfficialRepository.TabStop = true;
            lnkOfficialRepository.Text = "Official project: github.com/nanidev/wowfishbot";
            lnkOfficialRepository.LinkClicked += lnkOfficialRepository_LinkClicked;
            // 
            // lblFooter
            // 
            lblFooter.AutoSize = true;
            lblFooter.ForeColor = Color.FromArgb(71, 85, 105);
            lblFooter.Location = new Point(18, 20);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(89, 15);
            lblFooter.TabIndex = 3;
            lblFooter.Text = "Ready for setup";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(945, 700);
            Controls.Add(mainLayout);
            Controls.Add(footerPanel);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            MinimumSize = new Size(945, 700);
            Name = "Form1";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "wowfishbot";
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            titleBarControls.ResumeLayout(false);
            mainLayout.ResumeLayout(false);
            leftScrollPanel.ResumeLayout(false);
            leftScrollPanel.PerformLayout();
            leftLayout.ResumeLayout(false);
            grpTarget.ResumeLayout(false);
            targetLayout.ResumeLayout(false);
            targetLayout.PerformLayout();
            grpOptions.ResumeLayout(false);
            optionsLayout.ResumeLayout(false);
            optionsLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCastDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numClickDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPerformanceDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numThreshold).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPixelColorTolerance).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPixelNeighborhoodRadius).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPixelMatchScore).EndInit();
            grpAudio.ResumeLayout(false);
            audioLayout.ResumeLayout(false);
            audioLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRecordSeconds).EndInit();
            grpLog.ResumeLayout(false);
            footerPanel.ResumeLayout(false);
            footerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private static void ConfigureGroup(GroupBox group, string text, int index)
        {
            group.BackColor = Color.White;
            group.Dock = DockStyle.Fill;
            group.ForeColor = Color.FromArgb(30, 41, 59);
            group.Margin = new Padding(0, index == 0 ? 0 : 8, 0, 0);
            group.Padding = new Padding(8);
            group.Text = text;
        }

        private static Label MakeFieldLabel(Label label, string text)
        {
            label.AutoSize = true;
            label.Dock = DockStyle.Fill;
            label.ForeColor = Color.FromArgb(71, 85, 105);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            return label;
        }

        private static void ConfigureNumber(NumericUpDown number, decimal minimum, decimal maximum, decimal value, decimal increment)
        {
            number.DecimalPlaces = minimum % 1 == 0 && maximum % 1 == 0 ? 0 : 2;
            number.Increment = increment;
            number.Maximum = maximum;
            number.Minimum = minimum;
            number.Value = value;
            number.ThousandsSeparator = false;
        }
    }
}
