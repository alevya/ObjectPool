using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectPool;

namespace TestMain
{
    internal class ConnectionPool : ObjectPool<Connection>
    {
        protected override Connection _create()
        {
            return null;
        }

        public override bool Validate(Connection obj)
        {
            return true;
        }

        public override void Expire(Connection obj)
        {
            
        }
    }

    internal class Connection
    {
        
    }
}
