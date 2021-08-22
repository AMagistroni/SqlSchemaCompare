using SqlSchemaCompare.Core.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSchemaCompare.Core
{
    public class CompareSchemaManager
    {
        private readonly ISchemaBuilder _schemaBuilder;
        private readonly IDbObjectFactory _dbObjectFactory;
        private readonly IErrorWriter _errorWriter;
        public CompareSchemaManager(ISchemaBuilder schemaBuilder, IDbObjectFactory dbObjectFactory, IErrorWriter errorWriter)
        {
            _schemaBuilder = schemaBuilder;
            _dbObjectFactory = dbObjectFactory;
            _errorWriter = errorWriter;
        }

        public (string file1, string file2, string errors) Compare(string schema1, string schema2)
        {
            (var sourceObjects, var errorOriginSchema) = _dbObjectFactory.CreateObjectsForCompareOperation(schema1);
            (var destinationObjects, var errorDestinationSchema) = _dbObjectFactory.CreateObjectsForCompareOperation(schema2);

            var objectsSchemaResult1 = sourceObjects.Select(x => x.Sql).ToList();
            var objectsSchemaResult2 = destinationObjects.Select(x => x.Sql).ToList();

            StringBuilder stringBuilder1 = BuildStringBuilder(objectsSchemaResult1, objectsSchemaResult2);
            StringBuilder stringBuilder2 = BuildStringBuilder(objectsSchemaResult2, objectsSchemaResult1);

            var errors = _errorWriter.GetErrors(errorOriginSchema, errorDestinationSchema);

            return (stringBuilder1.ToString().Trim(), stringBuilder2.ToString().Trim(), errors.ToString());
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
