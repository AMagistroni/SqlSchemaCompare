using Shouldly;
using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.TSql;
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
            var schemaBuilder = new TSqlSchemaBuilder();
            var objectFactory = new TSqlObjectFactory();
            IErrorWriter errorWriter = new ErrorWriter();
            var compareSchemaManager = new CompareSchemaManager(schemaBuilder, objectFactory, errorWriter);
            (var file1, _, var errors) = compareSchemaManager.Compare(sql, "");

            file1.ShouldBe(
@"CREATE TABLE [dbo].[tbl_A]([ID] [int] IDENTITY(0,1) NOT NULL)
GO

CREATE TABLE [dbo].[tbl_Z]([ID] [int] IDENTITY(0,1) NOT NULL)
GO"
);
            errors.ShouldBeEmpty();
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
            var schemaBuilder = new TSqlSchemaBuilder();
            var objectFactory = new TSqlObjectFactory();
            IErrorWriter errorWriter = new ErrorWriter();
            var compareSchemaManager = new CompareSchemaManager(schemaBuilder, objectFactory, errorWriter);
            (var file1, var file2, var errors) = compareSchemaManager.Compare(sql, sql);

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
            var schemaBuilder = new TSqlSchemaBuilder();
            var objectFactory = new TSqlObjectFactory();
            IErrorWriter errorWriter = new ErrorWriter();
            var compareSchemaManager = new CompareSchemaManager(schemaBuilder, objectFactory, errorWriter);
            (var file1, _, var errors) = compareSchemaManager.Compare(sql, "");

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
            var schemaBuilder = new TSqlSchemaBuilder();
            var objectFactory = new TSqlObjectFactory();
            IErrorWriter errorWriter = new ErrorWriter();
            var compareSchemaManager = new CompareSchemaManager(schemaBuilder, objectFactory, errorWriter);
            (var file1, var file2, var errors) = compareSchemaManager.Compare(sql1, sql2);

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
