using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeWork_13
{
    public abstract class AClient
    {
        static public event Action<AClient, AClient, SqlMoney> MoneyReceived;
        static public event Action<AClient, AClient, SqlMoney> MoneySent;


        public static event Action<AClient, Credit> CreditOpend;
        public static event Action<AClient, Credit> CreditClosed;

        public static event Action<AClient, Deposit> DepositOpend;
        public static event Action<AClient, Deposit, SqlMoney> DepositClosed;
        public static event Action<string> NotEnoughMoney;

        Bank Bank { get; set; }
        public ulong Account { get; private set; }
        public SqlMoney Amount { get; private set; }
        public CreditHistory CreditHistory { get; private set; }
        public virtual string Info { get; set; }
        
        public List<Credit> Credits { get; private set; }
        public List<Deposit> Deposits { get; private set; }
        
        public AClient(Bank Bank, SqlMoney Amount, CreditHistory creditHistory = CreditHistory.Normal)
        {
            this.Bank = Bank;
            this.Amount = Amount;
            Credits = new List<Credit>();
            Deposits = new List<Deposit>();
            CreditHistory = creditHistory;

            FillCredits();
            FillDeposits();
        }
        public AClient(Bank Bank,ulong Account, SqlMoney Amount, CreditHistory creditHistory = CreditHistory.Normal): this(Bank ,Amount, creditHistory)
        {
            this.Account = Account;
        }

        
        public int WithdrawMoney(SqlMoney money)
        {
            if (Amount >= money)
            {
                Amount -= money;
                return 0;
            }
            
            return 1;
        }
        public void InputMoney(SqlMoney money)
        {
            Amount += money;
        }

        public void SendMoneyTo(AClient client, SqlMoney money)
        {
            if (Amount >= money)
            {
                WithdrawMoney(money);

                client.InputMoney(money);
                MoneySent?.Invoke(this, client, money);
            }
            else
            {
                var msg = $"У Клиента{this}\nНедостаточно средств, \nдля перевода клиенту{client}";
                NotEnoughMoney?.Invoke(msg);
            }
        }
        public void RequestMoneyFrom(AClient client, SqlMoney money)
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
            if (Amount >= credit.Amount)
            {
                Amount -= credit.Amount;
                Credits.Remove(credit);
                Bank.CloseCredit(credit);
                CreditClosed?.Invoke(this, credit);
            }
            else
            {
                var msg = $"У Клиента{this}\nНедостаточно средств, \nдля заркытия кредита!";
                NotEnoughMoney?.Invoke(msg);
            }
        }

        public int MakePayment(SqlMoney money)
        {
            if(WithdrawMoney(money)==0)
            {
                var msg = $"У Клиента{this}\nНедостаточно средств, \nдля платы по кредиту.";
                NotEnoughMoney?.Invoke(msg);
            }

            return WithdrawMoney(money);
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

        private void FillCredits()
        {
            var rnd = new Random();
            for(int i = 0; i < 2; i++)
            {
                var Amount = rnd.Next(100_000, 1_000_000);
                Credits.Add(new Credit(this, Amount));
            }
        }

        private void FillDeposits()
        {
            var rnd = new Random();
            for (int i = 0; i < 2; i++)
            {
                var Amount = rnd.Next(100_000, 1_000_000);
                Deposits.Add(new Deposit(this, Amount));
            }
        }
    }
}
