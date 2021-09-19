using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using System;
using System.Linq;
using System.Text;
using Index = SqlSchemaCompare.Core.DbStructures.Index;

namespace SqlSchemaCompare.Core.TSql
{
    public class TSqlSchemaBuilder : ISchemaBuilder
    {
        public string Build(DbObject dbObject, Operation operation, ResultProcessDbObject resultProcessDbObject)
        {
            return dbObject.DbObjectType switch
            {
                DbObjectType.Table => BuildCreateDropTable(dbObject as Table, operation),
                DbObjectType.TableDefaultContraint => BuildTableConstraint(dbObject as TableConstraint, operation, resultProcessDbObject),
                DbObjectType.TablePrimaryKeyContraint => BuildTableConstraint(dbObject as TableConstraint, operation, resultProcessDbObject),
                DbObjectType.TableForeignKeyContraint => BuildTableConstraint(dbObject as TableConstraint, operation, resultProcessDbObject),
                DbObjectType.Column => BuildColumn(dbObject as Table.Column, operation),
                DbObjectType.View => BuildView(dbObject as View, operation),
                DbObjectType.StoreProcedure => BuildStoreProcedure(dbObject as StoreProcedure, operation),
                DbObjectType.Function => BuildFunction(dbObject as Function, operation),
                DbObjectType.Schema => BuildCreateDrop(dbObject as Schema, "SCHEMA", operation),
                DbObjectType.Trigger => BuildTrigger(dbObject as Trigger, operation),
                DbObjectType.User => BuildUser(dbObject as User, operation),
                DbObjectType.Role => BuildCreateDrop(dbObject as Role, "ROLE", operation),
                DbObjectType.Type => BuildCreateDrop(dbObject as TypeDbObject, "TYPE", operation),
                DbObjectType.Index => BuildIndex(dbObject as Index, operation),
                DbObjectType.Member => BuildMember(dbObject as Member, operation),
                DbObjectType.EnableTrigger => BuildEnableTrigger(dbObject as Trigger.EnabledDbObject, operation),
                _ => throw new NotImplementedException(),
            };
        }

        public string GetStartCommentInLine()
        {
            return "--";
        }

        public string BuildUse(string databaseName) => $"USE {databaseName}";

        public string BuildSeparator()
        {
            return "GO\r\n";
        }
        private string BuildTableConstraint(TableConstraint constraint, Operation operation, ResultProcessDbObject resultProcessDbObject)
        {
            switch (operation)
            {
                case Operation.Create:
                    if (!resultProcessDbObject.GetDbObject(DbObjectType.Column, Operation.Create).Any(x => constraint.ColumnNames.Contains(x.Name)))
                    {
                        if (constraint is Table.TablePrimaryKeyConstraint)
                        {
                            return $"ALTER TABLE {constraint.ParentName} ADD {constraint.Sql}";
                        }
                        return constraint.Sql;
                    }
                    return string.Empty;
                case Operation.Drop:
                    if (String.IsNullOrEmpty(constraint.Name))
                    {
                        var tableSchema = constraint.Table.Schema.Replace("[", "").Replace("]", "");
                        var tableName = constraint.Table.Name.Replace("[", "").Replace("]", "");
                        return 
@$"DECLARE @PrimaryKeyName_{tableSchema}_{tableName} sysname =
(        
    SELECT CONSTRAINT_NAME
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_SCHEMA='{tableSchema}' AND TABLE_NAME = '{tableName}'
)

IF @PrimaryKeyName_{tableSchema}_{tableName} IS NOT NULL
BEGIN
    DECLARE @SQL_PK_{tableSchema}_{tableName} NVARCHAR(MAX) = 'ALTER TABLE {constraint.ParentName} DROP CONSTRAINT ' + @PrimaryKeyName_{tableSchema}_{tableName}
    EXEC sp_executesql @SQL_PK_{tableSchema}_{tableName};
END";
                    }
                    else
                    {
                        return $"ALTER TABLE {constraint.ParentName} DROP CONSTRAINT {constraint.Name}";
                    }
                default:
                    throw new NotSupportedException("Alter not supported on schema");
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

        private string BuildColumn(Table.Column column, Operation operation)
        {
            switch (operation)
            {
                case Operation.Create:
                    var sql = $"ALTER TABLE {column.ParentName} ADD { column.Sql}";
                    var constraintRelated = column.Table.Constraints.OfType<Table.TableDefaultConstraint>().SingleOrDefault(x => x.ColumnNames.Contains(column.Name));
                    if (constraintRelated != null)
                    {
                        sql = $"{sql} CONSTRAINT {constraintRelated.Name} DEFAULT {constraintRelated.Value}";
                    }
                    return sql;
                case Operation.Alter:
                    return $"ALTER TABLE {column.ParentName} ALTER COLUMN {column.Sql}";
                case Operation.Drop:
                    return $"ALTER TABLE {column.ParentName} DROP COLUMN {column.Name}";
                default:
                    throw new NotSupportedException("Alter not supported on schema");
            }
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
                Operation.Drop => $"DROP INDEX {dbObject.Identifier} ON {dbObject.ParentName}",
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
