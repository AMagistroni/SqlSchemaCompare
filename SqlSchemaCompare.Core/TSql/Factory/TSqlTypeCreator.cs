using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlTypeCreator : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var contextTable = context as TSqlParser.Create_typeContext;
            
            return new TypeDbObject
            {
                Sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)),
                Name = contextTable.name.name.GetText(),
                Schema = contextTable.name.schema.GetText(),
                Operation = Operation.Create
            };
        }
    }
}
