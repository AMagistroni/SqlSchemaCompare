using Shouldly;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlRoleTest
    {
        private readonly IList<DbObjectType> SelectedObjects;
        public TSqlRoleTest()
        {
            SelectedObjects = RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Role);
        }
        [Fact]
        public void CreateRole()
        {
            const string schemaSql = "CREATE ROLE [role]";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(schemaSql);
            var dbobject = dbObjects.Single() as Role;

            dbobject.Name.ShouldBe("[role]");
            dbobject.Schema.ShouldBeEmpty();
            dbobject.Identifier.ShouldBe("[role]");
            dbobject.Sql.ShouldBe(schemaSql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string origin = "CREATE ROLE [role]";
            const string destination = "CREATE ROLE [role]";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement

            const string origin = "CREATE ROLE [role]";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"CREATE ROLE [role]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaDropDbObject()
        {
            // When present db object in destination absent from origin
            // Expect updateSchema contains drop statement

            const string origin = "";
            const string destination =
@"CREATE ROLE [role]
GO

ALTER ROLE [role] ADD MEMBER [user]
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"DROP ROLE [role]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), new DbObjectType[] { DbObjectType.Role }, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select role db object, update schema is created without role
            const string origin = "CREATE ROLE [role]";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, [dbObjectTypes]);
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
