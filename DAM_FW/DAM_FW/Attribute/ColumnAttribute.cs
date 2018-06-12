using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW
{
    public class ColumnAttribute : System.Attribute
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        private DataType _Type;
        public DataType Type
        {
            get { return _Type; }
            private set { _Type = value; }
        }
        public ColumnAttribute(string name, DataType type)
        {
            this._Name = name;
            this._Type = type;
        }

    }
}
