namespace SqlSchemaCompare.Core.DbStructures
{
    public class Index : DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.Index;
        public string TableName { get; set; }
    }
}
