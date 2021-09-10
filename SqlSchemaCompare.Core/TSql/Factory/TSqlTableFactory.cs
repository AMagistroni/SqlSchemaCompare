using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;

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
                    table.AddConstraint(CreatePrimaryKeyConstraint(columnTree.table_constraint(), stream, table.Identifier));
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

        public Table.TableConstraint CreatePrimaryKeyConstraint(TSqlParser.Table_constraintContext constraintContext, ICharStream stream, string tableName)
        {
            return new Table.TableConstraint
            {
                Sql = stream.GetText(new Interval(constraintContext.start.StartIndex, constraintContext.stop.StopIndex)),
                Name = constraintContext.id_()[0].GetText(),
                ParentName = tableName,
                ColumnName = constraintContext.column_name_list_with_order().id_().Select(x => x.GetText()),
                ConstraintType = Table.TableConstraint.ConstraintTypes.PrimaryKey
            };
        }

        public Table.TableConstraint CreateAlterTable(ParserRuleContext context)
        {
            var alterTableContext = context as TSqlParser.Alter_tableContext;
            var tableName = alterTableContext.children[2].GetText();

            string name = string.Empty;
            string columnName = string.Empty;
            string value = string.Empty;

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
                if (constraint.DEFAULT() != null)
                {
                    value = constraint.default_value_column.GetText();
                }
            }
            return  new Table.TableConstraint
            {
                Sql = alterTableContext.Start.InputStream.GetText(new Interval(alterTableContext.start.StartIndex, alterTableContext.stop.StopIndex)),
                Name = name,
                ParentName = tableName,
                ColumnName = new List<string> {columnName},
                ConstraintType = constraintType,
                Value = value.ToString()
            };
        }
    }
}
