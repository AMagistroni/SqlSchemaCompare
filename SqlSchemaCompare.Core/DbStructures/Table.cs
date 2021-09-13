using System.Collections.Generic;

namespace SqlSchemaCompare.Core.DbStructures
{
    public class Table: DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.Table;
        public class Column : DbObject {
            public override DbObjectType DbObjectType => DbObjectType.Column;
            public IList<TableConstraint> Constraints { get; } = new List<TableConstraint>();
            public Table Table { get; init; }
        }
        public class TableConstraint : DbObject
        {            
            public enum ConstraintTypes
            {
                Default,
                ForeignKey,
                PrimaryKey
            }
            public override DbObjectType DbObjectType => DbObjectType.TableContraint;
            public IEnumerable<string> ColumnName { get; init; }
            public ConstraintTypes ConstraintType { get; init; }
            public string Value { get; init; }
            public Table Table { get; init; }
        }
        public IList<Column> Columns { get; } = new List<Column>();
        public IList<TableConstraint> Constraints { get; } = new List<TableConstraint>();
        public void AdColumns(Column Colum) => Columns.Add(Colum);
        public void AddConstraint(TableConstraint tableConstraint) => Constraints.Add(tableConstraint);
    }
}
