
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
            this.GrpMain = new System.Windows.Forms.GroupBox();
            this.btnOutputDirectory = new System.Windows.Forms.Button();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.lblOutputDirectory = new System.Windows.Forms.Label();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnLoadSchema = new System.Windows.Forms.Button();
            this.BtnSwapOriginDestination = new System.Windows.Forms.Button();
            this.btnDestinationSchema = new System.Windows.Forms.Button();
            this.btnOriginSchema = new System.Windows.Forms.Button();
            this.txtDestinationSchema = new System.Windows.Forms.TextBox();
            this.txtOriginSchema = new System.Windows.Forms.TextBox();
            this.lblDestinationSchema = new System.Windows.Forms.Label();
            this.lblOriginSchema = new System.Windows.Forms.Label();
            this.btnCreateUpdateFile = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.ofdOriginSchema = new System.Windows.Forms.OpenFileDialog();
            this.ofdDestinationSchema = new System.Windows.Forms.OpenFileDialog();
            this.lblInfo = new System.Windows.Forms.Label();
            this.fbdOutputDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.GrpUpdateSchema = new System.Windows.Forms.GroupBox();
            this.btnUpdateSchema = new System.Windows.Forms.Button();
            this.txtUpdateSchemaFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GrpCompare = new System.Windows.Forms.GroupBox();
            this.ofdUpdateSchemaFile = new System.Windows.Forms.OpenFileDialog();
            this.GrpDbObjects = new System.Windows.Forms.GroupBox();
            this.ChkOther = new System.Windows.Forms.CheckBox();
            this.ChkTrigger = new System.Windows.Forms.CheckBox();
            this.ChkTableType = new System.Windows.Forms.CheckBox();
            this.ChkSchema = new System.Windows.Forms.CheckBox();
            this.ChkUser = new System.Windows.Forms.CheckBox();
            this.ChkFunction = new System.Windows.Forms.CheckBox();
            this.ChkStoreProcedure = new System.Windows.Forms.CheckBox();
            this.ChkView = new System.Windows.Forms.CheckBox();
            this.ChkTable = new System.Windows.Forms.CheckBox();
            this.ChkAll = new System.Windows.Forms.CheckBox();
            this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.GrpMain.SuspendLayout();
            this.GrpUpdateSchema.SuspendLayout();
            this.GrpCompare.SuspendLayout();
            this.GrpDbObjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // GrpMain
            // 
            this.GrpMain.Controls.Add(this.btnOutputDirectory);
            this.GrpMain.Controls.Add(this.txtOutputDirectory);
            this.GrpMain.Controls.Add(this.lblOutputDirectory);
            this.GrpMain.Controls.Add(this.BtnClear);
            this.GrpMain.Controls.Add(this.BtnLoadSchema);
            this.GrpMain.Controls.Add(this.BtnSwapOriginDestination);
            this.GrpMain.Controls.Add(this.btnDestinationSchema);
            this.GrpMain.Controls.Add(this.btnOriginSchema);
            this.GrpMain.Controls.Add(this.txtDestinationSchema);
            this.GrpMain.Controls.Add(this.txtOriginSchema);
            this.GrpMain.Controls.Add(this.lblDestinationSchema);
            this.GrpMain.Controls.Add(this.lblOriginSchema);
            this.GrpMain.Location = new System.Drawing.Point(17, 20);
            this.GrpMain.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GrpMain.Name = "GrpMain";
            this.GrpMain.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.GrpMain.Size = new System.Drawing.Size(494, 278);
            this.GrpMain.TabIndex = 0;
            this.GrpMain.TabStop = false;
            this.GrpMain.Text = "Schema";
            // 
            // btnOutputDirectory
            // 
            this.btnOutputDirectory.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.folder;
            this.btnOutputDirectory.Location = new System.Drawing.Point(435, 162);
            this.btnOutputDirectory.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOutputDirectory.Name = "btnOutputDirectory";
            this.btnOutputDirectory.Size = new System.Drawing.Size(40, 36);
            this.btnOutputDirectory.TabIndex = 11;
            this.btnOutputDirectory.UseVisualStyleBackColor = true;
            this.btnOutputDirectory.Click += new System.EventHandler(this.BtnOutputDirectory_Click);
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.Enabled = false;
            this.txtOutputDirectory.Location = new System.Drawing.Point(159, 162);
            this.txtOutputDirectory.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.Size = new System.Drawing.Size(263, 27);
            this.txtOutputDirectory.TabIndex = 10;
            // 
            // lblOutputDirectory
            // 
            this.lblOutputDirectory.AutoSize = true;
            this.lblOutputDirectory.Location = new System.Drawing.Point(14, 165);
            this.lblOutputDirectory.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblOutputDirectory.Name = "lblOutputDirectory";
            this.lblOutputDirectory.Size = new System.Drawing.Size(118, 20);
            this.lblOutputDirectory.TabIndex = 9;
            this.lblOutputDirectory.Text = "Output directory";
            // 
            // BtnClear
            // 
            this.BtnClear.Enabled = false;
            this.BtnClear.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.gear;
            this.BtnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnClear.Location = new System.Drawing.Point(407, 225);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(70, 36);
            this.BtnClear.TabIndex = 8;
            this.BtnClear.Text = "Clear";
            this.BtnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnLoadSchema
            // 
            this.BtnLoadSchema.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.gear;
            this.BtnLoadSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnLoadSchema.Location = new System.Drawing.Point(14, 225);
            this.BtnLoadSchema.Name = "BtnLoadSchema";
            this.BtnLoadSchema.Size = new System.Drawing.Size(128, 36);
            this.BtnLoadSchema.TabIndex = 7;
            this.BtnLoadSchema.Text = "Load schema";
            this.BtnLoadSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnLoadSchema.UseVisualStyleBackColor = true;
            this.BtnLoadSchema.Click += new System.EventHandler(this.BtnLoadSchema_Click);
            // 
            // BtnSwapOriginDestination
            // 
            this.BtnSwapOriginDestination.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.change;
            this.BtnSwapOriginDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSwapOriginDestination.Location = new System.Drawing.Point(235, 61);
            this.BtnSwapOriginDestination.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSwapOriginDestination.Name = "BtnSwapOriginDestination";
            this.BtnSwapOriginDestination.Size = new System.Drawing.Size(86, 40);
            this.BtnSwapOriginDestination.TabIndex = 6;
            this.BtnSwapOriginDestination.Text = "Swap";
            this.BtnSwapOriginDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSwapOriginDestination.UseVisualStyleBackColor = true;
            this.BtnSwapOriginDestination.Click += new System.EventHandler(this.BtnSwapOriginDestination_Click);
            // 
            // btnDestinationSchema
            // 
            this.btnDestinationSchema.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.folder;
            this.btnDestinationSchema.Location = new System.Drawing.Point(437, 109);
            this.btnDestinationSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnDestinationSchema.Name = "btnDestinationSchema";
            this.btnDestinationSchema.Size = new System.Drawing.Size(40, 36);
            this.btnDestinationSchema.TabIndex = 5;
            this.btnDestinationSchema.UseVisualStyleBackColor = true;
            this.btnDestinationSchema.Click += new System.EventHandler(this.BtnDestinationSchema_Click);
            // 
            // btnOriginSchema
            // 
            this.btnOriginSchema.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.folder;
            this.btnOriginSchema.Location = new System.Drawing.Point(437, 26);
            this.btnOriginSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOriginSchema.Name = "btnOriginSchema";
            this.btnOriginSchema.Size = new System.Drawing.Size(40, 36);
            this.btnOriginSchema.TabIndex = 4;
            this.btnOriginSchema.UseVisualStyleBackColor = true;
            this.btnOriginSchema.Click += new System.EventHandler(this.BtnOriginSchema_Click);
            // 
            // txtDestinationSchema
            // 
            this.txtDestinationSchema.Enabled = false;
            this.txtDestinationSchema.Location = new System.Drawing.Point(161, 109);
            this.txtDestinationSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtDestinationSchema.Name = "txtDestinationSchema";
            this.txtDestinationSchema.Size = new System.Drawing.Size(263, 27);
            this.txtDestinationSchema.TabIndex = 3;
            // 
            // txtOriginSchema
            // 
            this.txtOriginSchema.Enabled = false;
            this.txtOriginSchema.Location = new System.Drawing.Point(161, 26);
            this.txtOriginSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtOriginSchema.Name = "txtOriginSchema";
            this.txtOriginSchema.Size = new System.Drawing.Size(263, 27);
            this.txtOriginSchema.TabIndex = 2;
            // 
            // lblDestinationSchema
            // 
            this.lblDestinationSchema.AutoSize = true;
            this.lblDestinationSchema.Location = new System.Drawing.Point(14, 109);
            this.lblDestinationSchema.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDestinationSchema.Name = "lblDestinationSchema";
            this.lblDestinationSchema.Size = new System.Drawing.Size(139, 20);
            this.lblDestinationSchema.TabIndex = 1;
            this.lblDestinationSchema.Text = "Destination schema";
            // 
            // lblOriginSchema
            // 
            this.lblOriginSchema.AutoSize = true;
            this.lblOriginSchema.Location = new System.Drawing.Point(14, 26);
            this.lblOriginSchema.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblOriginSchema.Name = "lblOriginSchema";
            this.lblOriginSchema.Size = new System.Drawing.Size(104, 20);
            this.lblOriginSchema.TabIndex = 0;
            this.lblOriginSchema.Text = "Origin schema";
            // 
            // btnCreateUpdateFile
            // 
            this.btnCreateUpdateFile.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.gear;
            this.btnCreateUpdateFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateUpdateFile.Location = new System.Drawing.Point(16, 96);
            this.btnCreateUpdateFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCreateUpdateFile.Name = "btnCreateUpdateFile";
            this.btnCreateUpdateFile.Size = new System.Drawing.Size(150, 36);
            this.btnCreateUpdateFile.TabIndex = 9;
            this.btnCreateUpdateFile.Text = "Create update file";
            this.btnCreateUpdateFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCreateUpdateFile.UseVisualStyleBackColor = true;
            this.btnCreateUpdateFile.Click += new System.EventHandler(this.BtnCreateUpdateFile_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.gear;
            this.btnCompare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCompare.Location = new System.Drawing.Point(14, 76);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(101, 36);
            this.btnCompare.TabIndex = 8;
            this.btnCompare.Text = "Compare";
            this.btnCompare.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(161, 37);
            this.txtSuffix.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(132, 27);
            this.txtSuffix.TabIndex = 7;
            this.txtSuffix.TextChanged += new System.EventHandler(this.TxtSuffix_TextChanged);
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(14, 40);
            this.lblSuffix.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(46, 20);
            this.lblSuffix.TabIndex = 6;
            this.lblSuffix.Text = "Suffix";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(17, 602);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(52, 20);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "lblInfo";
            // 
            // GrpUpdateSchema
            // 
            this.GrpUpdateSchema.Controls.Add(this.btnUpdateSchema);
            this.GrpUpdateSchema.Controls.Add(this.txtUpdateSchemaFile);
            this.GrpUpdateSchema.Controls.Add(this.label2);
            this.GrpUpdateSchema.Controls.Add(this.btnCreateUpdateFile);
            this.GrpUpdateSchema.Enabled = false;
            this.GrpUpdateSchema.Location = new System.Drawing.Point(16, 435);
            this.GrpUpdateSchema.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpUpdateSchema.Name = "GrpUpdateSchema";
            this.GrpUpdateSchema.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpUpdateSchema.Size = new System.Drawing.Size(494, 154);
            this.GrpUpdateSchema.TabIndex = 4;
            this.GrpUpdateSchema.TabStop = false;
            this.GrpUpdateSchema.Text = "Update schema";
            // 
            // btnUpdateSchema
            // 
            this.btnUpdateSchema.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.folder;
            this.btnUpdateSchema.Location = new System.Drawing.Point(437, 41);
            this.btnUpdateSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnUpdateSchema.Name = "btnUpdateSchema";
            this.btnUpdateSchema.Size = new System.Drawing.Size(40, 36);
            this.btnUpdateSchema.TabIndex = 14;
            this.btnUpdateSchema.UseVisualStyleBackColor = true;
            this.btnUpdateSchema.Click += new System.EventHandler(this.BtnUpdateSchema_Click);
            // 
            // txtUpdateSchemaFile
            // 
            this.txtUpdateSchemaFile.Location = new System.Drawing.Point(164, 46);
            this.txtUpdateSchemaFile.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtUpdateSchemaFile.Name = "txtUpdateSchemaFile";
            this.txtUpdateSchemaFile.Size = new System.Drawing.Size(263, 27);
            this.txtUpdateSchemaFile.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Update schema file";
            // 
            // GrpCompare
            // 
            this.GrpCompare.Controls.Add(this.txtSuffix);
            this.GrpCompare.Controls.Add(this.btnCompare);
            this.GrpCompare.Controls.Add(this.lblSuffix);
            this.GrpCompare.Enabled = false;
            this.GrpCompare.Location = new System.Drawing.Point(16, 300);
            this.GrpCompare.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpCompare.Name = "GrpCompare";
            this.GrpCompare.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpCompare.Size = new System.Drawing.Size(495, 125);
            this.GrpCompare.TabIndex = 5;
            this.GrpCompare.TabStop = false;
            this.GrpCompare.Text = "Compare";
            // 
            // ofdUpdateSchemaFile
            // 
            this.ofdUpdateSchemaFile.CheckFileExists = false;
            this.ofdUpdateSchemaFile.FileName = "openFileDialog1";
            // 
            // GrpDbObjects
            // 
            this.GrpDbObjects.Controls.Add(this.ChkOther);
            this.GrpDbObjects.Controls.Add(this.ChkTrigger);
            this.GrpDbObjects.Controls.Add(this.ChkTableType);
            this.GrpDbObjects.Controls.Add(this.ChkSchema);
            this.GrpDbObjects.Controls.Add(this.ChkUser);
            this.GrpDbObjects.Controls.Add(this.ChkFunction);
            this.GrpDbObjects.Controls.Add(this.ChkStoreProcedure);
            this.GrpDbObjects.Controls.Add(this.ChkView);
            this.GrpDbObjects.Controls.Add(this.ChkTable);
            this.GrpDbObjects.Controls.Add(this.ChkAll);
            this.GrpDbObjects.Enabled = false;
            this.GrpDbObjects.Location = new System.Drawing.Point(527, 31);
            this.GrpDbObjects.Name = "GrpDbObjects";
            this.GrpDbObjects.Size = new System.Drawing.Size(176, 558);
            this.GrpDbObjects.TabIndex = 6;
            this.GrpDbObjects.TabStop = false;
            this.GrpDbObjects.Text = "Db objects";
            this.GrpDbObjects.UseCompatibleTextRendering = true;
            // 
            // ChkOther
            // 
            this.ChkOther.AutoSize = true;
            this.ChkOther.Checked = true;
            this.ChkOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkOther.Location = new System.Drawing.Point(7, 306);
            this.ChkOther.Name = "ChkOther";
            this.ChkOther.Size = new System.Drawing.Size(114, 24);
            this.ChkOther.TabIndex = 10;
            this.ChkOther.Text = "Other object";
            this.ChkOther.UseVisualStyleBackColor = true;
            // 
            // ChkTrigger
            // 
            this.ChkTrigger.AutoSize = true;
            this.ChkTrigger.Checked = true;
            this.ChkTrigger.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkTrigger.Location = new System.Drawing.Point(5, 276);
            this.ChkTrigger.Name = "ChkTrigger";
            this.ChkTrigger.Size = new System.Drawing.Size(78, 24);
            this.ChkTrigger.TabIndex = 7;
            this.ChkTrigger.Text = "Trigger";
            this.ChkTrigger.UseVisualStyleBackColor = true;
            // 
            // ChkTableType
            // 
            this.ChkTableType.AutoSize = true;
            this.ChkTableType.Checked = true;
            this.ChkTableType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkTableType.Location = new System.Drawing.Point(6, 91);
            this.ChkTableType.Name = "ChkTableType";
            this.ChkTableType.Size = new System.Drawing.Size(100, 24);
            this.ChkTableType.TabIndex = 7;
            this.ChkTableType.Text = "Type table";
            this.ChkTableType.UseVisualStyleBackColor = true;
            // 
            // ChkSchema
            // 
            this.ChkSchema.AutoSize = true;
            this.ChkSchema.Checked = true;
            this.ChkSchema.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkSchema.Location = new System.Drawing.Point(6, 246);
            this.ChkSchema.Name = "ChkSchema";
            this.ChkSchema.Size = new System.Drawing.Size(83, 24);
            this.ChkSchema.TabIndex = 6;
            this.ChkSchema.Text = "Schema";
            this.ChkSchema.UseVisualStyleBackColor = true;
            // 
            // ChkUser
            // 
            this.ChkUser.AutoSize = true;
            this.ChkUser.Checked = true;
            this.ChkUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkUser.Location = new System.Drawing.Point(6, 214);
            this.ChkUser.Name = "ChkUser";
            this.ChkUser.Size = new System.Drawing.Size(60, 24);
            this.ChkUser.TabIndex = 5;
            this.ChkUser.Text = "User";
            this.ChkUser.UseVisualStyleBackColor = true;
            // 
            // ChkFunction
            // 
            this.ChkFunction.AutoSize = true;
            this.ChkFunction.Checked = true;
            this.ChkFunction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkFunction.Location = new System.Drawing.Point(6, 183);
            this.ChkFunction.Name = "ChkFunction";
            this.ChkFunction.Size = new System.Drawing.Size(87, 24);
            this.ChkFunction.TabIndex = 4;
            this.ChkFunction.Text = "Function";
            this.ChkFunction.UseVisualStyleBackColor = true;
            // 
            // ChkStoreProcedure
            // 
            this.ChkStoreProcedure.AutoSize = true;
            this.ChkStoreProcedure.Checked = true;
            this.ChkStoreProcedure.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkStoreProcedure.Location = new System.Drawing.Point(6, 152);
            this.ChkStoreProcedure.Name = "ChkStoreProcedure";
            this.ChkStoreProcedure.Size = new System.Drawing.Size(138, 24);
            this.ChkStoreProcedure.TabIndex = 3;
            this.ChkStoreProcedure.Text = "Store procedure";
            this.ChkStoreProcedure.UseVisualStyleBackColor = true;
            // 
            // ChkView
            // 
            this.ChkView.AutoSize = true;
            this.ChkView.Checked = true;
            this.ChkView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkView.Location = new System.Drawing.Point(6, 121);
            this.ChkView.Name = "ChkView";
            this.ChkView.Size = new System.Drawing.Size(63, 24);
            this.ChkView.TabIndex = 2;
            this.ChkView.Text = "View";
            this.ChkView.UseVisualStyleBackColor = true;
            // 
            // ChkTable
            // 
            this.ChkTable.AutoSize = true;
            this.ChkTable.Checked = true;
            this.ChkTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkTable.Location = new System.Drawing.Point(7, 58);
            this.ChkTable.Name = "ChkTable";
            this.ChkTable.Size = new System.Drawing.Size(66, 24);
            this.ChkTable.TabIndex = 1;
            this.ChkTable.Text = "Table";
            this.ChkTable.UseVisualStyleBackColor = true;
            // 
            // ChkAll
            // 
            this.ChkAll.AutoSize = true;
            this.ChkAll.Checked = true;
            this.ChkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkAll.Location = new System.Drawing.Point(7, 27);
            this.ChkAll.Name = "ChkAll";
            this.ChkAll.Size = new System.Drawing.Size(49, 24);
            this.ChkAll.TabIndex = 0;
            this.ChkAll.Text = "All";
            this.ChkAll.UseVisualStyleBackColor = true;
            this.ChkAll.CheckedChanged += new System.EventHandler(this.ChkAll_CheckedChanged);
            // 
            // BackgroundWorker
            // 
            this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadSchema);
            this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadSchemaCompleted);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(527, 602);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(176, 29);
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressBar.TabIndex = 15;
            this.ProgressBar.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 634);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.GrpDbObjects);
            this.Controls.Add(this.GrpCompare);
            this.Controls.Add(this.GrpUpdateSchema);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.GrpMain);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "MainForm";
            this.Text = "SqlSchemaCompare";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.GrpMain.ResumeLayout(false);
            this.GrpMain.PerformLayout();
            this.GrpUpdateSchema.ResumeLayout(false);
            this.GrpUpdateSchema.PerformLayout();
            this.GrpCompare.ResumeLayout(false);
            this.GrpCompare.PerformLayout();
            this.GrpDbObjects.ResumeLayout(false);
            this.GrpDbObjects.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

