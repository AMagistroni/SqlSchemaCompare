using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Linq;
using System.Text;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlTableFactory: FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var contextTable = context as TSqlParser.Create_tableContext;

            var table = new Table
            {
                Sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
                Name = contextTable.table_name().table.GetText(),
                Schema = contextTable.table_name().schema.GetText(),
                Operation = GetOperation(contextTable.GetChild(0).GetText())
            };

            foreach (var columnTree in contextTable.column_def_table_constraints().column_def_table_constraint())
            {
                var columnDefinition = columnTree.column_definition();
                if (columnDefinition != null)
                {
                    table.AdColumns(CreateColumn(columnTree, table));
                }
                else if (columnTree.table_constraint() != null)
                {
                    table.SetConstraint(stream.GetText(new Interval(columnTree.table_constraint().start.StartIndex, columnTree.table_constraint().stop.StopIndex)));
                }
            }
            return table;
        }

        private Table.Column CreateColumn(TSqlParser.Column_def_table_constraintContext columnTree, Table table)
        {
            var columnDefinition = columnTree.column_definition();

            return new Table.Column()
            {
                Sql = columnTree.Start.InputStream.GetText(new Interval(columnTree.start.StartIndex, columnTree.stop.StopIndex)),
                Name = columnDefinition.id_()[0].GetText(),
                ParentName = table.Identifier,
                Table = table
            };
        }

        public Table.TableConstraint CreateAlterTable(ParserRuleContext context)
        {
            var alterTableContext = context as TSqlParser.Alter_tableContext;
            var tableName = alterTableContext.children[2].GetText();

            string name = string.Empty;
            string columnName = string.Empty;
            StringBuilder value = new();

            Table.TableConstraint.ConstraintTypes constraintType = Table.TableConstraint.ConstraintTypes.ForeignKey;
            if ((alterTableContext.fk != null) || (alterTableContext.constraint != null))
            {
                columnName = alterTableContext.fk?.GetText();
                name = alterTableContext.constraint != null ? alterTableContext.constraint.GetText() : default;
                constraintType = Table.TableConstraint.ConstraintTypes.ForeignKey;
            }
            else if (alterTableContext.column_def_table_constraints() != null) {
                var constraint = ((TSqlParser.Column_def_table_constraintContext)alterTableContext.column_def_table_constraints().children[0]).table_constraint();
                if (constraint.CONSTRAINT() != null)
                {
                    name = constraint.id_()[0].GetText();
                }
                columnName = constraint.forColumn.GetText();
                constraintType = Table.TableConstraint.ConstraintTypes.Default;
                var @default = constraint.children.SingleOrDefault(x => x.GetText().ToUpper() == "DEFAULT");
                if (@default != null)
                {
                    var indexOf = constraint.children.IndexOf(@default);
                    value.Append(constraint.children[indexOf + 1].GetText());
                    value.Append(constraint.children[indexOf + 2].GetText());
                    value.Append(constraint.children[indexOf + 3].GetText());
                }
            }
            return  new Table.TableConstraint
            {
                Sql = alterTableContext.Start.InputStream.GetText(new Interval(alterTableContext.start.StartIndex, alterTableContext.stop.StopIndex)),
                Name = name,
                ParentName = tableName,
                ColumnName = columnName,
                ConstraintType = constraintType,
                Value = value.ToString()
            };
        }
    }
}
