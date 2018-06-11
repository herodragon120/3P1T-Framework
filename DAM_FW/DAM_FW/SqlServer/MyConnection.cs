using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW.SqlServer
{
    public abstract class MyConnection
    {
        protected string _connectionStr;
        public abstract void Open();
        public abstract void Close();
        public abstract SqlWhere<T> Select<T>() where T : new();
        public abstract int Insert<T>(T obj) where T : new();
        public abstract int Update<T>(T obj) where T : new();
        public abstract int Delete<T>(T obj) where T : new();
    }
}
