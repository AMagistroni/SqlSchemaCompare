using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlSimpleDbObjectFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            return new SimpleDbObject { Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex))};
        }
    }
}
