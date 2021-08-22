using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlSchemaFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var schemaContext = context as TSqlParser.Create_schemaContext;
            return new Schema()
            {
                Schema = string.Empty,
                Operation = GetOperation(schemaContext.GetChild(0).GetText()),
                Name = schemaContext.schema_name.GetText(),
                Sql = stream.GetText(new Interval(schemaContext.start.StartIndex, schemaContext.stop.StopIndex)),
            };
        }
    }
}
