using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace ClientMySQL
{
    class SocketClient
    {
        static string Thismessage;
        static int port = 11000;
        public static int ip = 1;
        //static byte[] bytes = new byte[1024];
        static byte[] bytes = new byte[8192];
        static Socket sender;
        static IPEndPoint ipEndPoint;
        static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        static RSAParameters privateKey;
        static RSAParameters publicKey;
        static string public_key, private_key;

        public static List<String> full_combo()
        {  
            IPHostEntry ipHost2 = Dns.GetHostEntry("Owltrooper");
            List<String> ips = new List<String>();
            foreach(IPAddress ip in ipHost2.AddressList)
            {
                ips.Add(ip.ToString());
            }
            return ips;
        }

        public static void Connect()
        {
            try
            {
                // Устанавливаем удаленную точку для сокета
                IPHostEntry ipHost = Dns.GetHostEntry("Owltrooper");
                IPAddress ipAddr = ipHost.AddressList[1];
                ipEndPoint = new IPEndPoint(ipAddr, port);
                sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Соединяем сокет с удаленной точкой
            }
            catch { }
        }
        public static string SendMessage(string message)
        {
            try
            {
                //public_key = Application.StartupPath + "\\public_key.txt";
                //private_key = Application.StartupPath + "\\private_key.txt";
                //import_keys(public_key, private_key);

                IPHostEntry ipHost = Dns.GetHostEntry("Owltrooper");
                IPAddress ipAddr = ipHost.AddressList[ip];
                ipEndPoint = new IPEndPoint(ipAddr, port);
                sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(ipEndPoint);
                byte[] msg = Encoding.UTF8.GetBytes(message);
                //byte[] msg = RSA_encrypt(message);
                // Отправляем данные через сокет
                int bytesSent = sender.Send(msg);
                // Получаем ответ от сервера
                int bytesRec = sender.Receive(bytes);
                // Освобождаем сокет
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                return Encoding.UTF8.GetString(bytes, 0, bytesRec);
                //return RSA_decrypt(bytes);
            }
            catch(Exception e) { return "fail"; }
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
