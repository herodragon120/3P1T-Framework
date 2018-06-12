using DAM_FW.SqlServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW.Mapping
{
    public abstract class Mapper
    {
        public T MapWithRelationship<T>(MyConnection cnn, DataRow dr) where T : new() 
        {
            T obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var columnMapping = FirstOrDefault(attributes, typeof(ColumnAttribute));

                if (columnMapping != null)
                {
                    var mapsTo = columnMapping as ColumnAttribute;
                    property.SetValue(obj, dr[mapsTo.Name]);
                }
            }

            MapOneToMany(cnn, dr, obj);
            MapToOne(cnn, dr, obj);

            return obj;
        }

        protected abstract void MapOneToMany<T>(MyConnection cnn, DataRow dr, T obj) where T : new();
        protected abstract void MapToOne<T>(MyConnection cnn, DataRow dr, T obj) where T : new();

        public string GetTableName<T>() where T : new()
        {
            var tableAttributes = typeof(T).GetCustomAttributes(typeof(TableAttribute), true);
            var tableAttribute = FirstOrDefault(tableAttributes, typeof(TableAttribute)) as TableAttribute;
            if (tableAttribute != null)
                return tableAttribute.Name;
            return string.Empty;
        }
        public List<PrimaryKeyAttribute> GetPrimaryKeys<T>() where T : new()
        {

            List<PrimaryKeyAttribute> primaryKeys = new List<PrimaryKeyAttribute>();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var primaryKeyAttribute = FirstOrDefault(attributes, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                    primaryKeys.Add(primaryKeyAttribute as PrimaryKeyAttribute);
            }

            if (primaryKeys.Count > 0)
                return primaryKeys;
            else
                return null;
        }
        public List<ForeignKeyAttribute> GetForeignKeys<T>(string relationshipID) where T : new()
        {
            List<ForeignKeyAttribute> foreignKeys = new List<ForeignKeyAttribute>();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var foreignKeyAttribute = FirstOrDefault(attributes, typeof(ForeignKeyAttribute));
                if (foreignKeyAttribute != null && (foreignKeyAttribute as ForeignKeyAttribute).RelationshipID == relationshipID)
                    foreignKeys.Add(foreignKeyAttribute as ForeignKeyAttribute);
            }

            if (foreignKeys.Count > 0)
                return foreignKeys;
            else
                return null;
        }
        public List<ColumnAttribute> GetColumns<T>() where T : new()
        {
            List<ColumnAttribute> columnAttributes = new List<ColumnAttribute>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var columnMapping = FirstOrDefault(attributes, typeof(ColumnAttribute));

                if (columnMapping != null)
                {
                    var mapsTo = columnMapping as ColumnAttribute;
                    columnAttributes.Add(mapsTo);
                }
            }

            if (columnAttributes.Count > 0)
                return columnAttributes;
            else
                return null;
        }
        public Dictionary<ColumnAttribute, object> GetColumnValues<T>(T obj) where T : new()
        {
            Dictionary<ColumnAttribute, object> listColumnValues = new Dictionary<ColumnAttribute, object>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var columnMapping = FirstOrDefault(attributes, typeof(ColumnAttribute));

                if (columnMapping != null)
                {
                    var mapsTo = columnMapping as ColumnAttribute;
                    listColumnValues.Add(mapsTo, property.GetValue(obj, null));
                }
            }

            if (listColumnValues.Count > 0)
                return listColumnValues;
            else
                return null;
        }
        public ColumnAttribute FindColumn(string name, Dictionary<ColumnAttribute, object> listColumValues)
        {
            foreach (ColumnAttribute column in listColumValues.Keys)
                if (column.Name == name)
                    return column;
            return null;
        }
        public ColumnAttribute FindColumn(string name, List<ColumnAttribute> listColumAttributes)
        {
            foreach (ColumnAttribute column in listColumAttributes)
                if (column.Name == name)
                    return column;
            return null;
        }
        protected object FirstOrDefault(object[] attributes, Type type)
        {
            foreach (var a in attributes)
            {
                if (a.GetType() == type)
                    return a;
            }
            return null;
        }
        protected object[] GetAll(object[] attributes, Type type)
        {
            object[] objArray = new object[0];
            foreach (var a in attributes)
            {
                if (a.GetType() == type)
                {
                    Array.Resize(ref objArray, objArray.Length + 1);
                    objArray[objArray.Length - 1] = a;
                }
            }
            return objArray;
        }
        public object GetFirst(IEnumerable source)
        {
            IEnumerator iter = source.GetEnumerator();

            if (iter.MoveNext())
            {
                return iter.Current;
            }
            return null;
        }
    }
}
