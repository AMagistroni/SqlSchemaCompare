namespace SqlSchemaCompare.Core.DbStructures
{
    public class Member : DbObject
    {
        public override DbObjectType DbObjectType => DbObjectType.Member;

        public string RoleName { get; set; }
    }
}
