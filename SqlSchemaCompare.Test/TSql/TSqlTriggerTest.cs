using Shouldly;
using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlTriggerTest
    {
        [Fact]
        public void CreateTrigger()
        {
            const string triggerSql =
@"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end";

            const string enableSql = "ENABLE TRIGGER [trg1] ON DATABASE";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(
@$"{triggerSql}
GO

{enableSql}
GO"
                ); ;
            var dbobject = dbObjects.Single() as Trigger;

            dbobject.Name.ShouldBe("[trg1]");
            dbobject.Schema.ShouldBeEmpty();
            dbobject.Identifier.ShouldBe("[trg1]");
            dbobject.Sql.ShouldBe(triggerSql);

            dbobject.EnableObject.Sql.ShouldBe(enableSql);
            dbobject.EnableObject.Name.ShouldBe("[trg1]");
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string databaseName = "dbName";

            const string origin =
    @"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

ENABLE TRIGGER [trg1] ON DATABASE
GO";
            const string destination =
    @"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

ENABLE TRIGGER [trg1] ON DATABASE
GO";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement
            const string databaseName = "dbName";

            const string origin =
@"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

DISABLE TRIGGER [trg1] ON DATABASE
GO";
            const string destination = "";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
    $@"USE [{databaseName}]
GO

CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

DISABLE TRIGGER [trg1] ON DATABASE
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaDropDbObject()
        {
            // When present db object in destination absent from origin
            // Expect updateSchema contains drop statement
            const string databaseName = "dbName";

            const string origin = "";
            const string destination =
    @"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

ENABLE TRIGGER [trg1] ON DATABASE
GO
";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
    $@"USE [{databaseName}]
GO

DROP TRIGGER [trg1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaAlterDbObject()
        {
            // When present db object in destination and in origin and are different
            // Expect updateSchema contains alter statement
            const string databaseName = "dbName";

            const string origin =
    @"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

ENABLE TRIGGER [trg1] ON DATABASE
GO
";
            const string destination =
@"CREATE TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

DISABLE TRIGGER [trg1] ON DATABASE
GO
";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
    $@"USE [{databaseName}]
GO

ALTER TRIGGER [trg1]
ON DATABASE 
for create_procedure, alter_procedure, drop_procedure,
	create_table, alter_table, drop_table,
	create_trigger, alter_trigger, drop_trigger,
	create_view, alter_view, drop_view,
	create_function, alter_function, drop_function,
	create_index, alter_index, drop_index
AS 
begin
	declare @variable int	
end
GO

ENABLE TRIGGER [trg1] ON DATABASE
GO

");
            errors.ShouldBeEmpty();
        }
    }
}
