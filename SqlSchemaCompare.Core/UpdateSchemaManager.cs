using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SqlSchemaCompare.Core.DbStructures.Table;

namespace SqlSchemaCompare.Core
{
    public class UpdateSchemaManager
    {
        private readonly ISchemaBuilder _schemaBuilder;
        public UpdateSchemaManager(ISchemaBuilder schemaBuilder)
        {
            _schemaBuilder = schemaBuilder;
        }

        private List<(DbObjectType DbObjectType, Operation Operation)> OrderItemSchema => new()
        {
            (DbObjectType.User, Operation.Create),
            (DbObjectType.User, Operation.Drop),
            (DbObjectType.User, Operation.Alter),

            (DbObjectType.Role, Operation.Create),
            (DbObjectType.Role, Operation.Drop),

            (DbObjectType.Member, Operation.Create),
            (DbObjectType.Member, Operation.Drop),

            (DbObjectType.Schema, Operation.Create),

            (DbObjectType.Table, Operation.Create),
            (DbObjectType.TableDefaultContraint, Operation.Drop),
            (DbObjectType.TableForeignKeyContraint, Operation.Drop),
            (DbObjectType.TablePrimaryKeyContraint, Operation.Drop),
            (DbObjectType.Index, Operation.Drop),
            (DbObjectType.Column, Operation.Rename),
            (DbObjectType.Column, Operation.Create),
            (DbObjectType.Column, Operation.Drop),
            (DbObjectType.Column, Operation.Alter),
            (DbObjectType.TablePrimaryKeyContraint, Operation.Create),
            (DbObjectType.TableForeignKeyContraint, Operation.Create),
            (DbObjectType.TableDefaultContraint, Operation.Create),
            (DbObjectType.Table, Operation.Drop),
            (DbObjectType.TableSet, Operation.Create),

            (DbObjectType.StoreProcedure, Operation.Drop),
            (DbObjectType.StoreProcedure, Operation.Create),
            (DbObjectType.StoreProcedure, Operation.Alter),

            (DbObjectType.Function, Operation.Drop),
            (DbObjectType.Function, Operation.Create),
            (DbObjectType.Function, Operation.Alter),

            (DbObjectType.View, Operation.Drop),
            (DbObjectType.View, Operation.Create),
            (DbObjectType.View, Operation.Alter),

            (DbObjectType.Trigger, Operation.Drop),
            (DbObjectType.Trigger, Operation.Alter),
            (DbObjectType.Trigger, Operation.Create),
            (DbObjectType.EnableTrigger, Operation.Enabled),
            (DbObjectType.EnableTrigger, Operation.Disabled),

            (DbObjectType.Type, Operation.Drop),
            (DbObjectType.Type, Operation.Create),

            (DbObjectType.Index, Operation.Create),

            (DbObjectType.Schema, Operation.Drop),
        };

        public string UpdateSchema(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, IEnumerable<DbObjectType> selectedObjectType)
        {
            ResultProcessDbObject resultProcessDbObject = new();

            ProcessUser(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessRole(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessMember(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessSchema(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessTable(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessGenericDbObject<StoreProcedure>(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessGenericDbObject<Function>(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessView(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessTrigger(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessDbObjectWithoutAlter<TypeDbObject>(sourceObjects, destinationObjects, resultProcessDbObject);

            StringBuilder updateSchemaStringBuild = new();
            if (resultProcessDbObject.OperationsOnDbObject.Count > 0)
            {
                var destinationDb = destinationObjects.OfType<Database>();
                if (destinationDb.Any())
                {
                    updateSchemaStringBuild.AppendLine(_schemaBuilder.BuildUse(destinationDb.First().Name));
                    updateSchemaStringBuild.AppendLine(_schemaBuilder.BuildSeparator());
                }

                foreach (var objectToWrite in OrderItemSchema)
                {
                    if (selectedObjectType.Contains(objectToWrite.DbObjectType))
                    {
                        var dbObjects = resultProcessDbObject.GetDbObject(objectToWrite.DbObjectType, objectToWrite.Operation);

                        foreach (var dbObject in dbObjects.ToList())
                        {
                            var sql = _schemaBuilder.Build(dbObject, objectToWrite.Operation, resultProcessDbObject);
                            if (!string.IsNullOrEmpty(sql))
                            {
                                updateSchemaStringBuild
                                    .AppendLine(sql)
                                    .AppendLine(_schemaBuilder.BuildSeparator());
                            }
                        }
                    }
                }
            }
            return updateSchemaStringBuild.ToString();
        }

        private void ProcessView(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            ProcessGenericDbObject<View>(sourceObjects, destinationObjects, resultProcessDbObject);
            var originDb = sourceObjects.OfType<View>();
            var destinationDb = destinationObjects.OfType<View>();
            foreach (var viewOrigin in originDb)
            {
                var destinationView = destinationDb.SingleOrDefault(x => x.Identifier == viewOrigin.Identifier);
                if (destinationView == null)
                    resultProcessDbObject.AddOperation(viewOrigin.Indexes, Operation.Create);
                else
                    ProcessSingleIndex(viewOrigin.Indexes, destinationView.Indexes, resultProcessDbObject);
            }
        }

        private void ProcessSingleIndex(IEnumerable<Index> originIndexes, IEnumerable<Index> destinationIndexes, ResultProcessDbObject resultProcessDbObject)
        {
            var indexToCreate = CreateDbObjectByName(originIndexes, destinationIndexes, resultProcessDbObject);
            var indexToDrop = DropDbObjectByName(originIndexes, destinationIndexes)
                                .GroupBy(x => x.Identifier)
                                .Select(x => x.First()).ToList();
            var identifierToDrop = indexToDrop.Select(x => x.Identifier);
            resultProcessDbObject.AddOperation(indexToDrop, Operation.Drop);

            var indexToAlter = originIndexes
                .Except(indexToCreate)
                .Where(x => !identifierToDrop.Contains(x.Identifier) && !destinationIndexes.Contains(x)) // Discard index dropped from index to alter
                .ToList(); //discard object present in origin, present in destination and equals

            resultProcessDbObject.AddOperation(indexToAlter, Operation.Drop);
            resultProcessDbObject.AddOperation(indexToAlter, Operation.Create);
        }

        private void ProcessMember(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Member>();
            var destinationDb = destinationObjects.OfType<Member>();

            var roleNameDropped = resultProcessDbObject.GetDbObject(DbObjectType.Role, Operation.Drop).Select(x => x.Name);
            var userNameDropped = resultProcessDbObject.GetDbObject(DbObjectType.User, Operation.Drop).Select(x => x.Name);
            destinationDb = destinationDb
                .Except(destinationDb.Where(x => roleNameDropped.Contains(x.RoleName)))
                .Except(destinationDb.Where(x => userNameDropped.Contains(x.Name)));

            CreateDbObjectByName(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessTrigger(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            (var toCreate, var toAlter, _) = ProcessGenericDbObject<Trigger>(sourceObjects, destinationObjects, resultProcessDbObject);

            foreach (var trigger in toCreate.Union(toAlter))
            {
                if (trigger.EnableObject.Enabled)
                    resultProcessDbObject.AddOperation<Trigger.EnabledDbObject>(trigger.EnableObject, Operation.Enabled);
                else
                    resultProcessDbObject.AddOperation<Trigger.EnabledDbObject>(trigger.EnableObject, Operation.Disabled);
            }
        }

        private void ProcessUser(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<User>();
            var destinationDb = destinationObjects.OfType<User>();

            var toCreate = CreateDbObjectByName(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName(originDb, destinationDb, resultProcessDbObject);

            var toAlter = originDb
                    .Except(toCreate)
                    .Where(x => !destinationDb.Contains(x)) //discard object present in origin, present in destination and equals
                    .ToList();
            resultProcessDbObject.AddOperation(toAlter, Operation.Alter);
        }

        private void ProcessRole(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Role>();
            var destinationDb = destinationObjects.OfType<Role>();

            CreateDbObject(originDb, destinationDb, resultProcessDbObject);
            DropDbObject(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessSchema(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Schema>();
            var destinationDb = destinationObjects.OfType<Schema>();

            CreateDbObject(originDb, destinationDb, resultProcessDbObject);
            DropDbObject(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessTable(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Table>();
            var destinationDb = destinationObjects.OfType<Table>();

            var destinationIdentifier = destinationDb.Select(x => x.Identifier);
            foreach (var tableOrigin in originDb)
            {
                if (!destinationIdentifier.Contains(tableOrigin.Identifier))
                {
                    if (tableOrigin.PrimaryKeyDefinedInsideCreateTable)
                        resultProcessDbObject.AddOperation(tableOrigin.Constraints.Where(x => x is not TablePrimaryKeyConstraint).ToList(), Operation.Create);
                    else
                        resultProcessDbObject.AddOperation(tableOrigin.Constraints, Operation.Create);

                    resultProcessDbObject.AddOperation(tableOrigin.Indexes, Operation.Create);
                    resultProcessDbObject.AddOperation<Table>(tableOrigin, Operation.Create);
                    resultProcessDbObject.AddOperation(tableOrigin.TableSetList, Operation.Create);
                }
                else
                {
                    var destinationTable = destinationDb.Single(x => x.Identifier == tableOrigin.Identifier);
                    var constraintToCreate = CreateDbObject(tableOrigin.Constraints, destinationTable.Constraints, resultProcessDbObject);

                    var constraintToDrop = DropDbObject(tableOrigin.Constraints, destinationTable.Constraints)
                        .GroupBy(x => x.Name)
                        .Select(x => x.First()).ToList();

                    var constraintsToAlter = tableOrigin.Constraints
                        .Except(constraintToCreate)
                        .Where(x => !destinationTable.Constraints.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

                    resultProcessDbObject.AddOperation(constraintToDrop, Operation.Drop);
                    resultProcessDbObject.AddOperation(constraintsToAlter, Operation.Drop);
                    resultProcessDbObject.AddOperation(constraintsToAlter, Operation.Create);

                    var primaryKeyToModify = (constraintToDrop.Union(constraintsToAlter)).SingleOrDefault(X => X.DbObjectType == DbObjectType.TablePrimaryKeyContraint);
                    if (primaryKeyToModify != null)
                    {
                        var relatedForeignKey = destinationObjects
                            .OfType<TableForeignKeyConstraint>()
                            .Where(x => x.TableIdentifierPrimaryKey == primaryKeyToModify.Table.Identifier).ToList();

                        resultProcessDbObject.AddOperation(relatedForeignKey, Operation.Drop);
                        resultProcessDbObject.AddOperation(relatedForeignKey, Operation.Create);
                    }

                    ProcessSingleIndex(tableOrigin.Indexes, destinationTable.Indexes, resultProcessDbObject);

                    if (tableOrigin != destinationTable)
                    {
                        //columns different
                        ProcessTableColumn(tableOrigin, destinationTable, resultProcessDbObject);
                    }
                    resultProcessDbObject.AddOperation(tableOrigin.TableSetList.Except(destinationTable.TableSetList).ToList(), Operation.Create);
                }
            }
            DropDbObject(originDb, destinationDb, resultProcessDbObject);
        }
        private void ProcessTableColumn(Table table, Table destinationTable, ResultProcessDbObject resultProcessDbObject)
        {
            IList<Column> columnsToAdd;
            IList<Column> columnsToDrop;
            IList<Column> columnsToAlter;
            IList<Column> columnsToRename;

            var columnsEqualsCaseInsensitive = table.Columns.Select(x => x.NameCaseInsensitive).Intersect(destinationTable.Columns.Select(x => x.NameCaseInsensitive));
            columnsToRename = table.Columns.Where(x => columnsEqualsCaseInsensitive.Contains(x.NameCaseInsensitive) && !destinationTable.Columns.Select(x => x.Name).Contains(x.Name)).ToList();
            foreach (var column in columnsToRename)
            {
                resultProcessDbObject.AddOperation<Column>(column, Operation.Rename,
                    destinationTable.Columns.Single(x => x.IdentifierCaseInsensitive == column.IdentifierCaseInsensitive).Name);
            }

            columnsToAdd = table.Columns
                            .Where(x =>
                                table.Columns
                                    .Select(x => x.NameCaseInsensitive)
                                    .Except(destinationTable.Columns.Select(x => x.NameCaseInsensitive))
                                    .Contains(x.NameCaseInsensitive)).ToList();

            columnsToDrop = destinationTable.Columns
               .Where(x =>
                   destinationTable.Columns
                       .Select(x => x.NameCaseInsensitive)
                       .Except(table.Columns.Select(x => x.NameCaseInsensitive))
                       .Contains(x.NameCaseInsensitive)).ToList();

            columnsToAlter = table.Columns
                .Except(columnsToAdd)
                .Where(x => !destinationTable.Columns.Contains(x, new CaseInsensitiveComparer())).ToList();

            resultProcessDbObject.AddOperation(columnsToAdd, Operation.Create);
            resultProcessDbObject.AddOperation(columnsToAlter, Operation.Alter);
            resultProcessDbObject.AddOperation(columnsToDrop, Operation.Drop);

            var columnsOfCostraintToDropAndCreate = destinationTable
                .Constraints.SelectMany(x => x.ColumnNames)
                .Intersect(columnsToAlter.Select(x => x.Name));

            var constraintToDropAndCreate = destinationTable.Constraints
                .Where(x => columnsOfCostraintToDropAndCreate.Intersect(x.ColumnNames).Any())
                .Except(resultProcessDbObject.GetDbObject(DbObjectType.TableDefaultContraint, Operation.Drop)
                    .Union(resultProcessDbObject.GetDbObject(DbObjectType.TableForeignKeyContraint, Operation.Drop))
                    .Union(resultProcessDbObject.GetDbObject(DbObjectType.TablePrimaryKeyContraint, Operation.Drop))) //constraint that we have to drop
                .ToList();
            resultProcessDbObject.AddOperation(constraintToDropAndCreate, Operation.Drop);
            resultProcessDbObject.AddOperation(constraintToDropAndCreate, Operation.Create);

            var columnsOfIndexToDropAndCreate = destinationTable
                .Indexes.SelectMany(x => x.ColumnNames)
                .Intersect(columnsToAlter.Select(x => x.Name));

            var indexToDropAndCreate = destinationTable.Indexes
                .Where(x => columnsOfIndexToDropAndCreate.Intersect(x.ColumnNames).Any())
                .Except(resultProcessDbObject.GetDbObject(DbObjectType.Index, Operation.Drop))//constraint that we have to drop
                .ToList();
            resultProcessDbObject.AddOperation(indexToDropAndCreate, Operation.Drop);
            resultProcessDbObject.AddOperation(indexToDropAndCreate, Operation.Create);
        }

        private void ProcessDbObjectWithoutAlter<T>(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var originDb = sourceObjects.OfType<T>();
            var destinationDb = destinationObjects.OfType<T>();

            var toCreate = CreateDbObjectByName<T>(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName<T>(originDb, destinationDb, resultProcessDbObject);

            var toAlter = originDb
                .Except(toCreate)
                .Where(x => !destinationDb.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

            resultProcessDbObject.AddOperation<T>(toAlter, Operation.Drop);
            resultProcessDbObject.AddOperation<T>(toAlter, Operation.Create);
        }

        private (List<T> toCreate, List<T> toAlter, List<T> toDrop) ProcessGenericDbObject<T>(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, ResultProcessDbObject resultProcessDbObject)
            where T : DbObject
        {
            var originDb = sourceObjects.OfType<T>();
            var destinationDb = destinationObjects.OfType<T>();

            var toCreate = CreateDbObject<T>(originDb, destinationDb, resultProcessDbObject);
            var toDrop = DropDbObject<T>(originDb, destinationDb, resultProcessDbObject);

            var toAlter = originDb
                .Except(toCreate)
                .Where(x => !destinationDb.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

            resultProcessDbObject.AddOperation<T>(toAlter, Operation.Alter);

            return (toCreate, toAlter, toDrop);
        }
        private List<T> CreateDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, ResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toCreate = sourceObjects
                .Where(dbObject => sourceObjects
                    .Select(x => x.Identifier)
                    .Except(destinationObjects.Select(x => x.Identifier)) //looking for complete name present in origin and absent from destination                    
                    .Contains(dbObject.Identifier)).ToList(); // object with completeName 
            resultProcessDbObject.AddOperation<T>(toCreate, Operation.Create);
            return toCreate;
        }
        private List<T> CreateDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, ResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toCreate = sourceObjects
                .Where(dbObject => sourceObjects
                    .Select(x => x.Name)
                    .Except(destinationObjects.Select(x => x.Name)) //looking for name present in origin and absent from destination                    
                    .Contains(dbObject.Name)).ToList(); // object with completeName 
            resultProcessDbObject.AddOperation<T>(toCreate, Operation.Create);
            return toCreate;
        }
        private List<T> DropDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, ResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toDrop = DropDbObject<T>(sourceObjects, destinationObjects);
            resultProcessDbObject.AddOperation<T>(toDrop, Operation.Drop);
            return toDrop;
        }
        private List<T> DropDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects) where T : DbObject
        {
            return destinationObjects
                .Where(dbObject => destinationObjects
                    .Select(x => x.Identifier)
                    .Except(sourceObjects.Select(x => x.Identifier)) //looking for complete name present in destination and absent from origin
                    .Contains(dbObject.Identifier)) // object with completeName to be dropped
                .ToList();
        }

        private List<T> DropDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, ResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toDrop = DropDbObjectByName(sourceObjects, destinationObjects);
            resultProcessDbObject.AddOperation<T>(toDrop, Operation.Drop);
            return toDrop;
        }
        private List<T> DropDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects) where T : DbObject
        {
            return destinationObjects
                .Where(dbObject => destinationObjects
                    .Select(x => x.Name)
                    .Except(sourceObjects.Select(x => x.Name)) //looking for complete name present in destination and absent from origin
                    .Contains(dbObject.Name)) // object with completeName to be dropped
                .ToList();
        }
    }
}
