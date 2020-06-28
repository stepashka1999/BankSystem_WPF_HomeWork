using System;
using System.Collections.Generic;
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

    public class Credit
    {
        public static event Action<AClient, Credit, SqlMoney> MakedPayment;
        
        public AClient Holder { get; private set; }
        public SqlMoney Amount { get; private set; }
        public int Percent { get; private set; }

        public int Month { get; private set; }

        public SqlMoney Payment;        

        public Credit(AClient Holder, SqlMoney Amount, int Month = 12)
        {
            this.Holder = Holder;
            Percent = (int)Holder.CreditHistory;
            this.Amount = Amount*(1 + (SqlMoney)(Percent)/100);
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
    }
}
