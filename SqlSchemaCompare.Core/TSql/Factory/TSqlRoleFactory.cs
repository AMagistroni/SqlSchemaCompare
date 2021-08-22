using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlRoleFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var roleContext = context as TSqlParser.Create_db_roleContext;
            return new Role()
            {
                Schema = string.Empty,
                Name = roleContext.role_name.GetText(),
                Operation = GetOperation(roleContext.GetChild(0).GetText()),
                Sql = stream.GetText(new Interval(roleContext.start.StartIndex, roleContext.stop.StopIndex)),
            };
        }
    }
}
