using Shouldly;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using SqlSchemaCompare.Test.Builder;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlFunctionTest
    {
        private readonly IList<DbObjectType> SelectedObjects;
        public TSqlFunctionTest()
        {
            SelectedObjects = RelatedDbObjectsConfiguration.GetRelatedDbObjects(DbObjectType.Function);
        }
        [Fact]
        public void CreateFunction()
        {
            const string functionSql = @"CREATE FUNCTION [dbo].[fn1] (@string VARCHAR(20), @pattern VARCHAR(150))
                RETURNS VARCHAR(300)
                AS
                BEGIN
                    DECLARE @str VARCHAR(300) = @string;                    
                    RETURN @str
                END";

            var objectFactory = new TSqlObjectFactory(ConfigurationBuilder.GetConfiguration());
            (var dbObjects, var errors) = objectFactory.CreateObjectsForUpdateOperation(functionSql);
            var dbobject = dbObjects.Single() as Function;

            dbobject.Name.ShouldBe("[fn1]");
            dbobject.Schema.ShouldBe("[dbo]");
            dbobject.Identifier.ShouldBe("[dbo].[fn1]");
            dbobject.Sql.ShouldBe(functionSql);
            errors.Count().ShouldBe(0);
        }

        [Fact]
        public void UpdateSchemaEqualsDbObject()
        {
            // When origin equals destination 
            // Expect updateSchema should be empty

            const string origin =
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
END
GO";
            const string destination =
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
END
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
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
END
GO";
            const string destination = "";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
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

            const string origin = "";
            const string destination =
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
END
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"DROP FUNCTION [dbo].[func1]
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
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN
    RETURN 3
END
GO";
            const string destination =
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN
    RETURN 10
END
GO";

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, SelectedObjects);

            updateSchema.ShouldBe(
@"ALTER FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN
    RETURN 3
END
GO

");
            errors.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(TestDbObjectGenerator.ListDbObjectTypeExceptOne), new DbObjectType[] { DbObjectType.Function }, MemberType = typeof(TestDbObjectGenerator))]
        public void UpdateSchemaNotSelectedDbObject(DbObjectType dbObjectTypes)
        {
            // When user not select Function db object, update schema is created without Function
            const string origin =
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN
    RETURN 3
END
GO";
            string destination = string.Empty;

            (string updateSchema, string errors) = UtilityTest.UpdateSchema(origin, destination, [dbObjectTypes]);
            updateSchema.ShouldBeEmpty();
            errors.ShouldBeEmpty();
        }
    }
}
