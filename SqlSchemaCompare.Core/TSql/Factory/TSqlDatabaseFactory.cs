using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlDatabaseFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var databaseContext = context as TSqlParser.Create_databaseContext;
            return new Database { 
                Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
                Name = databaseContext.database.GetText()
            };
        }
    }
}
