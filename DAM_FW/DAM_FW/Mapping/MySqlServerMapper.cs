using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM_FW.Mapping
{
    public class MySqlServerMapper:Mapper
    {
        protected override void MapOneToMany<T>(SqlServer.MyConnection cnn, System.Data.DataRow dr, T obj)
        {
            throw new NotImplementedException();
        }

        protected override void MapToOne<T>(SqlServer.MyConnection cnn, System.Data.DataRow dr, T obj)
        {
            throw new NotImplementedException();
        }
    }
}
