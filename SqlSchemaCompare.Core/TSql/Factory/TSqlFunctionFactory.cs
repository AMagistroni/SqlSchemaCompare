using Antlr4.Runtime;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlFunctionFactory: FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var functionContext = context as TSqlParser.Create_or_alter_functionContext;
            return new Function()
            {
                Sql = GetSqlWithoutGOStatement(context, stream),
                Name = functionContext.func_proc_name_schema().procedure.GetText(),
                Schema = functionContext.func_proc_name_schema().schema.GetText(),
                Operation = GetOperation(functionContext.GetChild(0).GetText())
            };
        }
    }
}
