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

            GrpCompare.Enabled = false;
            GrpUpdateSchema.Enabled = false;
            GrpDbObjects.Enabled = false;

            BtnClear.Enabled = false;
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

                ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
                IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();

                CompareSchemaManager schemaCompare = new(schemaBuilder);
                (var file1, var file2) = schemaCompare.Compare(currentOriginDbObjects, currentDestinationDbObjects, SelectedObjectType());

                File.WriteAllText(fileNameDiffOrigin, file1);
                File.WriteAllText(fileNameDiffDestination, file2);
                
                EnableMainForm(FileCompareCreated);
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

        private void LoadClearSchemaCompleted(string text, bool isAfterLoad)
        {
            lblInfo.Text = text;
            this.Enabled = true;

            btnOriginSchema.Enabled = !isAfterLoad;
            btnDestinationSchema.Enabled = !isAfterLoad;
            BtnSwapOriginDestination.Enabled = !isAfterLoad;
            BtnLoadSchema.Enabled = !isAfterLoad;
            BtnClear.Enabled = isAfterLoad;
            GrpCompare.Enabled = isAfterLoad;
            GrpDbObjects.Enabled = isAfterLoad;
            GrpUpdateSchema.Enabled = isAfterLoad;
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
                var updateSchema = updateSchemaManager.UpdateSchema(currentOriginDbObjects, currentDestinationDbObjects, txtDatabaseName.Text, SelectedObjectType());

                StringBuilder stringResult = new();
                stringResult.AppendLine($"{schemaBuilder.GetStartCommentInLine()} Update Schema {txtOriginSchema.Text} --> {txtDestinationSchema.Text}");
                stringResult.AppendLine(updateSchema);

                File.WriteAllText(txtUpdateSchemaFile.Text, stringResult.ToString());

                EnableMainForm(FileUpdateCreated);
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

        private void BtnLoadSchema_Click(object sender, EventArgs e)
        {
            if (!MandatoryFieldArePresent())
                return;

            var errorFile = GetErrorFileName();
            if (File.Exists(errorFile))
                File.Delete(errorFile);

            DisableMainForm(PleaseWait);
            var (origin, destination) = GetSchema();
            
            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            var loadSchemaManager = new LoadSchemaManager(dbObjectFactory, errorWriter);
            string errors;
            (currentOriginDbObjects, currentDestinationDbObjects, errors) = loadSchemaManager.LoadSchema(origin, destination);

            File.WriteAllText(errorFile, errors);

            LoadClearSchemaCompleted(string.IsNullOrEmpty(errors) ? SchemaLoaded : SchemaLoadedWithErrors, true);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            LoadClearSchemaCompleted(ChooseCompareUpdate, false);
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
            ChkIndex.Checked = ChkAll.Checked;
        }

        private IEnumerable<DbObjectType> SelectedObjectType()
        {
            var selectedObjectType = new List<DbObjectType>();
            if (ChkFunction.Checked)
                selectedObjectType.Add(DbObjectType.Function);
            if (ChkStoreProcedure.Checked)
                selectedObjectType.Add(DbObjectType.StoreProcedure);
            if (ChkTable.Checked)
            {
                selectedObjectType.Add(DbObjectType.Table);
                selectedObjectType.Add(DbObjectType.TableContraint);
                selectedObjectType.Add(DbObjectType.Column);
            }
            if (ChkUser.Checked)
            {
                selectedObjectType.Add(DbObjectType.User);
                selectedObjectType.Add(DbObjectType.Role);
                selectedObjectType.Add(DbObjectType.Member);
            }
                if (ChkView.Checked)
                selectedObjectType.Add(DbObjectType.View);
            if (ChkSchema.Checked)
                selectedObjectType.Add(DbObjectType.Schema);
            if (ChkTrigger.Checked)
            {
                selectedObjectType.Add(DbObjectType.Trigger);
                selectedObjectType.Add(DbObjectType.EnableTrigger);
            }
            if (ChkTableType.Checked)
                selectedObjectType.Add(DbObjectType.Type);
            if (ChkOther.Checked)
                selectedObjectType.Add(DbObjectType.Other);
            if (ChkIndex.Checked)
                selectedObjectType.Add(DbObjectType.Index);

            return selectedObjectType;
        }
    }
}
