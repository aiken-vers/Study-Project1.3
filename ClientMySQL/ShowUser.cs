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
    public partial class ShowUser : Form
    {
        public static Data.clients local_client;
        public ShowUser()
        {
            
            InitializeComponent();
            try
            {
                local_client = Worker.SelectedClient;
                FIO.Text = local_client.fio;
                passport.Text = local_client.passport;
                address.Text = local_client.address;
                number.Text = local_client.number;
                email.Text = local_client.email;
                checkAccess(Worker.access);
                foreach(Data.cards card in Worker.cards)
                {
                    if (card.idclient == local_client.id)
                        comboBox1.Items.Add("Карта:"+card.AccountId);
                }
                foreach (Data.deps dep in Worker.deps)
                {
                    if (dep.idclient == local_client.id)
                        comboBox1.Items.Add("Вклад:" + dep.AccountId);
                }                
            }
            catch { this.Hide(); }

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.BackColor= Color.MistyRose;
                FIO.Enabled = true;
                passport.Enabled = true;
                address.Enabled = true;
                number.Enabled = true;
                email.Enabled = true;
            }
            else
            {
                checkBox1.BackColor = Color.AliceBlue;
                FIO.Enabled = false;
                passport.Enabled = false;
                address.Enabled = false;
                number.Enabled = false;
                email.Enabled = false;
            }
        }
        private void checkAccess(string access)
        {
            FIO.Enabled = false;
            passport.Enabled = false;
            address.Enabled = false;
            number.Enabled = false;
            email.Enabled = false;
            CartNum.Enabled = false;
            pincode.Enabled = false;
            cvv2.Enabled = false;
            CardType.Enabled = false;
            balance.Enabled = false;
            duration.Enabled = false;
            if (access== "True")
            {
                checkBox1.Visible = true;
                button1.Visible = true;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CartNum.Enabled = true;
            pincode.Enabled = true;
            cvv2.Enabled = true;
            CardType.Enabled = true;
            balance.Enabled = true;
            duration.Enabled = true;
            CartNum.Focus();
            if (comboBox1.SelectedItem.ToString().StartsWith("Карта"))
            {
                label10.Text = "Номер карты:";
                label13.Visible = true; pincode.Visible = true; label14.Visible = true; cvv2.Visible = true;
                label11.Text = "Вид карты:"; label11.Location = new Point(12, 255); CardType.Location = new Point(152, 248);
                label12.Location = new Point(12, 283); balance.Location = new Point(152, 276);
                label15.Location = new Point(12, 309); duration.Location = new Point(152, 302);

                string[] val = comboBox1.SelectedItem.ToString().Split(':');
                foreach (Data.cards card in Worker.cards)
                {
                    if (card.AccountId == val[1])
                    {
                        CartNum.Text = card.AccountId;
                        pincode.Text = card.pincode;
                        cvv2.Text = card.cvv2;
                        CardType.Text = card.type;
                        balance.Text = card.balance + " руб.";
                        duration.Text = card.duration;
                    }
                }
            }
            else if (comboBox1.SelectedItem.ToString().StartsWith("Вклад"))
            {
                label10.Text = "Номер вклада:";
                label13.Visible = false; pincode.Visible = false; label14.Visible = false; cvv2.Visible = false;
                label11.Text = "Тип вклада:"; label11.Location = new Point(12, 229); CardType.Location = new Point(152, 222);
                label12.Location = new Point(12, 255); balance.Location = new Point(152, 248);
                label15.Location = new Point(12, 283); duration.Location = new Point(152, 276);

                string[] val = comboBox1.SelectedItem.ToString().Split(':');
                foreach (Data.deps dep in Worker.deps)
                {
                    if (dep.AccountId == val[1])
                    {
                        CartNum.Text = dep.AccountId;
                        CardType.Text = dep.type;
                        balance.Text = dep.balance + " руб.";
                        duration.Text = dep.duration;
                    }
                }
            }
            CartNum.Enabled = false;
            pincode.Enabled = false;
            cvv2.Enabled = false;
            CardType.Enabled = false;
            balance.Enabled = false;
            duration.Enabled = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool check = string.IsNullOrEmpty(FIO.Text) && string.IsNullOrEmpty(passport.Text);
            bool checkNum = int.TryParse(number.Text, out int r);
            bool numempt = string.IsNullOrEmpty(number.Text);
            bool mailempt = string.IsNullOrEmpty(email.Text);
            bool checkMail = email.Text.EndsWith("@gmail.com") || email.Text.EndsWith("@mail.ru") || email.Text.EndsWith("@yandex.ru");
           
            if (!check && numempt && mailempt)
            {
                string register = "/reduser;" + local_client.id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text;
                SendReg2(register, 13);
            }
            else if (!check && !numempt && !mailempt && checkNum && checkMail)
            {
                string register = "/reduser;" + local_client.id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + number.Text + ";" + email.Text;
                SendReg2(register, 11);
            }
            else if (!check && !numempt && mailempt && checkNum)
            {
                string register = "/reduser;" + local_client.id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + number.Text;
                SendReg2(register, 14);
            }
            else if (!check && numempt && !mailempt && checkMail)
            {
                string register = "/reduser;" + local_client.id + ";" + FIO.Text + ";" + passport.Text + ";" + address.Text
                                  + ";" + email.Text;
                SendReg2(register, 15);
            }

            else MessageBox.Show("Заполните все поля", "Ошибка");

        }
        public void SendReg2(string sql, int code)
        {
            string sql2 = null;           
            
            string coded = string.Format("/reduser{0}", code);
            sql2 = sql.Replace("/reduser", coded);

            SendReg3(sql2);
        }
        public void SendReg3(string sql)
        {            
            string register = SocketClient.SendMessage(sql);
            if (register == "success!")
            {
                Program.worker.Focus();
                this.Hide();
            }
            else MessageBox.Show(register, "Ошибка");           
        }
    }
}
