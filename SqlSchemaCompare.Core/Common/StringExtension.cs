using System.Collections.Generic;

namespace SqlSchemaCompare.Core.Common
{
    public static class StringExtension
    {
        public static bool StartsWithAny(this string value, List<string> list)
        {
            if (value is null)
                return false;

            foreach (var item in list)
            {
                if (value.StartsWith(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
