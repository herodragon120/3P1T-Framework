using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW
{
    public class TableAttribute : System.Attribute
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }
        public TableAttribute(string name)
        {
            this._Name = name;
        }
    }
}
