using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlStoreProcedureFactory: FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var storeProcedureContext = context as TSqlParser.Create_or_alter_procedureContext;
            return new StoreProcedure()
            {
                Sql = stream.GetText(new Interval(storeProcedureContext.start.StartIndex, storeProcedureContext.stop.StopIndex)),
                Name = storeProcedureContext.func_proc_name_schema().procedure.GetText(),
                Schema = storeProcedureContext.func_proc_name_schema().schema.GetText(),
                Operation = GetOperation(storeProcedureContext.GetChild(0).GetText())
            };
        }
    }
}
