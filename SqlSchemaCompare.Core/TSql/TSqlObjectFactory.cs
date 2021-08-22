using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;

namespace SqlSchemaCompare.Core.TSql
{
    public enum OperationParser
    {
        Update,
        Compare
    }
    public class TSqlObjectFactory: IDbObjectFactory
    {
        public (IEnumerable<DbObject> dbObjects, IEnumerable<ErrorParser> errors) CreateObjectsForUpdateOperation(string schema)
        {
            var commandTrim = schema.Trim();
            var stream = new AntlrInputStream(commandTrim);
            CaseChangingCharStream upper = new(stream, true);
            ITokenSource lexer = new TSqlLexer(upper);
            var token = new CommonTokenStream(lexer);

            var parser = new TSqlParser(token);
            TSqlParserUpdateListener listener  = new(stream);
            var errorListener = new ErrorListener();
            parser.AddErrorListener(errorListener);
            parser.AddParseListener(listener);
            parser.tsql_file();

            return (listener.DbObjects, errorListener.Errors);
        }

        public (IEnumerable<SimpleDbObject> dbObjects, IEnumerable<ErrorParser> errors) CreateObjectsForCompareOperation(string schema)
        {
            var commandTrim = schema.Trim();
            var stream = new AntlrInputStream(commandTrim);
            CaseChangingCharStream upper = new CaseChangingCharStream(stream, true);
            ITokenSource lexer = new TSqlLexer(upper);
            var token = new CommonTokenStream(lexer);

            var parser = new TSqlParser(token);
            TSqlParserCompareListener listener = new(stream);
            var errorListener = new ErrorListener();
            parser.AddErrorListener(errorListener);
            parser.AddParseListener(listener);
            parser.tsql_file();

            return (listener.SimpleDbObjects, errorListener.Errors);
        }
    }
}
