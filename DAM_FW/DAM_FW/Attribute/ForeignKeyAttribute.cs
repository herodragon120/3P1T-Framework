using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW
{
    public class ForeignKeyAttribute : System.Attribute
    {
        private string _RelationshipID;
        public string RelationshipID
        {
            get { return _RelationshipID; }
            private set { _RelationshipID = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            private set { _Name = value; }
        }

        private string _References;
        public string References
        {
            get { return _References; }
            private set { _References = value; }
        }

        public ForeignKeyAttribute(string relationshipid, string name , string references)
        {
            this._Name = name;
            this._RelationshipID = relationshipid;
            this._References = references;
        }
    }
}
