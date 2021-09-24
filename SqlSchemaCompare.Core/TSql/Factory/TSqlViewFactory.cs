using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlViewFactory: FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var viewContext = context as TSqlParser.Create_viewContext;
            var bodyContext = viewContext.select_statement_standalone();
            if (bodyContext.start.StartIndex < bodyContext.stop.StopIndex)
            {
                return new View()
                {
                    Sql = GetSqlWithoutGOStatement(context, stream),
                    Name = viewContext.simple_name().name.GetText(),
                    Schema = viewContext.simple_name().schema.GetText(),
                    Body = stream.GetText(new Interval(bodyContext.start.StartIndex, bodyContext.stop.StopIndex)),
                    Operation = GetOperation(viewContext.GetChild(0).GetText())
                };
            }
            else
                return null;
        }
    }
}
