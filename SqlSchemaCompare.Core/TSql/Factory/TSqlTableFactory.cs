using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using static SqlSchemaCompare.Core.DbStructures.Table;

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
                    table.AddConstraint(CreatePrimaryKeyConstraint(columnTree.table_constraint(), stream, table));
                    table.PrimaryKeyDefinedInsideCreateTable = true;
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

        private Table.TablePrimaryKeyConstraint CreatePrimaryKeyConstraint(TSqlParser.Table_constraintContext constraintContext, ICharStream stream, Table table)
        {
            return new Table.TablePrimaryKeyConstraint
            {
                Sql = stream.GetText(new Interval(constraintContext.start.StartIndex, constraintContext.stop.StopIndex)),
                Name = constraintContext.constraint?.GetText(),
                ParentName = table.Identifier,
                ColumnNames = constraintContext.column_name_list_with_order().id_().Select(x => x.GetText()),
                Table = table
            };
        }

        internal DbObject CreateAlterTable(ParserRuleContext context)
        {
            var alterTableContext = context as TSqlParser.Alter_tableContext;

            if ((alterTableContext.fk != null) || (alterTableContext.constraint != null))
            {
                return CreateForeignKeyConstraint(alterTableContext);
            }
            else if (alterTableContext.SET() is not null)
            {
                return new TableSet
                {
                    Sql = alterTableContext.Start.InputStream.GetText(new Interval(alterTableContext.start.StartIndex, alterTableContext.stop.StopIndex)),
                    Name = alterTableContext.constraint != null ? alterTableContext.constraint.GetText() : default,
                    ParentName = alterTableContext.children[2].GetText(),
                };
            }
            else if (alterTableContext.column_def_table_constraints() != null || alterTableContext.CHECK() != null || alterTableContext.NOCHECK() != null) {
                return CreateDefaultConstraint(alterTableContext);
            }            

            throw new NotImplementedException();
        }

        private Table.TableForeignKeyConstraint CreateForeignKeyConstraint(TSqlParser.Alter_tableContext alterTableContext)
        {            
            return new Table.TableForeignKeyConstraint
            {
                Sql = alterTableContext.Start.InputStream.GetText(new Interval(alterTableContext.start.StartIndex, alterTableContext.stop.StopIndex)),
                Name = alterTableContext.constraint != null ? alterTableContext.constraint.GetText() : default,
                ParentName = alterTableContext.children[2].GetText(),
                ColumnNames = new List<string> { alterTableContext.fk?.GetText() }
            };
        }

        private Table.TableDefaultConstraint CreateDefaultConstraint(TSqlParser.Alter_tableContext alterTableContext)
        {
            if (alterTableContext.column_def_table_constraints() is not null)
            {
                var constraint = ((TSqlParser.Column_def_table_constraintContext)alterTableContext.column_def_table_constraints().children[0]).table_constraint();
                return new Table.TableDefaultConstraint
                {
                    Sql = alterTableContext.Start.InputStream.GetText(new Interval(alterTableContext.start.StartIndex, alterTableContext.stop.StopIndex)),
                    Name = constraint.CONSTRAINT() != null ? constraint.id_()[0].GetText() : string.Empty,
                    ParentName = alterTableContext.children[2].GetText(),
                    ColumnNames = new List<string> { constraint.forColumn.GetText() },
                    Value = constraint.DEFAULT() != null ? constraint.default_value_column.GetText() : string.Empty
                };
            }
            else
            {
                return new Table.TableDefaultConstraint
                {
                    Sql = alterTableContext.Start.InputStream.GetText(new Interval(alterTableContext.start.StartIndex, alterTableContext.stop.StopIndex)),
                    ParentName = alterTableContext.children[2].GetText(),
                    Value = alterTableContext.search_condition().GetText()
                };
            }
        }
    }
}
