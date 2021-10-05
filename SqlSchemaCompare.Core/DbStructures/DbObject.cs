namespace SqlSchemaCompare.Core.DbStructures
{
    public abstract class DbObject
    {
        public Operation Operation { get; init; }
        public abstract DbObjectType DbObjectType { get; }
        public string Schema { get; init; }
        public string Name { get; init; }
        public string NameCaseInsensitive => Name.ToLower();
        public string Sql { get; set; }
        public string ParentName { get; init; }

        public static bool operator ==(DbObject dbObject1, DbObject dbObject2)
        {
            if (dbObject1 is null)
            {
                if (dbObject2 is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return dbObject1.Equals(dbObject2);
        }

        public static bool operator !=(DbObject dbObject1, DbObject dbObject2) => !(dbObject1 == dbObject2);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Sql == ((DbObject)obj).Sql;
        }

        public override int GetHashCode()
        {
            return Sql.GetHashCode();
        }

        public string Identifier
        {
            get
            {
                return string.IsNullOrEmpty(Schema) ? Name : $"{Schema}.{Name}";
            }
        }

        public string IdentifierCaseInsensitive => Identifier.ToLower();
    }
}
