using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConnectMySQL
{
    static class SQL
    {
        static MySqlConnection conn = DBUtils.GetDBConnection();
        static MySqlCommand cmd = new MySqlCommand();
        static MySqlDataReader reader;
        static string sql;
        public static void Connect()
        {           
            try
            {
                Console.WriteLine("Openning Connection ...");

                conn.Open();
                cmd.Connection = conn;

                Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }        
        public static string execute_sql(string sql)
        {
            try
            {
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                reader.Close();
                return "success!";
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string read_sql(string sql)
        {
            try
            {
                //Console.WriteLine(sql);
                string read="success;";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string val = Convert.ToString(reader.GetValue(0));
                        read += val;
                    }
                }
                else { read = "fail"; }
                reader.Close();
                return read;
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string execute  (string data)
        {
            try
            {                
                cmd.CommandText = data;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                reader.Close();
                return "success!";
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string auth(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                string sql = String.Format("SELECT auth('{0}', '{1}') AS idclient;", words[1], words[2]);
                //string sql = String.Format("SELECT idclient from logins WHERE login='{0}' AND password='{1}';", words[1], words[2]);
                string result=read_sql(sql);
                string[] check_client = result.Split(new Char[] { ';' });
                int check_id = 0;
                int.TryParse(check_client[1], out check_id);                
                result += ";client";

                if (check_id<=0)
                {
                    sql = String.Format("SELECT idworker from logins WHERE login='{0}' AND password='{1}';", words[1], words[2]);
                    result = read_sql(sql);
                    result += ";worker";
                }
                Console.WriteLine(result);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string userdata(string data)
        {
            try
            {
                Console.WriteLine(data);
                string result, fio=null, passport=null, address=null, number=null, email=null;
                string[] words = data.Split(new Char[] { ' ' });
                string sql = String.Format("SELECT * from clients WHERE idclients={0};", words[1]);
                Console.WriteLine(sql);
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fio = Convert.ToString(reader.GetValue(1));
                        passport = Convert.ToString(reader.GetValue(2));
                        address = Convert.ToString(reader.GetValue(3));
                        number = Convert.ToString(reader.GetValue(4));
                        email = Convert.ToString(reader.GetValue(5));
                    }
                }
                reader.Close();
                result = String.Format("{0};{1};{2};{3};{4}",fio, passport, address, number, email);
                Console.WriteLine(result);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string workerdata(string data)
        {
            try
            {
                Console.WriteLine(data);
                string result, fio = null, passport = null, access = null;
                string[] words = data.Split(new Char[] { ' ' });
                string sql = String.Format("SELECT * from workers WHERE idworkers={0};", words[1]);
                Console.WriteLine(sql);
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fio = Convert.ToString(reader.GetValue(1));
                        passport = Convert.ToString(reader.GetValue(3));
                        access = Convert.ToString(reader.GetValue(2));                        
                    }
                }
                reader.Close();
                result = String.Format("{0};{1};{2}", fio, passport, access);
                Console.WriteLine(result);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string usercards(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                string text = "";
                sql = "SELECT iddeposits, type, pincode, cvv2_code, duration, balance from cards WHERE idclient = " + words[1] + ";";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string AccountId = Convert.ToString(reader.GetValue(0));
                        string type = Convert.ToString(reader.GetValue(1));
                        string pincode = Convert.ToString(reader.GetValue(2));
                        string cvv2 = Convert.ToString(reader.GetValue(3));
                        string duration = Convert.ToString(reader.GetValue(4));
                        string balance = Convert.ToString(reader.GetValue(5));
                        text += String.Format("{0},{1},{2},{3},{4},{5};",
                            AccountId, type, pincode, cvv2, duration, balance);
                    }
                }
                else { text = "no accounts"; }
                Console.WriteLine(text);
                reader.Close();
                return text;
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return e.Message;
            }
        }
        public static string Insert_card(string data)
        {
            try
            {
                string sql = null;
                string[] words = data.Split(new Char[] { ';' });
                if (words.Length == 4)
                {
                    sql = String.Format("insert into cards(idclient, type, duration) values({0}, '{1}', card_duration({2}));",
                    words[1], words[2], words[3]);
                }
                else if(words.Length == 5)
                {
                    sql = String.Format("insert into cards(idclient, type, duration, balance) values({0}, '{1}', card_duration({2}), {3});",
                    words[1], words[2], words[3], words[4]);
                }
                Console.WriteLine(sql);
                string result = execute_sql(sql);                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Insert_dep(string data)
        {
            try
            {
                string sql = null;
                string[] words = data.Split(new Char[] { ';' });
                if (words.Length == 4)
                {
                    sql = String.Format("insert into deposits(idclient, type, duration) values({0}, '{1}', card_duration({2}));",
                    words[1], words[2], words[3]);
                }
                else if (words.Length == 5)
                {
                    if(words[2]== "кредитный")
                    sql = String.Format("insert into deposits(idclient, type, duration, balance, refill) values({0}, '{1}', card_duration({2}), {3}, 0);",
                    words[1], words[2], words[3], words[4]);
                    else if(words[2] == "накопительный")
                        sql = String.Format("insert into deposits(idclient, type, duration, balance, cut) values({0}, '{1}', card_duration({2}), {3}, 0);",
                    words[1], words[2], words[3], words[4]);
                }
                Console.WriteLine(sql);
                string result = execute_sql(sql);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Del_card(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                sql = "DELETE FROM `bankdata`.`cards` WHERE iddeposits= " + words[1] + ";";
                Console.WriteLine(sql);
                string result = execute_sql(sql);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Del_dep(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                sql = "DELETE FROM `bankdata`.`deposits` WHERE iddeposits= " + words[1] + ";";
                Console.WriteLine(sql);
                string result = execute_sql(sql);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string userdeps(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                string text = "";
                
                sql = "SELECT iddeposits, type, balance, duration, refill, cut from deposits WHERE idclient = " + words[1] + ";";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string AccountId = Convert.ToString(reader.GetValue(0));
                        string type = Convert.ToString(reader.GetValue(1));
                        string balance = Convert.ToString(reader.GetValue(2));
                        string duration = Convert.ToString(reader.GetValue(3));
                        string refill = Convert.ToString(reader.GetValue(4));
                        string cut = Convert.ToString(reader.GetValue(5));
                        text += String.Format("{0}!{1}!{2}!{3}!{4}!{5};",
                            AccountId, type,balance, duration, refill, cut);
                    }
                }
                else { text = "no deps"; }
                Console.WriteLine(text);
                reader.Close();
                return text;
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return e.Message;
            }
        }
        public static string userlogs(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                string text = "";

                sql = "select event, date, idevent, balance, operation from journal WHERE idclient =" + words[1] + ";";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string events= Convert.ToString(reader.GetValue(0));
                        string date = Convert.ToString(reader.GetValue(1));
                        string idevent = Convert.ToString(reader.GetValue(2));
                        string balance = Convert.ToString(reader.GetValue(3));
                        string operation = Convert.ToString(reader.GetValue(4));
                        text += String.Format("{0}!{1}!{2}!{3}!{4};",
                            events, date, idevent, balance, operation);
                    }
                }
                else { text = "no logs"; }
                Console.WriteLine(text);
                reader.Close();
                return text;
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return e.Message;
            }
        }
        public static string Insert_user(string data)
        {
            try
            {
                // код 11 - только clients && полный, код 13 - фио, паспорт, адрес
                // код 14 - 13+номер, код 15 - 13+email
                string[] words = data.Split(new Char[] { ';' });
                string sql = null;                 
                int n = words.Length;

                if (words[0].Contains("11"))
                sql = String.Format("INSERT INTO `bankdata`.`clients` (`FIO`, `passportid`, `Address`, `number`, `email`)" +
                        " VALUES('{0}', {1}, '{2}', {3}, '{4}');",
                        words[1], words[2], words[3], words[4], words[5]);
                else if (words[0].Contains("13"))
                    sql = String.Format("INSERT INTO `bankdata`.`clients` (`FIO`, `passportid`, `Address`)" +
                        " VALUES('{0}', {1}, '{2}');",
                        words[1], words[2], words[3]);
                else if (words[0].Contains("14"))
                    sql = String.Format("INSERT INTO `bankdata`.`clients` (`FIO`, `passportid`, `Address`, `number`)" +
                        " VALUES('{0}', {1}, '{2}', {3});",
                        words[1], words[2], words[3], words[4]);
                else if (words[0].Contains("15"))
                    sql = String.Format("INSERT INTO `bankdata`.`clients` (`FIO`, `passportid`, `Address`, `email`)" +
                        " VALUES('{0}', {1}, '{2}', '{3}');",
                        words[1], words[2], words[3], words[4]);
                Console.WriteLine(sql);
                string result = execute_sql(sql);

                string user_id = get_id(words[2]);                
                if (words[n-1].Length > 4)
                {
                    string sql2 = "INSERT INTO `bankdata`.`logins` (`login`, `password`, `idclient`) VALUES ('" + words[n-2] + "', '" + words[n-1] + "', '" + user_id + "');";
                    Console.WriteLine(sql2);
                    result = execute_sql(sql2);
                }                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Delete_user(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                string sql = null;

                if (int.TryParse(words[1], out int r))
                    sql = String.Format("DELETE FROM `bankdata`.`clients` WHERE idclients=" +
                            " '{0}';",
                            words[1]);

                Console.WriteLine(sql);
                string result = execute_sql(sql);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string reduser(string data)
        {
            try
            {
                // код 0 - logins полный, код 0- - без пароля
                // код 11 - только clients && полный, код 13 - фио, паспорт, адрес
                // код 14 - 13+номер, код 15 - 13+email
                string[] words = data.Split(new Char[] { ';' });
                string sql = null;
                int n = words.Length;
                if (words[0].Contains("11"))
                    sql = String.Format("UPDATE clients SET FIO='{1}', passportid={2}, Address='{3}', number={4}, email='{5}' WHERE idclients={0};",
                    words[1], words[2], words[3], words[4], words[5], words[6]);
                if (words[0].Contains("13"))
                    sql = String.Format("UPDATE clients SET FIO='{1}', passportid={2}, Address='{3}' WHERE idclients={0};",
                    words[1], words[2], words[3], words[4]);
                if (words[0].Contains("14"))
                    sql = String.Format("UPDATE clients SET FIO='{1}', passportid={2}, Address='{3}', number={4} WHERE idclients={0};",
                    words[1], words[2], words[3], words[4], words[5]);
                if (words[0].Contains("15"))
                    sql = String.Format("UPDATE clients SET FIO='{1}', passportid={2}, Address='{3}', email='{4}' WHERE idclients={0};",
                    words[1], words[2], words[3], words[4], words[5]);
                Console.WriteLine(sql);
                string result = execute_sql(sql);
                if (words[0].EndsWith("0"))
                { 
                    if (words[n-1].Length > 4)
                    {
                        string sql2 = String.Format("UPDATE logins SET login='{0}', password='{1}' WHERE idclient={2};",
                            words[n-2], words[n-1], words[1]);
                        Console.WriteLine(sql2);
                        result = execute_sql(sql2);
                    }
                }
                else if (words[0].EndsWith("0-"))
                {
                    string sql2 = String.Format("UPDATE logins SET login='{0}' WHERE idclient={1};",
                            words[n-1], words[1]);
                    Console.WriteLine(sql2);
                    result = execute_sql(sql2);
                }  
               
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Money(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ';' });
                string sql = null;
                int id = Int32.Parse(words[1]);
                int money = Int32.Parse(words[2]);
                if(id<62120000)
                {
                    if (money > 0) sql = string.Format("select money_dep_add({0},{1});", id, Math.Abs(money));
                    else if(money<0) sql = string.Format("select money_dep_sub({0},{1});", id, Math.Abs(money));
                }
                if (id > 62120000)
                {
                    if (money > 0) sql = string.Format("select money_card_add({0},{1});", id, Math.Abs(money));
                    else if (money < 0) sql = string.Format("select money_card_sub({0},{1});", id, Math.Abs(money));
                }
                string result = execute_sql(sql);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Transaction(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ';' });
                string sql = null;
                int id1 = Int32.Parse(words[1]);
                int id2 = Int32.Parse(words[2]);
                int money = Int32.Parse(words[3]);
                if(words[0]== "/tran")
                    sql = string.Format("select transact({0},{1},{2});", id1, id2, Math.Abs(money));
                else if(words[0] == "/tran_phone")
                    sql = string.Format("select tran_phone({0},{1},{2});", id1, id2, Math.Abs(money));
                
                string result = execute_sql(sql);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Delete_worker(string data)
        {
            try
            {
                string[] words = data.Split(new Char[] { ' ' });
                if (words[1] != "1")
                {
                    sql = "DELETE FROM `bankdata`.`workers` WHERE idworkers= " + words[1] + " ;";
                    Console.WriteLine(sql);
                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                    }
                    reader.Close();
                    return "success!";
                }
                else return "fail";

            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Insert_worker(string data)
        {
            try
            {
                // код 11 - только clients && полный, код 13 - фио, паспорт, адрес
                // код 14 - 13+номер, код 15 - 13+email
                string[] words = data.Split(new Char[] { ';' });
                string sql = null;
                int n = words.Length;

                sql = String.Format("INSERT INTO `bankdata`.`workers` (`FIO`, `access`, `passportid`)" +
                            " VALUES('{0}', {1}, {2});",
                            words[1], words[2], words[3]);
                
                Console.WriteLine(sql);
                string result = execute_sql(sql);
                string user_id = get_id_worker(words[3]);

                if (words[5].Length > 4)
                {
                    string sql2 = "INSERT INTO `bankdata`.`logins` (`login`, `password`, `idworker`) VALUES ('" + words[4] + "', '" + words[5] + "', '" + user_id + "');";
                    Console.WriteLine(sql2);
                    result = execute_sql(sql2);
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return "fail";
            }
        }
        public static string Workers(string data)
        {
            try
            {
                string text = "";
                sql = "SELECT * from workers;";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string id = Convert.ToString(reader.GetValue(0));
                        string name = Convert.ToString(reader.GetValue(1));
                        string access = Convert.ToString(reader.GetValue(2));
                        string passport = Convert.ToString(reader.GetValue(3));
                        string worker = id+";"+name + ";" + access+";"+passport;
                        text += worker + "!";
                    }
                }
                else { text = "no workers"; }
                Console.WriteLine(text);
                reader.Close();
                return text;
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return null;
            }
        }
        public static string Clients(string data)
        {
            try
            {
                string text = "";
                sql = "SELECT * from clients;";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string id = Convert.ToString(reader.GetValue(0));
                        string fio = Convert.ToString(reader.GetValue(1));
                        string passport = Convert.ToString(reader.GetValue(2));
                        string address = Convert.ToString(reader.GetValue(3));
                        string number = Convert.ToString(reader.GetValue(4));
                        string email = Convert.ToString(reader.GetValue(5));
                        string client = String.Format("{0};{1};{2};{3};{4};{5}", id, fio, passport, address, number, email);
                        text += client + "!";
                    }
                }
                else { text = "no clients"; }
                Console.WriteLine(text);
                reader.Close();
                return text;
            }
            catch (Exception e)
            {
                reader.Close();
                Console.WriteLine("Command Error: " + e.Message);
                return null;
            }
        }

        public static string get_id(string pass)
        {
            try
            {
                sql = "SELECT idclients from clients WHERE passportid="+pass+";";
                string text = read_sql(sql);
                string[] id = text.Split(new Char[] { ';' });
                return id[1];
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return null;
            }
        }
        public static string get_id_worker(string pass)
        {
            try
            {
                sql = "SELECT idworkers from workers WHERE passportid=" + pass + ";";
                string text = read_sql(sql);
                string[] id = text.Split(new Char[] { ';' });
                return id[1];
            }
            catch (Exception e)
            {
                Console.WriteLine("Command Error: " + e.Message);
                return null;
            }
        }
    }
}
