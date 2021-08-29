using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class MemberTest
    {
        [Fact]
        public void CreateSchema()
        {
            const string schemaSql = "ALTER ROLE [role] ADD MEMBER [member1]";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(schemaSql);
            var dbobject = dbObjects.Single() as Member;

            dbobject.Name.ShouldBe("[member1]");
            dbobject.Schema.ShouldBeNull();
            dbobject.Identifier.ShouldBe("[member1]");
            dbobject.Sql.ShouldBe(schemaSql);
            dbobject.RoleName.ShouldBe("[role]");
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string databaseName = "dbName";

            const string origin = "ALTER ROLE [role] ADD MEMBER [member1]";
            const string destination = "ALTER ROLE [role] ADD MEMBER [member1]";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Member });

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement
            const string databaseName = "dbName";

            const string origin = "ALTER ROLE [role] ADD MEMBER [member1]";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Member });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER ROLE [role] ADD MEMBER [member1]
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
            const string destination = "ALTER ROLE [role] ADD MEMBER [member1]";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Member });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER ROLE [role] DROP MEMBER [member1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), DbObjectType.Member, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select Member db object, update schema is created without Member            
            const string databaseName = "dbName";

            const string origin = "ALTER ROLE [role] ADD MEMBER [member1]";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
