using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataBase.DBInteract;

namespace DataBase
{
    internal class App
    {
        static void Main()
        {
            Console.WriteLine(db.clients.Find(450));
            Console.ReadLine();
        }
    }
}
