using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using static SqlSchemaCompare.Core.TSql.TSqlParser;

namespace SqlSchemaCompare.Core.TSql
{
    public sealed class TSqlParserUpdateListener : TSqlParserBaseListener
    {
        private readonly ICharStream _stream;
        private readonly TSqlTableFactory _tableFactory;
        private readonly TSqlViewFactory _viewFactory;
        private readonly TSqlFunctionFactory _functionFactory;
        private readonly TSqlStoreProcedureFactory _storeProcedureFactory;
        private readonly TSqlSchemaFactory _schemaFactory;
        private readonly TSqlTriggerFactory _triggerFactory;
        private readonly TSqlUserFactory _userFactory;
        private readonly TSqlRoleFactory _roleFactory;
        private readonly TSqlTypeCreator _typeFactory;
        private readonly TSqlIndexFactory _indexFactory;
        private readonly TSqlMemberFactory _memberFactory;

        private readonly IList<Type> DDLParserRule = new List<Type>()
            { typeof(Cfl_statementContext) };
        public TSqlParserUpdateListener(ICharStream stream)
        {
            _stream = stream;
            _tableFactory = new TSqlTableFactory();
            _viewFactory = new TSqlViewFactory();
            _functionFactory = new TSqlFunctionFactory();
            _storeProcedureFactory = new TSqlStoreProcedureFactory();
            _schemaFactory = new TSqlSchemaFactory();
            _triggerFactory = new TSqlTriggerFactory();
            _userFactory = new TSqlUserFactory();
            _roleFactory = new TSqlRoleFactory();
            _typeFactory = new TSqlTypeCreator();
            _indexFactory = new TSqlIndexFactory();
            _memberFactory = new TSqlMemberFactory();
        }
        public List<DbObject> DbObjects { get; } = new();
        public override void ExitAlter_database([NotNull] Alter_databaseContext context)
        {
            DbObjects.Add(new SimpleDbObject { Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)) });
        }

        public override void ExitCreate_columnstore_index([NotNull] Create_columnstore_indexContext context)
        {
            DbObjects.Add(new SimpleDbObject { Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)) });
        }
        public override void ExitCreate_table([NotNull] TSqlParser.Create_tableContext context)
        {
            if (!ObjectInsideDDL(context))
                DbObjects.Add(_tableFactory.Create(context, _stream));
        }

        public override void ExitCreate_view([NotNull] TSqlParser.Create_viewContext context)
        {
            DbObjects.Add(_viewFactory.Create(context, _stream));
        }

        public override void ExitCreate_or_alter_function([NotNull] TSqlParser.Create_or_alter_functionContext context)
        {
            DbObjects.Add(_functionFactory.Create(context, _stream));
        }

        public override void ExitCreate_or_alter_procedure([NotNull] TSqlParser.Create_or_alter_procedureContext context)
        {
            DbObjects.Add(_storeProcedureFactory.Create(context, _stream));
        }

        public override void ExitCreate_schema([NotNull] TSqlParser.Create_schemaContext context)
        {
            DbObjects.Add(_schemaFactory.Create(context, _stream));
        }

        public override void ExitCreate_or_alter_trigger([NotNull] TSqlParser.Create_or_alter_triggerContext context)
        {
            DbObjects.Add(_triggerFactory.Create(context, _stream));
        }

        public override void ExitCreate_user([NotNull] TSqlParser.Create_userContext context)
        {
            DbObjects.Add(_userFactory.Create(context, _stream));
        }
        public override void ExitCreate_db_role([NotNull] TSqlParser.Create_db_roleContext context)
        {
            DbObjects.Add(_roleFactory.Create(context, _stream));
        }
        
        public override void ExitEnable_trigger([NotNull] TSqlParser.Enable_triggerContext context)
        {
            var enabled = _triggerFactory.CreateEnable(context, _stream);
            var trigger = DbObjects.OfType<Trigger>().Single(x => x.Name == enabled.Name);
            trigger.SetEnabled(enabled);
        }

        public override void ExitDisable_trigger([NotNull] TSqlParser.Disable_triggerContext context)
        {
            var enabled = _triggerFactory.CreateDisable(context, _stream);
            var trigger = DbObjects.OfType<Trigger>().Single(x => x.Name == enabled.Name);
            trigger.SetEnabled(enabled);
        }
        public override void ExitAlter_db_role([NotNull] TSqlParser.Alter_db_roleContext context)
        {
            DbObjects.Add(_memberFactory.Create(context, _stream));
        }

        public override void ExitCreate_type([NotNull] TSqlParser.Create_typeContext context)
        {
            if (!ObjectInsideDDL(context))
                DbObjects.Add(_typeFactory.Create(context, _stream));
        }

        public override void ExitCreate_index([NotNull] TSqlParser.Create_indexContext context)
        {
            DbObjects.Add(_indexFactory.Create(context, _stream));
        }

        public override void ExitAlter_table([NotNull] TSqlParser.Alter_tableContext context)
        {
            var constraint = _tableFactory.CreateAlterTable(context);
            var role = DbObjects.OfType<Table>().Single(x => x.Identifier == constraint.ParentName);
            role.AddConstraint(constraint);
        }

        public override void ExitCreate_nonclustered_columnstore_index([NotNull] Create_nonclustered_columnstore_indexContext context)
        {
            DbObjects.Add(new SimpleDbObject { Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)) });
        }

        public override void ExitCreate_sequence([NotNull] Create_sequenceContext context)
        {
            DbObjects.Add(new SimpleDbObject { Sql = context.Start.InputStream.GetText(new Interval(context.start.StartIndex, context.stop.StopIndex)) });
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
