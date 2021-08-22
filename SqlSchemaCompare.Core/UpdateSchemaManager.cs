using SqlSchemaCompare.Core.Common;
using SqlSchemaCompare.Core.DbStructures;
using SqlSchemaCompare.Core.TSql;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSchemaCompare.Core
{
    public class UpdateSchemaManager
    {
        private readonly ISchemaBuilder _schemaBuilder;
        private readonly IDbObjectFactory _dbObjectFactory;
        private readonly IErrorWriter _errorWriter;
        public UpdateSchemaManager(ISchemaBuilder schemaBuilder, IDbObjectFactory dbObjectFactory, IErrorWriter errorWriter)
        {
            _schemaBuilder = schemaBuilder;
            _dbObjectFactory = dbObjectFactory;
            _errorWriter = errorWriter;
        }
        public (string updateSchema, string errors) UpdateSchema(string originSchema, string destinationSchema, string databaseName)
        {
            TSqlResultProcessDbObject resultProcessDbObject = new();
            
            (var sourceObjects, var errorOriginSchema) = _dbObjectFactory.CreateObjectsForUpdateOperation(originSchema);
            (var destinationObjects, var errorDestinationSchema) = _dbObjectFactory.CreateObjectsForUpdateOperation(destinationSchema);

            ProcessUser(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessRole(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessMember(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessSchema(sourceObjects, destinationObjects, Operation.Create, resultProcessDbObject);
            ProcessGenericDbObject<StoreProcedure>(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessGenericDbObject<Function>(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessGenericDbObject<View>(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessTrigger(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessTable(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessDbObjectWithoutAlter<TypeDbObject>(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcdessIndex(sourceObjects, destinationObjects, resultProcessDbObject);
            ProcessSchema(sourceObjects, destinationObjects, Operation.Drop, resultProcessDbObject);

            var errors = _errorWriter.GetErrors(errorOriginSchema, errorDestinationSchema);

            if (resultProcessDbObject.UpdateSchemaStringBuild.Length == 0)
            {
                return (string.Empty, errors.ToString());
            }
            else
            {
                StringBuilder useDb = new();
                useDb.AppendLine(_schemaBuilder.BuildUse(databaseName));
                useDb.AppendLine(_schemaBuilder.BuildSeparator());

                return ($"{useDb}{resultProcessDbObject.UpdateSchemaStringBuild}", errors.ToString());
            }
        }

        

        private void ProcessMember(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Member>();
            var destinationDb = destinationObjects.OfType<Member>();

            var roleNameDropped = resultProcessDbObject.ToDrop.OfType<Role>().Select(x => x.Name);
            destinationDb = destinationDb.Except(destinationDb.Where(x => roleNameDropped.Contains(x.RoleName)));

            CreateDbObjectByName<Member>(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName<Member>(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessTrigger(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
        {
            (var toCreate, var toAlter, var toDrop) = ProcessGenericDbObject<Trigger>(sourceObjects, destinationObjects, resultProcessDbObject);

            (toCreate.Union(toAlter)).ToList()
                .ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x.EnableObject, x.EnableObject.Enabled
                                                                                                ? Operation.Enabled
                                                                                                : Operation.Disabled))
                                .AppendLine(_schemaBuilder.BuildSeparator()));
        }

        private void ProcessUser(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<User>();
            var destinationDb = destinationObjects.OfType<User>();

            var toCreate = CreateDbObjectByName<User>(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName<User>(originDb, destinationDb, resultProcessDbObject);

            originDb
                .Except(toCreate)
                .Where(x => !destinationDb.Contains(x)) //discard object present in origin, present in destination and equals
                .ToList()
                    .ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                        .AppendLine(_schemaBuilder.Build(x, Operation.Alter))
                        .AppendLine(_schemaBuilder.BuildSeparator()));

        }

        private void ProcessRole(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Role>();
            var destinationDb = destinationObjects.OfType<Role>();

            CreateDbObject<Role>(originDb, destinationDb, resultProcessDbObject);
            DropDbObject<Role>(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessSchema(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, Operation operation, TSqlResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Schema>();
            var destinationDb = destinationObjects.OfType<Schema>();

            if (operation == Operation.Create)
                CreateDbObject<Schema>(originDb, destinationDb, resultProcessDbObject);
            else
                DropDbObject<Schema>(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessTable(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Table>();
            var destinationDb = destinationObjects.OfType<Table>();

            var destinationIdentifier = destinationDb.Select(x => x.Identifier);
            foreach (var tableOrigin in originDb)
            {
                if (!destinationIdentifier.Contains(tableOrigin.Identifier))
                {
                    //table new 
                    resultProcessDbObject.UpdateSchemaStringBuild
                        .AppendLine(_schemaBuilder.Build(tableOrigin, Operation.Create))
                        .AppendLine(_schemaBuilder.BuildSeparator());

                    tableOrigin
                        .Constraints.ToList()
                        .ForEach(x =>
                            resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                                .AppendLine(_schemaBuilder.BuildSeparator()));

                    resultProcessDbObject.AddToCreate(tableOrigin);
                }
                else
                {
                    var destinationTable = destinationDb.Single(x => x.Identifier == tableOrigin.Identifier);
                    if (tableOrigin != destinationTable)
                    {
                        //columns different
                        ProcessTableColumn(tableOrigin, destinationTable, resultProcessDbObject);
                    }
                    
                    var constraintToCreate = CreateDbObject<Table.TableConstraint>(tableOrigin.Constraints, destinationTable.Constraints, resultProcessDbObject);

                    var constraintToDrop = DropDbObject<Table.TableConstraint>(tableOrigin.Constraints, destinationTable.Constraints)
                        .GroupBy(x => x.Name)
                        .Select(x => x.First()).ToList();

                    constraintToDrop.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                        .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                                        .AppendLine(_schemaBuilder.BuildSeparator()));

                    var toAlter = tableOrigin.Constraints
                        .Except(constraintToCreate)
                        .Where(x => !destinationTable.Constraints.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

                    toAlter.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                        .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                                        .AppendLine(_schemaBuilder.BuildSeparator())
                                        .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                                        .AppendLine(_schemaBuilder.BuildSeparator()));
                }
            }
            DropDbObject<Table>(originDb, destinationDb, resultProcessDbObject);
        }

        private void ProcessTableColumn(Table table, Table destinationTable, TSqlResultProcessDbObject resultProcessDbObject)
        {
            IEnumerable<Table.Column> columnsToAdd;
            IEnumerable<Table.Column> columnsToDrop;
            IEnumerable<Table.Column> columnsToAlter;

            columnsToAdd = table.Columns
                            .Where(x =>
                                table.Columns
                                    .Select(x => x.Name)
                                    .Except(destinationTable.Columns.Select(x => x.Name))
                                    .Contains(x.Name));

            columnsToDrop = destinationTable.Columns
               .Where(x =>
                   destinationTable.Columns
                       .Select(x => x.Name)
                       .Except(table.Columns.Select(x => x.Name))
                       .Contains(x.Name));

            columnsToAlter = table.Columns
                .Except(columnsToAdd)
                .Where(x => !destinationTable.Columns.Contains(x)); //discard object present in origin, present in destination and equals                                                                    

            columnsToAdd
                .ToList()
                .ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                    .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                    .AppendLine(_schemaBuilder.BuildSeparator()));

            columnsToDrop
                .ToList()
                .ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                    .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                    .AppendLine(_schemaBuilder.BuildSeparator()));

            columnsToAlter
                .ToList()
                .ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                    .AppendLine(_schemaBuilder.Build(x, Operation.Alter))
                    .AppendLine(_schemaBuilder.BuildSeparator()));
        }

        private void ProcdessIndex(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
        {
            var originDb = sourceObjects.OfType<Index>();
            var destinationDb = destinationObjects.OfType<Index>();

            var tableNameToDrop = resultProcessDbObject.ToDrop.OfType<Table>().Select(x => x.Identifier);
            destinationDb = destinationDb.Except(destinationDb.Where(x => tableNameToDrop.Contains(x.TableName)));

            var toCreate = CreateDbObjectByName<Index>(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName<Index>(originDb, destinationDb, resultProcessDbObject);

            var toAlter = originDb
                .Except(toCreate)
                .Where(x => !destinationDb.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

            toAlter.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                                .AppendLine(_schemaBuilder.BuildSeparator())
                                .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                                .AppendLine(_schemaBuilder.BuildSeparator()));
        }

        private void ProcessDbObjectWithoutAlter<T>(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject) where T: DbObject
        {
            var originDb = sourceObjects.OfType<T>();
            var destinationDb = destinationObjects.OfType<T>();

            var toCreate = CreateDbObjectByName<T>(originDb, destinationDb, resultProcessDbObject);
            DropDbObjectByName<T>(originDb, destinationDb, resultProcessDbObject);

            var toAlter = originDb
                .Except(toCreate)
                .Where(x => !destinationDb.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

            toAlter.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                                .AppendLine(_schemaBuilder.BuildSeparator())
                                .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                                .AppendLine(_schemaBuilder.BuildSeparator()));
        }

        private (List<T> toCreate, List<T> toAlter, List<T> toDrop) ProcessGenericDbObject<T>(IEnumerable<DbObject> sourceObjects, IEnumerable<DbObject> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject)
            where T: DbObject
        {
            var originDb = sourceObjects.OfType<T>();
            var destinationDb = destinationObjects.OfType<T>();

            var toCreate = CreateDbObject<T>(originDb, destinationDb, resultProcessDbObject);
            var toDrop = DropDbObject<T>(originDb, destinationDb, resultProcessDbObject);

            var toAlter = originDb
                .Except(toCreate)
                .Where(x => !destinationDb.Contains(x)).ToList(); //discard object present in origin, present in destination and equals

            toAlter.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x, Operation.Alter))
                                .AppendLine(_schemaBuilder.BuildSeparator()));

            return (toCreate, toAlter, toDrop);
        }
        private List<T> CreateDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toCreate = CreateDbObject<T>(sourceObjects, destinationObjects);
            resultProcessDbObject.AddToCreate<T>(toCreate);

            toCreate.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                    .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                                    .AppendLine(_schemaBuilder.BuildSeparator()));

            return toCreate;
        }
        private List<T> CreateDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects) where T : DbObject
        {
            return sourceObjects
                .Where(dbObject => sourceObjects
                    .Select(x => x.Identifier)
                    .Except(destinationObjects.Select(x => x.Identifier)) //looking for complete name present in origin and absent from destination
                    .OrderBy(x => x).ToList() // list of completeName to be created
                    .Contains(dbObject.Identifier)).ToList(); // object with completeName 
        }
        private List<T> CreateDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toCreate = CreateDbObjectByName<T>(sourceObjects, destinationObjects);
            resultProcessDbObject.AddToCreate<T>(toCreate);

            toCreate.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                    .AppendLine(_schemaBuilder.Build(x, Operation.Create))
                                    .AppendLine(_schemaBuilder.BuildSeparator()));

            return toCreate;
        }
        private List<T> CreateDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects) where T : DbObject
        {
            return sourceObjects
                .Where(dbObject => sourceObjects
                    .Select(x => x.Name)
                    .Except(destinationObjects.Select(x => x.Name)) //looking for complete name present in origin and absent from destination
                    .OrderBy(x => x).ToList() // list of completeName to be created
                    .Contains(dbObject.Name)).ToList(); // object with completeName 
        }
        private List<T> DropDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toDrop = DropDbObject<T>(sourceObjects, destinationObjects);
            resultProcessDbObject.AddToDrop<T>(toDrop);

            toDrop.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                                .AppendLine(_schemaBuilder.BuildSeparator()));
            return toDrop;
        }
        private List<T> DropDbObject<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects) where T : DbObject
        {
            return destinationObjects
                .Where(dbObject => destinationObjects
                    .Select(x => x.Identifier)
                    .Except(sourceObjects.Select(x => x.Identifier)) //looking for complete name present in destination and absent from origin
                    .OrderBy(x => x).ToList() // list of completeName to be dropped
                    .Contains(dbObject.Identifier)) // object with completeName to be dropped
                .ToList();
        }
        private void DropDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects, TSqlResultProcessDbObject resultProcessDbObject) where T : DbObject
        {
            var toDrop = DropDbObjectByName<T>(sourceObjects, destinationObjects);
            resultProcessDbObject.AddToDrop<T>(toDrop);
            toDrop.ForEach(x => resultProcessDbObject.UpdateSchemaStringBuild
                                .AppendLine(_schemaBuilder.Build(x, Operation.Drop))
                                .AppendLine(_schemaBuilder.BuildSeparator()));
        }
        private List<T> DropDbObjectByName<T>(IEnumerable<T> sourceObjects, IEnumerable<T> destinationObjects) where T : DbObject
        {
            return destinationObjects
                .Where(dbObject => destinationObjects
                    .Select(x => x.Name)
                    .Except(sourceObjects.Select(x => x.Name)) //looking for complete name present in destination and absent from origin
                    .OrderBy(x => x).ToList() // list of completeName to be dropped
                    .Contains(dbObject.Name)) // object with completeName to be dropped
                .ToList();
        }
    }
}
