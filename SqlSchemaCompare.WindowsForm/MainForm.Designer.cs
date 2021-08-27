
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
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
            this.btnOutputDirectory = new System.Windows.Forms.Button();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.lblOutputDirectory = new System.Windows.Forms.Label();
            this.fbdOutputDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdateSchema = new System.Windows.Forms.Button();
            this.txtUpdateSchemaFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ofdUpdateSchemaFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.BtnSwapOriginDestination);
            this.groupBoxMain.Controls.Add(this.btnDestinationSchema);
            this.groupBoxMain.Controls.Add(this.btnOriginSchema);
            this.groupBoxMain.Controls.Add(this.txtDestinationSchema);
            this.groupBoxMain.Controls.Add(this.txtOriginSchema);
            this.groupBoxMain.Controls.Add(this.lblDestinationSchema);
            this.groupBoxMain.Controls.Add(this.lblOriginSchema);
            this.groupBoxMain.Location = new System.Drawing.Point(17, 20);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBoxMain.Size = new System.Drawing.Size(494, 168);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Schema";
            // 
            // BtnSwapOriginDestination
            // 
            this.BtnSwapOriginDestination.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.change;
            this.BtnSwapOriginDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSwapOriginDestination.Location = new System.Drawing.Point(237, 74);
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
            this.btnDestinationSchema.Location = new System.Drawing.Point(437, 122);
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
            this.txtDestinationSchema.Location = new System.Drawing.Point(161, 122);
            this.txtDestinationSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtDestinationSchema.Name = "txtDestinationSchema";
            this.txtDestinationSchema.Size = new System.Drawing.Size(263, 27);
            this.txtDestinationSchema.TabIndex = 3;
            // 
            // txtOriginSchema
            // 
            this.txtOriginSchema.Location = new System.Drawing.Point(161, 26);
            this.txtOriginSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtOriginSchema.Name = "txtOriginSchema";
            this.txtOriginSchema.Size = new System.Drawing.Size(263, 27);
            this.txtOriginSchema.TabIndex = 2;
            // 
            // lblDestinationSchema
            // 
            this.lblDestinationSchema.AutoSize = true;
            this.lblDestinationSchema.Location = new System.Drawing.Point(14, 122);
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
            this.btnCreateUpdateFile.Location = new System.Drawing.Point(14, 134);
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
            this.btnCompare.Location = new System.Drawing.Point(14, 130);
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
            this.txtSuffix.Location = new System.Drawing.Point(161, 30);
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
            this.lblInfo.Location = new System.Drawing.Point(17, 581);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(52, 20);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "lblInfo";
            // 
            // btnOutputDirectory
            // 
            this.btnOutputDirectory.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.folder;
            this.btnOutputDirectory.Location = new System.Drawing.Point(437, 70);
            this.btnOutputDirectory.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOutputDirectory.Name = "btnOutputDirectory";
            this.btnOutputDirectory.Size = new System.Drawing.Size(40, 36);
            this.btnOutputDirectory.TabIndex = 2;
            this.btnOutputDirectory.UseVisualStyleBackColor = true;
            this.btnOutputDirectory.Click += new System.EventHandler(this.BtnOutputDirectory_Click);
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.Location = new System.Drawing.Point(161, 70);
            this.txtOutputDirectory.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.Size = new System.Drawing.Size(263, 27);
            this.txtOutputDirectory.TabIndex = 1;
            // 
            // lblOutputDirectory
            // 
            this.lblOutputDirectory.AutoSize = true;
            this.lblOutputDirectory.Location = new System.Drawing.Point(14, 78);
            this.lblOutputDirectory.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblOutputDirectory.Name = "lblOutputDirectory";
            this.lblOutputDirectory.Size = new System.Drawing.Size(118, 20);
            this.lblOutputDirectory.TabIndex = 0;
            this.lblOutputDirectory.Text = "Output directory";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdateSchema);
            this.groupBox1.Controls.Add(this.txtUpdateSchemaFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtDatabaseName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnCreateUpdateFile);
            this.groupBox1.Location = new System.Drawing.Point(17, 378);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(494, 174);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update schema";
            // 
            // btnUpdateSchema
            // 
            this.btnUpdateSchema.Image = global::SqlSchemaCompare.WindowsForm.Properties.Resources.folder;
            this.btnUpdateSchema.Location = new System.Drawing.Point(437, 74);
            this.btnUpdateSchema.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnUpdateSchema.Name = "btnUpdateSchema";
            this.btnUpdateSchema.Size = new System.Drawing.Size(40, 36);
            this.btnUpdateSchema.TabIndex = 14;
            this.btnUpdateSchema.UseVisualStyleBackColor = true;
            this.btnUpdateSchema.Click += new System.EventHandler(this.BtnUpdateSchema_Click);
            // 
            // txtUpdateSchemaFile
            // 
            this.txtUpdateSchemaFile.Location = new System.Drawing.Point(162, 78);
            this.txtUpdateSchemaFile.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtUpdateSchemaFile.Name = "txtUpdateSchemaFile";
            this.txtUpdateSchemaFile.Size = new System.Drawing.Size(263, 27);
            this.txtUpdateSchemaFile.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Update schema file";
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(162, 30);
            this.txtDatabaseName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(114, 27);
            this.txtDatabaseName.TabIndex = 11;
            this.txtDatabaseName.TextChanged += new System.EventHandler(this.TxtDatabaseName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Database name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOutputDirectory);
            this.groupBox2.Controls.Add(this.txtSuffix);
            this.groupBox2.Controls.Add(this.txtOutputDirectory);
            this.groupBox2.Controls.Add(this.btnCompare);
            this.groupBox2.Controls.Add(this.lblOutputDirectory);
            this.groupBox2.Controls.Add(this.lblSuffix);
            this.groupBox2.Location = new System.Drawing.Point(17, 196);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(495, 174);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Compare";
            // 
            // ofdUpdateSchemaFile
            // 
            this.ofdUpdateSchemaFile.CheckFileExists = false;
            this.ofdUpdateSchemaFile.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 608);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.groupBoxMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "MainForm";
            this.Text = "SqlSchemaCompare";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
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
        private System.Windows.Forms.Button btnOutputDirectory;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.Label lblOutputDirectory;
        private System.Windows.Forms.FolderBrowserDialog fbdOutputDirectory;
        private System.Windows.Forms.Button btnCreateUpdateFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdateSchema;
        private System.Windows.Forms.TextBox txtUpdateSchemaFile;
        private System.Windows.Forms.OpenFileDialog ofdUpdateSchemaFile;
        private System.Windows.Forms.Button BtnSwapOriginDestination;
    }
}

