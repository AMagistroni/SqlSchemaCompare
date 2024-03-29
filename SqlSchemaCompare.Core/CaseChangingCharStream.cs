﻿using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace SqlSchemaCompare.Core
{
    /// <summary>
    /// Constructs a new CaseChangingCharStream wrapping the given <paramref name="stream"/> forcing
    /// all characters to upper case or lower case.
    /// </summary>
    /// <param name="stream">The stream to wrap.</param>
    /// <param name="upper">If true force each symbol to upper case, otherwise force to lower.</param>
    public class CaseChangingCharStream(ICharStream stream, bool upper) : ICharStream
    {
        public int Index
        {
            get
            {
                return stream.Index;
            }
        }

        public int Size
        {
            get
            {
                return stream.Size;
            }
        }

        public string SourceName
        {
            get
            {
                return stream.SourceName;
            }
        }

        public void Consume()
        {
            stream.Consume();
        }

        [return: NotNull]
        public string GetText(Interval interval)
        {
            return stream.GetText(interval);
        }
        //Modified from LA to La
        public int La(int i)
        {
            //Modified from LA to La
            int c = stream.La(i);

            if (c <= 0)
            {
                return c;
            }

            char o = (char)c;

            if (upper)
            {
                return (int)char.ToUpperInvariant(o);
            }

            return (int)char.ToLowerInvariant(o);
        }

        public int Mark()
        {
            return stream.Mark();
        }

        public void Release(int marker)
        {
            stream.Release(marker);
        }

        public void Seek(int index)
        {
            stream.Seek(index);
        }
    }
}
