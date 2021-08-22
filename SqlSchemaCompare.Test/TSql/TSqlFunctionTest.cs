using Shouldly;
using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Linq;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class TSqlFunctionTest
    {
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

            var objectFactory = new TSqlObjectFactory();
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
            const string databaseName = "dbName";

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

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

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
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
END
GO";
            const string destination = "";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

CREATE FUNCTION [dbo].[func1] (@par int)
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
            const string databaseName = "dbName";

            const string origin = "";
            const string destination =
@"CREATE FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN	
    RETURN 3
END
GO";

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

DROP FUNCTION [dbo].[func1]
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

            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();
            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            (string updateSchema, string errors) = updateSchemaManager.UpdateSchema(origin, destination, databaseName);

            updateSchema.ShouldBe(
$@"USE [{databaseName}]
GO

ALTER FUNCTION [dbo].[func1] (@par int)
RETURNS INT
AS
BEGIN
    RETURN 3
END
GO

");
            errors.ShouldBeEmpty();
        }
    }
}
