using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using Xunit;

namespace SqlSchemaCompare.Test.TSql
{
    public class ErrorWriterTest
    {
        [Fact]
        public void OrderStatement()
        {
            string sql =
@"CREATE TABLE [dbo].[tbl_Z]([ID] [int] IDENTITY(0,1) NOT NU)
GO";
            var (file1, file2, errors) = UtilityTest.Compare(sql, sql, new DbObjectType[] { DbObjectType.Table });

            errors.ShouldBe(
@"**************** ORIGIN **************** 
----------------------------------------
Offending token: NU
Line: 1, CharPosition: 56
no viable alternative at input 'NOTNU'
**************** DESTINATION **************** 
----------------------------------------
Offending token: NU
Line: 1, CharPosition: 56
no viable alternative at input 'NOTNU'
");            
        }
    }
}
