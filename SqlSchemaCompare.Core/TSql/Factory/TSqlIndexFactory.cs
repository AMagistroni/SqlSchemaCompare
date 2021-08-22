using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlIndexFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var indexContext = context as TSqlParser.Create_indexContext;
            return new Index
            {
                Sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
                Name = indexContext.id_()[0].GetText(),
                Schema = string.Empty,
                Operation = GetOperation(indexContext.GetChild(0).GetText()),
                TableName = indexContext.table_name().GetText()
            };
        }
    }
}
