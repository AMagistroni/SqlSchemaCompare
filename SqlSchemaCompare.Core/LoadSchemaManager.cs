using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;

namespace SqlSchemaCompare.Core
{
    public class LoadSchemaManager(IDbObjectFactory dbObjectFactory, IErrorWriter errorWriter)
    {
        private readonly IDbObjectFactory _dbObjectFactory = dbObjectFactory;
        private readonly IErrorWriter _errorWriter = errorWriter;

        public (IEnumerable<DbObject> originObjects, IEnumerable<DbObject> destinationObjects, string errors) LoadSchema(string origin, string destination)
        {
            (var originObjects, var errorOriginSchema) = _dbObjectFactory.CreateObjectsForUpdateOperation(origin);
            (var destinationObjects, var errorDestinationSchema) = _dbObjectFactory.CreateObjectsForUpdateOperation(destination);

            return (originObjects, destinationObjects, _errorWriter.GetErrors(errorOriginSchema, errorDestinationSchema));
        }
    }
}
