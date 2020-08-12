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
    public partial class Register3 : Form
    {
        public Register3()
        {
            InitializeComponent();
            Thread check1 = new Thread(checker);

            check1.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int access = 0;
            if (checkBox1.Checked) access = 1;
            bool check = string.IsNullOrEmpty(FIO.Text) && string.IsNullOrEmpty(passport.Text);
            bool check2 = loginCheck.Text == "✘";
            bool check3 = passCheck.Text == "✘";
            if (!check && !check2 && !check3)
            {
                string sql = "/addworker;" + FIO.Text + ";" + access + ";" + passport.Text
                                  + ";" + login.Text + ";" + Data.GetMD5(password.Text);
                string register = SocketClient.SendMessage(sql);
                if (register == "success!")
                {
                    Program.worker.Focus();
                    this.Hide();
                }
                else MessageBox.Show(register, "Ошибка");
            }            
            else MessageBox.Show("Заполните все поля корректно", "Ошибка");
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
        
        private void LoginCheck_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Логин должен представлять собой уникальную последовательность 5-12 символов.", "Требования к логину");

        }

        private void PassCheck_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Пароль должен представлять собой уникальную последовательность 5-16 символов, должен содержать буквы и цифры.", "Требования к паролю");

        }
    }
}
