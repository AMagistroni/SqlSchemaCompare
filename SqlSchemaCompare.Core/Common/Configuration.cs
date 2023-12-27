using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public class Configuration
    {
        public List<string> DiscardObjects { get; set; } = [];
        public List<string> DiscardSchemas { get; set; } = [];
    }
}
