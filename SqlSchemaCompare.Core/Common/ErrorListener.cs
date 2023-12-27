using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public class ErrorParser(string offendingToken, string msg, int line, int charPositionInLine)
    {
        public string OffendingToken { get; } = offendingToken;
        public string Message { get; } = msg;
        public int Line { get; } = line;
        public int CharPositionInLine { get; } = charPositionInLine;
    }
    public class ErrorListener: BaseErrorListener
    {
        public IList<ErrorParser> Errors { get; } = new List<ErrorParser>();
        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            Errors.Add(new ErrorParser(offendingSymbol.Text, msg, line, charPositionInLine));
        }
    }
}
