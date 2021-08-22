namespace SqlSchemaCompare.Core.DbStructures
{
    public class Trigger: DbObject
    {
        public class EnabledDbObject: DbObject
        {
            public bool Enabled { get; set; }

            public override DbObjectType DbObjectType => DbObjectType.EnableTrigger;
        }
        public override DbObjectType DbObjectType => DbObjectType.Trigger;
        public EnabledDbObject EnableObject { get; private set; }
        public void SetEnabled(EnabledDbObject enabled) => EnableObject = enabled;
    }
}
