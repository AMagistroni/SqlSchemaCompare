using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;

namespace SqlSchemaCompare.Core.Common
{
    public static class ConfigurationFilter
    {
        public static IEnumerable<DbObject> FilterByConfiguration(Configuration configuration, IEnumerable<DbObject> dbObjects)
        {
            return dbObjects
                .Except(dbObjects.Where(x => configuration.DiscardSchemas.Contains(x.Schema)))
                .Except(dbObjects.Where(x => x.ParentName.StartsWithAny(configuration.DiscardSchemas)))
                .Except(dbObjects.Where(x => configuration.DiscardObjects.Contains(x.Identifier)))
                .Except(dbObjects.Where(x => configuration.DiscardObjects.Contains(x.ParentName)))
                .Except(dbObjects.Where(x => configuration.DiscardSchemas.Contains(x.Name)));
        }
    }
}
