using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;

namespace SqlSchemaCompare.Core.Common
{
    public static class RelatedDbObjectsConfiguration
    {
        private static readonly List<List<DbObjectType>> RelatedDbObjects =
        [
            [DbObjectType.Function],
            [DbObjectType.StoreProcedure],
            [DbObjectType.Table, DbObjectType.TableDefaultContraint, DbObjectType.TableForeignKeyContraint, DbObjectType.TablePrimaryKeyContraint, DbObjectType.Column, DbObjectType.Index, DbObjectType.TableSet],
            [DbObjectType.User, DbObjectType.Role, DbObjectType.Member],
            [DbObjectType.View, DbObjectType.Index],
            [DbObjectType.Schema],
            [DbObjectType.Trigger, DbObjectType.EnableTrigger],
            [DbObjectType.Type],
            [DbObjectType.Other],
        ];

        public static List<DbObjectType> GetRelatedDbObjects(DbObjectType dbObject)
        {
            return RelatedDbObjects.Single(x => x.Contains(dbObject));
        }
    }
}
