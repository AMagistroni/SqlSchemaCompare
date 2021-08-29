using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Collections.Generic;

namespace SqlSchemaCompare.Test
{
    public class TestDbObjectGenerator
    {
        public static IEnumerable<object[]> ListDbObjectTypeExceptOne(DbObjectType except)
        {
            foreach (DbObjectType dbObject in Enum.GetValues(typeof(DbObjectType)))
            {
                if (dbObject != except)
                    yield return new object[] { dbObject };
            }
        }
    }
}
