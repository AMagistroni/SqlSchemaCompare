using Shouldly;
using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class UserTest
    {
        [Fact]
        public void CreateUser()
        {
            const string sql = "CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var dbobject = dbObjects.Single() as User;

            dbobject.Name.ShouldBe("[user]");
            dbobject.Schema.ShouldBe("[dbo]");
            dbobject.Identifier.ShouldBe("[user]");
            dbobject.Sql.ShouldBe(sql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void CreateUserWithoutSchema()
        {
            const string sql = "CREATE USER [user] FOR LOGIN [login]";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var dbobject = dbObjects.Single() as User;

            dbobject.Name.ShouldBe("[user]");
            dbobject.Schema.ShouldBeNull();
            dbobject.Identifier.ShouldBe("[user]");
            dbobject.Sql.ShouldBe(sql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string databaseName = "dbName";

            const string origin =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO";
            const string destination =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
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
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
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

CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
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
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP USER [user]
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
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO";
            const string destination =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[sch1]
GO";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER USER [user] WITH DEFAULT_SCHEMA=[dbo]
GO

");
            errors.ShouldBeEmpty();
        }
    }
}
