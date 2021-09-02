using System.Collections.Generic;

namespace SqlSchemaCompare.Core.DbStructures
{
    public class Table: DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.Table;
        public class Column : DbObject {
            public override DbObjectType DbObjectType => DbObjectType.Column;
        }
        public class TableConstraint : DbObject
        {            
            public override DbObjectType DbObjectType => DbObjectType.TableContraint;
            public string ColumnNameForeighKey { get; set; }
        }
        public IList<Column> Columns { get; } = new List<Column>();
        public IList<TableConstraint> Constraints { get; } = new List<TableConstraint>();
        public string Constraint { get; private set; }

        public void AdColumns(Column Colum) => Columns.Add(Colum);
        public void AddConstraint(TableConstraint tableConstraint) => Constraints.Add(tableConstraint);
        public void SetConstraint(string constraint) 
        {
            Constraint = constraint; 
        }
    }
}
