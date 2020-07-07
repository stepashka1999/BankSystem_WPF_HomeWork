using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_13
{
    public class Deposit
    {
        

        public AClient Holder { get; private set; }
        public SqlMoney Amount { get; private set; }
        public int Percent { get => 8; }

        public int Month { get; private set; }

        SqlMoney Payment;

        public Deposit(AClient Holder, SqlMoney Amount, int Month = 12)
        {
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
    }
}
