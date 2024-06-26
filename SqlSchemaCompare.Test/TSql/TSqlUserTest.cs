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
    public class TSqlUserTest
    {
        private readonly IList<DbObjectType> SelectedObjects;
        public TSqlUserTest()
        {
            SelectedObjects = RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.User);
        }

        [Fact]
        public void CreateUser()
        {
            const string sql = "CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]";

            var objectFactory = new TSqlObjectFactory(ConfigurationBuilder.GetConfiguration());
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var dbobject = dbObjects.Single() as User;

            dbobject.Name.ShouldBe("[user]");
            dbobject.Schema.ShouldBe("[dbo]");
            dbobject.Identifier.ShouldBe("[user]");
            dbobject.Sql.ShouldBe(sql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void CreateUserWithoutSchema()
        {
            const string sql = "CREATE USER [user] FOR LOGIN [login]";

            var objectFactory = new TSqlObjectFactory(ConfigurationBuilder.GetConfiguration());
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(sql);
            var dbobject = dbObjects.Single() as User;

            dbobject.Name.ShouldBe("[user]");
            dbobject.Schema.ShouldBeNull();
            dbobject.Identifier.ShouldBe("[user]");
            dbobject.Sql.ShouldBe(sql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty

            const string origin =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO";
            const string destination =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
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
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
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
            const string destination =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO

ALTER ROLE [role] ADD MEMBER [user]
GO
";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"DROP USER [user]
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
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA = [dbo]
GO";
            const string destination =
@"CREATE USER [user] FOR LOGIN [login] WITH DEFAULT_SCHEMA = [sch1]
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER USER [user] WITH DEFAULT_SCHEMA = [dbo], LOGIN = [user_login]
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), new DbObjectType[] { DbObjectType.User, DbObjectType.Role, DbObjectType.Member }, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select user db object, update schema is created without user

            const string origin =
@"CREATE USER [user] FOR LOGIN [user_login] WITH DEFAULT_SCHEMA=[dbo]
GO";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, [dbObjectTypes]);
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
