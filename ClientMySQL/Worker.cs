using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMySQL
{
    public partial class Worker : Form
    {
        public static string id, fio, passport, access;
        public static string log_id = "0"; public static string event_filter = "Вся активность";
        public static string Graf_period = "week"; public static int index = 0;
        public static DateTime logs_date = DateTime.Today.AddYears(-1000);
        public static List<Data.cards> cards = new List<Data.cards>();
        public static List<Data.deps> deps = new List<Data.deps>();
        public static List<Data.logs> logs = new List<Data.logs>();
        public static List<Data.workers> works = new List<Data.workers>();
        public static List<Data.clients> slaves = new List<Data.clients>();
        public static Data.clients SelectedClient;
        public Worker()
        {
            string[] auth_words = Login.auth.Split(new Char[] { ';' });
            id = auth_words[1];
            InitializeComponent();
            update_data();
            update_users();
            update_workers();
            update_deps();
            update_list();
            update_logs();

            radioClients.Checked = true;
            radioButton6.Checked = true;
            if (access != "True")
            {
                button2.Visible = true;
                button3.Visible = true;
            }
            comboBox1.SelectedIndex = 0; comboBox2.SelectedIndex = 0; comboBox3.SelectedIndex = 0; comboBox4.SelectedIndex = 0;

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

        private void RadioClients_CheckedChanged(object sender, EventArgs e)
        {
            radioWorkers.BackColor = Color.AliceBlue;
            radioClients.BackColor = Color.LightSteelBlue;
            if (access == "True")
            {
                button2.Visible = true;
                button3.Visible = true;
            }           
            button5.Visible = true;
            button6.Visible = false;
            button9.Visible = false;
            update_list();
        }

        private void update_data()
        {
            string userdata = SocketClient.SendMessage("/workerdata " + id);
            string[] data = userdata.Split(new Char[] { ';' });
            label1.Text = "ФИО: " + data[0]; fio = data[0];
            label2.Text = "Номер паспорта: " + data[1]; passport = data[1];
            access = data[2];            
        }
        private void update_users()
        {
            slaves.Clear();
            string data = SocketClient.SendMessage("/showclients");
            if(data!= "no clients")
            {
                string[] users = data.Split(new Char[] { '!' });
                foreach (string userdata in users)
                {
                    if (userdata == "") break;
                    string[] user = userdata.Split(new Char[] { ';' });
                    string id = user[0];
                    string fio = user[1];
                    string passport = user[2];
                    string address = user[3];
                    string number = user[4];
                    string email = user[5];
                    Data.clients client = new Data.clients(id, fio, passport, address, number, email);
                    slaves.Add(client);
                }
            }
            
        }
        private void update_workers()
        {
            works.Clear();
            string data = SocketClient.SendMessage("/showorkers");
            if (data != "no workers")
            {
                string[] users = data.Split(new Char[] { '!' });
                foreach (string workdata in users)
                {
                    if (workdata == "") break;
                    string[] user = workdata.Split(new Char[] { ';' });
                    string id = user[0];
                    string fio = user[1];
                    string passport = user[3];
                    string access = user[2];
                    Data.workers worker = new Data.workers(id, fio,access, passport);
                    works.Add(worker);
                }
            }
        }
        private void update_list()
        {
            listBox1.Items.Clear();
            if(radioClients.Checked == true)
            {
                foreach (Data.clients client in slaves)
                    listBox1.Items.Add(client.toList(cards, deps));
            }
            else if(radioWorkers.Checked == true)
            {
                foreach (Data.workers worker in works)
                    listBox1.Items.Add(worker);
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            update_logs();
        }

        private void RadioWorkers_CheckedChanged(object sender, EventArgs e)
        {
            radioClients.BackColor = Color.AliceBlue;
            radioWorkers.BackColor = Color.LightSteelBlue;
            button2.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            if (access == "True")
            {
                button6.Visible = true;
                button9.Visible = true;
            }            
            update_list();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Program.reg1 = new Register();
            Program.reg1.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int deluser = listBox1.SelectedIndex;
                string del = SocketClient.SendMessage("/deluser " + slaves[deluser].id);
                listBox1.Items.RemoveAt(deluser);
            }
            else
            {
                MessageBox.Show("Выберите клиента, которого хотите удалить", "Ошибка");
            }           
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                Program.ShowUser = new ShowUser();
                Program.ShowUser.Show();
            }
            else
            {
                MessageBox.Show("Выберите клиента, информацию о котором хотите посмотреть", "Ошибка");
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioClients.Checked)
                    SelectedClient = slaves[listBox1.SelectedIndex];
            }
            catch { }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            update_data();
            update_users();
            update_workers();
            update_deps();
            update_list();
            update_logs();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Все пользователи")
            {
                log_id = "0";
                IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
                LogsTolist(logs_date, Ordered);
            }
            else
            {
                string[] val = comboBox2.SelectedItem.ToString().Split(',');
                log_id = val[0];
                IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
                LogsTolist(logs_date, Ordered, log_id);
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today.AddYears(-1000);
            IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
            LogsTolist(logs_date, Ordered, log_id);
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today.AddMonths(-1);
            IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
            LogsTolist(logs_date, Ordered, log_id);
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today.AddDays(-7);
            IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
            LogsTolist(logs_date, Ordered, log_id);
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            logs_date = DateTime.Today;
            IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
            LogsTolist(logs_date, Ordered, log_id);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            event_filter = comboBox1.SelectedItem.ToString();
            IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
            LogsTolist(logs_date, Ordered, log_id);
        }

        private void update_deps()
        {
            cards.Clear();
            deps.Clear();
            foreach (Data.clients client in slaves)
            {
                string useraccount = SocketClient.SendMessage("/usercards " + client.id);                
                if (useraccount != "no accounts")
                {
                    string[] account = useraccount.Split(new Char[] { ';' });
                    foreach (string card in account)
                    {
                        if (!String.IsNullOrEmpty(card))
                        {
                            string[] val = card.Split(new Char[] { ',' });
                            Data.cards acc = new Data.cards(val[0], val[1], val[2], val[3], val[4], val[5], client.id);
                            cards.Add(acc);
                        }
                    }
                }
                string userdeps = SocketClient.SendMessage("/userdeps " + client.id);
                if (userdeps != "no deps")
                {
                    string[] account = userdeps.Split(new Char[] { ';' });
                    foreach (string dep in account)
                    {
                        if (!String.IsNullOrEmpty(dep))
                        {
                            string[] val = dep.Split(new Char[] { '!' });
                            Data.deps d = new Data.deps(val[0], val[1], val[2], val[3], val[4], val[5], client.id);
                            deps.Add(d);
                        }
                    }
                }
            }            
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int deluser = listBox1.SelectedIndex;
                if (works[deluser].id != "1")
                {
                    string del = SocketClient.SendMessage("/delworker " + works[deluser].id);
                    listBox1.Items.RemoveAt(deluser);
                }
                else MessageBox.Show("Данный аккаунт администратора не подлежит удалению", "Ошибка");
            }
            else
            {
                MessageBox.Show("Выберите сотрудника, которого хотите уволить", "Ошибка");
            }
                       
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Program.reg3 = new Register3();
            Program.reg3.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sinus(Graf_period);
        }

        private void Sinus(string period="year")
        {            
            if (checkBox1.Checked == true)
            {
                index++;
                chart1.Series.Add("new"+index);                
                chart1.Series[index].LegendText = "Все";
                chart1.Series[index].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            }
            else {
                index = 0;
                chart1.Series.Clear();
                chart1.Series.Add("null");
                chart1.Series[index].LegendText = "Все";
                chart1.Series[index].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            }  

            if (log_id != "0") chart1.Series[index].LegendText = comboBox3.SelectedItem.ToString();
            else chart1.Series[index].LegendText = "Все";
            if (period == "year")
            {
                chart1.Series[index].Points.Clear();                
                for (DateTime xdate = logs_date; xdate <= DateTime.Today; xdate = xdate.AddMonths(1))
                {
                    int n = 0;
                    foreach (Data.logs log in logs)
                    {
                        if (log.date >= xdate && log.date < xdate.AddMonths(1))
                            if (log_id == "0" || log.idclient == log_id)
                                if (LogsFilter(log.events))
                                    n++;
                    }
                    chart1.Series[index].Points.AddXY(xdate.ToString("MMM"), n);
                }
            }
            else if (period == "month")
            {
                chart1.Series[index].Points.Clear();
                for (DateTime xdate = logs_date; xdate <= DateTime.Today; xdate = xdate.AddDays(1))
                {
                    int n = 0;
                    foreach (Data.logs log in logs)
                    {
                        if (log.date >= xdate && log.date < xdate.AddDays(1))
                                if (log_id == "0" || log.idclient == log_id)
                                    if (LogsFilter(log.events))
                                        n++;
                    }
                    chart1.Series[index].Points.AddXY(xdate.ToString("MM.dd"), n);
                }
            }
            else if (period == "week")
            {
                chart1.Series[index].Points.Clear();
                for (DateTime xdate = logs_date; xdate <= DateTime.Today; xdate = xdate.AddDays(1))
                {
                    int n = 0;
                    foreach (Data.logs log in logs)
                    {
                        if (log.date >= xdate && log.date < xdate.AddDays(1))
                                if (log_id == "0" || log.idclient == log_id)
                                    if (LogsFilter(log.events))
                                        n++;
                    }
                    chart1.Series[index].Points.AddXY(xdate.ToString("ddd"), n);
                }
            }
            else if (period == "day")
            {
                chart1.Series[index].Points.Clear();
                for (DateTime xdate = logs_date; xdate < DateTime.Now.AddHours(24); xdate = xdate.AddHours(1))
                {
                    int n = 0;
                    foreach (Data.logs log in logs)
                    {
                        if (log.date >= xdate && log.date < xdate.AddHours(1))
                            if (log_id == "0" || log.idclient == log_id)
                                if (LogsFilter(log.events))
                                    n++;
                    }
                    chart1.Series[index].Points.AddXY(xdate.ToString("HH:mm"), n);
                }
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Graf_period = "year";
            logs_date = DateTime.Today.AddYears(-1);
            Sinus("year");
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Graf_period = "month";
            logs_date = DateTime.Today.AddMonths(-1);
            Sinus("month");
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Graf_period = "week";
            logs_date = DateTime.Today.AddDays(-7);
            Sinus("week");
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Graf_period = "day";
            logs_date = DateTime.Now.AddHours(-24);
            Sinus("day");
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Все пользователи")
            {
                log_id = "0";
            }
            else
            {
                string[] val = comboBox3.SelectedItem.ToString().Split(',');
                log_id = val[0];
            }
            Sinus(Graf_period);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            event_filter = comboBox4.SelectedItem.ToString();
            Sinus(Graf_period);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Generate_word2();
        }

        private void update_logs()
        {
            comboBox2.Items.Clear(); comboBox2.Items.Add("Все пользователи");
            comboBox3.Items.Clear(); comboBox3.Items.Add("Все пользователи");
            logs.Clear();
            foreach (Data.clients client in slaves)
            {
                string id_fio = client.id +", "+ client.fio;
                comboBox2.Items.Add(id_fio);
                comboBox3.Items.Add(id_fio);
                string data_logs = SocketClient.SendMessage("/userlogs " + client.id);
                
                if (data_logs != "no logs")
                {
                    string[] account = data_logs.Split(new Char[] { ';' });
                    foreach (string card in account)
                    {
                        if (!String.IsNullOrEmpty(card))
                        {
                            string[] val = card.Split(new Char[] { '!' });
                            Data.logs value = new Data.logs(val[1], val[0], val[2], val[3], val[4], client.id);
                            logs.Add(value);
                        }
                    }
                    IEnumerable<Data.logs> Ordered = logs.OrderBy(x => x.date);
                    LogsTolist(logs_date,Ordered, log_id);                    
                }
            }            
        }
        private void LogsTolist(DateTime d, IEnumerable<Data.logs> logs, string client_id="0")
        {
            listBox3.Items.Clear(); //42, 32
            listBox3.Items.Add(String.Format("{0,-36} {1, -60} {2,10} {3,10} {4,10}",
                            "Дата", "Событие", "Карта/счёт", "Остаток", "Перевод"));
            foreach (Data.logs value in logs)
            {
                if (value.date >= d)
                {
                    if(client_id=="0"||value.idclient==client_id)
                    if(LogsFilter(value.events))
                    if (value.events == "Открыт вклад")
                        listBox3.Items.Add(String.Format("{0,-26} ({5}){1, -56} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation, value.idclient));
                    else if (value.events == "Снятие наличных")
                        listBox3.Items.Add(String.Format("{0,-26} ({5}){1, -54} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation, value.idclient));
                    else if (value.events == "Внесение наличных")
                        listBox3.Items.Add(String.Format("{0,-26} ({5}){1, -52} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation, value.idclient));
                    else if (value.events == "Перевод наличных (пополнение)")
                        listBox3.Items.Add(String.Format("{0,-26} ({5}){1, -42} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation, value.idclient));
                    else if (value.events == "Перевод наличных (списание)")
                        listBox3.Items.Add(String.Format("{0,-26} ({5}){1, -44} {2,-15} {3,-14} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation, value.idclient));
                    else
                        listBox3.Items.Add(String.Format("{0,-26} ({5}){1, -55} {2,-15} {3,-12} {4,0}",
                        value.date, value.events, value.idevent, value.balance, value.operation, value.idclient));
                }
            }
        }
        private static bool LogsFilter(string events)
        {
            if (event_filter == "Вся активность")
                return true;
            else if (event_filter == "Регистрация")
                if (events == "Пользователь был зарегистрирован")
                    return true;
                else return false;
            else if (event_filter == "Авторизация")
                if (events == "Пользователь вошёл в систему")
                    return true;
                else return false;
            else if (event_filter == "Оформление")
                if (events == "Заведена карта"|| events == "Открыт вклад")
                    return true;
                else return false;
            else if (event_filter == "Пополнение")
                if (events == "Внесение наличных")
                    return true;
                else return false;
            else if (event_filter == "Снятие")
                if (events == "Снятие наличных")
                    return true;
                else return false;
            else if (event_filter == "Перевод наличных")
                if (events == "Перевод наличных (пополнение)"|| events == "Перевод наличных (списание)")
                    return true;
                else return false;
            return true;
        }

        // ГЕНЕРАЦИЯ ОТЧЁТА В WORD
        private void Generate_word2()
        {
            // ПУТЬ К ШАБЛОНУ ПО ДАННЫМ ФОРМЫ
            string pathToTemplate = Application.StartupPath + "\\Log2.doc";

            // ПУТЬ К ВРЕМЕННОМУ ФАЙЛУ ДЛЯ ВСТАВКИ
            string pathToInsertingDoc = String.Format("{0}\\{1}{2}{3}", Application.StartupPath,
                "Выписка активности клиентов за ", DateTime.Today.ToString("MM/dd/yyyy"), ".doc");
            WordDocument wordDoc;
            try
            {
                wordDoc = new WordDocument(pathToTemplate);
                //wordDoc.Visible = true;
                wordDoc.ReplaceAllStrings("@period", Graf_period);
                wordDoc.ReplaceAllStrings("@filter", event_filter);
                
                wordDoc.SetSelectionToBookmark("clients", true);
                int tableNum2 = 0;
                foreach (Data.clients client in slaves)
                {
                    string info = string.Format("ID клиента: {0}\n" +
                        "Клиент: {1}\n" +
                        "Номер телефона: {2}\n" +
                        "Электронная почта: {3}\n",
                        client.id, client.fio, client.number, client.email);                    
                    wordDoc.Selection.Text = info;
                    
                    wordDoc.InsertParagraphAfter();
                    wordDoc.Selection.Text = "@before_table";
                    //wordDoc.InsertPageAtEnd();
                    wordDoc.InsertParagraphAfter();
                    wordDoc.InsertParagraphAfter();
                    wordDoc.Selection.Text = "@after_table";

                    wordDoc.SetSelectionToText("@before_table");
                    wordDoc.InsertTable(1, 5);
                    tableNum2++;
                    wordDoc.SelectTable(tableNum2);

                    int logCount = 1;
                    foreach (Data.logs log in logs)
                        if (log.idclient == client.id)
                            logCount++;

                    int addRowsCount2 = logCount;
                    for (int addRowsNum = 1; addRowsNum <= addRowsCount2; addRowsNum++)
                    {
                        wordDoc.SetSelectionToCell(addRowsNum + 1, 1);
                        wordDoc.Selection.Text = "Дата";
                        wordDoc.SetSelectionToCell(addRowsNum + 1, 2);
                        wordDoc.Selection.Text = "Событие";
                        wordDoc.SetSelectionToCell(addRowsNum + 1, 3);
                        wordDoc.Selection.Text = "Номер карты/счёта";
                        wordDoc.SetSelectionToCell(addRowsNum + 1, 4);
                        wordDoc.Selection.Text = "Остаток";
                        wordDoc.SetSelectionToCell(addRowsNum + 1, 5);
                        wordDoc.Selection.Text = "Перевод";
                    }
                    for (int addRowsNum = 1; addRowsNum <= addRowsCount2; addRowsNum++)
                    {
                            if (logs[addRowsNum - 1].date >= logs_date)
                            {
                                if (log_id == "0" || logs[addRowsNum - 1].idclient == client.id)
                                    if (LogsFilter(logs[addRowsNum - 1].events))
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
                    wordDoc.SetSelectionToText("@after_table");
                    wordDoc.Selection.Text = "";

                    wordDoc.InsertParagraphAfter();
                }
                

                //wordDoc.ReplaceAllStrings("@id", id);
                //wordDoc.ReplaceAllStrings("@fio", fio);
                //wordDoc.ReplaceAllStrings("@number", number);
                //wordDoc.ReplaceAllStrings("@email", email);
                //ВЫБОР И ЗАПОЛНЕНИЕ ИМЕЮЩЕЙСЯ ТАБЛИЦЫ


                using (File.Create(pathToInsertingDoc))
                {

                }
                wordDoc.Save(pathToInsertingDoc);
                wordDoc.Close();
                //wordDoc._application.Quit();
                //System.GC.Collect();
                
                    wordDoc = new WordDocument(pathToInsertingDoc);
                    wordDoc.Visible = true;
                    System.GC.Collect();
                
            }
            catch (Exception error)
            {
                MessageBox.Show("Ошибка при открытии шаблона Word. Подробности " + error.Message);
                return;
            }
        }
    }
}
