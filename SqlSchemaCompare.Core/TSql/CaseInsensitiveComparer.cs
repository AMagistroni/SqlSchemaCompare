using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SqlSchemaCompare.Core.TSql
{
    public class CaseInsensitiveComparer : IEqualityComparer<DbObject>
    {
        public bool Equals(DbObject x, DbObject y)
        {
            return string.Equals(x.Sql, y.Sql, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] DbObject obj)
        {
            return obj.GetHashCode();
        }
    }
}
