using System.Collections.Generic;

namespace SqlSchemaCompare.Core.DbStructures
{
    public class View: DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.View;
        public string Body { get; init; }
        public IList<Index> Indexes { get; } = new List<Index>();        
        public void AddIndex(Index index) => Indexes.Add(index);
    }
}
