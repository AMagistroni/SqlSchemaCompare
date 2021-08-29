﻿using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlStoreProcedureTest
    {
        [Fact]
        public void CreateStoreProcedure()
        {
            const string storeSql = @"CREATE PROCEDURE [dbo].[sp1] 
@par1 as bit = 0
AS
BEGIN
	select * from [dbo].[table1]
END";

            var objectFactory = new TSqlObjectFactory();
            (var objects, var error) = objectFactory.CreateObjectsForUpdateOperation(storeSql);
            var dbobject = objects.Single() as StoreProcedure;

            dbobject.Name.ShouldBe("[sp1]");
            dbobject.Schema.ShouldBe("[dbo]");
            dbobject.Identifier.ShouldBe("[dbo].[sp1]");
            dbobject.Sql.ShouldBe(storeSql);
            error.Count().ShouldBe(0);
        }
        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            const string databaseName = "dbName";

            const string origin =
@"CREATE PROCEDURE [dbo].[proc]	
	@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END 
GO";
            const string destination =
@"CREATE PROCEDURE [dbo].[proc]	
	@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END 
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.StoreProcedure });

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
@"CREATE PROCEDURE [dbo].[proc]
@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.StoreProcedure });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

CREATE PROCEDURE [dbo].[proc]
@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END
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
@"CREATE PROCEDURE [dbo].[proc]	
@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END 
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.StoreProcedure });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP PROCEDURE [dbo].[proc]
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
@"CREATE PROCEDURE [dbo].[proc]	
@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END
GO";
            const string destination =
@"CREATE PROCEDURE [dbo].[proc]	
@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL2]
END
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.StoreProcedure });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER PROCEDURE [dbo].[proc]	
@par as bit = 0
AS
BEGIN
	SELECT * from [DBO].[TBL1]
END
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact] 
        public void CreateStoreProcedureWithTableInside()
        {
            // When a store procedure contain a table definition for example tmp table,
            // this table doesn't have to be added to DbObjects

            const string storeSql =
@"CREATE PROCEDURE [dbo].[proc]	
@par as bit = 0
AS
BEGIN
	CREATE TABLE #tmpTbl  (ID int)
END
GO";

            var objectFactory = new TSqlObjectFactory();
            (var objects, var error) = objectFactory.CreateObjectsForUpdateOperation(storeSql);
            objects.Count().ShouldBe(1);

            var dbobject = objects.Single() as StoreProcedure;
            dbobject.Identifier.ShouldBe("[dbo].[proc]");
            error.Count().ShouldBe(0);
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), DbObjectType.StoreProcedure, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select StoreProcedure db object, update schema is created without StoreProcedure
            const string databaseName = "dbName";
            string origin =
$@"USE [{databaseName}]
GO

CREATE PROCEDURE [dbo].[proc]	
@par as bit = 0
AS
BEGIN
	CREATE TABLE #tmpTbl  (ID int)
END
GO";

            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
