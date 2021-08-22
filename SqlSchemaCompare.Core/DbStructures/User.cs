namespace SqlSchemaCompare.Core.DbStructures
{
    public class User : DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.User;
        
        new public string Identifier
        {
            get
            {
                return Name;
            }
        }
    }
}
