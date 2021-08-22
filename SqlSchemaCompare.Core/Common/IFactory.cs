using Antlr4.Runtime;
using SqlSchemaCompare.Core.DbStructures;

namespace SqlSchemaCompare.Core.Common
{
    public interface IFactory
    {
        DbObject Create(ParserRuleContext context, ICharStream stream);
    }
}
