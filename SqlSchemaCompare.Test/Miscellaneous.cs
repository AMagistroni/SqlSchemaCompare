using Shouldly;
using SqlSchemaCompare.Core.DbStructures;
using Xunit;

namespace SqlSchemaCompare.Test
{
    public class Miscellaneous
    {
        [Fact]
        public void DbObjectEquals_TwoObjectsNull()
        {
            DbObject dbObject1 = null;
            DbObject dbObject2 = null;

            (dbObject1 == dbObject2).ShouldBeTrue();
        }

        [Fact]
        public void DbObjectEquals_OneObjectNull()
        {
            DbObject dbObject1 = new StoreProcedure();
            DbObject dbObject2 = null;

            (dbObject2 != dbObject1).ShouldBeTrue();
            (dbObject1 != dbObject2).ShouldBeTrue();
        }

        [Fact]
        public void DbObjectEquals_TwoDifferentObjectsEquals()
        {
            DbObject dbObject1 = new StoreProcedure();
            DbObject dbObject2 = new StoreProcedure();
            (dbObject1 == dbObject2).ShouldBeTrue();
        }
    }
}
