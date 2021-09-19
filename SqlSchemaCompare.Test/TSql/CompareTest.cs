using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class CompareTest
    {
        [Fact]
        public void OrderStatement()
        {
            string sql =
@"CREATE TABLE [dbo].[tbl_Z]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl_A]([ID] [int] IDENTITY(0,1) NOT NULL)
GO
";
            var (file1, file2, errors) = UtilityTest.Compare(sql, "", new DbObjectType[] { DbObjectType.Table});

            file1.ShouldBe(
@"CREATE TABLE [dbo].[tbl_A]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl_Z]([ID] [int] IDENTITY(0,1) NOT NULL)
GO"
);
            errors.ShouldBeEmpty();
            file2.ShouldBeEmpty();
        }

        [Fact]
        public void CompareEqualFile()
        {
            string sql =
@"CREATE TABLE [dbo].[tbl_A]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl_Z]([ID] [int] IDENTITY(0,1) NOT NULL)
GO
";
            var (file1, file2, errors) = UtilityTest.Compare(sql, sql, new DbObjectType[] { DbObjectType.Table });

            file1.ShouldBeEmpty();
            file2.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void DiscardStatementNotUsefull()
        {
            string sql =
@"
/****** Object:  UserDefinedFunction [dbo].[fntPDRGetDateFornitura]    Script Date: 12/08/2021 00:14:12 ******/
SET ANSI_NULLS ON
GO
";

            //SET QUOTED_IDENTIFIER ON
            var (file1, _, errors) = UtilityTest.Compare(sql, "", new DbObjectType[] { DbObjectType.Table });

            file1.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void SchemaAreDifferent()
        {
            string sql1 =
@"CREATE TABLE [dbo].[only_file_1]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl]([ID] [int] IDENTITY(0,1) NOT NULL)
GO
";

            string sql2 =
@"CREATE TABLE [dbo].[tbl]([ID] [int] IDENTITY(0,1) NULL)
GO

CREATE TABLE [dbo].[only_file_2]([ID] [int] IDENTITY(0,1) NOT NULL)
GO
";
            var (file1, file2, errors) = UtilityTest.Compare(sql1, sql2, new DbObjectType[] { DbObjectType.Table });

            file1.ShouldBe(
@"CREATE TABLE [dbo].[only_file_1]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl]([ID] [int] IDENTITY(0,1) NOT NULL)
GO");
            file2.ShouldBe(
@"CREATE TABLE [dbo].[only_file_2]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl]([ID] [int] IDENTITY(0,1) NULL)
GO");
            errors.ShouldBeEmpty();
        }
    }
}
