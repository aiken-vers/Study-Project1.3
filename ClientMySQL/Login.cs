using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMySQL
{
    public partial class Login : Form
    {
        public static string auth;
        public static string login;
        static int index=1;
        public Login()
        {
            //SocketClient.Connect();
            InitializeComponent();
            List<String> ips = SocketClient.full_combo();
            foreach(string ip in ips)
            {
                server.Items.Add(ip);
            }
            server.SelectedIndex = server.Items.Count - 1;
            server.SelectedIndex = index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=null&&textBox2.Text != null)
            {
                Login.login = textBox1.Text;
                //string pass = Data.GetMD5(textBox2.Text);
                string pass = textBox2.Text;
                auth = SocketClient.SendMessage("/auth " + textBox1.Text+" "+ pass);
                if (auth.StartsWith("success"))
                {
                    if (auth.Contains("client"))
                    {
                        Program.client = new Client();
                        Program.client.Show();
                        this.Hide();
                    }
                    else if (auth.Contains("worker"))
                    {
                        Program.worker = new Worker();
                        Program.worker.Show();
                        this.Hide();
                    }
                } else { label1.Text = "Неверное имя пользователя или пароль"; }
            } else { label1.Text = "Заполните все поля"; }
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.reg1 = new Register();
            Program.reg1.Show();
            this.Hide();
        }        
        private void Server_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            index = server.SelectedIndex;
            SocketClient.ip = server.SelectedIndex;
        }
    }
}
