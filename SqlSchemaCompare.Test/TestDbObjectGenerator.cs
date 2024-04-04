using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Collections.Generic;
using Xunit;

namespace SqlSchemaCompare.Test
{
    public static class TestDbObjectGenerator
    {
        public static TheoryData<DbObjectType> ListDbObjectTypeExceptOne(IList<DbObjectType> except)
        {
            TheoryData<DbObjectType> response = [];
            foreach (DbObjectType dbObject in Enum.GetValues(typeof(DbObjectType)))
            {
                if (!except.Contains(dbObject))
                    response.Add(dbObject);
            }
            return response;
        }
    }
}
