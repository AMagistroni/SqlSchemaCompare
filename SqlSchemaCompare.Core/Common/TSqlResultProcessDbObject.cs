using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Linq;

namespace SqlSchemaCompare.Core.Common
{
    public class ResultProcessDbObject
    {
        public class OperationOnDbObject
        {
            public DbObject DbObject { get; init; }
            public Operation Operation { get; init; }
            public string Parameter { get; init; }
        }
        public List<OperationOnDbObject> OperationsOnDbObject { get; } = [];
        public void AddOperation<T>(DbObject dbObjects, Operation operation, string parameter = null) where T : DbObject
        {
            OperationsOnDbObject.Add(new OperationOnDbObject { DbObject = dbObjects, Operation = operation, Parameter = parameter } );
        }
        public void AddOperation<T>(IList<T> dbObjects, Operation operation) where T : DbObject
        {
            dbObjects.ToList().ForEach(x => AddOperation<T>(x, operation));
        }

        public IEnumerable<DbObject> GetDbObject(DbObjectType dbObjectType, Operation operation)
        {
            return OperationsOnDbObject.Where(x => x.DbObject.DbObjectType == dbObjectType && x.Operation == operation).Select(x => x.DbObject);
        }
    }
}