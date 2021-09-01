using SqlSchemaCompare.Core.DbStructures;
using System.Collections.Generic;
using System.Text;

namespace SqlSchemaCompare.Core.TSql
{
    public class TSqlResultProcessDbObject
    {

        public StringBuilder UpdateSchemaStringBuild { get; set; } = new();
        public List<DbObject> ToDrop { get; private set; } = new();
        public List<DbObject> ToAlter { get; private set; } = new();
        public List<DbObject> ToCreate { get; private set; } = new();
        public List<DbObject> ToEnable { get; private set; } = new();
        public List<DbObject> ToDisable { get; private set; } = new();

        public void AddToAlter<T>(IList<T> dbObjects) where T: DbObject
        {
            ToAlter.AddRange(dbObjects);
        }
        public void AddToCreate<T>(IList<T> dbObjects) where T : DbObject
        {
            ToCreate.AddRange(dbObjects);
        }
        public void AddToDrop<T>(IList<T> dbObjects) where T : DbObject
        {
            ToDrop.AddRange(dbObjects);
        }

        public void AddToAlter<T>(T dbObjects) where T : DbObject
        {
            ToAlter.Add(dbObjects);
        }
        public void AddToCreate<T>(T dbObjects) where T : DbObject
        {
            ToCreate.Add(dbObjects);
        }
        public void AddToDrop<T>(T dbObjects) where T : DbObject
        {
            ToDrop.Add(dbObjects);
        }
        public void AddToEnable<T>(T dbObjects) where T : DbObject
        {
            ToEnable.Add(dbObjects);
        }
        public void AddToDisable<T>(T dbObjects) where T : DbObject
        {
            ToDisable.Add(dbObjects);
        }
    }
}