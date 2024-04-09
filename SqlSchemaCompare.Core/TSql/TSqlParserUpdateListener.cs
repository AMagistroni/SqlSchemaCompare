using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using static SqlSchemaCompare.Core.DbStructures.Table;
using static SqlSchemaCompare.Core.TSql.TSqlParser;

namespace SqlSchemaCompare.Core.TSql
{
    public sealed class TSqlParserUpdateListener(ICharStream stream, Configuration configuration) : TSqlParserBaseListener
    {
        private readonly ICharStream _stream = stream;
        private readonly TSqlTableFactory _tableFactory = new(configuration);
        private readonly TSqlViewFactory _viewFactory = new();
        private readonly TSqlFunctionFactory _functionFactory = new();
        private readonly TSqlStoreProcedureFactory _storeProcedureFactory = new();
        private readonly TSqlSchemaFactory _schemaFactory = new();
        private readonly TSqlTriggerFactory _triggerFactory = new();
        private readonly TSqlUserFactory _userFactory = new();
        private readonly TSqlRoleFactory _roleFactory = new();
        private readonly TSqlTypeCreator _typeFactory = new();
        private readonly TSqlIndexFactory _indexFactory = new(configuration);
        private readonly TSqlMemberFactory _memberFactory = new();
        private readonly TSqlSimpleDbObjectFactory _simpleDbObjectFactory = new();
        private readonly TSqlDatabaseFactory _databaseFactory = new();

        private readonly IList<Type> DDLParserRule = [typeof(Cfl_statementContext), typeof(Create_or_alter_procedureContext)];

        public List<DbObject> DbObjects { get; } = [];
        public override void ExitAlter_database([NotNull] Alter_databaseContext context)
        {
            DbObjects.Add(_simpleDbObjectFactory.Create(context, _stream));
        }

        public override void ExitCreate_columnstore_index([NotNull] Create_columnstore_indexContext context)
        {
            DbObjects.Add(_simpleDbObjectFactory.Create(context, _stream));
        }
        public override void ExitCreate_table([NotNull] Create_tableContext context)
        {
            if (!ObjectInsideDDL(context))
                DbObjects.Add(_tableFactory.Create(context, _stream));
        }

        public override void ExitCreate_view([NotNull] Create_viewContext context)
        {
            DbObjects.Add(_viewFactory.Create(context, _stream));
        }

        public override void ExitCreate_or_alter_function([NotNull] Create_or_alter_functionContext context)
        {
            DbObjects.Add(_functionFactory.Create(context, _stream));
        }

        public override void ExitCreate_or_alter_procedure([NotNull] Create_or_alter_procedureContext context)
        {
            DbObjects.Add(_storeProcedureFactory.Create(context, _stream));
        }

        public override void ExitCreate_schema([NotNull] Create_schemaContext context)
        {
            DbObjects.Add(_schemaFactory.Create(context, _stream));
        }

        public override void ExitCreate_or_alter_trigger([NotNull] Create_or_alter_triggerContext context)
        {
            DbObjects.Add(_triggerFactory.Create(context, _stream));
        }

        public override void ExitCreate_user([NotNull] Create_userContext context)
        {
            DbObjects.Add(_userFactory.Create(context, _stream));
        }
        public override void ExitCreate_db_role([NotNull] Create_db_roleContext context)
        {
            DbObjects.Add(_roleFactory.Create(context, _stream));
        }

        public override void ExitEnable_trigger([NotNull] Enable_triggerContext context)
        {
            var enabled = TSqlTriggerFactory.CreateEnable(context, _stream);
            var trigger = DbObjects.OfType<Trigger>().Single(x => x.Name == enabled.Name);
            trigger.SetEnabled(enabled);
            DbObjects.Add(enabled);
        }

        public override void ExitDisable_trigger([NotNull] Disable_triggerContext context)
        {
            var enabled = TSqlTriggerFactory.CreateDisable(context, _stream);
            var trigger = DbObjects.OfType<Trigger>().Single(x => x.Name == enabled.Name);
            trigger.SetEnabled(enabled);
            DbObjects.Add(enabled);
        }
        public override void ExitAlter_db_role([NotNull] Alter_db_roleContext context)
        {
            DbObjects.Add(_memberFactory.Create(context, _stream));
        }

        public override void ExitCreate_type([NotNull] Create_typeContext context)
        {
            if (!ObjectInsideDDL(context))
                DbObjects.Add(_typeFactory.Create(context, _stream));
        }

        public override void ExitCreate_index([NotNull] Create_indexContext context)
        {
            if (!ObjectInsideDDL(context))
            {
                var index = _indexFactory.Create(context, _stream);
                var table = DbObjects.OfType<Table>().SingleOrDefault(x => x.Identifier == index.ParentName);
                if (table != null)
                {
                    table.AddIndex(index as DbStructures.Index);
                }
                else
                {
                    var view = DbObjects.OfType<View>().SingleOrDefault(x => x.Identifier == index.ParentName);
                    view.AddIndex(index as DbStructures.Index);
                }
                DbObjects.Add(index);
            }
        }

        public override void ExitAlter_index([NotNull] Alter_indexContext context)
        {
            if (!ObjectInsideDDL(context))
            {
                var index = TSqlIndexFactory.CreateAlter(context, _stream);
                var table = DbObjects.OfType<Table>().SingleOrDefault(x => x.Identifier == index.ParentName);
                table.AddIndex(index as DbStructures.Index);
                DbObjects.Add(index);
            }
        }

        public override void ExitAlter_table([NotNull] Alter_tableContext context)
        {
            if (!ObjectInsideDDL(context))
            {
                var dbObject = TSqlTableFactory.CreateAlterTable(context);
                var table = DbObjects.OfType<Table>().Single(x => x.Identifier == dbObject.ParentName);
                if (dbObject is TableConstraint)
                {
                    var constraint = dbObject as TableConstraint;
                    table.AddConstraint(constraint);
                    constraint.SetTable(table);
                    DbObjects.Add(constraint);
                }
                else
                {
                    table.AddSet(dbObject as TableSet);
                }
            }
        }

        public override void ExitCreate_nonclustered_columnstore_index([NotNull] Create_nonclustered_columnstore_indexContext context)
        {
            DbObjects.Add(_simpleDbObjectFactory.Create(context, _stream));
        }

        public override void ExitCreate_sequence([NotNull] Create_sequenceContext context)
        {
            DbObjects.Add(_simpleDbObjectFactory.Create(context, _stream));
        }
        public override void ExitCreate_database([NotNull] Create_databaseContext context)
        {
            DbObjects.Add(_databaseFactory.Create(context, _stream));
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
