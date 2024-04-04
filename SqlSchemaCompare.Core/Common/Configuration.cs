using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public class Configuration
    {
        public List<string> DiscardObjects { get; set; } = [];
        public List<string> DiscardSchemas { get; set; } = [];
        public TableConfiguration TableConfiguration { get; set; }
    }

    public class TableConfiguration
    {
        public bool DiscardWithOnPrimary { get; set; }
    }
}
