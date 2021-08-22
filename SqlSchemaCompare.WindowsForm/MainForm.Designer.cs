
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
            this.groupBoxMain.Location = new System.Drawing.Point(21, 25);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBoxMain.Size = new System.Drawing.Size(617, 210);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Schema";
            // 
            // BtnSwapOriginDestination
            // 
            this.BtnSwapOriginDestination.Image = ((System.Drawing.Image)(resources.GetObject("BtnSwapOriginDestination.Image")));
            this.BtnSwapOriginDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSwapOriginDestination.Location = new System.Drawing.Point(296, 92);
            this.BtnSwapOriginDestination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtnSwapOriginDestination.Name = "BtnSwapOriginDestination";
            this.BtnSwapOriginDestination.Size = new System.Drawing.Size(107, 50);
            this.BtnSwapOriginDestination.TabIndex = 6;
            this.BtnSwapOriginDestination.Text = "Swap";
            this.BtnSwapOriginDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnSwapOriginDestination.UseVisualStyleBackColor = true;
            this.BtnSwapOriginDestination.Click += new System.EventHandler(this.BtnSwapOriginDestination_Click);
            // 
            // btnDestinationSchema
            // 
            this.btnDestinationSchema.Image = ((System.Drawing.Image)(resources.GetObject("btnDestinationSchema.Image")));
            this.btnDestinationSchema.Location = new System.Drawing.Point(546, 152);
            this.btnDestinationSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnDestinationSchema.Name = "btnDestinationSchema";
            this.btnDestinationSchema.Size = new System.Drawing.Size(50, 45);
            this.btnDestinationSchema.TabIndex = 5;
            this.btnDestinationSchema.UseVisualStyleBackColor = true;
            this.btnDestinationSchema.Click += new System.EventHandler(this.BtnDestinationSchema_Click);
            // 
            // btnOriginSchema
            // 
            this.btnOriginSchema.Image = ((System.Drawing.Image)(resources.GetObject("btnOriginSchema.Image")));
            this.btnOriginSchema.Location = new System.Drawing.Point(546, 33);
            this.btnOriginSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnOriginSchema.Name = "btnOriginSchema";
            this.btnOriginSchema.Size = new System.Drawing.Size(50, 45);
            this.btnOriginSchema.TabIndex = 4;
            this.btnOriginSchema.UseVisualStyleBackColor = true;
            this.btnOriginSchema.Click += new System.EventHandler(this.BtnOriginSchema_Click);
            // 
            // txtDestinationSchema
            // 
            this.txtDestinationSchema.Location = new System.Drawing.Point(201, 152);
            this.txtDestinationSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtDestinationSchema.Name = "txtDestinationSchema";
            this.txtDestinationSchema.Size = new System.Drawing.Size(328, 31);
            this.txtDestinationSchema.TabIndex = 3;
            // 
            // txtOriginSchema
            // 
            this.txtOriginSchema.Location = new System.Drawing.Point(201, 33);
            this.txtOriginSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtOriginSchema.Name = "txtOriginSchema";
            this.txtOriginSchema.Size = new System.Drawing.Size(328, 31);
            this.txtOriginSchema.TabIndex = 2;
            // 
            // lblDestinationSchema
            // 
            this.lblDestinationSchema.AutoSize = true;
            this.lblDestinationSchema.Location = new System.Drawing.Point(17, 152);
            this.lblDestinationSchema.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDestinationSchema.Name = "lblDestinationSchema";
            this.lblDestinationSchema.Size = new System.Drawing.Size(167, 25);
            this.lblDestinationSchema.TabIndex = 1;
            this.lblDestinationSchema.Text = "Destination schema";
            // 
            // lblOriginSchema
            // 
            this.lblOriginSchema.AutoSize = true;
            this.lblOriginSchema.Location = new System.Drawing.Point(17, 33);
            this.lblOriginSchema.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOriginSchema.Name = "lblOriginSchema";
            this.lblOriginSchema.Size = new System.Drawing.Size(126, 25);
            this.lblOriginSchema.TabIndex = 0;
            this.lblOriginSchema.Text = "Origin schema";
            // 
            // btnCreateUpdateFile
            // 
            this.btnCreateUpdateFile.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateUpdateFile.Image")));
            this.btnCreateUpdateFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateUpdateFile.Location = new System.Drawing.Point(17, 168);
            this.btnCreateUpdateFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreateUpdateFile.Name = "btnCreateUpdateFile";
            this.btnCreateUpdateFile.Size = new System.Drawing.Size(187, 45);
            this.btnCreateUpdateFile.TabIndex = 9;
            this.btnCreateUpdateFile.Text = "Create update file";
            this.btnCreateUpdateFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCreateUpdateFile.UseVisualStyleBackColor = true;
            this.btnCreateUpdateFile.Click += new System.EventHandler(this.BtnCreateUpdateFile_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Image = ((System.Drawing.Image)(resources.GetObject("btnCompare.Image")));
            this.btnCompare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCompare.Location = new System.Drawing.Point(17, 162);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(126, 45);
            this.btnCompare.TabIndex = 8;
            this.btnCompare.Text = "Compare";
            this.btnCompare.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(201, 37);
            this.txtSuffix.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(164, 31);
            this.txtSuffix.TabIndex = 7;
            this.txtSuffix.TextChanged += new System.EventHandler(this.TxtSuffix_TextChanged);
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(17, 50);
            this.lblSuffix.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(56, 25);
            this.lblSuffix.TabIndex = 6;
            this.lblSuffix.Text = "Suffix";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(21, 726);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(63, 25);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "lblInfo";
            // 
            // btnOutputDirectory
            // 
            this.btnOutputDirectory.Image = ((System.Drawing.Image)(resources.GetObject("btnOutputDirectory.Image")));
            this.btnOutputDirectory.Location = new System.Drawing.Point(546, 88);
            this.btnOutputDirectory.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnOutputDirectory.Name = "btnOutputDirectory";
            this.btnOutputDirectory.Size = new System.Drawing.Size(50, 45);
            this.btnOutputDirectory.TabIndex = 2;
            this.btnOutputDirectory.UseVisualStyleBackColor = true;
            this.btnOutputDirectory.Click += new System.EventHandler(this.BtnOutputDirectory_Click);
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.Location = new System.Drawing.Point(201, 88);
            this.txtOutputDirectory.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.Size = new System.Drawing.Size(328, 31);
            this.txtOutputDirectory.TabIndex = 1;
            // 
            // lblOutputDirectory
            // 
            this.lblOutputDirectory.AutoSize = true;
            this.lblOutputDirectory.Location = new System.Drawing.Point(17, 98);
            this.lblOutputDirectory.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOutputDirectory.Name = "lblOutputDirectory";
            this.lblOutputDirectory.Size = new System.Drawing.Size(144, 25);
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
            this.groupBox1.Location = new System.Drawing.Point(21, 472);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(617, 217);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update schema";
            // 
            // btnUpdateSchema
            // 
            this.btnUpdateSchema.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateSchema.Image")));
            this.btnUpdateSchema.Location = new System.Drawing.Point(546, 92);
            this.btnUpdateSchema.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnUpdateSchema.Name = "btnUpdateSchema";
            this.btnUpdateSchema.Size = new System.Drawing.Size(50, 45);
            this.btnUpdateSchema.TabIndex = 14;
            this.btnUpdateSchema.UseVisualStyleBackColor = true;
            this.btnUpdateSchema.Click += new System.EventHandler(this.BtnUpdateSchema_Click);
            // 
            // txtUpdateSchemaFile
            // 
            this.txtUpdateSchemaFile.Location = new System.Drawing.Point(203, 98);
            this.txtUpdateSchemaFile.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtUpdateSchemaFile.Name = "txtUpdateSchemaFile";
            this.txtUpdateSchemaFile.Size = new System.Drawing.Size(328, 31);
            this.txtUpdateSchemaFile.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 25);
            this.label2.TabIndex = 12;
            this.label2.Text = "Update schema file";
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(203, 37);
            this.txtDatabaseName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(141, 31);
            this.txtDatabaseName.TabIndex = 11;
            this.txtDatabaseName.TextChanged += new System.EventHandler(this.TxtDatabaseName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 25);
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
            this.groupBox2.Location = new System.Drawing.Point(21, 245);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(619, 217);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 760);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.groupBoxMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
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

