using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW
{
    public class PrimaryKeyAttribute : System.Attribute
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        private bool _AutoID;
        public bool AutoID
        {
            get { return _AutoID; }
            private set { _AutoID = value; }
        }
        PrimaryKeyAttribute (string name , bool autoid)
        {
            this._AutoID = autoid;
            this._Name = name;
        }
    }
}
