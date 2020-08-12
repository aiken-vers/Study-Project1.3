using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientMySQL
{
    public partial class checkPass : Form
    {
        static string local_login;
        public static string local_pass; 
        public checkPass(string login)
        {
            local_login = login;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pass = Data.GetMD5(textBox1.Text);
            string auth = SocketClient.SendMessage("/auth " + local_login + " " + pass);
            if (auth.StartsWith("success"))
            {
                local_pass = textBox1.Text;
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else { textBox1.Clear(); label1.Text = "Неверный пароль!";
                Thread blink = new Thread(red);
                blink.Start();
            }
        }
        private void red()
        {
            label1.ForeColor = Color.Crimson;
            Thread.Sleep(200);
            label1.ForeColor = Color.Black;
            Thread.Sleep(200);
            label1.ForeColor = Color.Crimson;
            Thread.Sleep(200);
            label1.ForeColor = Color.Black;
        }

    }
}
