using Shouldly;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class CreateDatabaseTest
    {
        private readonly IList<DbObjectType> SelectedObjects;
        public CreateDatabaseTest()
        {
            RelatedDbObjectsConfiguration relatedDbObjectsConfiguration = new();
            SelectedObjects = relatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.StoreProcedure);
        }

        [Fact]
        public void DatabaseEquals_UpdateSchemaIsEmpty()
        {
            const string origin =
@"CREATE DATABASE [db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db', FILENAME = N'C:\Data\DB.mdf' , SIZE = 1902592KB , MAXSIZE = UNLIMITED, FILEGROWTH = 524288KB )
 LOG ON 
( NAME = N'db_log', FILENAME = N'C:\Data\db_log.ldf' , SIZE = 2098432KB , MAXSIZE = 2048GB , FILEGROWTH = 524288KB )
GO

CREATE PROCEDURE [dbo].[proc]	
    @par as bit = 0
AS
BEGIN
    SELECT * from [DBO].[TBL1]
END 
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, origin, SelectedObjects);

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void DatabaseDifferent_UpdateContainsUseStatement()
        {
            const string origin =
@"CREATE DATABASE [db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db', FILENAME = N'C:\Data\DB.mdf' , SIZE = 1902592KB , MAXSIZE = UNLIMITED, FILEGROWTH = 524288KB )
 LOG ON 
( NAME = N'db_log', FILENAME = N'C:\Data\db_log.ldf' , SIZE = 2098432KB , MAXSIZE = 2048GB , FILEGROWTH = 524288KB )
GO

CREATE PROCEDURE [dbo].[proc]	
    @par as bit = 0
AS
BEGIN
    SELECT * from [DBO].[TBL]
END 
GO";
            const string destination =
@"CREATE DATABASE [db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db', FILENAME = N'C:\Data\DB.mdf' , SIZE = 1902592KB , MAXSIZE = UNLIMITED, FILEGROWTH = 524288KB )
 LOG ON 
( NAME = N'db_log', FILENAME = N'C:\Data\db_log.ldf' , SIZE = 2098432KB , MAXSIZE = 2048GB , FILEGROWTH = 524288KB )
GO

CREATE PROCEDURE [dbo].[proc]	
    @par as bit = 0
AS
BEGIN
    SELECT * from [DBO].[TBL1]
END 
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"USE [db]
GO

ALTER PROCEDURE [dbo].[proc]	
    @par as bit = 0
AS
BEGIN
    SELECT * from [DBO].[TBL]
END
GO

");
            errors.ShouldBeEmpty();
        }
    }
}
