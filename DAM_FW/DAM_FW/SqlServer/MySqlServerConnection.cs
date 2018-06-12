using DAM_FW.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW.SqlServer
{
    public class MySqlServerConnection:MyConnection
    {
        private SqlConnection _cnn;
        private SqlCommand _cmd;
        private string _query;
        private MySqlServerConnection(string connectionStr)
        {
            this._connectionStr = connectionStr;
            _cnn = new SqlConnection(this._connectionStr);

            _cmd = new SqlCommand();
            _cmd.Connection = _cnn;
            _cmd.CommandType = CommandType.Text;
        }
        public override void Open()
        {
            if (_cnn.State != System.Data.ConnectionState.Open)
                _cnn.Open();
        }

        public override void Close()
        {
            if (_cnn.State == System.Data.ConnectionState.Open)
                _cnn.Close();
        }

        public override int Delete<T>(T obj)
        {
            MySqlServerMapper mapper = new MySqlServerMapper();
            string tableName = mapper.GetTableName<T>();
            List<PrimaryKeyAttribute> primaryKeys = mapper.GetPrimaryKeys<T>();
            Dictionary<ColumnAttribute, object> listColumnValues = mapper.GetColumnValues<T>(obj);

            string query = string.Empty;
            foreach (PrimaryKeyAttribute primaryKey in primaryKeys)
            {
                //tìm cot khóa chính
                ColumnAttribute column = mapper.FindColumn(primaryKey.Name, listColumnValues);
                if (column != null)
                {
                    string format = "{0} = {1}, ";
                    if (column.Type == DataType.NCHAR || column.Type == DataType.NVARCHAR)
                        format = "{0} = N'{1}', ";
                    else if (column.Type == DataType.CHAR || column.Type == DataType.VARCHAR)
                        format = "{0} = '{1}', ";

                    query += string.Format(format, primaryKey.Name, listColumnValues[column]);
                }
            }
            if (!string.IsNullOrEmpty(query))
            {
                query = query.Substring(0, query.Length - 2);
                query = string.Format("DELETE {0} WHERE {1}", tableName, query);
            }
            return ExecuteNonQuery(query);
        }
        public override int Insert<T>(T obj)
        {
            MySqlServerMapper mapper = new MySqlServerMapper();

            string tableName = mapper.GetTableName<T>();
            List<PrimaryKeyAttribute> primaryKeys = mapper.GetPrimaryKeys<T>();
            Dictionary<ColumnAttribute, object> listColumnNameValues = mapper.GetColumnValues<T>(obj);
            string query = string.Empty;
            if (listColumnNameValues.Count != 0)
            {
                string columnStr = string.Empty;
                string valueStr = string.Empty;

                foreach (ColumnAttribute column in listColumnNameValues.Keys)
                {
                    bool isAutoID = false;
                    foreach (PrimaryKeyAttribute primaryKey in primaryKeys)
                    {
                        if (column.Name == primaryKey.Name && primaryKey.AutoID)
                        {
                            isAutoID = true;
                            break;
                        }
                    }

                    if (!isAutoID)
                    {
                        string format = "{0}, ";
                        if (column.Type == DataType.NCHAR || column.Type == DataType.NVARCHAR)
                            format = "N'{0}', ";
                        else if (column.Type == DataType.CHAR || column.Type == DataType.VARCHAR)
                            format = "'{0}', ";
                        columnStr += string.Format("{0}, ", column.Name);
                        valueStr += string.Format(format, listColumnNameValues[column]);
                    }
                }
                if (!string.IsNullOrEmpty(columnStr) && !string.IsNullOrEmpty(valueStr))
                {
                    columnStr = columnStr.Substring(0, columnStr.Length - 2);
                    valueStr = valueStr.Substring(0, valueStr.Length - 2);
                    query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, columnStr, valueStr);
                }
            }
            return ExecuteNonQuery(query);
        }

        public override int Update<T>(T obj)
        {
            MySqlServerMapper mapper = new MySqlServerMapper();

            string tableName = mapper.GetTableName<T>();
            List<PrimaryKeyAttribute> primaryKeys = mapper.GetPrimaryKeys<T>();
            Dictionary<ColumnAttribute, object> listColumnValues = mapper.GetColumnValues<T>(obj);
            string query = string.Empty;
            if (listColumnValues != null && primaryKeys != null)
            {
                string setStr = string.Empty;

                foreach (ColumnAttribute column in listColumnValues.Keys)
                {
                    string format = "{0} = {1}, ";
                    if (column.Type == DataType.NCHAR || column.Type == DataType.NVARCHAR)
                        format = "{0} = N'{1}', ";
                    else if (column.Type == DataType.CHAR || column.Type == DataType.VARCHAR)
                        format = "{0} = '{1}', ";

                    setStr += string.Format(format, column.Name, listColumnValues[column]);
                }
                if (!string.IsNullOrEmpty(setStr))
                    setStr = setStr.Substring(0, setStr.Length - 2);

                foreach (PrimaryKeyAttribute primaryKey in primaryKeys)
                {
                    ColumnAttribute column = mapper.FindColumn(primaryKey.Name, listColumnValues);
                    if (column != null)
                    {
                        string format = "{0} = {1}, ";
                        if (column.Type == DataType.NCHAR || column.Type == DataType.NVARCHAR)
                            format = "{0} = N'{1}', ";
                        else if (column.Type == DataType.CHAR || column.Type == DataType.VARCHAR)
                            format = "{0} = '{1}', ";

                        query += string.Format(format, primaryKey.Name, listColumnValues[column]);
                    }
                }
                if (!string.IsNullOrEmpty(query))
                {
                    query = query.Substring(0, query.Length - 2);
                    query = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, setStr, query);
                }
            }
            return ExecuteNonQuery(query);
        }
        public override SqlWhere<T> Select<T>()
        {
            throw new NotImplementedException();
        }


        protected int ExecuteNonQuery(string query)
        {
            _cmd.CommandText = query;
            return _cmd.ExecuteNonQuery();
        }
        protected List<T> ExecuteQuery<T>(string query) where T : new()
        {
            _cmd.CommandText = query;

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(_cmd);
            adapter.Fill(dt);

            List<T> res = new List<T>();
            MySqlServerConnection cnn = new MySqlServerConnection(_connectionStr);
            MySqlServerMapper mapper = new MySqlServerMapper();

            foreach (DataRow dr in dt.Rows)
                res.Add(mapper.MapWithRelationship<T>(cnn, dr));

            return res;
        }
    }
}
