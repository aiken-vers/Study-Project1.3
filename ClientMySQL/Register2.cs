using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ClientMySQL
{
    public partial class Register2 : Form
    {
        string id;
        public Register2()
        {
            id = Client.id;
            InitializeComponent();
            login.Text = Login.login;
            FIO.Text = Client.fio;
            passport.Text = Client.passport;
            address.Text = Client.address;
            number.Text = Client.number;
            email.Text = Client.email;
            Thread check1 = new Thread(checker);
            check1.Start();
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            bool check = string.IsNullOrEmpty(FIO.Text) && string.IsNullOrEmpty(passport.Text);
            bool checkNum = int.TryParse(number.Text, out int r);
            bool numempt = string.IsNullOrEmpty(number.Text);
            bool mailempt = string.IsNullOrEmpty(email.Text);
            bool checkMail = email.Text.EndsWith("@gmail.com") || email.Text.EndsWith("@mail.ru") || email.Text.EndsWith("@yandex.ru");            
            bool check4 = login.Text == Login.login;
            if (!check && numempt && mailempt)
            {                
                string register = "/reduser;" +id+";"+ FIO.Text + ";" + passport.Text + ";" + address.Text;
                SendReg2(register,13);
            }
            else if (!check && !numempt && !mailempt && checkNum && checkMail)
            {
                string register = "/reduser;" + id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + number.Text + ";" + email.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg2(register,11);
            }
            else if (!check && !numempt && mailempt && checkNum)
            {
                string register = "/reduser;" + id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + number.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg2(register, 14);
            }
            else if (!check && numempt && !mailempt && checkMail)
            {
                string register = "/reduser;" + id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + email.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg2(register, 15);
            }

            else label6.Text = "Заполните все поля";
        }

        public void SendReg2(string sql, int code)
        {
            bool check2 = loginCheck.Text == "✘";
            bool check3 = passCheck.Text == "✘";
            string sql2 = null;

            if (!check2 && !check3)
            {
                string coded = string.Format("/reduser{0}{1}",code, "0");
                sql2=sql.Replace("/reduser", coded);
                sql2 += ";" + login.Text + ";" + Data.GetMD5(password.Text);
            }                
            else if(!check2 && check3)
            {
                string coded = string.Format("/reduser{0}{1}", code, "0-");
                sql2 = sql.Replace("/reduser", coded);
                sql2 += ";" + login.Text;
            }
            else
            {
                string coded = string.Format("/reduser{0}", code);
                sql2 = sql.Replace("/reduser", coded);
            }
            SendReg3(sql2);
        }
        public void SendReg3(string sql)
        {
            Program.check = new checkPass(Login.login);
            if (Program.check.ShowDialog() == DialogResult.OK)
            {
                string register = SocketClient.SendMessage(sql);
                if (register == "success!")
                {
                    Program.client.Focus();
                    this.Hide();
                }
                else label6.Text = register;
            }
            else label6.Text = "Авторизация не пройдена";
        }

        public void checker()
        {
            try
            {
                while (true)
                {
                    //loginCheck.PerformClick();
                    Invoke(new Action(() => checklogin()));
                    Invoke(new Action(() => checkpassword()));
                    Thread.Sleep(10);
                }
            }
            catch { Thread.CurrentThread.Abort(); }
        }
        public void checklogin()
        {
            try
            {
                if (login.Text.Length >= 5)
                {
                    loginCheck.Text = "✔";
                    loginCheck.ForeColor = Color.LimeGreen;
                }
                else
                {
                    loginCheck.Text = "✘";
                    loginCheck.ForeColor = Color.Crimson;
                }

            }
            catch
            {
                loginCheck.Text = "✘";
                loginCheck.ForeColor = Color.Crimson;
            }
        }
        public void checkpassword()
        {
            try
            {
                int letter = 0, digit = 0;
                foreach (char c in password.Text)
                {
                    if (char.IsLetter(c)) letter++;
                    if (char.IsDigit(c)) digit++;
                }
                if (password.Text.Length >= 5 && letter > 0 && digit > 0)
                {
                    passCheck.Text = "✔";
                    passCheck.ForeColor = Color.LimeGreen;
                }
                else
                {
                    passCheck.Text = "✘";
                    passCheck.ForeColor = Color.Crimson;
                }

            }
            catch
            {
                passCheck.Text = "✘";
                passCheck.ForeColor = Color.Crimson;
            }
        }       

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string del = SocketClient.SendMessage("/deluser " + id);
            Program.login = new Login();
            Program.login.Show();
            this.Hide();
        }
    }
}
