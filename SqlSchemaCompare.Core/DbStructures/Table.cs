using SqlSchemaCompare.Core.DbStructures;
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

        public class TableDefaultConstraint : TableConstraint
        {
            public override DbObjectType DbObjectType => DbObjectType.TableDefaultContraint;
            public string Value { get; init; }
        }

        public class TableForeignKeyConstraint : TableConstraint
        {
            public override DbObjectType DbObjectType => DbObjectType.TableForeignKeyContraint;
        }

        public class TablePrimaryKeyConstraint : TableConstraint
        {
            public override DbObjectType DbObjectType => DbObjectType.TablePrimaryKeyContraint;
        }

        public class TableSet : DbObject
        {
            public override DbObjectType DbObjectType => DbObjectType.TableSet;
        }

        public IList<Column> Columns { get; } = new List<Column>();
        public IList<TableConstraint> Constraints { get; } = new List<TableConstraint>();
        public IList<TableSet> TableSetList { get; } = new List<TableSet>();
        public IList<Index> Indexes { get; } = new List<Index>();
        public void AdColumns(Column Colum) => Columns.Add(Colum);
        public void AddConstraint(TableConstraint tableConstraint) => Constraints.Add(tableConstraint);
        public void AddSet(TableSet tableSet) => TableSetList.Add(tableSet);
        public void AddIndex(Index index) => Indexes.Add(index);
        public bool PrimaryKeyDefinedInsideCreateTable { get; set; }
    }
}

public abstract class TableConstraint : DbObject
{
    public Table Table { get; set; }
    public IEnumerable<string> ColumnNames { get; init; }

    public void SetTable(Table table)
    {
        Table = table;
    }
}