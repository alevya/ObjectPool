using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMain
{
    class Program
    {
        static void Main(string[] args)
        {
            var connPool = new ConnectionPool();
            var conn = connPool.CheckOut();

            Console.ReadLine();

            connPool.CheckIn(conn);
        }
    }
}
