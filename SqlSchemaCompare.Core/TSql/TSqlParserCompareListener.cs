using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Collections.Generic;
using static SqlSchemaCompare.Core.TSql.TSqlParser;

namespace SqlSchemaCompare.Core.TSql
{
    public sealed class TSqlParserCompareListener : TSqlParserBaseListener
    {
        private readonly ICharStream _stream;

        private readonly IList<Type> DDLParserRule = new List<Type>()
            { typeof(Cfl_statementContext) };
        public TSqlParserCompareListener(ICharStream stream)
        {
            _stream = stream;
        }
        public List<SimpleDbObject> SimpleDbObjects { get; } = new();

        public override void ExitAlter_database([NotNull] Alter_databaseContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_columnstore_index([NotNull] Create_columnstore_indexContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_nonclustered_columnstore_index([NotNull] Create_nonclustered_columnstore_indexContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_sequence([NotNull] Create_sequenceContext context)
        {
            AddObject(context);
        }
        public override void ExitCreate_table([NotNull] TSqlParser.Create_tableContext context)
        {
            if (!ObjectInsideDDL(context))
                AddObject(context);
        }

        public override void ExitCreate_view([NotNull] TSqlParser.Create_viewContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_or_alter_function([NotNull] TSqlParser.Create_or_alter_functionContext context)
        {
            AddObject(context); ;
        }

        public override void ExitCreate_or_alter_procedure([NotNull] TSqlParser.Create_or_alter_procedureContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_schema([NotNull] TSqlParser.Create_schemaContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_or_alter_trigger([NotNull] TSqlParser.Create_or_alter_triggerContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_user([NotNull] TSqlParser.Create_userContext context)
        {
            AddObject(context);
        }
        public override void ExitCreate_db_role([NotNull] TSqlParser.Create_db_roleContext context)
        {
            AddObject(context);
        }

        public override void ExitEnable_trigger([NotNull] TSqlParser.Enable_triggerContext context)
        {
            AddObject(context);
        }

        public override void ExitDisable_trigger([NotNull] TSqlParser.Disable_triggerContext context)
        {
            AddObject(context);
        }
        public override void ExitAlter_db_role([NotNull] TSqlParser.Alter_db_roleContext context)
        {
            AddObject(context);
        }

        public override void ExitCreate_type([NotNull] TSqlParser.Create_typeContext context)
        {
            if (!ObjectInsideDDL(context))
                AddObject(context);
        }

        public override void ExitCreate_index([NotNull] TSqlParser.Create_indexContext context)
        {
            AddObject(context);
        }

        public override void ExitAlter_table([NotNull] TSqlParser.Alter_tableContext context)
        {
            AddObject(context);
        }

        public void AddObject([NotNull] ParserRuleContext context)
        {
            SimpleDbObjects.Add(new SimpleDbObject { Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)) });
        }

        private bool ObjectInsideDDL(RuleContext context)
        {
            if (context.parent is null)
                return false;

            if (DDLParserRule.Contains(context.parent.GetType()))
                return true;
            else
                return ObjectInsideDDL(context.parent);
        }
    }
}
