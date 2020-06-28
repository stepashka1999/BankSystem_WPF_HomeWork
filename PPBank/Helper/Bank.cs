using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PPBank
{
    public class Bank
    {
        ListBox transactionListBox;

        public string Name { get; private set; }
        public List<AClient> Clients { get; private set; }
        public List<Credit> Credits { get; private set; }
        public List<Deposit> Deposits { get; private set; }

        public Departament SimpleDepartament { get; private set; }
        public LegalDepartament EntityDepartament { get; private set; }
        public VIPDepartament VipDepartament { get; private set; }
        public List<Employee> Employees { get; private set; }

        public Bank(string Name)
        {
            this.Name = Name;

            Fill();
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
            Credits.Remove(credit);
        }
        public void CloseDeposit(Deposit deposit)
        {
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
            Clients.Remove(client);
        }

        #region Fill
        private List<AClient> FillClientsList()
        {
            var list = new List<AClient>();

            list.AddRange(FillClients());
            list.AddRange(FillClients(true));
            list.AddRange(FillEntitys());

            return list;
        }
        private List<AClient> FillClients(bool isVip = false)
        {
            var list = new List<AClient>();
            var rnd = new Random();

            for (int i = 0; i < 3; i++)
            {
                var FirstName = isVip ? "FName_" + i : "FName_" + i + i;
                var SecondName = isVip ? "LName_" + i : "LName_" + i + i;

                var FAccoutn = rnd.Next(10_000, 100_000);
                var SAccoutn = rnd.Next(1_000_000, 10_000_000);
                ulong Account = (ulong)(FAccoutn * 10_000_000 + SAccoutn);
                var Amount = rnd.Next(10_000, 100_000);

                list.Add(new Client(this, FirstName, SecondName, isVip, Account, Amount));
            }

            return list;
        }

        private List<AClient> FillEntitys()
        {
            var list = new List<AClient>();
            var rnd = new Random();

            for (int i = 0; i < 3; i++)
            {
                var Name = "Organization_" + i;

                var FAccoutn = rnd.Next(10_000, 100_000);
                var SAccoutn = rnd.Next(1_000_000, 10_000_000);
                ulong Account = (ulong)(FAccoutn * (1_000_000) + SAccoutn);
                var Amount = rnd.Next(100_000, 1_000_000);

                list.Add(new Entity(this, Name, Account, Amount));
            }

            return list;
        }

        private void Fill()
        {
            Clients = FillClientsList();
            Deposits = FillDeposits();
            Credits = FillCredits();

            SimpleDepartament = new Departament(this, "SimpleDepartament");
            EntityDepartament = new LegalDepartament(this, "EntityDepartament");
            VipDepartament = new VIPDepartament(this, "VipDepartament");

            Employees = GetEmployees();
        }
        private List<Credit> FillCredits()
        {
            var list = new List<Credit>();
            foreach (var client in Clients)
            {
                list.AddRange(client.Credits);
            }
            return list;
        }
        private List<Deposit> FillDeposits()
        {
            var list = new List<Deposit>();
            foreach (var client in Clients)
            {
                list.AddRange(client.Deposits);
            }
            return list;
        }

        #endregion

        public List<AClient> GetClients(Func<AClient, bool> predicate)
        {
            return Clients.Where(predicate).ToList();
        }

        public void DeleteEmployee(Employee employee, TreeView tv)
        {
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

        public void AddEmployee(Employee employee, TreeView tv)
        {
           var dep = employee.Departament;
           AddEmployee(employee);

           if(dep is Departament)
           {
                SimpleDepartament.AddEmployee(employee);
                (tv.Items[0] as TreeViewItem).ItemsSource = SimpleDepartament.Employees;
           }
           else if(dep is VIPDepartament)
           {
                VipDepartament.AddEmployee(employee);
                (tv.Items[1] as TreeViewItem).ItemsSource = VipDepartament.Employees;
            }
           else if(dep is LegalDepartament)
           {
                EntityDepartament.AddEmployee(employee);
                (tv.Items[2] as TreeViewItem).ItemsSource = EntityDepartament.Employees;
            }

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

            Employees.Add(employee);
        }

        #region Fill TreeView
        public void FillTreeViewEmployees(TreeView tv)
        {
            var tvItemD = new TreeViewItem();

            SimpleDepartament.Fill(); 
            tvItemD.Header = SimpleDepartament.Name;
            tvItemD.ItemsSource = SimpleDepartament.Employees;
            tv.Items.Add(tvItemD);

            var tvItemED = new TreeViewItem();
            EntityDepartament.Fill();
            tvItemED.Header = EntityDepartament.Name;
            tvItemED.ItemsSource = EntityDepartament.Employees;
            tv.Items.Add(tvItemED);

            var tvItemV = new TreeViewItem();
            VipDepartament.Fill();
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
            transactionListBox = lb;

            AClient.MoneyReceived += AClient_MoneyReceived;
            AClient.MoneySent += AClient_MoneySent;

            AClient.CreditClosed += Credit_CreditClosed;
            AClient.CreditOpend += Credit_CreditOpend;
            Credit.MakedPayment += Credit_MakedPayment;

            AClient.DepositClosed += Deposit_MakedPayment;
            AClient.DepositOpend += Deposit_DepositOpend;
            AClient.NotEnoughMoney += AClient_NotEnoughMoney;
        }

        private void AClient_NotEnoughMoney(string obj)
        {
            transactionListBox.Items.Add(obj);
        }

        private void Deposit_MakedPayment(AClient arg1, Deposit arg2, System.Data.SqlTypes.SqlMoney arg3)
        {
            var pattern = $"Клиент {arg1} закрыл депозит.\nСумма: {arg2.Amount}\nПроцент: {arg2.Percent}";
            transactionListBox.Items.Add(pattern);
        }

        private void Deposit_DepositOpend(AClient arg1, Deposit arg2)
        {
            var pattern = $"Клиент {arg1} открыл депозит:\nСрок: {arg2.Month}\nСумма: {arg2.Amount}\nПроцент: {arg2.Percent}";
            transactionListBox.Items.Add(pattern);
        }



        private void Credit_MakedPayment(AClient arg1, Credit arg2, System.Data.SqlTypes.SqlMoney arg3)
        {
            var creditInfo = $"Сумма: {arg2.Amount} \nСрок: {arg2.Month} \nПроцент: {arg2.Percent}";
            var pattern = $"Клиент {arg1} сделал взнос по кредиту\nна сумму {arg3}. Credit: \n{creditInfo}";
            transactionListBox.Items.Add(pattern);
        }

        private void Credit_CreditOpend(AClient arg1, Credit arg2)
        {
            var creditInfo = $"Сумма: {arg2.Amount} \nСрок: {arg2.Month}";
            var pattern = $"Клиент {arg1} Открыл кредит. Credit: \n{creditInfo}";
            transactionListBox.Items.Add(pattern);
        }

        private void Credit_CreditClosed(AClient arg1, Credit arg2)
        {
            var pattern = $"Клиент {arg1} закрыл кредит.";
            transactionListBox.Items.Add(pattern);
        }

        private void AClient_MoneySent(AClient arg1, AClient arg2, System.Data.SqlTypes.SqlMoney arg3)
        {
            var pattern = $"Клиент {arg1} отправил\n{arg3} руб.\nклиенту {arg2}";
            transactionListBox.Items.Add(pattern);
        }

        private void AClient_MoneyReceived(AClient arg1, AClient arg2, System.Data.SqlTypes.SqlMoney arg3)
        {
            var pattern = $"Клиент {arg1} \nполучил {arg3} руб. \nот клиента {arg2}";
            transactionListBox.Items.Add(pattern);
        }
    }
}
