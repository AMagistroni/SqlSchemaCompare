using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public interface IDbObjectFactory
    {
        public (IEnumerable<DbObject> dbObjects, IEnumerable<ErrorParser> errors) CreateObjectsForUpdateOperation(string schema);
        public (IEnumerable<SimpleDbObject> dbObjects, IEnumerable<ErrorParser> errors) CreateObjectsForCompareOperation(string schema);
    }
}
