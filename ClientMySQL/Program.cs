using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace ClientMySQL
{
    static class Program
    {
        public static Client client;
        public static Login login;
        public static Register reg1;
        public static Register2 reg2;
        public static Register3 reg3;
        public static new_card new_card;
        public static new_dep new_dep;
        public static checkPass check;
        public static Worker worker;
        public static ShowUser ShowUser;
        public static Mutex Mone=new Mutex();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());            

        }
    }
}
