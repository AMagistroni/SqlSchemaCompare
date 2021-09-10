using Shouldly;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlTableTest
    {
        private IList<DbObjectType> SelectedObjects;
        public TSqlTableTest()
        {
            RelatedDbObjectsConfiguration relatedDbObjectsConfiguration = new();
            SelectedObjects = relatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Table);
        }

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

            table.Constraints.Single().Sql.ShouldBe(constraint);
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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { DbObjectType.Table });

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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { DbObjectType.Table, DbObjectType.Schema });

            updateSchema.ShouldBe(
@"DROP TABLE [schema].[table]
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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [dbo].[TBL] ADD [columnToAdd] [nvarchar](20) NOT NULL
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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [dbo].[TBL] DROP CONSTRAINT [FK_Name1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void AlterTableAlterColumnDropConstraint()
        {
            // When present db object in destination and in origin and are different
            // Expect updateSchema contains alter statement     

            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [nvarchar](20) NULL)
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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [dbo].[TBL] DROP CONSTRAINT [FK_Name1]
GO

ALTER TABLE [dbo].[TBL] ALTER COLUMN [column1] [nvarchar](20) NULL
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

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [dbo].[TBL] DROP CONSTRAINT [constraintName]
GO

ALTER TABLE [dbo].[TBL] ADD  CONSTRAINT [constraintName]  DEFAULT ((0)) FOR [column1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void DropConstraintBeforeDropColumn()
        {
            const string origin =
@"CREATE TABLE [dbo].[TBL] ([ID] [int] NOT NULL)
GO

";

            const string destination =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] NOT NULL,
	[column1] [int] NOT NULL)
GO

ALTER TABLE [dbo].[TBL] WITH CHECK ADD CONSTRAINT [FK_constraint] FOREIGN KEY([column1])
REFERENCES [dbo].[tbl2] ([ID])
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [dbo].[TBL] DROP CONSTRAINT [FK_constraint]
GO

ALTER TABLE [dbo].[TBL] DROP COLUMN [column1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void DropConstraintBeforeAlterColumn()
        {
            const string origin =
@"CREATE TABLE [dbo].[tbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [int] NOT NULL)
GO

CREATE TABLE [dbo].[tblLookup](
	[ID] [int] NOT NULL,
	[description] [nvarchar](50) NOT NULL,
        CONSTRAINT [PK] PRIMARY KEY CLUSTERED 
        (
	        [ID] ASC, [description] DESC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [db]
) ON [db]
GO

ALTER TABLE [dbo].[tbl] WITH CHECK ADD CONSTRAINT [FK_constraint] FOREIGN KEY([column1])
REFERENCES [dbo].[tblLookup] ([ID])


";

            const string destination =
@"CREATE TABLE [dbo].[tbl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[column1] [tinyint] NOT NULL)
GO

CREATE TABLE [dbo].[tblLookup](
	[ID] [tinyint] NOT NULL,
	[description] [nvarchar](50) NOT NULL,
        CONSTRAINT [PK] PRIMARY KEY CLUSTERED 
        (
	        [ID] ASC, [description] DESC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [db]
) ON [db]
GO

ALTER TABLE [dbo].[tbl] WITH CHECK ADD CONSTRAINT [FK_constraint] FOREIGN KEY([column1])
REFERENCES [dbo].[tblLookup] ([ID])
GO

";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [dbo].[tbl] DROP CONSTRAINT [FK_constraint]
GO

ALTER TABLE [dbo].[tblLookup] DROP CONSTRAINT [PK]
GO

ALTER TABLE [dbo].[tbl] ALTER COLUMN [column1] [int] NOT NULL
GO

ALTER TABLE [dbo].[tblLookup] ALTER COLUMN [ID] [int] NOT NULL
GO

ALTER TABLE [dbo].[tbl] WITH CHECK ADD CONSTRAINT [FK_constraint] FOREIGN KEY([column1])
REFERENCES [dbo].[tblLookup] ([ID])
GO

ALTER TABLE ADD CONSTRAINT [PK] PRIMARY KEY CLUSTERED 
        (
	        [ID] ASC, [description] DESC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [db]
GO

");
            errors.ShouldBeEmpty();
        }


        [Fact]
        public void AddColumnAndAddDefaultConstraintOnSameStatement()
        {
            const string origin =
@"CREATE TABLE [schema].[tbl] (col1 INT not null, [col2] [datetime] NOT NULL)
GO

ALTER TABLE [schema].[tbl] ADD CONSTRAINT [constraint] DEFAULT (getdate()) FOR [col2]
GO

";

            const string destination =
@"CREATE TABLE [schema].[tbl] (col1 INT not null)
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER TABLE [schema].[tbl] ADD [col2] [datetime] NOT NULL CONSTRAINT [constraint] DEFAULT (getdate())
GO

");
            errors.ShouldBeEmpty();
        }


        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), new DbObjectType[] { DbObjectType.Table, DbObjectType.Column, DbObjectType.TableContraint }, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select table db object, update schema is created without table
            const string origin =
@"CREATE TABLE [dbo].[TBL] (
	[ID] [int] NOT NULL,
	[column1] [int] NOT NULL)
GO

ALTER TABLE [dbo].[TBL] ADD CONSTRAINT [constraintName]  DEFAULT ((0)) FOR [column1]
GO
";

            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
