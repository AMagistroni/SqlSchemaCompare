using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public class Configuration
    {
        public List<string> DiscardObjects { get; set; } = new List<string>();
        public List<string> DiscardSchemas { get; set; } = new List<string>();
    }
}
