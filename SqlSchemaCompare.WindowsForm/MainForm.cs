using Newtonsoft.Json;
using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System;
using System.Collections.Generic;
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
        private const string ApplicationName = "Sql compare";
        private const string SchemaLoaded = "The schemas are loaded.";
        private const string SchemaLoadedWithErrors = "The schemas are loaded with errors";
        private const string ChooseCompareUpdate = "Choose compare or update";

        private IErrorWriter errorWriter;

        private IEnumerable<DbObject> currentOriginDbObjects;
        private IEnumerable<DbObject> currentDestinationDbObjects;
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
            txtConfiguration.Text = formSettings.Configuration;

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
            if (txtOriginSchema.Text?.Length == 0 || !File.Exists(txtOriginSchema.Text))
            {
                MessageBox.Show("Select origin generated schema", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            txtDestinationSchema.Text = txtDestinationSchema.Text.Trim();
            if (txtDestinationSchema.Text?.Length == 0 || !File.Exists(txtDestinationSchema.Text))
            {
                MessageBox.Show("Select destination generated schema", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (string.Equals(txtDestinationSchema.Text, txtOriginSchema.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Origin and destination are the same", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            txtOutputDirectory.Text = txtOutputDirectory.Text.Trim();
            if (txtOutputDirectory.Text?.Length == 0)
            {
                MessageBox.Show("Select an output directory", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                EnableDisableMainForm(PleaseWait, false);

                string fileNameDiffOrigin = GetFileNameDiff(txtOriginSchema.Text);
                string fileNameDiffDestination = GetFileNameDiff(txtDestinationSchema.Text);

                if (File.Exists(fileNameDiffOrigin))
                    File.Delete(fileNameDiffOrigin);

                if (File.Exists(fileNameDiffDestination))
                    File.Delete(fileNameDiffDestination);

                var errorFile = $"{txtOutputDirectory.Text}\\ErrorCompare.txt";
                if (File.Exists(errorFile))
                    File.Delete(errorFile);

                ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
                IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();

                CompareSchemaManager schemaCompare = new(GetConfiguration(), schemaBuilder);
                (var file1, var file2) = schemaCompare.Compare(currentOriginDbObjects, currentDestinationDbObjects, SelectedObjectType());

                File.WriteAllText(fileNameDiffOrigin, file1);
                File.WriteAllText(fileNameDiffDestination, file2);

                EnableDisableMainForm(FileCompareCreated, true);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + exc.StackTrace);
                EnableDisableMainForm(string.Empty, true);
            }
        }

        private Configuration GetConfiguration()
        {
            return string.IsNullOrEmpty(txtConfiguration.Text)
                ? new Configuration()
                : JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(txtConfiguration.Text));
        }
        private string GetFileNameDiff(string fullPathFileName)
        {
            return Path.HasExtension(fullPathFileName)
                ? $"{txtOutputDirectory.Text}\\{Path.GetFileNameWithoutExtension(fullPathFileName)}{formSettings.Suffix}.{Path.GetExtension(fullPathFileName)}"
                : $"{txtOutputDirectory.Text}\\{Path.GetFileName(fullPathFileName)}{formSettings.Suffix}";
        }

        private void EnableDisableMainForm(string text, bool enable)
        {
            lblInfo.Text = text;

            GrpCompare.Enabled = enable;
            GrpDbObjects.Enabled = enable;
            GrpUpdateSchema.Enabled = enable;
            GrpMain.Enabled = enable;

            if (enable)
                ProgressBar.Hide();
            else
                ProgressBar.Show();
        }

        private void LoadClearSchemaCompleted(string text, bool isAfterLoad)
        {
            lblInfo.Text = text;

            btnOutputDirectory.Enabled = !isAfterLoad;
            btnOriginSchema.Enabled = !isAfterLoad;
            btnDestinationSchema.Enabled = !isAfterLoad;
            BtnLoadSchema.Enabled = !isAfterLoad;
            BtnSwapOriginDestination.Enabled = !isAfterLoad;
            btnConfiguration.Enabled = !isAfterLoad;
            GrpMain.Enabled = true;

            GrpCompare.Enabled = isAfterLoad;
            GrpDbObjects.Enabled = isAfterLoad;
            GrpUpdateSchema.Enabled = isAfterLoad;
            BtnClear.Enabled = isAfterLoad;
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
                if (txtUpdateSchemaFile.Text?.Length == 0)
                {
                    MessageBox.Show("Select an update schema file", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (File.Exists(txtUpdateSchemaFile.Text))
                    File.Delete(txtUpdateSchemaFile.Text);

                var errorFile = GetErrorFileName("ErrorsUpdateSchema.txt");
                if (File.Exists(errorFile))
                    File.Delete(errorFile);

                EnableDisableMainForm(PleaseWait, false);

                var (origin, destination) = GetSchema();

                TSqlSchemaBuilder schemaBuilder = new();
                UpdateSchemaManager updateSchemaManager = new(GetConfiguration(), schemaBuilder);
                var updateSchema = updateSchemaManager.UpdateSchema(currentOriginDbObjects, currentDestinationDbObjects, SelectedObjectType());

                StringBuilder stringResult = new();
                stringResult.Append(schemaBuilder.GetStartCommentInLine()).Append(" Update Schema ").Append(txtOriginSchema.Text).Append(" --> ").AppendLine(txtDestinationSchema.Text);
                stringResult.AppendLine(updateSchema);

                File.WriteAllText(txtUpdateSchemaFile.Text, stringResult.ToString());

                EnableDisableMainForm(FileUpdateCreated, true);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + exc.StackTrace);
                EnableDisableMainForm(string.Empty, true);
            }
        }

        private string GetErrorFileName(string suffix)
        {
            return txtOutputDirectory.Text.EndsWith('\\') ? txtOutputDirectory.Text + suffix : $"{txtOutputDirectory.Text}\\{suffix}";
        }

        private void BtnSwapOriginDestination_Click(object sender, EventArgs e)
        {
            (txtDestinationSchema.Text, txtOriginSchema.Text) = (txtOriginSchema.Text, txtDestinationSchema.Text);
            formSettings.OriginSchema = txtOriginSchema.Text;
            formSettings.DestinationSchema = txtDestinationSchema.Text;
            formSettings.Save();
        }

        private void BtnLoadSchema_Click(object sender, EventArgs e)
        {
            if (!MandatoryFieldArePresent())
                return;

            var errorFile = GetErrorFileName("ErrorsLoadSchema.txt");
            if (File.Exists(errorFile))
                File.Delete(errorFile);

            EnableDisableMainForm(PleaseWait, false);
            var (origin, destination) = GetSchema();

            ParametersLoad parameters = new(errorFile, origin, destination);
            BackgroundWorker.RunWorkerAsync(parameters);
        }

        private void LoadSchema(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ParametersLoad parameters = e.Argument as ParametersLoad;
            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            var loadSchemaManager = new LoadSchemaManager(dbObjectFactory, errorWriter);

            string errors;
            (currentOriginDbObjects, currentDestinationDbObjects, errors) = loadSchemaManager.LoadSchema(parameters.Origin, parameters.Destination);
            File.WriteAllText(parameters.ErrorFile, errors);
            e.Result = errors;
        }

        private void LoadSchemaCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Hide();
            LoadClearSchemaCompleted(string.IsNullOrEmpty(e.Result as string) ? SchemaLoaded : SchemaLoadedWithErrors, true);
        }
        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            ChkFunction.Checked = ChkAll.Checked;
            ChkStoreProcedure.Checked = ChkAll.Checked;
            ChkTable.Checked = ChkAll.Checked;
            ChkUser.Checked = ChkAll.Checked;
            ChkView.Checked = ChkAll.Checked;
            ChkSchema.Checked = ChkAll.Checked;
            ChkTrigger.Checked = ChkAll.Checked;
            ChkTableType.Checked = ChkAll.Checked;
            ChkOther.Checked = ChkAll.Checked;
        }

        private List<DbObjectType> SelectedObjectType()
        {
            var selectedObjectType = new List<DbObjectType>();
            if (ChkFunction.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Function));
            if (ChkStoreProcedure.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.StoreProcedure));
            if (ChkTable.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Table));
            if (ChkUser.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.User));
            if (ChkView.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.View));
            if (ChkSchema.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Schema));
            if (ChkTrigger.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Trigger));
            if (ChkTableType.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Type));
            if (ChkOther.Checked)
                selectedObjectType.AddRange(RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Other));

            return selectedObjectType;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            LoadClearSchemaCompleted(ChooseCompareUpdate, false);
        }

        private void BtnConfiguration_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdConfiguration.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtConfiguration.Text = ofdConfiguration.FileName;
                formSettings.Configuration = txtConfiguration.Text;
                formSettings.Save();
            }
        }

        private void BtnGetConfiguration_Click(object sender, EventArgs e)
        {
            var conf = new Configuration
            {
                DiscardObjects = ["[schema].[table to discard]"],
                DiscardSchemas = ["[schema to discard]"],
            };
            Clipboard.SetText(JsonConvert.SerializeObject(conf));
            lblInfo.Text = "Configuration copied into clipboard";
        }

        public record ParametersLoad(string ErrorFile, string Origin, string Destination);
    }
}
