using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlSchemaCompare.Core.Common
{
    public static class EnumExtension
    {
        public static IEnumerable<TEnum> GetEnumList<TEnum>(this Enum enumerator) where TEnum : Enum
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
}
