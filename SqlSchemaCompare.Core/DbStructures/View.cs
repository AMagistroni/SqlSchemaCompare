namespace SqlSchemaCompare.Core.DbStructures
{
    public class View: DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.View;
        public string Body { get; init; }
    }
}
