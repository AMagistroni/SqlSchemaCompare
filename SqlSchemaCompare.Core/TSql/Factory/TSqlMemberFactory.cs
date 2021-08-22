using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlMemberFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var memberContext = context as TSqlParser.Alter_db_roleContext;
            return new Member
            {
                Name = memberContext.database_principal.GetText(),
                RoleName = memberContext.role_name.GetText(),
                Sql = stream.GetText(new Interval(memberContext.start.StartIndex, memberContext.stop.StopIndex)),
            };
        }
    }
}
