
namespace SqlSchemaCompare.WindowsForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GrpMain = new System.Windows.Forms.GroupBox();
            btnConfiguration = new System.Windows.Forms.Button();
            txtConfiguration = new System.Windows.Forms.TextBox();
            lblConfiguration = new System.Windows.Forms.Label();
            btnOutputDirectory = new System.Windows.Forms.Button();
            txtOutputDirectory = new System.Windows.Forms.TextBox();
            lblOutputDirectory = new System.Windows.Forms.Label();
            BtnClear = new System.Windows.Forms.Button();
            BtnLoadSchema = new System.Windows.Forms.Button();
            BtnSwapOriginDestination = new System.Windows.Forms.Button();
            btnDestinationSchema = new System.Windows.Forms.Button();
            btnOriginSchema = new System.Windows.Forms.Button();
            txtDestinationSchema = new System.Windows.Forms.TextBox();
            txtOriginSchema = new System.Windows.Forms.TextBox();
            lblDestinationSchema = new System.Windows.Forms.Label();
            lblOriginSchema = new System.Windows.Forms.Label();
            btnCreateUpdateFile = new System.Windows.Forms.Button();
            btnCompare = new System.Windows.Forms.Button();
            txtSuffix = new System.Windows.Forms.TextBox();
            lblSuffix = new System.Windows.Forms.Label();
            ofdOriginSchema = new System.Windows.Forms.OpenFileDialog();
            ofdDestinationSchema = new System.Windows.Forms.OpenFileDialog();
            lblInfo = new System.Windows.Forms.Label();
            fbdOutputDirectory = new System.Windows.Forms.FolderBrowserDialog();
            GrpUpdateSchema = new System.Windows.Forms.GroupBox();
            btnUpdateSchema = new System.Windows.Forms.Button();
            txtUpdateSchemaFile = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            GrpCompare = new System.Windows.Forms.GroupBox();
            ofdUpdateSchemaFile = new System.Windows.Forms.OpenFileDialog();
            GrpDbObjects = new System.Windows.Forms.GroupBox();
            ChkOther = new System.Windows.Forms.CheckBox();
            ChkTrigger = new System.Windows.Forms.CheckBox();
            ChkTableType = new System.Windows.Forms.CheckBox();
            ChkSchema = new System.Windows.Forms.CheckBox();
            ChkUser = new System.Windows.Forms.CheckBox();
            ChkFunction = new System.Windows.Forms.CheckBox();
            ChkStoreProcedure = new System.Windows.Forms.CheckBox();
            ChkView = new System.Windows.Forms.CheckBox();
            ChkTable = new System.Windows.Forms.CheckBox();
            ChkAll = new System.Windows.Forms.CheckBox();
            BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ProgressBar = new System.Windows.Forms.ProgressBar();
            ofdConfiguration = new System.Windows.Forms.OpenFileDialog();
            btnGetConfiguration = new System.Windows.Forms.Button();
            GrpMain.SuspendLayout();
            GrpUpdateSchema.SuspendLayout();
            GrpCompare.SuspendLayout();
            GrpDbObjects.SuspendLayout();
            SuspendLayout();
            // 
            // GrpMain
            // 
            GrpMain.Controls.Add(btnGetConfiguration);
            GrpMain.Controls.Add(btnConfiguration);
            GrpMain.Controls.Add(txtConfiguration);
            GrpMain.Controls.Add(lblConfiguration);
            GrpMain.Controls.Add(btnOutputDirectory);
            GrpMain.Controls.Add(txtOutputDirectory);
            GrpMain.Controls.Add(lblOutputDirectory);
            GrpMain.Controls.Add(BtnClear);
            GrpMain.Controls.Add(BtnLoadSchema);
            GrpMain.Controls.Add(BtnSwapOriginDestination);
            GrpMain.Controls.Add(btnDestinationSchema);
            GrpMain.Controls.Add(btnOriginSchema);
            GrpMain.Controls.Add(txtDestinationSchema);
            GrpMain.Controls.Add(txtOriginSchema);
            GrpMain.Controls.Add(lblDestinationSchema);
            GrpMain.Controls.Add(lblOriginSchema);
            GrpMain.Location = new System.Drawing.Point(21, 25);
            GrpMain.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            GrpMain.Name = "GrpMain";
            GrpMain.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            GrpMain.Size = new System.Drawing.Size(618, 459);
            GrpMain.TabIndex = 0;
            GrpMain.TabStop = false;
            GrpMain.Text = "Schema";
            // 
            // btnConfiguration
            // 
            btnConfiguration.Image = Properties.Resources.folder;
            btnConfiguration.Location = new System.Drawing.Point(547, 256);
            btnConfiguration.Name = "btnConfiguration";
            btnConfiguration.Size = new System.Drawing.Size(50, 45);
            btnConfiguration.TabIndex = 14;
            btnConfiguration.UseVisualStyleBackColor = true;
            btnConfiguration.Click += BtnConfiguration_Click;
            // 
            // txtConfiguration
            // 
            txtConfiguration.Enabled = false;
            txtConfiguration.Location = new System.Drawing.Point(195, 263);
            txtConfiguration.Name = "txtConfiguration";
            txtConfiguration.Size = new System.Drawing.Size(332, 31);
            txtConfiguration.TabIndex = 13;
            // 
            // lblConfiguration
            // 
            lblConfiguration.AutoSize = true;
            lblConfiguration.Location = new System.Drawing.Point(19, 260);
            lblConfiguration.Name = "lblConfiguration";
            lblConfiguration.Size = new System.Drawing.Size(121, 25);
            lblConfiguration.TabIndex = 12;
            lblConfiguration.Text = "Configuration";
            // 
            // btnOutputDirectory
            // 
            btnOutputDirectory.Image = Properties.Resources.folder;
            btnOutputDirectory.Location = new System.Drawing.Point(544, 202);
            btnOutputDirectory.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            btnOutputDirectory.Name = "btnOutputDirectory";
            btnOutputDirectory.Size = new System.Drawing.Size(50, 45);
            btnOutputDirectory.TabIndex = 11;
            btnOutputDirectory.UseVisualStyleBackColor = true;
            btnOutputDirectory.Click += BtnOutputDirectory_Click;
            // 
            // txtOutputDirectory
            // 
            txtOutputDirectory.Enabled = false;
            txtOutputDirectory.Location = new System.Drawing.Point(199, 202);
            txtOutputDirectory.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            txtOutputDirectory.Name = "txtOutputDirectory";
            txtOutputDirectory.Size = new System.Drawing.Size(328, 31);
            txtOutputDirectory.TabIndex = 10;
            // 
            // lblOutputDirectory
            // 
            lblOutputDirectory.AutoSize = true;
            lblOutputDirectory.Location = new System.Drawing.Point(18, 206);
            lblOutputDirectory.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblOutputDirectory.Name = "lblOutputDirectory";
            lblOutputDirectory.Size = new System.Drawing.Size(144, 25);
            lblOutputDirectory.TabIndex = 9;
            lblOutputDirectory.Text = "Output directory";
            // 
            // BtnClear
            // 
            BtnClear.Enabled = false;
            BtnClear.Image = Properties.Resources.gear;
            BtnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            BtnClear.Location = new System.Drawing.Point(509, 388);
            BtnClear.Margin = new System.Windows.Forms.Padding(4);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(88, 45);
            BtnClear.TabIndex = 8;
            BtnClear.Text = "Clear";
            BtnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnClear.UseVisualStyleBackColor = true;
            BtnClear.Click += BtnClear_Click;
            // 
            // BtnLoadSchema
            // 
            BtnLoadSchema.Image = Properties.Resources.gear;
            BtnLoadSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            BtnLoadSchema.Location = new System.Drawing.Point(18, 388);
            BtnLoadSchema.Margin = new System.Windows.Forms.Padding(4);
            BtnLoadSchema.Name = "BtnLoadSchema";
            BtnLoadSchema.Size = new System.Drawing.Size(160, 45);
            BtnLoadSchema.TabIndex = 7;
            BtnLoadSchema.Text = "Load schema";
            BtnLoadSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnLoadSchema.UseVisualStyleBackColor = true;
            BtnLoadSchema.Click += BtnLoadSchema_Click;
            // 
            // BtnSwapOriginDestination
            // 
            BtnSwapOriginDestination.Image = Properties.Resources.change;
            BtnSwapOriginDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            BtnSwapOriginDestination.Location = new System.Drawing.Point(294, 76);
            BtnSwapOriginDestination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            BtnSwapOriginDestination.Name = "BtnSwapOriginDestination";
            BtnSwapOriginDestination.Size = new System.Drawing.Size(108, 50);
            BtnSwapOriginDestination.TabIndex = 6;
            BtnSwapOriginDestination.Text = "Swap";
            BtnSwapOriginDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnSwapOriginDestination.UseVisualStyleBackColor = true;
            BtnSwapOriginDestination.Click += BtnSwapOriginDestination_Click;
            // 
            // btnDestinationSchema
            // 
            btnDestinationSchema.Image = Properties.Resources.folder;
            btnDestinationSchema.Location = new System.Drawing.Point(546, 136);
            btnDestinationSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            btnDestinationSchema.Name = "btnDestinationSchema";
            btnDestinationSchema.Size = new System.Drawing.Size(50, 45);
            btnDestinationSchema.TabIndex = 5;
            btnDestinationSchema.UseVisualStyleBackColor = true;
            btnDestinationSchema.Click += BtnDestinationSchema_Click;
            // 
            // btnOriginSchema
            // 
            btnOriginSchema.Image = Properties.Resources.folder;
            btnOriginSchema.Location = new System.Drawing.Point(546, 32);
            btnOriginSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            btnOriginSchema.Name = "btnOriginSchema";
            btnOriginSchema.Size = new System.Drawing.Size(50, 45);
            btnOriginSchema.TabIndex = 4;
            btnOriginSchema.UseVisualStyleBackColor = true;
            btnOriginSchema.Click += BtnOriginSchema_Click;
            // 
            // txtDestinationSchema
            // 
            txtDestinationSchema.Enabled = false;
            txtDestinationSchema.Location = new System.Drawing.Point(201, 136);
            txtDestinationSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            txtDestinationSchema.Name = "txtDestinationSchema";
            txtDestinationSchema.Size = new System.Drawing.Size(328, 31);
            txtDestinationSchema.TabIndex = 3;
            // 
            // txtOriginSchema
            // 
            txtOriginSchema.Enabled = false;
            txtOriginSchema.Location = new System.Drawing.Point(201, 32);
            txtOriginSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            txtOriginSchema.Name = "txtOriginSchema";
            txtOriginSchema.Size = new System.Drawing.Size(328, 31);
            txtOriginSchema.TabIndex = 2;
            // 
            // lblDestinationSchema
            // 
            lblDestinationSchema.AutoSize = true;
            lblDestinationSchema.Location = new System.Drawing.Point(18, 136);
            lblDestinationSchema.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblDestinationSchema.Name = "lblDestinationSchema";
            lblDestinationSchema.Size = new System.Drawing.Size(167, 25);
            lblDestinationSchema.TabIndex = 1;
            lblDestinationSchema.Text = "Destination schema";
            // 
            // lblOriginSchema
            // 
            lblOriginSchema.AutoSize = true;
            lblOriginSchema.Location = new System.Drawing.Point(18, 32);
            lblOriginSchema.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblOriginSchema.Name = "lblOriginSchema";
            lblOriginSchema.Size = new System.Drawing.Size(126, 25);
            lblOriginSchema.TabIndex = 0;
            lblOriginSchema.Text = "Origin schema";
            // 
            // btnCreateUpdateFile
            // 
            btnCreateUpdateFile.Image = Properties.Resources.gear;
            btnCreateUpdateFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnCreateUpdateFile.Location = new System.Drawing.Point(20, 120);
            btnCreateUpdateFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnCreateUpdateFile.Name = "btnCreateUpdateFile";
            btnCreateUpdateFile.Size = new System.Drawing.Size(188, 45);
            btnCreateUpdateFile.TabIndex = 9;
            btnCreateUpdateFile.Text = "Create update file";
            btnCreateUpdateFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnCreateUpdateFile.UseVisualStyleBackColor = true;
            btnCreateUpdateFile.Click += BtnCreateUpdateFile_Click;
            // 
            // btnCompare
            // 
            btnCompare.Image = Properties.Resources.gear;
            btnCompare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnCompare.Location = new System.Drawing.Point(18, 95);
            btnCompare.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            btnCompare.Name = "btnCompare";
            btnCompare.Size = new System.Drawing.Size(126, 45);
            btnCompare.TabIndex = 8;
            btnCompare.Text = "Compare";
            btnCompare.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnCompare.UseVisualStyleBackColor = true;
            btnCompare.Click += BtnCompare_Click;
            // 
            // txtSuffix
            // 
            txtSuffix.Location = new System.Drawing.Point(201, 46);
            txtSuffix.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            txtSuffix.Name = "txtSuffix";
            txtSuffix.Size = new System.Drawing.Size(164, 31);
            txtSuffix.TabIndex = 7;
            txtSuffix.TextChanged += TxtSuffix_TextChanged;
            // 
            // lblSuffix
            // 
            lblSuffix.AutoSize = true;
            lblSuffix.Location = new System.Drawing.Point(18, 50);
            lblSuffix.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblSuffix.Name = "lblSuffix";
            lblSuffix.Size = new System.Drawing.Size(56, 25);
            lblSuffix.TabIndex = 6;
            lblSuffix.Text = "Suffix";
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new System.Drawing.Point(21, 871);
            lblInfo.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new System.Drawing.Size(63, 25);
            lblInfo.TabIndex = 2;
            lblInfo.Text = "lblInfo";
            // 
            // GrpUpdateSchema
            // 
            GrpUpdateSchema.Controls.Add(btnUpdateSchema);
            GrpUpdateSchema.Controls.Add(txtUpdateSchemaFile);
            GrpUpdateSchema.Controls.Add(label2);
            GrpUpdateSchema.Controls.Add(btnCreateUpdateFile);
            GrpUpdateSchema.Enabled = false;
            GrpUpdateSchema.Location = new System.Drawing.Point(20, 663);
            GrpUpdateSchema.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GrpUpdateSchema.Name = "GrpUpdateSchema";
            GrpUpdateSchema.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GrpUpdateSchema.Size = new System.Drawing.Size(618, 192);
            GrpUpdateSchema.TabIndex = 4;
            GrpUpdateSchema.TabStop = false;
            GrpUpdateSchema.Text = "Update schema";
            // 
            // btnUpdateSchema
            // 
            btnUpdateSchema.Image = Properties.Resources.folder;
            btnUpdateSchema.Location = new System.Drawing.Point(546, 51);
            btnUpdateSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            btnUpdateSchema.Name = "btnUpdateSchema";
            btnUpdateSchema.Size = new System.Drawing.Size(50, 45);
            btnUpdateSchema.TabIndex = 14;
            btnUpdateSchema.UseVisualStyleBackColor = true;
            btnUpdateSchema.Click += BtnUpdateSchema_Click;
            // 
            // txtUpdateSchemaFile
            // 
            txtUpdateSchemaFile.Location = new System.Drawing.Point(205, 58);
            txtUpdateSchemaFile.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            txtUpdateSchemaFile.Name = "txtUpdateSchemaFile";
            txtUpdateSchemaFile.Size = new System.Drawing.Size(328, 31);
            txtUpdateSchemaFile.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 58);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(163, 25);
            label2.TabIndex = 12;
            label2.Text = "Update schema file";
            // 
            // GrpCompare
            // 
            GrpCompare.Controls.Add(txtSuffix);
            GrpCompare.Controls.Add(btnCompare);
            GrpCompare.Controls.Add(lblSuffix);
            GrpCompare.Enabled = false;
            GrpCompare.Location = new System.Drawing.Point(20, 494);
            GrpCompare.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GrpCompare.Name = "GrpCompare";
            GrpCompare.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            GrpCompare.Size = new System.Drawing.Size(619, 156);
            GrpCompare.TabIndex = 5;
            GrpCompare.TabStop = false;
            GrpCompare.Text = "Compare";
            // 
            // ofdUpdateSchemaFile
            // 
            ofdUpdateSchemaFile.CheckFileExists = false;
            ofdUpdateSchemaFile.FileName = "openFileDialog1";
            // 
            // GrpDbObjects
            // 
            GrpDbObjects.Controls.Add(ChkOther);
            GrpDbObjects.Controls.Add(ChkTrigger);
            GrpDbObjects.Controls.Add(ChkTableType);
            GrpDbObjects.Controls.Add(ChkSchema);
            GrpDbObjects.Controls.Add(ChkUser);
            GrpDbObjects.Controls.Add(ChkFunction);
            GrpDbObjects.Controls.Add(ChkStoreProcedure);
            GrpDbObjects.Controls.Add(ChkView);
            GrpDbObjects.Controls.Add(ChkTable);
            GrpDbObjects.Controls.Add(ChkAll);
            GrpDbObjects.Enabled = false;
            GrpDbObjects.Location = new System.Drawing.Point(659, 39);
            GrpDbObjects.Margin = new System.Windows.Forms.Padding(4);
            GrpDbObjects.Name = "GrpDbObjects";
            GrpDbObjects.Padding = new System.Windows.Forms.Padding(4);
            GrpDbObjects.Size = new System.Drawing.Size(220, 772);
            GrpDbObjects.TabIndex = 6;
            GrpDbObjects.TabStop = false;
            GrpDbObjects.Text = "Db objects";
            GrpDbObjects.UseCompatibleTextRendering = true;
            // 
            // ChkOther
            // 
            ChkOther.AutoSize = true;
            ChkOther.Checked = true;
            ChkOther.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkOther.Location = new System.Drawing.Point(9, 382);
            ChkOther.Margin = new System.Windows.Forms.Padding(4);
            ChkOther.Name = "ChkOther";
            ChkOther.Size = new System.Drawing.Size(137, 29);
            ChkOther.TabIndex = 10;
            ChkOther.Text = "Other object";
            ChkOther.UseVisualStyleBackColor = true;
            // 
            // ChkTrigger
            // 
            ChkTrigger.AutoSize = true;
            ChkTrigger.Checked = true;
            ChkTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkTrigger.Location = new System.Drawing.Point(6, 345);
            ChkTrigger.Margin = new System.Windows.Forms.Padding(4);
            ChkTrigger.Name = "ChkTrigger";
            ChkTrigger.Size = new System.Drawing.Size(92, 29);
            ChkTrigger.TabIndex = 7;
            ChkTrigger.Text = "Trigger";
            ChkTrigger.UseVisualStyleBackColor = true;
            // 
            // ChkTableType
            // 
            ChkTableType.AutoSize = true;
            ChkTableType.Checked = true;
            ChkTableType.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkTableType.Location = new System.Drawing.Point(8, 114);
            ChkTableType.Margin = new System.Windows.Forms.Padding(4);
            ChkTableType.Name = "ChkTableType";
            ChkTableType.Size = new System.Drawing.Size(119, 29);
            ChkTableType.TabIndex = 7;
            ChkTableType.Text = "Type table";
            ChkTableType.UseVisualStyleBackColor = true;
            // 
            // ChkSchema
            // 
            ChkSchema.AutoSize = true;
            ChkSchema.Checked = true;
            ChkSchema.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkSchema.Location = new System.Drawing.Point(8, 308);
            ChkSchema.Margin = new System.Windows.Forms.Padding(4);
            ChkSchema.Name = "ChkSchema";
            ChkSchema.Size = new System.Drawing.Size(100, 29);
            ChkSchema.TabIndex = 6;
            ChkSchema.Text = "Schema";
            ChkSchema.UseVisualStyleBackColor = true;
            // 
            // ChkUser
            // 
            ChkUser.AutoSize = true;
            ChkUser.Checked = true;
            ChkUser.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkUser.Location = new System.Drawing.Point(8, 268);
            ChkUser.Margin = new System.Windows.Forms.Padding(4);
            ChkUser.Name = "ChkUser";
            ChkUser.Size = new System.Drawing.Size(73, 29);
            ChkUser.TabIndex = 5;
            ChkUser.Text = "User";
            ChkUser.UseVisualStyleBackColor = true;
            // 
            // ChkFunction
            // 
            ChkFunction.AutoSize = true;
            ChkFunction.Checked = true;
            ChkFunction.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkFunction.Location = new System.Drawing.Point(8, 229);
            ChkFunction.Margin = new System.Windows.Forms.Padding(4);
            ChkFunction.Name = "ChkFunction";
            ChkFunction.Size = new System.Drawing.Size(106, 29);
            ChkFunction.TabIndex = 4;
            ChkFunction.Text = "Function";
            ChkFunction.UseVisualStyleBackColor = true;
            // 
            // ChkStoreProcedure
            // 
            ChkStoreProcedure.AutoSize = true;
            ChkStoreProcedure.Checked = true;
            ChkStoreProcedure.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkStoreProcedure.Location = new System.Drawing.Point(8, 190);
            ChkStoreProcedure.Margin = new System.Windows.Forms.Padding(4);
            ChkStoreProcedure.Name = "ChkStoreProcedure";
            ChkStoreProcedure.Size = new System.Drawing.Size(165, 29);
            ChkStoreProcedure.TabIndex = 3;
            ChkStoreProcedure.Text = "Store procedure";
            ChkStoreProcedure.UseVisualStyleBackColor = true;
            // 
            // ChkView
            // 
            ChkView.AutoSize = true;
            ChkView.Checked = true;
            ChkView.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkView.Location = new System.Drawing.Point(8, 151);
            ChkView.Margin = new System.Windows.Forms.Padding(4);
            ChkView.Name = "ChkView";
            ChkView.Size = new System.Drawing.Size(75, 29);
            ChkView.TabIndex = 2;
            ChkView.Text = "View";
            ChkView.UseVisualStyleBackColor = true;
            // 
            // ChkTable
            // 
            ChkTable.AutoSize = true;
            ChkTable.Checked = true;
            ChkTable.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkTable.Location = new System.Drawing.Point(9, 72);
            ChkTable.Margin = new System.Windows.Forms.Padding(4);
            ChkTable.Name = "ChkTable";
            ChkTable.Size = new System.Drawing.Size(78, 29);
            ChkTable.TabIndex = 1;
            ChkTable.Text = "Table";
            ChkTable.UseVisualStyleBackColor = true;
            // 
            // ChkAll
            // 
            ChkAll.AutoSize = true;
            ChkAll.Checked = true;
            ChkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkAll.Location = new System.Drawing.Point(9, 34);
            ChkAll.Margin = new System.Windows.Forms.Padding(4);
            ChkAll.Name = "ChkAll";
            ChkAll.Size = new System.Drawing.Size(58, 29);
            ChkAll.TabIndex = 0;
            ChkAll.Text = "All";
            ChkAll.UseVisualStyleBackColor = true;
            ChkAll.CheckedChanged += ChkAll_CheckedChanged;
            // 
            // BackgroundWorker
            // 
            BackgroundWorker.DoWork += LoadSchema;
            BackgroundWorker.RunWorkerCompleted += LoadSchemaCompleted;
            // 
            // ProgressBar
            // 
            ProgressBar.Location = new System.Drawing.Point(659, 819);
            ProgressBar.Margin = new System.Windows.Forms.Padding(4);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new System.Drawing.Size(220, 36);
            ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            ProgressBar.TabIndex = 15;
            ProgressBar.Visible = false;
            // 
            // ofdConfiguration
            // 
            ofdConfiguration.FileName = "Configuration.json";
            // 
            // btnGetConfiguration
            // 
            btnGetConfiguration.Image = Properties.Resources.gear;
            btnGetConfiguration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnGetConfiguration.Location = new System.Drawing.Point(408, 337);
            btnGetConfiguration.Name = "btnGetConfiguration";
            btnGetConfiguration.Size = new System.Drawing.Size(189, 44);
            btnGetConfiguration.TabIndex = 15;
            btnGetConfiguration.Text = "Get Configuration";
            btnGetConfiguration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnGetConfiguration.UseVisualStyleBackColor = true;
            btnGetConfiguration.Click += BtnGetConfiguration_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(934, 905);
            Controls.Add(ProgressBar);
            Controls.Add(GrpDbObjects);
            Controls.Add(GrpCompare);
            Controls.Add(GrpUpdateSchema);
            Controls.Add(lblInfo);
            Controls.Add(GrpMain);
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            Name = "MainForm";
            Text = "SqlSchemaCompare";
            Load += MainForm_Load;
            GrpMain.ResumeLayout(false);
            GrpMain.PerformLayout();
            GrpUpdateSchema.ResumeLayout(false);
            GrpUpdateSchema.PerformLayout();
            GrpCompare.ResumeLayout(false);
            GrpCompare.PerformLayout();
            GrpDbObjects.ResumeLayout(false);
            GrpDbObjects.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox GrpMain;
        private System.Windows.Forms.Button btnDestinationSchema;
        private System.Windows.Forms.Button btnOriginSchema;
        private System.Windows.Forms.TextBox txtDestinationSchema;
        private System.Windows.Forms.TextBox txtOriginSchema;
        private System.Windows.Forms.Label lblDestinationSchema;
        private System.Windows.Forms.Label lblOriginSchema;
        private System.Windows.Forms.OpenFileDialog ofdOriginSchema;
        private System.Windows.Forms.OpenFileDialog ofdDestinationSchema;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.FolderBrowserDialog fbdOutputDirectory;
        private System.Windows.Forms.Button btnCreateUpdateFile;
        private System.Windows.Forms.GroupBox GrpUpdateSchema;
        private System.Windows.Forms.GroupBox GrpCompare;
        private System.Windows.Forms.OpenFileDialog ofdUpdateSchemaFile;
        private System.Windows.Forms.Button BtnSwapOriginDestination;
        private System.Windows.Forms.Button BtnLoadSchema;
        private System.Windows.Forms.GroupBox GrpDbObjects;
        private System.Windows.Forms.CheckBox ChkUser;
        private System.Windows.Forms.CheckBox ChkFunction;
        private System.Windows.Forms.CheckBox ChkStoreProcedure;
        private System.Windows.Forms.CheckBox ChkView;
        private System.Windows.Forms.CheckBox ChkTable;
        private System.Windows.Forms.CheckBox ChkAll;
        private System.Windows.Forms.CheckBox ChkOther;
        private System.Windows.Forms.CheckBox ChkTrigger;
        private System.Windows.Forms.CheckBox ChkTableType;
        private System.Windows.Forms.CheckBox ChkSchema;
        private System.ComponentModel.BackgroundWorker BackgroundWorker;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button btnOutputDirectory;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.Label lblOutputDirectory;
        private System.Windows.Forms.Button btnUpdateSchema;
        private System.Windows.Forms.TextBox txtUpdateSchemaFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblConfiguration;
        private System.Windows.Forms.Button btnConfiguration;
        private System.Windows.Forms.TextBox txtConfiguration;
        private System.Windows.Forms.OpenFileDialog ofdConfiguration;
        private System.Windows.Forms.Button btnGetConfiguration;
    }
}

