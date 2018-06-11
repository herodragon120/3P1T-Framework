using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW
{
    public class OneToOneAttribute : System.Attribute
    {
        private string _RelationshipID;
        public string RelationshipID
        {
            get { return _RelationshipID; }
            private set { _RelationshipID = value; }
        }

        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            private set { _TableName = value; }
        }
        public OneToOneAttribute(string relationshipid, string tablename)
        {
            this._RelationshipID = relationshipid;
            this._TableName = tablename;
        }
    }
}
