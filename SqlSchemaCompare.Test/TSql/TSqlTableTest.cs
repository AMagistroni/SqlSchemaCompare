using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlTableTest
    {
        private const string databaseName = "dbName";
        [Fact]
        public void CreateTable()
        {
            const string constraint = @"CONSTRAINT [PK_TableName_Id] PRIMARY KEY CLUSTERED 
						(
							[Id] ASC
						) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [db1]";

			var sqlTable = $@"CREATE TABLE [schema].[TableName](
							[Id] [int] IDENTITY(1,1) NOT NULL,
							[col1] [char](8) NULL,
						{constraint}
						) ON [db1]";

			var sql = $@"{sqlTable}
						GO";

			var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
			var table = dbObjects.Single() as Table;

			table.Name.ShouldBe("[TableName]");
			table.Schema.ShouldBe("[schema]");
			table.Sql.ShouldBe(sqlTable);

			var column1 = table.Columns.ElementAt(0);
			column1.Name.ShouldBe("[Id]");
			column1.Sql.ShouldBe("[Id] [int] IDENTITY(1,1) NOT NULL");

			var column2 = table.Columns.ElementAt(1);
			column2.Name.ShouldBe("[col1]");
			column2.Sql.ShouldBe("[col1] [char](8) NULL");

			table.Constraint.ShouldBe(constraint);
            errors.Count().ShouldBe(0);
		}
        [Fact]
        public void EqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty
            
            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
    [Description1] [varchar](max) NULL,
    [Description2] varchar(max) NULL,
	[column1] [Date] NOT NULL)
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name1] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name1]
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name2] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name2]
GO
";
            const string destination =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
    [Description1] [varchar](max) NULL,
    [Description2] varchar(max) NULL,
	[column1] [Date] NOT NULL)
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name1] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name1]
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name2] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name2]
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Table });

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void CreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement

            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [Date] NOT NULL)
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name1] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name1]
GO
";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Table });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [Date] NOT NULL)
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name1] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void DropDbObject()
        {
            // When present db object in destination absent from origin
            // Expect updateSchema contains drop statement

            const string origin = "";
            const string destination =
@"
CREATE SCHEMA [schema]
GO

CREATE TABLE [schema].[table] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [Date] NOT NULL)
GO

ALTER TABLE [schema].[table]  WITH NOCHECK ADD  CONSTRAINT [FK_Name1] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

CREATE NONCLUSTERED INDEX [indexName] ON [schema].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO

ALTER TABLE [schema].[table] CHECK CONSTRAINT [FK_Name1]
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Table, DbObjectType.Schema });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP TABLE [schema].[table]
GO

DROP SCHEMA [schema]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void AlterDbObject()
        {
            // When present db object in destination and in origin and are different
            // Expect updateSchema contains alter statement

            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[columnToAdd] [nvarchar](20) NOT NULL,
    [columnToAlter] [nvarchar](20) NOT NULL)
GO";
            const string destination =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[columnToDrop] [nvarchar](20) NOT NULL,
    [columnToAlter] [nvarchar](20) NULL)
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Table, DbObjectType.Column });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER TABLE [dbo].[TBL] ADD COLUMN [columnToAdd] [nvarchar](20) NOT NULL
GO

ALTER TABLE [dbo].[TBL] DROP COLUMN [columnToDrop]
GO

ALTER TABLE [dbo].[TBL] ALTER COLUMN [columnToAlter] [nvarchar](20) NOT NULL
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void AlterTableDropConstraint()
        {
            // When present db object in destination and in origin and are different
            // Expect updateSchema contains alter statement     

            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [nvarchar](20) NOT NULL)
GO";
            const string destination =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [nvarchar](20) NOT NULL)
GO

ALTER TABLE [dbo].[TBL]  WITH NOCHECK ADD  CONSTRAINT [FK_Name1] FOREIGN KEY([column1])
    REFERENCES [dbo].[TBL2] ([ID])
    ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TBL] CHECK CONSTRAINT [FK_Name1]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Table });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER TABLE [dbo].[TBL] DROP CONSTRAINT [FK_Name1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void AlterTableWithTableConstraint()
        {
            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] NOT NULL,
	[column1] [Date] NOT NULL)
GO

ALTER TABLE [dbo].[TBL] ADD  CONSTRAINT [constraintName]  DEFAULT (getdate()) FOR [column1]
GO
";

            var objectFactory = new TSqlObjectFactory();
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(origin);
            var table = dbObjects.Single() as Table;

            table.Identifier.ShouldBe("[dbo].[TBL]");

            var constraint = table.Constraints.Single();
            constraint.Name.ShouldBe("[constraintName]");
            constraint.ParentName.ShouldBe("[dbo].[TBL]");
            constraint.Schema.ShouldBeNull();
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void DifferentConstraintValueForSameConstraint()
        {
            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] NOT NULL,
	[column1] [int] NOT NULL)
GO

ALTER TABLE [dbo].[TBL] ADD  CONSTRAINT [constraintName]  DEFAULT ((0)) FOR [column1]
GO
";

            const string destination =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] NOT NULL,
	[column1] [int] NOT NULL)
GO

ALTER TABLE [dbo].[TBL] ADD  CONSTRAINT [constraintName]  DEFAULT ((1)) FOR [column1]
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Table });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER TABLE [dbo].[TBL] DROP CONSTRAINT [constraintName]
GO

ALTER TABLE [dbo].[TBL] ADD  CONSTRAINT [constraintName]  DEFAULT ((0)) FOR [column1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), DbObjectType.Table, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select table db object, update schema is created without table
            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] NOT NULL,
	[column1] [int] NOT NULL)
GO

ALTER TABLE [dbo].[TBL] ADD  CONSTRAINT [constraintName]  DEFAULT ((0)) FOR [column1]
GO
";

            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
