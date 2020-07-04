using PPBank.SQL_DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PPBank
{
    public class Bank
    {
        //ListBox transactionListBox;
        public SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            InitialCatalog = "MSSQLLocal_TestDb",
            IntegratedSecurity = true,
            Pooling = true
        };

        Dispatcher dispatcher; //MainWindow Dispatcher
        ObservableCollection<Log> Logs;

        public string Name { get; private set; }
        public List<AClient> Clients { get; private set; }
        public List<Credit> Credits { get; private set; }
        public List<Deposit> Deposits { get; private set; }

        public Departament SimpleDepartament { get; private set; }
        public LegalDepartament EntityDepartament { get; private set; }
        public VIPDepartament VipDepartament { get; private set; }
        public List<Employee> Employees { get; private set; }

        public Bank(string Name, Dispatcher dispatcher)
        {
            this.Name = Name;

            //var fillTask = Task.Factory.StartNew(Fill);
            //fillTask.Wait();
            //fillTask.Dispose();
            DbFill();
            
            this.dispatcher = dispatcher;
            //Fill();
        }


        //Fill data from Data Base
        #region DataBaseFill

        private async void DbFillAsync()
        {
            await Task.Factory.StartNew(DbFill);
        }

        private void DbFill()
        {
            Credits = new List<Credit>();
            Deposits = new List<Deposit>();
            Logs = new ObservableCollection<Log>();
                                
            DbFillClients(connectionStringBuilder);
            DbFillEntitys(connectionStringBuilder);
            DbFillEmployees(connectionStringBuilder);
            
            GetEmployees();
        }

        private void DbFillClients(SqlConnectionStringBuilder connectionStringBuilder)
        {
            Clients = new List<AClient>();
            var sqlGetClients = @"SELECT * FROM Clients";

            using (var connector = new SqlConnector(connectionStringBuilder))
            {
                using (var reader = connector.GetData(sqlGetClients))
                {
                    while (reader.Read())
                    {
                        var currentClient = ConvertSqlData.ToClient(reader, this);

                        GetCredits((int)reader[0], connectionStringBuilder, currentClient);
                        GetDeposits((int)reader[0], connectionStringBuilder, currentClient);

                        Clients.Add(currentClient);
                    }
                }
            }
        }

        private void GetCredits(int index, SqlConnectionStringBuilder connectionStringBuilder, AClient holder, int entity = 0)
        {
            var sqlCreditQuery = $@"SELECT * FROM Credits
                                        WHERE HolderId = {index} AND isEntity = {entity}";
            using (var connector = new SqlConnector(connectionStringBuilder))
            {
                using (var creditReader = connector.GetData(sqlCreditQuery))
                {
                    while (creditReader.Read())
                    {
                        var credit = ConvertSqlData.ToCredit(creditReader, holder);
                        if (credit != null)
                        {
                            Credits.Add(credit);
                            holder.Credits.Add(credit);
                        }
                    }
                }
            }
        }

        private void GetDeposits(int index, SqlConnectionStringBuilder connectionStringBuilder, AClient holder, int entity = 0)
        {
            var sqlDepositQuery = $@"SELECT * FROM Deposits
                                        WHERE HolderId = {index} AND isEntity = {entity}";
            using (var connector = new SqlConnector(connectionStringBuilder))
            {
                using (var depositReader = connector.GetData(sqlDepositQuery))
                {
                    while (depositReader.Read())
                    {
                        var deposit = ConvertSqlData.ToDeposit(depositReader, holder);
                        if (deposit != null)
                        {
                            holder.Deposits.Add(deposit);
                            Deposits.Add(deposit);
                        }
                    }
                }
            }
        }

        private void DbFillEntitys(SqlConnectionStringBuilder connectionStringBuilder)
        {
            var sqlGetClients = @"SELECT * FROM Entitys";
           
            using (var connector = new SqlConnector(connectionStringBuilder))
            {
                using (var reader = connector.GetData(sqlGetClients))
                {
                    while (reader.Read())
                    {
                        var currentClient = ConvertSqlData.ToEntity(reader, this);

                        GetCredits((int)reader[0], connectionStringBuilder, currentClient, 1);
                        GetDeposits((int)reader[0], connectionStringBuilder, currentClient, 1);

                        Clients.Add(currentClient);
                    }
                }
            }
        }

        private void DbFillEmployees(SqlConnectionStringBuilder connectionStringBuilder)
        {
            SimpleDepartament = new Departament(this, "SimpleDepartament");
            EntityDepartament = new LegalDepartament(this, "EntityDepartament");
            VipDepartament = new VIPDepartament(this, "VipDepartament");

            using (var connector = new SqlConnector(connectionStringBuilder))
            {

                // 1 - Simple Departament
                DbFillEmployees(connector, SimpleDepartament, 1);

                // 2 - VipDepartametn
                DbFillEmployees(connector, VipDepartament, 2);

                // 3 - Entity Departametn
                DbFillEmployees(connector, EntityDepartament, 3);
            }
                
        }

        private void DbFillEmployees(SqlConnector connector, ADepartament departament, int index)
        {
            var sqlQuery = $@"SELECT * FROM Employees
                              WHERE DepartamentId = {index}";

            using (var reader = connector.GetData(sqlQuery))
            {
                while(reader.Read())
                {
                    var currentEmployee = ConvertSqlData.ToEmployee(reader, departament);
                    if (currentEmployee != null) departament.AddEmployee(currentEmployee);
                }
            }    
        }

        #endregion

        public void UpdateAllData()
        {
            BankDB.UpdateAll(connectionStringBuilder, this);
        }
        private List<Employee> GetEmployees()
        {
            var emplList = new List<Employee>();

            if (SimpleDepartament != null) emplList.AddRange(SimpleDepartament.Employees);

            if (VipDepartament != null) emplList.AddRange(VipDepartament.Employees);

            if (EntityDepartament != null) emplList.AddRange(EntityDepartament.Employees);

            return emplList;
        }

        public void CloseCredit(Credit credit)
        {
            BankDB.DeleteCredit(connectionStringBuilder, credit);
            Credits.Remove(credit);
        }
        public void CloseDeposit(Deposit deposit)
        {
            BankDB.DeleteDeposit(connectionStringBuilder, deposit);
            Deposits.Remove(deposit);
        }


        public void AddClient(AClient client)
        {

            if (client.Account == default(long) || client.Amount == default)
            {
                throw new UnfilledInstanceExeption();
            }

            Clients.Add(client);
        }

        public void DeleteClient(AClient client)
        {
            DeleteAClietnFromDB(client);
            Clients.Remove(client);
        }
        private void DeleteAClietnFromDB(AClient client)
        {
            if (client is Client) BankDB.DeleteClient(connectionStringBuilder, (Client)client);
            else if (client is Entity) BankDB.DeleteEntity(connectionStringBuilder ,(Entity)client);
        }

        #region Fill
        //private List<AClient> FillClientsList()
        //{
        //    var list = new List<AClient>();

        //    list.AddRange(FillClients());
        //    list.AddRange(FillClients(true));
        //    list.AddRange(FillEntitys());

        //    return list;
        //}
        //private List<AClient> FillClients(bool isVip = false)
        //{
        //    var list = new List<AClient>();
        //    var rnd = new Random();

        //    for (int i = 0; i < 3; i++)
        //    {
        //        var FirstName = isVip ? "FName_" + i : "FName_" + i + i;
        //        var SecondName = isVip ? "LName_" + i : "LName_" + i + i;

        //        var FAccoutn = rnd.Next(10_000, 100_000);
        //        var SAccoutn = rnd.Next(1_000_000, 10_000_000);
        //        long Account = (long)(FAccoutn * 10_000_000 + SAccoutn);
        //        var Amount = rnd.Next(10_000, 100_000);

        //        list.Add(new Client(this, FirstName, SecondName, isVip, Account, Amount));
        //    }

        //    return list;
        //}

        //private List<AClient> FillEntitys()
        //{
        //    var list = new List<AClient>();
        //    var rnd = new Random();

        //    for (int i = 0; i < 3; i++)
        //    {
        //        var Name = "Organization_" + i;

        //        var FAccoutn = rnd.Next(10_000, 100_000);
        //        var SAccoutn = rnd.Next(1_000_000, 10_000_000);
        //        long Account = (long)(FAccoutn * (1_000_000) + SAccoutn);
        //        var Amount = rnd.Next(100_000, 1_000_000);

        //        list.Add(new Entity(this, Name, Account, Amount));
        //    }

        //    return list;
        //}

        //private void Fill()
        //{
        //    Clients = FillClientsList();
        //    Deposits = FillDeposits();
        //    Credits = FillCredits();

        //    SimpleDepartament = new Departament(this, "SimpleDepartament");
        //    EntityDepartament = new LegalDepartament(this, "EntityDepartament");
        //    VipDepartament = new VIPDepartament(this, "VipDepartament");

        //    Employees = GetEmployees();

        //    Logs = new ObservableCollection<Log>();
        //}
        //private List<Credit> FillCredits()
        //{
        //    var list = new List<Credit>();
        //    foreach (var client in Clients)
        //    {
        //        list.AddRange(client.Credits);
        //    }
        //    return list;
        //}
        //private List<Deposit> FillDeposits()
        //{
        //    var list = new List<Deposit>();
        //    foreach (var client in Clients)
        //    {
        //        list.AddRange(client.Deposits);
        //    }
        //    return list;
        //}

        #endregion

        public List<AClient> GetClients(Func<AClient, bool> predicate)
        {
            return Clients.Where(predicate).ToList();
        }

        public void DeleteEmployee(Employee employee, TreeView tv)
        {
            BankDB.DeleteEmployee(connectionStringBuilder, employee);
            var dep = employee.Departament;


            if (dep is Departament)
            {
                SimpleDepartament.RemoveEmployee(employee);
                (tv.Items[0] as TreeViewItem).ItemsSource = SimpleDepartament.Employees;
            }
            else if (dep is VIPDepartament)
            {
                VipDepartament.RemoveEmployee(employee);
                (tv.Items[1] as TreeViewItem).ItemsSource = VipDepartament.Employees;
            }
            else if (dep is LegalDepartament)
            {
                EntityDepartament.RemoveEmployee(employee);
                (tv.Items[2] as TreeViewItem).ItemsSource = EntityDepartament.Employees;
            }

            Employees.Remove(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            BankDB.DeleteEmployee(connectionStringBuilder, employee);
            var dep = employee.Departament;

            if (dep is Departament)
            {
                SimpleDepartament.RemoveEmployee(employee);
            }
            else if (dep is VIPDepartament)
            {
                VipDepartament.RemoveEmployee(employee);
            }
            else if (dep is LegalDepartament)
            {
                EntityDepartament.RemoveEmployee(employee);
            }

            Employees.Remove(employee);
        }

        public void AddEmployee(Employee employee)
        {
            if(employee.FirstName == null || employee.LastName == null || employee.Phone == default(long) || employee.Departament == null )
            {
                throw new UnfilledInstanceExeption();
            }

            var dep = employee.Departament;

            if (dep is Departament)
            {
                SimpleDepartament.AddEmployee(employee);
            }
            else if (dep is VIPDepartament)
            {
                VipDepartament.AddEmployee(employee);
            }
            else if (dep is LegalDepartament)
            {
                EntityDepartament.AddEmployee(employee);
            }

           // BankDB.AddEmployee(connectionStringBuilder, employee);
            Employees.Add(employee);
        }

        #region Fill TreeView
        public void FillTreeViewEmployees(TreeView tv)
        {
            var tvItemD = new TreeViewItem();

            //SimpleDepartament.Fill(); 
            tvItemD.Header = SimpleDepartament.Name;
            tvItemD.ItemsSource = SimpleDepartament.Employees;
            tv.Items.Add(tvItemD);

            var tvItemED = new TreeViewItem();
            
            //EntityDepartament.Fill();
            tvItemED.Header = EntityDepartament.Name;
            tvItemED.ItemsSource = EntityDepartament.Employees;
            tv.Items.Add(tvItemED);

            var tvItemV = new TreeViewItem();
            
            //VipDepartament.Fill();
            tvItemV.Header = VipDepartament.Name;
            tvItemV.ItemsSource = VipDepartament.Employees;
            tv.Items.Add(tvItemV);

            Employees = GetEmployees();
        }

        public void FillTreeViewClients(TreeView tv)
        {
            var header = "Clients";
            Func<AClient, bool> func = x => (x is Client) && (x as Client).isVIP == false;

            var item = FillTreeViewItem(header, func);
            
            tv.Items.Add(item);

            header = "VIP Clients";
            func = x => (x is Client) && (x as Client).isVIP == true;

            item = FillTreeViewItem(header, func);

            tv.Items.Add(item);

            header = "Entitys";
            func = x => (x is Entity);

            item = FillTreeViewItem(header, func);

            tv.Items.Add(item);
        }

        public TreeViewItem FillTreeViewItem(TreeViewItem item, Func<AClient, bool> delegat)
        {
            var header = item.Header;
            item = new TreeViewItem();
            item.Header = header;

            item.ItemsSource = Clients.Where(delegat);
            item.DataContext = Clients.Where(delegat).ToList();

            return item;
        }

        private TreeViewItem FillTreeViewItem(string Header, Func<AClient,bool> delegat)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = Header;

            item.ItemsSource = Clients.Where(delegat);
            item.DataContext = Clients.Where(delegat).ToList();

            return item;
        }

        #endregion

        public void AddLog(string log)
        {
            Logs.Add(new Log() { Text = log});
        }
        public void MakePaymentTask()
        {
            var doTask = Task.Factory.StartNew(MakePayment);
            doTask.Wait();
            doTask.Dispose();
        }

        public void MakePayment()
        {
            foreach(var credit in Credits)
            {
                credit.MakePayment();
            }

            foreach(var deposit in Deposits)
            {
                deposit.MakePayment();
            }
        }

        public void SetTransactionList(ListBox lb)
        {
            lb.ItemsSource = Logs;   
            //transactionListBox = lb;

            AClient.MoneyReceived += AClient_MoneyReceived;
            AClient.MoneySent += AClient_MoneySent;

            AClient.CreditClosed += Credit_CreditClosed;
            AClient.CreditOpend += Credit_CreditOpend;
            Credit.MakedPayment += Credit_MakedPayment;

            AClient.DepositClosed += Deposit_MakedPayment;
            AClient.DepositOpend += Deposit_DepositOpend;
            AClient.NotEnoughMoney += AClient_NotEnoughMoney;
        }


        #region Event Handlers


        private async void AClient_NotEnoughMoney(string obj)
        {
            await dispatcher.InvokeAsync(() => Logs.Add(new Log() { Text = obj }));
            //transactionListBox.Items.Add(obj);
        }

        private async void Deposit_MakedPayment(AClient arg1, Deposit arg2, decimal arg3)
        {          
            var pattern = $"Клиент {arg1} закрыл депозит.\nСумма: {arg2.Amount}\nПроцент: {arg2.Percent}";

            await dispatcher.InvokeAsync(()=> Logs.Add(new Log() { Text = pattern }));
            // transactionListBox.Items.Add(pattern);
        }

        private async void Deposit_DepositOpend(AClient arg1, Deposit arg2)
        {
            var pattern = $"Клиент {arg1} открыл депозит:\nСрок: {arg2.Month}\nСумма: {arg2.Amount}\nПроцент: {arg2.Percent}";

            await dispatcher.InvokeAsync(() => Logs.Add(new Log() { Text = pattern }));
            //transactionListBox.Items.Add(pattern);
        }

        private async void Credit_MakedPayment(AClient arg1, Credit arg2, decimal arg3)
        {
            var creditInfo = $"Сумма: {arg2.Amount} \nСрок: {arg2.Month} \nПроцент: {arg2.Percent}";
            var pattern = $"Клиент {arg1} сделал взнос по кредиту\nна сумму {arg3}. Credit: \n{creditInfo}";
                        
            await dispatcher.InvokeAsync(()=> Logs.Add(new Log() { Text = pattern }));
            //transactionListBox.Items.Add(pattern);
        }

        private async void Credit_CreditOpend(AClient arg1, Credit arg2)
        {
            var creditInfo = $"Сумма: {arg2.Amount} \nСрок: {arg2.Month}";
            var pattern = $"Клиент {arg1} Открыл кредит. Credit: \n{creditInfo}";

            await dispatcher.InvokeAsync(() => Logs.Add(new Log() { Text = pattern }));
            //transactionListBox.Items.Add(pattern);
        }

        private async void Credit_CreditClosed(AClient arg1, Credit arg2)
        {
            var pattern = $"Клиент {arg1} закрыл кредит.";

            await dispatcher.InvokeAsync(() => Logs.Add(new Log() { Text = pattern }));
            //transactionListBox.Items.Add(pattern);
        }

        private async void AClient_MoneySent(AClient arg1, AClient arg2, decimal arg3)
        {
            var pattern = $"Клиент {arg1} отправил\n{arg3} руб.\nклиенту {arg2}";

            await dispatcher.InvokeAsync(() => Logs.Add(new Log() { Text = pattern }));
            //transactionListBox.Items.Add(pattern);
        }

        private async void AClient_MoneyReceived(AClient arg1, AClient arg2, decimal arg3)
        {
            var pattern = $"Клиент {arg1} \nполучил {arg3} руб. \nот клиента {arg2}";

            await dispatcher.InvokeAsync(() => Logs.Add(new Log() { Text = pattern }));
            //transactionListBox.Items.Add(pattern);
        }

        #endregion

    }
}
