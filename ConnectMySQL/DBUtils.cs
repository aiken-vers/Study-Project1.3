﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConnectMySQL
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "bankdata";
            string username = "root";
            string password = "123456Qq";
            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }
    }
}
