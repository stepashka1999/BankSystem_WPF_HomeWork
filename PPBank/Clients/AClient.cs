using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PPBank
{
    public abstract class AClient: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        static public event Action<AClient, AClient, decimal> MoneyReceived;
        static public event Action<AClient, AClient, decimal> MoneySent;

        public static event Action<AClient, Credit> CreditOpend;
        public static event Action<AClient, Credit> CreditClosed;

        public static event Action<AClient, Deposit> DepositOpend;
        public static event Action<AClient, Deposit, decimal> DepositClosed;
        public static event Action<string> NotEnoughMoney;

        Bank Bank { get; set; }

        public int Id { get; private set; }

        private long account;
        public long Account { get => account; private set { account = value; OnPropertyChanged(nameof(Account)); } }

        private decimal amount;
        public decimal Amount { get => amount; private set { amount = value; OnPropertyChanged(nameof(amount)); } }
        public CreditHistory CreditHistory { get; private set; }
        public virtual string Info { get; set; }
        
        public List<Credit> Credits { get; private set; }
        public List<Deposit> Deposits { get; private set; }
        
        public AClient(Bank Bank, decimal Amount, int Id, CreditHistory creditHistory = CreditHistory.Normal)
        {
            this.Id = Id;
            this.Bank = Bank;
            this.Amount = Amount;
            Credits = new List<Credit>();
            Deposits = new List<Deposit>();
            CreditHistory = creditHistory;

            //FillCredits();
            //FillDeposits();
        }
        public AClient(Bank Bank,long Account, decimal Amount, int Id, CreditHistory creditHistory = CreditHistory.Normal): this(Bank, Amount, Id, creditHistory)
        {
            this.Account = Account;
        }

        
        public int WithdrawMoney(decimal money)
        {
            if (Amount >= money)
            {
                Amount -= money;
                return 0;
            }

            throw new NotEnoughtMoneyExeption();
        }
        public void InputMoney(decimal money)
        {
            Amount += money;
        }

        public void SendMoneyTo(AClient client, decimal money)
        {
            try
            {
                WithdrawMoney(money);
                client.InputMoney(money);
                MoneySent?.Invoke(this, client, money);
            }
            catch(NotEnoughtMoneyExeption)
            {
                var msg = $"У Клиента{this}\nНедостаточно средств, \nдля перевода клиенту{client}";
                NotEnoughMoney?.Invoke(msg);
            }    
        }
        public void RequestMoneyFrom(AClient client, decimal money)
        {
            if (client.Amount >= money)
            {
                InputMoney(money);
                client.WithdrawMoney(money);
                MoneyReceived?.Invoke(this, client, money);
            }
            else MessageBox.Show("На счете данного клиента недостаточно средств.");
        }


        public void CloseCredit(Credit credit)
        {
            try
            { 
                WithdrawMoney(credit.Amount);
                Amount -= credit.Amount;
                Credits.Remove(credit);
                Bank.CloseCredit(credit);
                CreditClosed?.Invoke(this, credit);
            }
            catch(NotEnoughtMoneyExeption)
            {
                var msg = $"У Клиента{this}\nНедостаточно средств, \nдля заркытия кредита!";
                NotEnoughMoney?.Invoke(msg);
            }
        }

        public int MakePayment(decimal money)
        {
            try
            {
                var res = WithdrawMoney(money);
                return res;
            }
            catch(NotEnoughtMoneyExeption)
            {
                var msg = $"У Клиента{this}\nНедостаточно средств, \nдля платы по кредиту.";
                NotEnoughMoney?.Invoke(msg);
                return -1;
            }
        }

        public void CloseDeposit(Deposit deposit)
        {
            Amount += deposit.Amount;
            Deposits.Remove(deposit);
            Bank.CloseDeposit(deposit);
            DepositClosed?.Invoke(this, deposit, deposit.Amount);
        }

        public void OpenCredit(Credit credit)
        {
            CreditOpend?.Invoke(this, credit);
            Credits.Add(credit);
        }
        
        public void OpenDeposit(Deposit deposit)
        {
            Amount -= deposit.Amount;
            Deposits.Add(deposit);
            DepositOpend?.Invoke(this, deposit);
        }

        //private void FillCredits()
        //{
        //    var rnd = new Random();
        //    for(int i = 0; i < 2; i++)
        //    {
        //        var Amount = rnd.Next(100_000, 1_000_000);
        //        Credits.Add(new Credit(this, Amount));
        //    }
        //}

        //private void FillDeposits()
        //{
        //    var rnd = new Random();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        var Amount = rnd.Next(100_000, 1_000_000);
        //        Deposits.Add(new Deposit(this, Amount));
        //    }
        //}

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
