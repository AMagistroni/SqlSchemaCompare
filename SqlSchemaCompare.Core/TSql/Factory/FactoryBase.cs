using SqlSchemaCompare.Core.DbStructures;
using System;

namespace SqlSchemaCompare.Core.TSql.Factory
{
    public abstract class FactoryBase
    {
        protected Operation GetOperation(string operation)
        {
            operation = operation.ToUpper();
            if (operation == "CREATE")
                return Operation.Create;
            else if (operation == "ALTER")
                return Operation.Alter;
            else throw new NotImplementedException();
        }
    }
}
