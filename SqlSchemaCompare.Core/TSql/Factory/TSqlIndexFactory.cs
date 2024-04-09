using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public partial class TSqlIndexFactory(Configuration configuration) : FactoryBase, IFactory
    {
        private readonly Configuration _configuration = configuration;

        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var indexContext = context as TSqlParser.Create_indexContext;
            var columnsInclude = indexContext.column_name_list() != null ? indexContext.column_name_list().id_().Select(x => x.GetText()) : [];

            var sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex));
            if (_configuration.TableConfiguration.DiscardWithOnPrimary)
            {
                sql = RegexWithOnPrimary().Replace(sql, "");
            }

            return new Index
            {
                Sql = sql,
                Name = indexContext.id_()[0].GetText(),
                Schema = string.Empty,
                Operation = GetOperation(indexContext.GetChild(0).GetText()),
                ParentName = indexContext.table_name().GetText(),
                ColumnNames = indexContext.column_name_list_with_order().id_().Select(x => x.GetText())
                                .Union(columnsInclude)
            };
        }

        public static DbObject CreateAlter(ParserRuleContext context, ICharStream stream)
        {
            var indexContext = context as TSqlParser.Alter_indexContext;
            return new Index
            {
                Sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
                Name = indexContext.id_().GetText(),
                Schema = string.Empty,
                Operation = GetOperation(indexContext.GetChild(0).GetText()),
                ParentName = indexContext.table_name().GetText(),
                ColumnNames = []
            };
        }

        [GeneratedRegex(@"WITH\s*\([^)]*\)\s*ON\s*\[\w+\]")]
        private static partial Regex RegexWithOnPrimary();
    }
}
