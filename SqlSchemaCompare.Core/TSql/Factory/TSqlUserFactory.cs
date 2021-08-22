using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlUserFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var userContext = context as TSqlParser.Create_userContext;
            return new User
            {
                Sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
                Name = userContext.user_name.GetText(),
                Schema = userContext.schema_name?.GetText(),
                Operation = GetOperation(userContext.GetChild(0).GetText())
            };
        }
    }
}
