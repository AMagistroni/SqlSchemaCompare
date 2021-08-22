using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public class TSqlTriggerFactory : FactoryBase, IFactory
    {
        public DbObject Create(ParserRuleContext context, ICharStream stream)
        {
            var triggerContext = context as TSqlParser.Create_or_alter_triggerContext;
            return new Trigger()
            {
                Sql = stream.GetText(new Interval(triggerContext.start.StartIndex, triggerContext.stop.StopIndex)),
                Name = triggerContext.create_or_alter_ddl_trigger().simple_name().GetText(),
                Schema = string.Empty,
                Operation = GetOperation(triggerContext.GetChild(0).GetChild(0).GetText())
            };
        }
        public Trigger.EnabledDbObject CreateEnable(ParserRuleContext context, ICharStream stream)
        {
            var triggerContext = context as TSqlParser.Enable_triggerContext;
            return new Trigger.EnabledDbObject()
            {
                Sql = stream.GetText(new Interval(triggerContext.start.StartIndex, triggerContext.stop.StopIndex)),
                Name = triggerContext.trigger_name.GetText(),
                Enabled = true
            };
        }
        public Trigger.EnabledDbObject CreateDisable(ParserRuleContext context, ICharStream stream)
        {
            var triggerContext = context as TSqlParser.Disable_triggerContext;
            return new Trigger.EnabledDbObject()
            {
                Sql = stream.GetText(new Interval(triggerContext.start.StartIndex, triggerContext.stop.StopIndex)),
                Name = triggerContext.trigger_name.GetText(),
                Enabled = false
            };
        }
    }
}
