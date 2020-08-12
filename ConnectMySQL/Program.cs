using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConnectMySQL
{
    class Program
    {
        static void Main(string[] args)
        {
            SQL.Connect();
            Listener.StartListen();
            while (Console.ReadLine() != "exit") { }
        }
    }
}
