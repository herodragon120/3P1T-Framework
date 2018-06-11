using DAM_FW.SqlServer;
using System;
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
        { }
        protected abstract void MapOneToMany<T>(MyConnection cnn, DataRow dr, T obj) where T : new();
        protected abstract void MapToOne<T>(MyConnection cnn, DataRow dr, T obj) where T : new();

        public string GetTableName<T>() where T : new()
        { }
        public List<PrimaryKeyAttribute> GetPrimaryKeys<T>() where T : new()
        { }
        public List<ForeignKeyAttribute> GetForeignKeys<T>(string relationshipID) where T : new()
        { }
        public List<ColumnAttribute> GetColumns<T>() where T : new()
        { }
        public Dictionary<ColumnAttribute, object> GetColumnValues<T>(T obj)
        { }
        public ColumnAttribute FindColumn(string name, Dictionary<ColumnAttribute, object> listColumValues)
        { }
        public ColumnAttribute FindColumn(string name, List<ColumnAttribute> listColumAttributes)
        { }
        protected object FirstOrDefault(object[] attributes, Type type)
        { }
        protected object[] GetAll(object[] attributes, Type type)
        { }
        public object GetFirst(IEnumerable source)
        { }
    }
}
