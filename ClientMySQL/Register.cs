using System;
using System.Collections.Generic;
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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
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
            bool check2 = loginCheck.Text == "✘";
            bool check3 = passCheck.Text == "✘";
            if (!check&&!check2&&!check3&& !numempt && !mailempt && checkNum&& checkMail)
            {
                string register = "/adduser11;" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + number.Text + ";" + email.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg(register);                
            }
            else if(!check && !check2 && !check3 && numempt && mailempt)
            {
                string register = "/adduser13;" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg(register);
            }
            else if(!check && !check2 && !check3 && !numempt && mailempt && checkNum)
            {
                string register = "/adduser14;" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + number.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg(register);
            }
            else if(!check && !check2 && !check3 && numempt && !mailempt && checkMail)
            {
                string register = "/adduser15;" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + email.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                SendReg(register);
            }
            else label6.Text = "Заполните все поля корректно";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Логин должен представлять собой уникальную последовательность 5-12 символов.", "Требования к логину");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Пароль должен представлять собой уникальную последовательность 5-16 символов, должен содержать буквы и цифры.", "Требования к паролю");
        }

        private void SendReg(string sql)
        {
            string register = SocketClient.SendMessage(sql);
            if (register == "success!")
            {
                Login alpha = new Login();
                alpha.Show();
                this.Hide();
            }
            else label6.Text = register;
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
                int letter=0, digit = 0;
                foreach(char c in password.Text)
                {
                    if (char.IsLetter(c)) letter++;
                    if (char.IsDigit(c)) digit++;
                }
                if (password.Text.Length >= 5&&letter>0&&digit>0)
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

        public void checker()
        {
            try
            {
                while (true)
                {
                    Invoke(new Action(() => checklogin()));
                    Invoke(new Action(() => checkpassword()));
                    Thread.Sleep(10);
                }
            }
            catch { Thread.CurrentThread.Abort(); }    
        }        
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
