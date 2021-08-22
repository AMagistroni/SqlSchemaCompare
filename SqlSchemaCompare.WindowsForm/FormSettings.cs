using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaCompare.WindowsForm
{
    sealed class FormSettings : ApplicationSettingsBase
    {
        [UserScopedSettingAttribute()]
        public String OriginSchema
        {
            get { return (String)this["OriginSchema"]; }
            set { this["OriginSchema"] = value; }
        }

        [UserScopedSettingAttribute()]
        public String DestinationSchema
        {
            get { return (String)this["DestinationSchema"]; }
            set { this["DestinationSchema"] = value; }
        }
        [UserScopedSettingAttribute()]
        public String OutputDirectory
        {
            get { return (String)this["OutputDirectory"]; }
            set { this["OutputDirectory"] = value; }
        }

        [UserScopedSettingAttribute()]
        public String UpdateSchemaFile
        {
            get { return (String)this["UpdateSchemaFile"]; }
            set { this["UpdateSchemaFile"] = value; }
        }

        [UserScopedSettingAttribute()]
        public String DatabaseName
        {
            get { return (String)this["DatabaseName"]; }
            set { this["DatabaseName"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("_diff")]
        public String Suffix
        {
            get { return (String)this["Suffix"]; }
            set { this["Suffix"] = value; }
        }
    }
}
