using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlViewTest
    {
        [Fact]
        public void CreateView()
        {
            const string body = "SELECT * FROM Db.dbo.Table1";

            var viewSql = $@"CREATE VIEW [dbo].[view]
	                    AS
	                    {body}";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(viewSql);
            var dbobject = dbObjects.Single() as View;

            dbobject.Name.ShouldBe("[view]");
            dbobject.Schema.ShouldBe("[dbo]");
            dbobject.Identifier.ShouldBe("[dbo].[view]");
            dbobject.Body.ShouldBe(body);
            dbobject.Sql.ShouldBe(viewSql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty

            const string origin =
@"CREATE VIEW [dbo].[vw1]
AS
SELECT * FROM [dbo].[tbl1]
GO";
            const string destination =
@"CREATE VIEW [dbo].[vw1]
AS
SELECT * FROM [dbo].[tbl1]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { DbObjectType.View });

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement

            const string origin =
@"CREATE VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl1]
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { DbObjectType.View });

            updateSchema.ShouldBe(
@"CREATE VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl1]
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
@"CREATE VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl1]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { DbObjectType.View });

            updateSchema.ShouldBe(
@"DROP VIEW [dbo].[vw1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaAlterDbObject()
        {
            // When present db object in destination and in origin and are different
            // Expect updateSchema contains alter statement
        
            const string origin =
@"CREATE VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl1]
GO";
            const string destination =
@"CREATE VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl2]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { DbObjectType.View });

            updateSchema.ShouldBe(
@"ALTER VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), DbObjectType.View, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select view db object, update schema is created without view

            const string origin =
@"CREATE VIEW [dbo].[vw1]
AS
    SELECT * FROM [dbo].[tbl1]
GO";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
