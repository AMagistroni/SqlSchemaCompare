using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSchemaCompare.Core
{
    public class CompareSchemaManager(Configuration configuration, ISchemaBuilder schemaBuilder)
    {
        public (string file1, string file2) Compare(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, IEnumerable<DbObjectType> selectedObjectType)
        {
            var sourceObjectsFilteredSelected = sourceObjects.Where(x => selectedObjectType.Contains(x.DbObjectType));
            var destinationObjectsFilteredSelected = destinationObjects.Where(x => selectedObjectType.Contains(x.DbObjectType));

            var sourceFilteredByConfiguration = ConfigurationFilter.FilterByConfiguration(configuration, sourceObjectsFilteredSelected);
            var destinationFilteredByConfiguration = ConfigurationFilter.FilterByConfiguration(configuration, destinationObjectsFilteredSelected);

            var objectsSchemaResult1 = sourceFilteredByConfiguration.Select(x => x.Sql).ToList();
            var objectsSchemaResult2 = destinationFilteredByConfiguration.Select(x => x.Sql).ToList();

            StringBuilder stringBuilder1 = BuildStringBuilder(objectsSchemaResult1, objectsSchemaResult2);
            StringBuilder stringBuilder2 = BuildStringBuilder(objectsSchemaResult2, objectsSchemaResult1);

            return (stringBuilder1.ToString().Trim(), stringBuilder2.ToString().Trim());
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
                                .AppendLine(schemaBuilder.BuildSeparator()));

            return stringBuilder;
        }
    }
}
