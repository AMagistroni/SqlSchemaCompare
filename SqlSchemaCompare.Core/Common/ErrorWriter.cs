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

        private static StringBuilder GetErrors(IEnumerable<ErrorParser> errors, bool origin)
        {
            StringBuilder errorSchemaStringBuild = new();
            if (errors.Any())
            {
                if (origin)
                    errorSchemaStringBuild.AppendLine("**************** ORIGIN **************** ");
                else
                    errorSchemaStringBuild.AppendLine("**************** DESTINATION **************** ");

                errors.ToList()
                    .ForEach(x => errorSchemaStringBuild
                                    .AppendLine("----------------------------------------")
                                    .Append("Offending token: ").AppendLine(x.OffendingToken)
                                    .Append("Line: ").Append(x.Line).Append(", CharPosition: ").Append(x.CharPositionInLine).AppendLine()
                                    .AppendLine(x.Message));
            }

            return errorSchemaStringBuild;
        }
    }
}
