using SqlSchemaCompare.Core.Common;

namespace SqlSchemaCompare.Test.Builder
{
    public static class ConfigurationBuilder
    {
        public static Configuration GetConfiguration()
            => new() { TableConfiguration = new TableConfiguration() { DiscardWithOnPrimary = false } };
        public static Configuration GetConfiguration(bool discardWithOnPrimary)
            => new() { TableConfiguration = new TableConfiguration() { DiscardWithOnPrimary = discardWithOnPrimary } };
    }
}
