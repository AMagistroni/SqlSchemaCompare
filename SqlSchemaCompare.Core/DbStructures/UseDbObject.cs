namespace SqlSchemaCompare.Core.DbStructures
{
    public class UseDbObject : DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.UseDatabase;
    }
}
