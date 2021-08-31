namespace SqlSchemaCompare.Core.DbStructures
{
    public class User : DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.User;
        public string Login { get; init; }

        new public string Identifier
        {
            get
            {
                return Name;
            }
        }
    }
}
