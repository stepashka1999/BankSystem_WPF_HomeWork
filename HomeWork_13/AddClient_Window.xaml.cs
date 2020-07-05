using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PPBank;

namespace HomeWork_13
{
    enum CreationType
    {
        Client,
        Employee,
        Organisation
    }

    /// <summary>
    /// Логика взаимодействия для AddClient_Window.xaml
    /// </summary>
    partial class AddClient_Window : Window
    {
        Bank bank;
        SqlConnectionStringBuilder stringBuilder;

        public Action<Client> ClientCreated;
        public Action<Employee> EmployeeCreated;
        public Action<Entity> EntityCreated;

        CreationType creationType;

        public AddClient_Window(Bank bank, SqlConnectionStringBuilder stringBuilder)
        {
            InitializeComponent();

            DateTime.Now.ToShortTimeString();

            creationType = CreationType.Client;

            ClientType_cb.SelectedIndex = 0;
            DepartamentType_cb.SelectedIndex = 0;

            this.stringBuilder = stringBuilder;
            this.bank = bank;
            EmplUI(Visibility.Hidden);
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            switch(creationType)
            {
                case CreationType.Client:

                    var client = CreateClient();

                    if(client != null)
                    {
                        ClientCreated?.Invoke(client);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Некоторое(-ые) поле(-я) заполненный некорректно.");
                        return;
                    }

                    break;
                case CreationType.Employee:

                    var empl = CreateEmployee();

                    if(empl != null)
                    {
                        EmployeeCreated?.Invoke(empl);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Некоторое(-ые) поле(-я) заполненный некорректно.");
                        return;
                    }

                    break;
                case CreationType.Organisation:

                    var entity = CreateEntity();

                    if(entity != null)
                    {
                        EntityCreated?.Invoke(entity);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Некоторое(-ые) поле(-я) заполненный некорректно.");
                        return;
                    }
                    
                    break;
            }
        }

        private Client CreateClient()
        {
            var rnd = new Random();

            var FirstName = FName_tb.Text;
            var SecondName = LName_tb.Text;
            var isVip = VIP_cb.IsChecked;
            decimal money;

            if (String.IsNullOrEmpty(FirstName) || String.IsNullOrEmpty(SecondName) || !decimal.TryParse(Money_tb.Text, out money))
            {
                return null;
            }


            var FAccoutn = (long)rnd.Next(10_000, 100_000);
            var SAccoutn = (long)rnd.Next(1_000_000, 10_000_000);
            long Account = (FAccoutn * 10_000_000 + SAccoutn);

            

            BankDB.AddClient(bank.connectionStringBuilder, FirstName, SecondName, (bool)isVip, Account, money);

            var client = BankDB.GetLastClient(stringBuilder, bank);

            return client;
        }


        private Employee CreateEmployee()
        {
            var FirstName = FName_tb.Text;
            var SecondName = LName_tb.Text;

            var phoneText = Phone_mtb.Text;


            long Phone = GetInt(phoneText);

            ADepartament dep = null;
            int depId = 0;

            switch (DepartamentType_cb.SelectedIndex)
            {
                case 0:
                    depId = 1;
                    dep = bank.SimpleDepartament;
                    break;
                case 1:
                    depId = 3;
                    dep = bank.EntityDepartament;
                    break;
                case 2:
                    depId = 2;
                    dep = bank.VipDepartament;
                    break;
            }

            if (String.IsNullOrEmpty(FirstName) || String.IsNullOrEmpty(SecondName) || Phone.ToString().Length < 11)
            {
                return null;
            }

            BankDB.AddEmployee(stringBuilder, FirstName, SecondName, Phone, depId);
            var empl = BankDB.GetLastEmployee(stringBuilder, dep);

            return empl;
        }

        private long GetInt(string str)
        {
            string res = "";
            foreach(var simbol in str)
            {
                if (char.IsDigit(simbol)) res += simbol;
            }

            return Convert.ToInt64(res);
        }

        private Entity CreateEntity()
        {
            var rnd = new Random();

            var Name = FName_tb.Text;
            var FAccoutn = rnd.Next(10_000, 100_000);
            var SAccoutn = rnd.Next(1_000_000, 10_000_000);
            long Account = (FAccoutn * (1_000_000) + SAccoutn);
            decimal money;

            if (string.IsNullOrEmpty(Name) || !decimal.TryParse(Money_tb.Text, out money))
            {
                return null;
            }
            BankDB.AddEntity(stringBuilder, Name, Account, money);
            var entity = BankDB.GetLastEntity(stringBuilder, bank);

            return entity;
        }

        private void EntityUI()
        {
            EmplUI(Visibility.Hidden);

            LName_lbl.Visibility = Visibility.Hidden;
            LName_tb.Visibility = Visibility.Hidden;
            DepartamentType_cb.Visibility = Visibility.Hidden;
            Phone_lbl.Visibility = Visibility.Hidden;
            Phone_mtb.Visibility = Visibility.Hidden;
            VIP_cb.Visibility = Visibility.Hidden;

            FName_lbl.Content = "Name";

        }

        private void EmplUI(Visibility visible)
        {
            FName_lbl.Content = "First Name";
            Phone_lbl.Visibility = visible;
            Phone_mtb.Visibility = visible;
            DepartamentType_cb.Visibility = visible;
            DepType_lbl.Visibility = visible;
            
            VIP_cb.Visibility = visible == Visibility.Visible? Visibility.Hidden : Visibility.Visible;
            Money_lbl.Visibility = visible == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            Money_tb.Visibility = visible == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void ClientType_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var content = (ClientType_cb.SelectedItem as ComboBoxItem).Content;
            if (content.Equals("Client"))
            {
                creationType = CreationType.Client;
                EmplUI(Visibility.Hidden);
            }
            else if(content.Equals("Employee"))
            {
                creationType = CreationType.Employee;
                EmplUI(Visibility.Visible);
            }
            else if(content.Equals("Organisation"))
            {
                creationType = CreationType.Organisation;
                EntityUI();
            }
        }

        private void Money_tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.Text, 0));
        }
    }
}
