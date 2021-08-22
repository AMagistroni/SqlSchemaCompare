using Shouldly;
using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlSchemaTest
    {
        [Fact]
        public void CreateSchema()
        {
            const string schemaSql = "CREATE SCHEMA [sch1]";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(schemaSql);
            var dbobject = dbObjects.Single() as Schema;

            dbobject.Name.ShouldBe("[sch1]");
            dbobject.Schema.ShouldBeEmpty();
            dbobject.Identifier.ShouldBe("[sch1]");
            dbobject.Sql.ShouldBe(schemaSql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string databaseName = "dbName";

            const string origin = "CREATE SCHEMA [sch1]";
            const string destination = "CREATE SCHEMA [sch1]";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement
            const string databaseName = "dbName";

            const string origin = "CREATE SCHEMA [sch1]";
            const string destination = "";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

CREATE SCHEMA [sch1]
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
            const string destination = "CREATE SCHEMA [sch1]";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP SCHEMA [sch1]
GO

");
            errors.ShouldBeEmpty();
        }
    }
}
