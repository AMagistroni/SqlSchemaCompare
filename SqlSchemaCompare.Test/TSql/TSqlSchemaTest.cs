﻿using Shouldly;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using SqlSchemaCompare.Test.Builder;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlSchemaTest
    {
        private readonly IList<DbObjectType> SelectedObjects;
        public TSqlSchemaTest()
        {
            SelectedObjects = RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Schema);
        }
        [Fact]
        public void CreateSchema()
        {
            const string schemaSql = "CREATE SCHEMA [sch1]";

            var objectFactory = new TSqlObjectFactory(ConfigurationBuilder.GetConfiguration());
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(schemaSql);
            var dbobject = dbObjects.Single() as Schema;

            dbobject.Name.ShouldBe("[sch1]");
            dbobject.Schema.ShouldBeEmpty();
            dbobject.Identifier.ShouldBe("[sch1]");
            dbobject.Sql.ShouldBe(schemaSql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty

            const string origin = "CREATE SCHEMA [sch1]";
            const string destination = "CREATE SCHEMA [sch1]";

            (string updateSchema, _) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBeEmpty();
        }

        [Fact]
        public void UpdateSchemaCreateDbObject()
        {
            // When present db object in origin absent from destination
            // Expect updateSchema contains create statement

            const string origin = "CREATE SCHEMA [sch1]";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"CREATE SCHEMA [sch1]
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
            const string destination = "CREATE SCHEMA [sch1]";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"DROP SCHEMA [sch1]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), new DbObjectType[] { DbObjectType.Schema }, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select schema db object, update schema is created without schema            

            const string origin = "CREATE SCHEMA [sch1]";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, [dbObjectTypes]);
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
