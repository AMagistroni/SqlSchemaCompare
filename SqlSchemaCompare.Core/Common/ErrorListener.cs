using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public class ErrorParser
    {
        public string OffendingToken { get; private set; }
        public string Message { get; private set; }
        public int Line { get; private set; }
        public int CharPositionInLine { get; private set; }
        public ErrorParser(string offendingToken, string msg, int line, int charPositionInLine)
        {
            OffendingToken = offendingToken;
            Message = msg;
            Line = line;
            CharPositionInLine = charPositionInLine;
        }
    }
    public class ErrorListener: BaseErrorListener
    {
        public IList<ErrorParser> Errors { get; private set; } = new List<ErrorParser>();
        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e) 
        {
            Errors.Add(new ErrorParser(offendingSymbol.Text, msg, line, charPositionInLine));
        }
    }
}
