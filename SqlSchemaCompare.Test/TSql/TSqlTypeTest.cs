using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlTypeTest
    {
        [Fact]
        public void CreateType()
        {
            var sqlTable = @"CREATE TYPE [schema].[type1] AS TABLE (
							[Id] [int] IDENTITY(1,1) NOT NULL,
							[col1] [char](8) NULL)";

            var sql = $@"{sqlTable}
						GO";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var typeDbObject = dbObjects.Single() as TypeDbObject;

            typeDbObject.Name.ShouldBe("[type1]");
            typeDbObject.Schema.ShouldBe("[schema]");
            typeDbObject.Sql.ShouldBe(sqlTable);
            errors.Count().ShouldBe(0);
        }
        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string databaseName = "dbName";

            const string origin =
    @"CREATE TYPE [schema].[type1] AS TABLE (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [nvarchar](20) NOT NULL)
GO";
            const string destination =
    @"CREATE TYPE [schema].[type1] AS TABLE (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [nvarchar](20) NOT NULL)
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Type });

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
@"CREATE TYPE [schema].[type1] AS TABLE (
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [column1] [nvarchar](20) NOT NULL)
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Type });

            updateSchema.ShouldBe(
    $@"USE [{databaseName}]
GO

CREATE TYPE [schema].[type1] AS TABLE (
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [column1] [nvarchar](20) NOT NULL)
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
@"CREATE TYPE [schema].[type1] AS TABLE (
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [column1] [nvarchar](20) NOT NULL)
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Type });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP TYPE [schema].[type1]
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
@"CREATE TYPE [dbo].[TBL] AS TABLE (
    [ID] [int] NOT NULL,
    [columnDifferent] [nvarchar](20) NOT NULL)
GO";
            const string destination =
@"CREATE TYPE [dbo].[TBL] AS TABLE (
    [ID] [int] NOT NULL,
    [columnDifferent] [nvarchar](20) NULL)
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Type });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP TYPE [dbo].[TBL]
GO

CREATE TYPE [dbo].[TBL] AS TABLE (
    [ID] [int] NOT NULL,
    [columnDifferent] [nvarchar](20) NOT NULL)
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), DbObjectType.Type, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select type db object, update schema is created without type
            const string databaseName = "dbName";

            const string origin =
@"CREATE TYPE [dbo].[TBL] AS TABLE (
    [ID] [int] NOT NULL,
    [columnDifferent] [nvarchar](20) NOT NULL)
GO";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}