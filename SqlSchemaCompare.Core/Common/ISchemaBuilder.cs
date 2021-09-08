using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;

namespace SqlSchemaCompare.Core.Common
{
    public interface ISchemaBuilder
    {
        public string BuildUse(string databaseName);
        public string Build(DbObject dbObject, Operation operation, ResultProcessDbObject objectToElaborate);
        public string BuildSeparator();
        public string GetStartCommentInLine();
    }
}
