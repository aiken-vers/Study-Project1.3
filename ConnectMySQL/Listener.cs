using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;

namespace ConnectMySQL
{
    class Listener
    {
        static string reply;
        static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        static RSAParameters publicKey;
        static RSAParameters privateKey;

        public static void StartListen()
        {
            try
            {
                string public_key = AppDomain.CurrentDomain.BaseDirectory + "\\public_key.txt";
                string private_key = AppDomain.CurrentDomain.BaseDirectory + "\\private_key.txt";
                import_keys(public_key, private_key);
                //string Host = "localhost";
                string Host = "Owltrooper";
                // Устанавливаем для сокета локальную конечную точку
                IPHostEntry ipHost = Dns.GetHostEntry(Host);
                //IPAddress ipAddr = ipHost.AddressList[1];
                //IPAddress ipAddr = ipHost.AddressList[9];
                //IPAddress ipAddr = IPAddress.Parse("192.168.1.2");
                Console.WriteLine("Список адресов хоста: "+Host);
                int i = -1;
                foreach (IPAddress ip in ipHost.AddressList)
                {
                    i++;
                    Console.WriteLine("["+i+"] "+ip);                    
                }
                IPAddress ipAddr = ipHost.AddressList[i];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
                // Создаем сокет Tcp/Ip
                Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;
                    // Мы дождались клиента, пытающегося с нами соединиться
                    //byte[] bytes = new byte[1024];
                    byte[] bytes = new byte[8192];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    //data += RSA_decrypt(bytes);

                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n\n");
                    if (data.StartsWith("whoami")) reply = "you are a user";
                    else if (data.StartsWith("/auth")) reply = SQL.auth(data);
                    else if (data.StartsWith("/userdata")) reply = SQL.userdata(data);
                    else if (data.StartsWith("/workerdata")) reply = SQL.workerdata(data);
                    else if (data.StartsWith("/reduser")) reply = SQL.reduser(data);
                    else if (data.StartsWith("/usercards")) reply = SQL.usercards(data);
                    else if (data.StartsWith("/new_card")) reply = SQL.Insert_card(data);
                    else if (data.StartsWith("/money")) reply = SQL.Money(data);
                    else if (data.StartsWith("/tran")) reply = SQL.Transaction(data);
                    else if (data.StartsWith("/tran_phone")) reply = SQL.Transaction(data);
                    else if (data.StartsWith("/new_dep")) reply = SQL.Insert_dep(data);
                    else if (data.StartsWith("/del_card")) reply = SQL.Del_card(data);
                    else if (data.StartsWith("/del_dep")) reply = SQL.Del_dep(data);
                    else if (data.StartsWith("/userdeps")) reply = SQL.userdeps(data);
                    else if (data.StartsWith("/userlogs")) reply = SQL.userlogs(data);
                    else if (data.StartsWith("/adduser")) reply = SQL.Insert_user(data);
                    else if (data.StartsWith("/deluser")) reply = SQL.Delete_user(data);
                    else if (data.StartsWith("/addworker")) reply = SQL.Insert_worker(data);
                    else if (data.StartsWith("/delworker")) reply = SQL.Delete_worker(data);
                    else if (data.StartsWith("/showorkers")) reply = SQL.Workers(data);
                    else if (data.StartsWith("/showclients")) reply = SQL.Clients(data);
                    else reply = SQL.execute(data);
                    //else reply = "unknown command";

                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    //byte[] msg = RSA_encrypt(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");                        
                        break;
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
        public static void import_keys(string kpublic, string kprivate)
        {
            StreamReader sr = new StreamReader(kpublic);
            string keytxt = sr.ReadToEnd();
            RSA.FromXmlString(keytxt);
            sr.Close();

            sr = new StreamReader(kprivate);
            keytxt = sr.ReadToEnd();
            RSA.FromXmlString(keytxt);
            sr.Close();
        }

        public static byte[] RSA_encrypt(string toEncrypt)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] encBytes = RSA.Encrypt(byteConverter.GetBytes(toEncrypt), false);
            return encBytes;
        }
        public static string RSA_decrypt(byte[] encBytes)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] decBytes = RSA.Decrypt(encBytes, false);
            return byteConverter.GetString(decBytes);
        }

    }
}
