using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSchemaCompare.Core.Common
{
    public class ErrorWriter : IErrorWriter
    {
        public string GetErrors(IEnumerable<ErrorParser> errorsOrigin, IEnumerable<ErrorParser> errorsDestination)
        {
            StringBuilder errorSchemaStringBuild = GetErrors(errorsOrigin, true)
                            .Append(GetErrors(errorsDestination, false));
            return errorSchemaStringBuild.ToString();
        }

        private StringBuilder GetErrors(IEnumerable<ErrorParser> errors, bool origin)
        {
            StringBuilder errorSchemaStringBuild = new();
            if (errors.Count() > 0)
            {
                if (origin)
                    errorSchemaStringBuild.AppendLine("**************** ORIGIN **************** ");
                else
                    errorSchemaStringBuild.AppendLine("**************** DESTINATION **************** ");

                errors.ToList()
                    .ForEach(x => errorSchemaStringBuild
                                    .AppendLine("----------------------------------------")
                                    .AppendLine($"Offending token: {x.OffendingToken}")
                                    .AppendLine($"Line: {x.Line}, CharPosition: {x.CharPositionInLine}")
                                    .AppendLine(x.Message));
            }

            return errorSchemaStringBuild;
        }
    }
}
