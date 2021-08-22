using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.TSql;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SqlSchemaCompare.WindowsForm
{
    public partial class MainForm : Form
    {
        private FormSettings formSettings;
        private const string PleaseWait = "Please wait...";
        private const string FileCompareCreated = "Files for compare created";
        private const string FileUpdateCreated = "File update schema created";
        private const string FileUpdateCreatedWithErrors = "File update schema created with errors";
        private const string FileCompareCreatedWithErrors = "File compare created with errors";
        private const string ApplicationName = "Sql compare";
        private IErrorWriter errorWriter;
        public MainForm()
        {
            InitializeComponent();
            lblInfo.Text = "";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            formSettings = new FormSettings();
            formSettings.Reload();
            txtOriginSchema.Text = formSettings.OriginSchema;
            txtDestinationSchema.Text = formSettings.DestinationSchema;
            txtSuffix.Text = formSettings.Suffix;
            txtOutputDirectory.Text = formSettings.OutputDirectory;
            txtUpdateSchemaFile.Text = formSettings.UpdateSchemaFile;
            txtDatabaseName.Text = formSettings.DatabaseName;

            errorWriter = new ErrorWriter();
        }

        private void BtnOriginSchema_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdOriginSchema.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOriginSchema.Text = ofdOriginSchema.FileName;
                formSettings.OriginSchema = txtOriginSchema.Text;
                formSettings.Save();
            }
        }

        private void BtnDestinationSchema_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdDestinationSchema.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtDestinationSchema.Text = ofdDestinationSchema.FileName;
                formSettings.DestinationSchema = txtDestinationSchema.Text;
                formSettings.Save();
            }
        }

        private void BtnOutputDirectory_Click(object sender, EventArgs e)
        {
            DialogResult result = fbdOutputDirectory.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputDirectory.Text = fbdOutputDirectory.SelectedPath;
                formSettings.OutputDirectory = txtOutputDirectory.Text;
                formSettings.Save();
            }
        }

        private bool MandatoryFieldArePresent()
        {
            txtOriginSchema.Text = txtOriginSchema.Text;
            if (txtOriginSchema.Text == string.Empty || !File.Exists(txtOriginSchema.Text))
            {
                MessageBox.Show("Select origin generated schema", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            txtDestinationSchema.Text = txtDestinationSchema.Text.Trim();
            if (txtDestinationSchema.Text == string.Empty || !File.Exists(txtDestinationSchema.Text))
            {
                MessageBox.Show("Select destination generated schema", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (string.Equals(txtDestinationSchema.Text, txtOriginSchema.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Origin and destination are the same", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private (string origin, string destination) GetSchema()
        {
            return (File.ReadAllText(txtOriginSchema.Text).Trim(), File.ReadAllText(txtDestinationSchema.Text).Trim());
        }
        private void BtnCompare_Click(object sender, EventArgs e)
        {
            try
            {
                if (!MandatoryFieldArePresent())
                    return;

                txtOutputDirectory.Text = txtOutputDirectory.Text.Trim();
                if (txtOutputDirectory.Text == string.Empty)
                {
                    MessageBox.Show("Select un output directory", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DisableMainForm(PleaseWait);

                string fileNameDiffOrigin = GetFileNameDiff(txtOriginSchema.Text);
                string fileNameDiffDestination = GetFileNameDiff(txtDestinationSchema.Text);

                if (File.Exists(fileNameDiffOrigin))
                    File.Delete(fileNameDiffOrigin);

                if (File.Exists(fileNameDiffDestination))
                    File.Delete(fileNameDiffDestination);

                var errorFile = $"{txtOutputDirectory.Text}\\ErrorCompare.txt";
                if (File.Exists(errorFile))
                    File.Delete(errorFile);

                var (origin, destination) = GetSchema();

                ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
                IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();

                CompareSchemaManager schemaCompare = new(schemaBuilder, dbObjectFactory, errorWriter);
                (var file1, var file2, var errors) = schemaCompare.Compare(origin, destination);

                File.WriteAllText(fileNameDiffOrigin, file1);
                File.WriteAllText(fileNameDiffDestination, file2);
                File.WriteAllText(errorFile, errors);
                EnableMainForm(string.IsNullOrEmpty(errors) ? FileCompareCreated : FileCompareCreatedWithErrors);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + exc.StackTrace);
                EnableMainForm("");
            }
        }
        private string GetFileNameDiff(string fileName)
        {
            var indexDot = fileName.LastIndexOf(".");
            if (indexDot == 0)
                return $"{fileName}{formSettings.Suffix}";
            else
                return $"{fileName.Substring(0, indexDot)}{formSettings.Suffix}{fileName.Substring(indexDot)}";
        }

        private void DisableMainForm(string text)
        {
            lblInfo.Text = text;
            this.Enabled = false;
        }

        private void EnableMainForm(string text)
        {
            lblInfo.Text = text;
            this.Enabled = true;
        }

        private void BtnUpdateSchema_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdUpdateSchemaFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtUpdateSchemaFile.Text = ofdUpdateSchemaFile.FileName;
                formSettings.UpdateSchemaFile = txtUpdateSchemaFile.Text;
                formSettings.Save();
            }
        }

        private void TxtDatabaseName_TextChanged(object sender, EventArgs e)
        {
            formSettings.DatabaseName = txtDatabaseName.Text;
            formSettings.Save();
        }

        private void TxtSuffix_TextChanged(object sender, EventArgs e)
        {
            formSettings.Suffix = txtSuffix.Text;
            formSettings.Save();
        }

        private void BtnCreateUpdateFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (!MandatoryFieldArePresent())
                    return;

                txtUpdateSchemaFile.Text = txtUpdateSchemaFile.Text.Trim();
                if (txtUpdateSchemaFile.Text == string.Empty)
                {
                    MessageBox.Show("Select a file diff", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                txtDatabaseName.Text = txtDatabaseName.Text.Trim();
                if (txtDatabaseName.Text == string.Empty)
                {
                    MessageBox.Show("Select a database name", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (File.Exists(txtUpdateSchemaFile.Text))
                    File.Delete(txtUpdateSchemaFile.Text);

                var errorFile = GetErrorFileName();
                if (File.Exists(errorFile))
                    File.Delete(errorFile);

                DisableMainForm(PleaseWait);

                var (origin, destination) = GetSchema();

                IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
                ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
                UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
                (var updateSchema, var errors) = updateSchemaManager.UpdateSchema(origin, destination, txtDatabaseName.Text);

                StringBuilder stringResult = new();
                stringResult.AppendLine($"{schemaBuilder.GetStartCommentInLine()} Update Schema {txtOriginSchema.Text} --> {txtDestinationSchema.Text}");
                stringResult.AppendLine(updateSchema);

                File.WriteAllText(txtUpdateSchemaFile.Text, stringResult.ToString());
                File.WriteAllText(errorFile, errors);

                EnableMainForm(string.IsNullOrEmpty(errors) ? FileUpdateCreated : FileUpdateCreatedWithErrors);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + exc.StackTrace);
                EnableMainForm("");
            }
        }

        private string GetErrorFileName()
        {
            var indexDot = txtUpdateSchemaFile.Text.LastIndexOf('.');
            if (indexDot > 0)
                return $"{txtUpdateSchemaFile.Text.Substring(0, indexDot)}_errorsUpdateSchema.{txtUpdateSchemaFile.Text.Substring(indexDot + 1)}";
            else
                return $"{txtUpdateSchemaFile.Text}_errors";
        }

        private void BtnSwapOriginDestination_Click(object sender, EventArgs e)
        {
            var swap = txtOriginSchema.Text;
            txtOriginSchema.Text = txtDestinationSchema.Text;
            txtDestinationSchema.Text = swap;
        }
    }
}
