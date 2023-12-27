using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.DbStructures;
using System;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public abstract class FactoryBase
    {
        protected static Operation GetOperation(string operation)
        {
            operation = operation.ToUpper();
            if (operation == "CREATE")
                return Operation.Create;
            else if (operation == "ALTER")
                return Operation.Alter;
            else throw new NotSupportedException();
        }

        protected static string GetSqlWithoutGOStatement(ParserRuleContext context, ICharStream stream)
        {
            var sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)).Trim();
            if (sql.EndsWith("GO", StringComparison.OrdinalIgnoreCase))
                sql = sql[0..^2].Trim();

            return sql;
        }
    }
}
