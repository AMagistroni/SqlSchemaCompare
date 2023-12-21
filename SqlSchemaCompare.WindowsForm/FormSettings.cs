using System.Configuration;

namespace SqlSchemaCompare.WindowsForm
{
    internal sealed class FormSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        public string OriginSchema
        {
            get => (string)this[nameof(OriginSchema)];
            set => this[nameof(OriginSchema)] = value;
        }

        [UserScopedSetting()]
        public string DestinationSchema
        {
            get => (string)this[nameof(DestinationSchema)];
            set => this[nameof(DestinationSchema)] = value;
        }
        [UserScopedSetting()]
        public string Configuration
        {
            get => (string)this[nameof(Configuration)];
            set => this[nameof(Configuration)] = value;
        }

        [UserScopedSetting()]
        public string OutputDirectory
        {
            get => (string)this[nameof(OutputDirectory)];
            set => this[nameof(OutputDirectory)] = value;
        }

        [UserScopedSetting()]
        public string UpdateSchemaFile
        {
            get => (string)this[nameof(UpdateSchemaFile)];
            set => this[nameof(UpdateSchemaFile)] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("_diff")]
        public string Suffix
        {
            get => (string)this[nameof(Suffix)];
            set => this[nameof(Suffix)] = value;
        }
    }
}
