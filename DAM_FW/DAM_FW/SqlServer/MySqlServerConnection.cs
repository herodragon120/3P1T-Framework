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

        public override SqlWhere<T> Select<T>()
        {
            throw new NotImplementedException();
        }

        public override int Insert<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public override int Update<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public override int Delete<T>(T obj)
        {
            MySqlServerMapper mapper = new MySqlServerMapper();
            string tableName = mapper.GetTableName<T>();
            List<PrimaryKeyAttribute> primaryKeys = mapper.GetPrimaryKeys<T>();
            Dictionary<ColumnAttribute, object> listColumnValues = mapper.GetColumnValues<T>(obj);

            string whereStr = string.Empty;
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

                    whereStr += string.Format(format, primaryKey.Name, listColumnValues[column]);
                }
            }
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = whereStr.Substring(0, whereStr.Length - 2);
                _query = string.Format("DELETE {0} WHERE {1}", tableName, whereStr);
            } 
        }

        protected int ExecuteNonQuery()
        {
            _cmd.CommandText = _query;
            return _cmd.ExecuteNonQuery();
        }
        //protected List<T> ExecuteQuery<T>() where T : new()
        //{
        //    _cmd.CommandText = _query;

        //    DataTable dt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(_cmd);
        //    adapter.Fill(dt);

        //    List<T> res = new List<T>();
        //    SCOSqlConnection cnn = new SCOSqlConnection(_connectionString);
        //    SqlMapper mapper = new SqlMapper();

        //    foreach (DataRow dr in dt.Rows)
        //        res.Add(mapper.MapWithRelationship<T>(cnn, dr));

        //    return res;
        //}
    }
}
