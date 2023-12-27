using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Linq;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlIndexFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var indexContext = context as TSqlParser.Create_indexContext;
            var columnsInclude = indexContext.column_name_list() != null ? indexContext.column_name_list().id_().Select(x => x.GetText()) : Enumerable.Empty<string>();
            return new Index
            {
                Sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
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
                ColumnNames = Enumerable.Empty<string>()
            };
        }
    }
}
