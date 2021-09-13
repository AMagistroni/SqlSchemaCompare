using System.Collections.Generic;

namespace SqlSchemaCompare.Core.DbStructures
{
    public class Index : DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.Index;
        public IEnumerable<string> ColumnNames { get; init; }
    }
}
