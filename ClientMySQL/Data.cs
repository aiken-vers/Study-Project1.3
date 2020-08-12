using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;

namespace ClientMySQL
{
    public class Data
    {
        public class logs
        {
            public DateTime date { get; set; }
            public string idclient { get; set; }
            public string events { get;set; }
            public string idevent { get; set; }
            public string balance { get; set; }
            public string operation { get; set; }

            public logs(string d, string e, string i=null, string b=null, string o=null, string idc="0")
            {
                date = DateTime.Parse(d);
                events = e;
                idevent = i;
                balance = b;
                operation = o;
                idclient = idc;
            }
        }
        public class clients
        {
            public string id { get; set; }
            public string fio { get; set; }
            public string passport { get; set; }
            public string address { get; set; }
            public string number { get; set; }           
            public string email { get; set; }
            public clients(string i, string f, string p, string a, string n, string e)
            {
                id = i;
                fio = f;
                passport = p;
                address = a;
                number = n;
                email = e;
            }
            public string toList(List<Data.cards> cards, List<Data.deps> deps)
            {
                int cards_count = 0;
                int deps_count = 0;
                foreach(Data.cards card in cards)
                {
                    if (card.idclient == id)
                        cards_count++;
                }
                foreach (Data.deps dep in deps)
                {
                    if (dep.idclient == id)
                        deps_count++;
                }
                string local_fio=fio.PadRight(50);
                return string.Format("{0}: {1} карты:{2}, вклады:{3}",
                    id, local_fio, cards_count, deps_count);
            }
        }
        public class workers
        {
            public string id { get; set; }
            public string fio { get; set; }
            public string passport { get; set; }
            public string access { get; set; }
            public workers(string i, string f, string a, string p)
            {
                id = i;
                fio = f;
                passport = p;
                access = a;
            }
            public override string ToString()
            {
                string status = "обычный";
                if (access == "True")
                    status = "Привилегированный";
                return string.Format("{0}: {1}, {2}",
                    id, fio, status);
            }
        }
        public class cards
        {
            public string AccountId { get; set; }
            public string idclient { get; set; }
            public string type { get; set; }
            public string pincode { get; set; }
            public string cvv2 { get; set; }
            public string duration;
            public string Duration
            {
                get { return this.duration; }
                set {
                    string[] d = value.Split(new Char[] { ' ' });
                    this.duration = d[0];
                }
            }
            public string balance { get; set; }
            public cards(string a, string t, string p, string c, string d, string b, string i="0")
            {
                AccountId = a;
                type = t;
                pincode = p;
                cvv2 = c;
                Duration = d;
                balance = b;
                idclient = i;
            }
        }
        public class deps
        {
            public string AccountId { get; set; }
            public string idclient { get; set; }
            public string type { get; set; }
            public string duration;
            public string Duration
            {
                get { return this.duration; }
                set
                {
                    string[] d = value.Split(new Char[] { ' ' });
                    this.duration = d[0];
                }
            }
            public string balance { get; set; }
            public bool refill { get; set; }
            public bool cut { get; set; }
            public deps(string a, string t, string b, string d, string r, string c, string i="0")
            {
                AccountId = a;
                type = t;
                Duration = d;
                balance = b;
                idclient = i;
                if (r == "True") refill = true;
                else refill = false;
                if (c == "True") cut = true;
                else cut = false;
            }
        }
        public static string GetMD5(string input)
        {
            byte[] Data = Encoding.ASCII.GetBytes(input);
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Data)).Replace("-", string.Empty);
        }
    }
}