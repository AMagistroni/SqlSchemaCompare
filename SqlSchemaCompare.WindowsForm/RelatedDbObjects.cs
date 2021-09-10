using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;

namespace SqlSchemaCompare.WindowsForm
{
    public class RelatedDbObjectsConfiguration
    {
        public static List<List<DbObjectType>> RelatedDbObjects = new List<List<DbObjectType>>
        {
            new List<DbObjectType> { DbObjectType.Function },
            new List<DbObjectType> { DbObjectType.StoreProcedure },
            new List<DbObjectType> { DbObjectType.Table, DbObjectType.TableContraint, DbObjectType.Column },
            new List<DbObjectType> { DbObjectType.User, DbObjectType.Role, DbObjectType.Member },
            new List<DbObjectType> { DbObjectType.View },
            new List<DbObjectType> { DbObjectType.Schema },
            new List<DbObjectType> { DbObjectType.Trigger, DbObjectType.EnableTrigger },
            new List<DbObjectType> { DbObjectType.Type },
            new List<DbObjectType> { DbObjectType.Other},
            new List<DbObjectType> { DbObjectType.Index }
        };

        public List<DbObjectType> GetRelatedDbObjects(DbObjectType dbObject)
        {
            return RelatedDbObjects.Single(x => x.Contains(dbObject));
        }
    }
}
