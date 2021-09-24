namespace SqlSchemaCompare.Core.DbStructures
{
    public enum DbObjectType
    {
        Table,
        View,
        StoreProcedure,
        Function, 
        Schema,
        Trigger,
        User,
        Role,
        Member,
        Type,
        Index,
        TableDefaultContraint,
        TableForeignKeyContraint,
        TablePrimaryKeyContraint,
        TableSet,
        Column,
        EnableTrigger,
        Database,
        Other
    }
}
