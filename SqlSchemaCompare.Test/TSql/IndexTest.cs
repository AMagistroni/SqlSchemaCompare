using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class IndexTest
    {
        [Fact]
        public void CreateIndex()
        {
            const string sql = 
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]";

            var objectFactory = new TSqlObjectFactory();
            (var objects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var dbobject = objects.Single() as Index;

            dbobject.Name.ShouldBe("[indexName]");
            dbobject.Schema.ShouldBeEmpty();
            dbobject.Identifier.ShouldBe("[indexName]");
            dbobject.Sql.ShouldBe(sql);
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
            const string databaseName = "dbName";

            const string origin =
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            const string destination =
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Index });

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
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Index });

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
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
            const string databaseName = "dbName";

            const string origin = "";
            const string destination =
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Index });

            updateSchema.ShouldBe(
    $@"USE [{databaseName}]
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
            const string databaseName = "dbName";

            const string origin =
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
    [ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            const string destination =
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC,
    [par]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { DbObjectType.Index });

            updateSchema.ShouldBe(
    $@"USE [{databaseName}]
GO

DROP INDEX [indexName] ON [dbo].[table]
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
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), DbObjectType.Index, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select Index db object, update schema is created without Index
            const string databaseName = "dbName";

            const string origin =
@"CREATE NONCLUSTERED INDEX [indexName] ON [dbo].[table]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [indexTable]
GO";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, databaseName, new DbObjectType[] { dbObjectTypes });
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}