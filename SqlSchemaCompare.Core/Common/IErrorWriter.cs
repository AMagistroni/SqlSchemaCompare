using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public interface IErrorWriter
    {
        public string GetErrors(IEnumerable<ErrorParser> errorsOrigin, IEnumerable<ErrorParser> errorsDestination);
    }
}
