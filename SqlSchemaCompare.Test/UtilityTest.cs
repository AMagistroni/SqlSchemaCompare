using SqlSchemaCompare.Core;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlSchemaCompare.Test
{
    public class UtilityTest
    {

        public static (string file1, string file2, string errors) Compare(string originSchema, string destinationSchema, IEnumerable<DbObjectType> dbObjectTypes)
        {
            var schemaBuilder = new TSqlSchemaBuilder();
            var dbObjectFactory = new TSqlObjectFactory();
            IErrorWriter errorWriter = new ErrorWriter();

            var loadSchemaManager = new LoadSchemaManager(dbObjectFactory, errorWriter);
            var (originDbObjects, destinationDbObjects, errors) = loadSchemaManager.LoadSchema(originSchema, destinationSchema);

            var compareSchemaManager = new CompareSchemaManager(schemaBuilder);
            var (file1, file2) = compareSchemaManager.Compare(originDbObjects, destinationDbObjects, dbObjectTypes);

            return (file1, file2, errors);
        }

        public static (string updateFile, string errors) UpdateSchema(string originSchema, string destinationSchema, IEnumerable<DbObjectType> dbObjectTypes)
        {
            IDbObjectFactory dbObjectFactory = new TSqlObjectFactory();
            ISchemaBuilder schemaBuilder = new TSqlSchemaBuilder();
            IErrorWriter errorWriter = new ErrorWriter();

            var loadSchemaManager = new LoadSchemaManager(dbObjectFactory, errorWriter);
            var (originDbObjects, destinationDbObjects, errors) = loadSchemaManager.LoadSchema(originSchema, destinationSchema);

            UpdateSchemaManager updateSchemaManager = new(schemaBuilder, dbObjectFactory, errorWriter);
            string updateSchema= updateSchemaManager.UpdateSchema(originDbObjects, destinationDbObjects, dbObjectTypes);

            return (updateSchema, errors);
        }        
    }
}
