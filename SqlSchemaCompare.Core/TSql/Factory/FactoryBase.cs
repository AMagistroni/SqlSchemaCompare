using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.DbStructures;
using System;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public abstract class FactoryBase
    {
        protected Operation GetOperation(string operation)
        {
            operation = operation.ToUpper();
            if (operation == "CREATE")
                return Operation.Create;
            else if (operation == "ALTER")
                return Operation.Alter;
            else throw new NotImplementedException();
        }

        protected string GetSqlWithoutGOStatement(ParserRuleContext context, ICharStream stream)
        {
            var sql = stream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)).Trim();
            if (sql.ToUpper().EndsWith("GO"))
                sql = sql[0..^2].Trim();

            return sql;
        }
    }
}
