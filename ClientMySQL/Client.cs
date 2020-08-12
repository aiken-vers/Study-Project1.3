using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientMySQL
{
    public partial class Client : Form
    {
        public static string id, fio, passport, address, number, email;
        public static DateTime logs_date = DateTime.Today.AddYears(-1000);
        public static double earned = 0, cut = 0;
        public static List<Data.cards> cards = new List<Data.cards>();
        public static List<Data.deps> deps = new List<Data.deps>();
        public static List<Data.logs> logs = new List<Data.logs>();

        private void button4_Click(object sender, EventArgs e)
        {
            update_data();
            update_deps();
        }

        public Client()
        {
            string[] auth_words = Login.auth.Split(new Char[] { ';' });
            id = auth_words[1];
            InitializeComponent();
            update_data();
            update_deps();
            update_logs();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private void update_data()
        {
            string userdata = SocketClient.SendMessage("/userdata " + id);
            string[] data = userdata.Split(new Char[] { ';' });
            label1.Text = "ФИО: " + data[0]; fio = data[0];
            label2.Text = "Номер паспорта: " + data[1]; passport = data[1];
            label3.Text = "Домашний адрес: " + data[2]; address = data[2];
            label7.Text = "Номер телефона: " + data[3]; number = data[3];
            label8.Text = "почта: " + data[4]; email = data[4];
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                del_card(cards[listBox1.SelectedIndex]);
                update_deps();
            }
        }
        private void del_card(Data.cards card)
        {
            string del = SocketClient.SendMessage("/del_card " + card.AccountId);
        }
        private void del_dep(Data.deps dep)
        {
            string del = SocketClient.SendMessage("/del_dep " + dep.AccountId);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                del_dep(deps[listBox2.SelectedIndex]);
                update_deps();
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Program.new_dep = new new_dep();
            Program.new_dep.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today.AddYears(-1000);
            LogsTolist(logs_date);
        }

        private void update_deps()
        {
            string useraccount = SocketClient.SendMessage("/usercards " + id);
            cards.Clear();
            listBox1.Items.Clear();
            listBox4.Items.Clear();
            if (useraccount != "no accounts")
            {
                string[] account = useraccount.Split(new Char[] { ';' });
                foreach (string card in account)
                {
                    if (!String.IsNullOrEmpty(card))
                    {
                        string[] val = card.Split(new Char[] { ',' });
                        Data.cards acc = new Data.cards(val[0], val[1], val[2], val[3], val[4], val[5]);
                        cards.Add(acc);
                        listBox1.Items.Add(String.Format("{0}, {1}, {2}, {3} руб.",
                            acc.AccountId, acc.type, acc.duration, acc.balance));
                        listBox4.Items.Add(String.Format("{0}, {1} руб.",
                            acc.AccountId, acc.balance));
                    }
                }
            }
            
            string userdeps = SocketClient.SendMessage("/userdeps " + id);
            deps.Clear();
            listBox2.Items.Clear();
            listBox5.Items.Clear();
            //listBox2.Items.Add(userdeps);
            if (userdeps != "no deps")
            {
                string[] account = userdeps.Split(new Char[] { ';' });
                foreach (string dep in account)
                {
                    if (!String.IsNullOrEmpty(dep))
                    {
                        string[] val = dep.Split(new Char[] { '!' });
                        Data.deps d = new Data.deps(val[0], val[1], val[2], val[3], val[4], val[5]);
                        deps.Add(d);
                        listBox2.Items.Add(String.Format("{0}, {1}, {2}, {3} руб.",
                            d.AccountId, d.type, d.duration, d.balance));
                        listBox5.Items.Add(String.Format("{0}, {1} руб.",
                            d.AccountId, d.balance));
                    }
                }
            }
        }
        // ФИЛЬТРАЦИЯ ЗАПИСЕЙ В ЖУРНАЛЕ ПО ДАТЕ: ЗА ПОСЛЕДНИЙ МЕСЯЦ, ЗА НЕДЕЛЮ, ЗА СЕГОДЯ
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today.AddMonths(-1);
            LogsTolist(logs_date);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today.AddDays(-7);
            LogsTolist(logs_date);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today;
            LogsTolist(logs_date);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Generate_word();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            update_logs();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int money = 0;
            if (rad100.Checked) money = 100;
            else if (rad500.Checked) money = 500;
            else if (rad1000.Checked) money = 1000;
            else if (rad5000.Checked) money = 5000;

            string sql = null;
            if (listBox4.SelectedIndex != -1)
            {
                sql = string.Format("/money;{0};{1}",
                    cards[listBox4.SelectedIndex].AccountId, money);
                string result = SocketClient.SendMessage(sql);
                update_deps();
            }
            else if (listBox5.SelectedIndex != -1)
            {
                if (deps[listBox5.SelectedIndex].refill != false)
                {
                    sql = string.Format("/money;{0};{1}",
                    deps[listBox5.SelectedIndex].AccountId, money);
                    string result = SocketClient.SendMessage(sql);
                    update_deps();
                }
                else { MessageBox.Show("Данный вклад не поддерживает пополнение денежных средств", "Ошибка"); }
            }
            
        }

        private void ListBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox4.SelectedIndex = -1;
        }

        private void ListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox5.SelectedIndex = -1;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            int money = 0;
            if (rad100.Checked) money = -100;
            else if (rad500.Checked) money = -500;
            else if (rad1000.Checked) money = -1000;
            else if (rad5000.Checked) money = -5000;

            string sql = null;
            if (listBox4.SelectedIndex != -1)
            {
                sql = string.Format("/money;{0};{1}",
                    cards[listBox4.SelectedIndex].AccountId, money);
                string result = SocketClient.SendMessage(sql);
                update_deps();
            }
            else if (listBox5.SelectedIndex != -1)
            {
                if (deps[listBox5.SelectedIndex].cut != false)
                {
                    sql = string.Format("/money;{0};{1}",
                                        deps[listBox5.SelectedIndex].AccountId, money);
                    string result = SocketClient.SendMessage(sql);
                    update_deps();
                }
                else { MessageBox.Show("Данный вклад не поддерживает снятие денежных средств", "Ошибка"); }
            }            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox4.SelectedIndex != -1)
                {
                    int numberid;
                    int cash;
                    string old_balance =cards[listBox4.SelectedIndex].balance.Replace('.',',');
                    double bal = double.Parse(old_balance);
                    if (int.TryParse(number_id.Text, out numberid) && int.TryParse(tran_cash.Text, out cash))
                    {
                        if (bal >= cash)
                        {
                            if (tran_type.SelectedItem.ToString() == "По номеру карты")
                            {
                                string sql = string.Format("/tran;{0};{1};{2}",
                                    cards[listBox4.SelectedIndex].AccountId, number_id.Text, tran_cash.Text);
                                string result = SocketClient.SendMessage(sql);
                                update_deps();
                            }
                            else if (tran_type.SelectedItem.ToString() == "По номеру телефона")
                            {
                                string sql = string.Format("/tran_phone;{0};{1};{2}",
                                    cards[listBox4.SelectedIndex].AccountId, number_id.Text, tran_cash.Text);
                                string result = SocketClient.SendMessage(sql);
                                update_deps();
                            }
                        }
                        else { MessageBox.Show("На карте недостаточно средств", "Ошибка"); }
                    }
                    else
                    {
                        MessageBox.Show("Введены некорректные данные", "Ошибка");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите карту, с которой хотите перевести деньги", "Ошибка");
                }
            }
            catch { MessageBox.Show("Введены некорректные данные", "Ошибка"); }
        }

        private void tran_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tran_type.SelectedItem.ToString() == "По номеру карты")
            {
                label11.Text = "Номер карты";
                label11.Location = new Point(282, 267);
            }
            else if (tran_type.SelectedItem.ToString() == "По номеру телефона")
            {
                label11.Text = "Номер телефона";
                label11.Location = new Point(267, 267);
            }
        }

        private void toMail_CheckedChanged(object sender, EventArgs e)
        {
            if (!Client.email.Contains("@"))
            {
                toMail.Checked = false;
                MessageBox.Show("К вашему аккаунту не привязана почта", "Ошибка");
            }
        }

        private void update_logs()
        {
            string data_logs = SocketClient.SendMessage("/userlogs " + id);
            logs.Clear();
            if (data_logs != "no logs")
            {
                string[] account = data_logs.Split(new Char[] { ';' });
                foreach (string card in account)
                {
                    if (!String.IsNullOrEmpty(card))
                    {
                        string[] val = card.Split(new Char[] { '!' });
                        Data.logs value = new Data.logs(val[1], val[0], val[2], val[3], val[4]);
                        logs.Add(value);
                    }
                }
                if (checkBox1.Checked)
                    CashToList(textBox2.Text, textBox1.Text);
                else
                    LogsTolist(logs_date);

            }
        }
        private void LogsTolist(DateTime d)
        {
            listBox3.Items.Clear();
            listBox3.Items.Add(String.Format("{0,-42} {1, -60} {2,10} {3,10} {4,10}",
                            "Дата", "Событие", "Карта/счёт", "Остаток", "Перевод"));
            foreach (Data.logs value in logs)
            {
                if (value.date >= d)
                {
                    //string local_date = value.date.ToString().PadRight(32);
                    //string local_event = value.events.PadRight(56);
                    //string local_idevent = value.idevent.PadRight(13);
                    //string local_balance = value.balance.PadRight(10);
                    //string local_operation = value.operation.PadRight(10);
                    //listBox3.Items.Add(String.Format("{0} {1} {2} {3} {4}",
                    //    local_date, local_event, local_idevent, local_balance, local_operation));

                    if (value.events == "Открыт вклад")
                        listBox3.Items.Add(String.Format("{0,-32} {1, -56} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation));
                    else if (value.events == "Снятие наличных")
                        listBox3.Items.Add(String.Format("{0,-32} {1, -54} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation));
                    else if (value.events == "Внесение наличных")
                        listBox3.Items.Add(String.Format("{0,-32} {1, -52} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation));
                    else if (value.events == "Перевод наличных (пополнение)")
                        listBox3.Items.Add(String.Format("{0,-32} {1, -42} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation));
                    else if (value.events == "Перевод наличных (списание)")
                        listBox3.Items.Add(String.Format("{0,-32} {1, -44} {2,-15} {3,-14} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation));
                    else
                        listBox3.Items.Add(String.Format("{0,-32} {1, -55} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation));
                }
            }
        }
        private void CashToList(string s1, string s2)
        {
            DateTime from, to;
            if(DateTime.TryParse(s1,out from)&& DateTime.TryParse(s2, out to))
            {
                listBox3.Items.Clear();
                earned = 0; cut=0;
                foreach(Data.logs value in logs)
                {
                    if (value.date >= from && value.date <= to)
                    {
                        if (value.events == "Внесение наличных")
                            earned += double.Parse(value.operation.Replace('.',','));
                        else if (value.events == "Перевод наличных (пополнение)")
                            earned += double.Parse(value.operation.Replace('.', ','));
                        if (value.events == "Снятие наличных")
                            cut += double.Parse(value.operation.Replace('.', ','));
                        else if (value.events == "Перевод наличных (списание)")
                            cut += double.Parse(value.operation.Replace('.', ','));
                    }
                }
                listBox3.Items.Add(String.Format("Всего зачислений: {0} руб.", earned));
                listBox3.Items.Add(String.Format("Всего расходов: {0} руб.", cut));
                listBox3.Items.Add(String.Format("{0,-42} {1, -60} {2,10} {3,16}",
                                "Дата", "Событие", "Карта/счёт", "Сумма"));
                foreach (Data.logs value in logs)
                {
                    if (value.date >= from && value.date <= to)
                    {
                        if (value.events == "Снятие наличных")
                            listBox3.Items.Add(String.Format("{0,-32} {1, -54} {2,-20} -{3,-14}",
                            value.date, value.events, value.idevent, value.operation));
                        else if (value.events == "Внесение наличных")
                            listBox3.Items.Add(String.Format("{0,-32} {1, -51} {2,-20} +{3,-14}",
                            value.date, value.events, value.idevent, value.operation));
                        else if (value.events == "Перевод наличных (пополнение)")
                            listBox3.Items.Add(String.Format("{0,-32} {1, -52} {2,-20} +{3,-14}",
                            value.date, value.events.Replace(" (пополнение)", ""), value.idevent, value.operation));
                        else if (value.events == "Перевод наличных (списание)")
                            listBox3.Items.Add(String.Format("{0,-32} {1, -52} {2,-20} -{3,-14}",
                            value.date, value.events, value.idevent.Replace(" (списание)", ""), value.operation));
                    }
                }
            }
            
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Program.reg2 = new Register2();
            Program.reg2.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Program.new_card = new new_card();
            Program.new_card.Show();
        }
        // ГЕНЕРАЦИЯ ОТЧЁТА В WORD
        private void Generate_word()
        {
            string pathToTemplate;
            string pathToInsertingDoc;
            if (checkBox1.Checked)
            {
                // ПУТЬ К ШАБЛОНУ ПО ДАННЫМ ФОРМЫ
                pathToTemplate = Application.StartupPath + "\\Log3.doc";

                // ПУТЬ К ВРЕМЕННОМУ ФАЙЛУ ДЛЯ ВСТАВКИ
                pathToInsertingDoc = String.Format("{0}\\{1}{2}{3}", Application.StartupPath,
                    "Анализ финансов лицевого счёта за ", DateTime.Today.ToString("MM/dd/yyyy"), ".doc");
            }
            else
            {
                // ПУТЬ К ШАБЛОНУ ПО ДАННЫМ ФОРМЫ
                pathToTemplate = Application.StartupPath + "\\Log.doc";
                // ПУТЬ К ВРЕМЕННОМУ ФАЙЛУ ДЛЯ ВСТАВКИ
                pathToInsertingDoc = String.Format("{0}\\{1}{2}{3}", Application.StartupPath,
                    "Выписка лицевого счёта за ", DateTime.Today.ToString("MM/dd/yyyy"), ".doc");                
            }
            WordDocument wordDoc;
            try
            {                
                wordDoc = new WordDocument(pathToTemplate);
                wordDoc.ReplaceAllStrings("@id", id);
                wordDoc.ReplaceAllStrings("@fio", fio);
                wordDoc.ReplaceAllStrings("@number", number);
                wordDoc.ReplaceAllStrings("@email", email);
                if (checkBox1.Checked)
                {
                    wordDoc.ReplaceAllStrings("@from", textBox2.Text);
                    wordDoc.ReplaceAllStrings("@to", textBox1.Text);
                    wordDoc.ReplaceAllStrings("@earned", earned.ToString());
                    wordDoc.ReplaceAllStrings("@cut", cut.ToString());
                }
                    
                //ВЫБОР И ЗАПОЛНЕНИЕ ИМЕЮЩЕЙСЯ ТАБЛИЦЫ
                int tableNum = 1;
                wordDoc.SelectTable(tableNum);
                int addRowsCount = logs.Count;
                
                for (int addRowsNum = 1; addRowsNum <= addRowsCount; addRowsNum++)
                {
                    if(logs[addRowsNum - 1].date >= logs_date)
                    {
                       
                        if (checkBox1.Checked)
                        {                            
                            if(logs[addRowsNum - 1].events== "Снятие наличных"||
                                logs[addRowsNum - 1].events == "Внесение наличных"||
                                logs[addRowsNum - 1].events == "Перевод наличных (пополнение)" ||
                                logs[addRowsNum - 1].events == "Перевод наличных (списание)")
                            {
                                wordDoc.AddRowToTable();
                                wordDoc.SetSelectionToCell(addRowsNum + 1, 1);
                                wordDoc.Selection.Text = logs[addRowsNum - 1].date.ToString();
                                wordDoc.SetSelectionToCell(addRowsNum + 1, 2);
                                if(logs[addRowsNum - 1].events == "Перевод наличных (пополнение)"
                                    || logs[addRowsNum - 1].events == "Перевод наличных (списание)")
                                    wordDoc.Selection.Text = "Перевод наличных";
                                else
                                    wordDoc.Selection.Text = logs[addRowsNum - 1].events;
                                wordDoc.SetSelectionToCell(addRowsNum + 1, 3);
                                wordDoc.Selection.Text = logs[addRowsNum - 1].idevent;
                                wordDoc.SetSelectionToCell(addRowsNum + 1, 4);
                                if(logs[addRowsNum - 1].events == "Перевод наличных (пополнение)" ||
                                    logs[addRowsNum - 1].events == "Внесение наличных")
                                    wordDoc.Selection.Text = '+'+logs[addRowsNum - 1].operation;
                                else if(logs[addRowsNum - 1].events == "Перевод наличных (списание)" ||
                                    logs[addRowsNum - 1].events == "Снятие наличных")
                                    wordDoc.Selection.Text = '-' + logs[addRowsNum - 1].operation;
                                else
                                    wordDoc.Selection.Text = logs[addRowsNum - 1].operation;
                            }                            
                        }
                        else
                        {
                            wordDoc.AddRowToTable();
                            wordDoc.SetSelectionToCell(addRowsNum + 1, 1);
                            wordDoc.Selection.Text = logs[addRowsNum - 1].date.ToString();
                            wordDoc.SetSelectionToCell(addRowsNum + 1, 2);
                            wordDoc.Selection.Text = logs[addRowsNum - 1].events;
                            wordDoc.SetSelectionToCell(addRowsNum + 1, 3);
                            wordDoc.Selection.Text = logs[addRowsNum - 1].idevent;
                            wordDoc.SetSelectionToCell(addRowsNum + 1, 4);
                            wordDoc.Selection.Text = logs[addRowsNum - 1].balance;
                            wordDoc.SetSelectionToCell(addRowsNum + 1, 5);
                            wordDoc.Selection.Text = logs[addRowsNum - 1].operation;
                        }                        
                    }                    
                }

                using (File.Create(pathToInsertingDoc))
                {

                }
                wordDoc.Save(pathToInsertingDoc);
                wordDoc.Close();
                if (toMail.Checked)
                {
                    Mail.SendDoc(pathToInsertingDoc);
                    System.GC.Collect();
                    MessageBox.Show("Отчёт был отправлен на вашу электронную почту", "Уведомление");
                }
                else
                {
                    wordDoc = new WordDocument(pathToInsertingDoc);
                    wordDoc.Visible = true;
                    System.GC.Collect();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Ошибка при открытии шаблона Word. Подробности " + error.Message);
                return;
            }
        }           
    }
}
