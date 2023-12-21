using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSchemaCompare.Core
{
    public class CompareSchemaManager
    {
        private readonly ISchemaBuilder _schemaBuilder;
        private readonly Configuration _configuration;
        public CompareSchemaManager(Configuration configuration, ISchemaBuilder schemaBuilder)
        {
            _schemaBuilder = schemaBuilder;
            _configuration = configuration;
        }

        public (string file1, string file2) Compare(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, IEnumerable<DbObjectType> selectedObjectType)
        {
            var sourceObjectsFilteredSelected = sourceObjects.Where(x => selectedObjectType.Contains(x.DbObjectType));
            var destinationObjectsFilteredSelected = destinationObjects.Where(x => selectedObjectType.Contains(x.DbObjectType));

            var sourceFilteredByConfiguration = FilterByConfiguration(sourceObjectsFilteredSelected);
            var destinationFilteredByConfiguration = FilterByConfiguration(destinationObjectsFilteredSelected);

            var objectsSchemaResult1 = sourceFilteredByConfiguration.Select(x => x.Sql).ToList();
            var objectsSchemaResult2 = destinationFilteredByConfiguration.Select(x => x.Sql).ToList();

            StringBuilder stringBuilder1 = BuildStringBuilder(objectsSchemaResult1, objectsSchemaResult2);
            StringBuilder stringBuilder2 = BuildStringBuilder(objectsSchemaResult2, objectsSchemaResult1);

            return (stringBuilder1.ToString().Trim(), stringBuilder2.ToString().Trim());
        }

        private IEnumerable<DbObject> FilterByConfiguration(IEnumerable<DbObject> dbObjects)
        {
            return dbObjects
                .Except(dbObjects.Where(x => _configuration.DiscardSchemas.Contains(x.Schema)))
                .Except(dbObjects.Where(x => x.ParentName.StartsWithAny(_configuration.DiscardSchemas)))
                .Except(dbObjects.Where(x => _configuration.DiscardObjects.Contains(x.Identifier)))
                .Except(dbObjects.Where(x => _configuration.DiscardObjects.Contains(x.ParentName)))
                .Except(dbObjects.Where(x => _configuration.DiscardSchemas.Contains(x.Name)));
        }

        private StringBuilder BuildStringBuilder(List<string> objectsSchema1, List<string> objectsSchema2)
        {
            StringBuilder stringBuilder = new();

            objectsSchema1 = objectsSchema1
                .Except(objectsSchema2)
                .ToList();

            objectsSchema1
                .ForEach(x => x = x.Trim());

            objectsSchema1
                .OrderBy(x => x)
                .ToList()
                .ForEach(x => stringBuilder
                                .AppendLine(x)
                                .AppendLine(_schemaBuilder.BuildSeparator()));

            return stringBuilder;
        }
    }
}
