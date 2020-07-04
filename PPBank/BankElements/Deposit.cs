using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PPBank
{
    public class Deposit: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Id { get; private set; }
        public AClient Holder { get; private set; }

        private decimal amount;
        public decimal Amount { get => amount; private set { amount = value; OnPropertyChanged(nameof(Amount)); } }
        public int Percent { get => 8; }

        private int month;
        public int Month { get => month; private set { month = value; OnPropertyChanged(nameof(Month)); } }

        decimal Payment;


        public Deposit(AClient Holder, decimal Amount, int id, int Month = 12)
        {
            Id = id;
            this.Holder = Holder;
            this.Amount = Amount;
            this.Month = Month;
            Payment = (decimal)Percent / 100;

        }

        public void MakePayment()
        {
            Amount += Payment;
            Month--;

            if (Month == 0)
            {
                Holder.InputMoney(Amount);
                Holder.CloseDeposit(this);               
            }
        }

        public override string ToString()
        {
            return $"{Amount} | {Percent}% | {Month}m";
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
