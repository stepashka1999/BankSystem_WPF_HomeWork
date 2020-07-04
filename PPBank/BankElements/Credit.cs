using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public enum CreditHistory
    {
        Good = 10,
        Normal = 25,
        Bad = 40
    }

    public class Credit: INotifyPropertyChanged
    {
        public static event Action<AClient, Credit, decimal> MakedPayment;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; private set; }
        public AClient Holder { get; private set; }

        private decimal amount;
        public decimal Amount { get => amount; private set { amount = value; OnPropertyChanged(nameof(Amount)); } }

        public int Percent { get; private set; }

        private int month;
        public int Month { get => month; private set { month = value; OnPropertyChanged(nameof(Month)); } }

        public decimal Payment;        

        public Credit(AClient Holder, decimal Amount, int id, int Month = 12)
        {
            Id = id;
            this.Holder = Holder;
            Percent = (int)Holder.CreditHistory;
            this.Amount = Amount*(1 + (decimal)(Percent)/100);
            this.Month = Month;
            Payment = Amount / Month;

        }

        public void MakePayment()
        {
            if(Holder.MakePayment(Payment) == 0)
            {
                Amount -= Payment;
                Month--;
                if (Amount == 0 || Month == 0)
                {
                    Holder.CloseCredit(this);
                }
                
                MakedPayment?.Invoke(Holder, this, Payment);
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
