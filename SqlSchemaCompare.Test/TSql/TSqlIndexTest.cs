using Shouldly;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlIndexTest
    {
        private IList<DbObjectType> SelectedObjects;
        public TSqlIndexTest()
        {
            RelatedDbObjectsConfiguration relatedDbObjectsConfiguration = new();
            SelectedObjects = relatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Index);
        }
        [Fact]
        public void CreateIndex()
        {
            const string sql =
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL, [col1] int )
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
) INCLUDE ([col1]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            var objectFactory = new TSqlObjectFactory();
            (var objects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var table = objects.First() as Table;

            table.Indexes.Single().Name.ShouldBe("[indexName]");
            table.Indexes.Single().Schema.ShouldBeEmpty();
            table.Indexes.Single().Identifier.ShouldBe("[indexName]");
            table.Indexes.Single().ColumnNames.ShouldContain("[ID]");
            table.Indexes.Single().ColumnNames.ShouldContain("[col1]");
            table.Indexes.Single().ColumnNames.Count().ShouldBe(2);
            table.Indexes.Single().Sql.ShouldBe(
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
) INCLUDE ([col1]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]");
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void CreateClusteredIndex()
        {
            const string sql ="CREATE CLUSTERED COLUMNSTORE INDEX [indexName] ON [dbo].[table] WITH (DROP_EXISTING = OFF) ON [PRIMARY]";

            var objectFactory = new TSqlObjectFactory();
            (var objects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);

            objects.Single().DbObjectType.ShouldBe(DbObjectType.Other);
            objects.Single().Sql.ShouldBe(sql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty

            const string origin =
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            const string destination =
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement

            const string origin =
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO

");
            errors.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaDropDbObject()
        {
            // When present db object in destination absent from origin
            // Expect updateSchema contains drop statement

            const string origin = "CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)";
            const string destination =
@"CREATE DATABASE [dbName]
GO

CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"USE [dbName]
GO

DROP INDEX [indexName] ON [dbo].[table]
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
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            const string destination =
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC,
    [par]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"DROP INDEX [indexName] ON [dbo].[table]
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), new DbObjectType[] { DbObjectType.Index, DbObjectType.Table }, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select Index db object, update schema is created without Index

            const string origin =
@"CREATE TABLE [dbo].[table] ([ID] [INT] NOT NULL)
GO

CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}