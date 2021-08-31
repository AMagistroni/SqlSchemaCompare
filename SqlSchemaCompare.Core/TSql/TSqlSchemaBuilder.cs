using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Text;
using Index = SqlSchemaCompare.Core.DbStructures.Index;

namespace SqlSchemaCompare.Core.TSql
{
    public class TSqlSchemaBuilder : ISchemaBuilder
    {
        public string Build(DbObject dbObject, Operation operation)
        {
            switch (dbObject.DbObjectType)
            {
                case DbObjectType.Table:
                    return BuildCreateDropTable(dbObject as Table, operation);
                case DbObjectType.TableContraint:
                    return BuildTableConstraint(dbObject as Table.TableConstraint, operation);
                case DbObjectType.Column:
                    return BuildAlterTableElement("COLUMN", dbObject as Table.Column, operation);
                case DbObjectType.View:
                    return BuildView(dbObject as View, operation);
                case DbObjectType.StoreProcedure:
                    return BuildStoreProcedure(dbObject as StoreProcedure, operation);
                case DbObjectType.Function:
                    return BuildFunction(dbObject as Function, operation);
                case DbObjectType.Schema:
                    return BuildCreateDrop(dbObject as Schema, "SCHEMA", operation);
                case DbObjectType.Trigger:
                    return BuildTrigger(dbObject as Trigger, operation);
                case DbObjectType.User:
                    return BuildUser(dbObject as User, operation);
                case DbObjectType.Role:
                    return BuildCreateDrop(dbObject as Role, "ROLE", operation);
                case DbObjectType.Type:
                    return BuildCreateDrop(dbObject as TypeDbObject, "TYPE", operation);
                case DbObjectType.Index:
                    return BuildIndex(dbObject as Index, operation);
                case DbObjectType.Member:
                    return BuildMember(dbObject as Member, operation);
                case DbObjectType.EnableTrigger:
                    return BuildEnableTrigger(dbObject as Trigger.EnabledDbObject, operation);
                default:
                    throw new NotImplementedException();
            }
        }

        public string GetStartCommentInLine()
        {
            return "--";
        }

        public string BuildUse(string databaseName)
        {
            if (databaseName.Contains("["))
                return $"USE {databaseName}";
            else
                return $"USE [{databaseName}]";
        }

        public string BuildSeparator()
        {
            return "GO\r\n";
        }
        private string BuildTableConstraint(Table.TableConstraint alterTable, Operation operation)
        {
            return operation switch
            {
                Operation.Create => alterTable.Sql,
                Operation.Drop => $"ALTER TABLE {alterTable.ParentName} DROP CONSTRAINT {alterTable.Name}",
                _ => throw new NotSupportedException("Alter not supported on schema"),
            };
        }

        private string BuildMember(Member member, Operation operation)
        {
            return operation switch
            {
                Operation.Create => member.Sql,
                Operation.Drop => $"ALTER ROLE {member.RoleName} DROP MEMBER {member.Name}",
                _ => throw new NotSupportedException("Alter not supported on schema"),
            };
        }

        private string BuildAlterTableElement(String itemName, DbObject dbObject, Operation operation)
        {
            return operation switch
            {
                Operation.Create => $"ALTER TABLE {dbObject.ParentName} ADD {itemName} {dbObject.Sql}",
                Operation.Drop => $"ALTER TABLE {dbObject.ParentName} DROP {itemName} {dbObject.Name}",
                Operation.Alter => $"ALTER TABLE {dbObject.ParentName} ALTER {itemName} {dbObject.Sql}",
                _ => throw new NotSupportedException("Alter not supported on schema"),
            };
        }

        private string BuildUser(User user, Operation operation)
        {
            switch (operation)
            {
                case Operation.Create:
                    return user.Sql;
                case Operation.Alter:
                    var stringBuilder = new StringBuilder();

                    if (!string.IsNullOrEmpty(user.Schema))
                        stringBuilder.Append($"DEFAULT_SCHEMA = { user.Schema}");

                    if (!string.IsNullOrEmpty(user.Login))
                    {
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append(", ");

                        stringBuilder.Append($"LOGIN = { user.Login}");
                    }

                    stringBuilder.Insert(0, $"ALTER USER { user.Name} WITH ");
                    return stringBuilder.ToString();
                case Operation.Drop:
                    return $"DROP USER {user.Name}";
                default:
                    throw new NotSupportedException($"Operation not supported on store {user}");
            }
        }

        private string BuildTrigger(Trigger trigger, Operation operation)
        {
            return BuildGenericDbObjects("TRIGGER", trigger, operation);
        }
        private string BuildEnableTrigger(Trigger.EnabledDbObject enabledDbObject, Operation operation)
        {
            var partialSql = enabledDbObject.Sql[enabledDbObject.Sql.IndexOf(" ")..];
            if (operation == Operation.Enabled)
                return $"ENABLE{partialSql}";
            else
                return $"DISABLE{partialSql}";
        }

        private string BuildFunction(Function function, Operation operation)
        {
            return BuildGenericDbObjects("FUNCTION", function, operation);
        }

        private string BuildStoreProcedure(StoreProcedure storeProcedure, Operation operation)
        {
            return BuildGenericDbObjects("PROCEDURE", storeProcedure, operation);
        }

        private string BuildCreateDropTable(Table table, Operation operation)
        {
            return BuildGenericDbObjects("TABLE", table, operation);
        }

        private string BuildCreateDrop(DbObject dbObject, string objectName, Operation operation)
        {
            return operation switch
            {
                Operation.Create => dbObject.Sql,
                Operation.Drop => $"DROP {objectName} {dbObject.Identifier}",
                _ => throw new NotSupportedException($"Operation not supported on TYPE"),
            };
        }

        private string BuildIndex(Index dbObject, Operation operation)
        {
            return operation switch
            {
                Operation.Create => dbObject.Sql,
                Operation.Drop => $"DROP INDEX {dbObject.Identifier} ON {dbObject.TableName}",
                _ => throw new NotSupportedException($"Operation not supported on TYPE"),
            };
        }

        private string BuildView(View view, Operation operation)
        {
            return BuildGenericDbObjects("VIEW", view, operation);
        }

        private string BuildGenericDbObjects(string objectName, DbObject dbObject, Operation operation)
        {
            return operation switch
            {
                Operation.Create => dbObject.Sql,
                Operation.Alter => $"ALTER {RemoveStartString("CREATE", dbObject.Sql)}",
                Operation.Drop => $"DROP {objectName.ToUpper()} {dbObject.Identifier}",
                _ => throw new NotSupportedException($"Operation not supported on {objectName}"),
            };
        }

        private string RemoveStartString(string startString, string schema)
        {
            var indexStart = schema.IndexOf(startString, StringComparison.OrdinalIgnoreCase);
            if (indexStart != 0)
                throw new Exception("Not found");

            return schema[startString.Length..].Trim();
        }
    }
}
