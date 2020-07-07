using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{ 
    public class Client:AClient
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool isVIP { get; private set; }

        public override string Info { get => $"First Name: {FirstName}\nLast Name: {LastName}\n" +
                                             $"Account: {Account}\nAmount: {Amount}\n" +
                                             $"Status: {(isVIP? "VIP" : "Default")}\n" +
                                             $"Credits: {Credits.Count}\nDeposits: {Deposits.Count}\n" +
                                             $"Credit History: {CreditHistory}"; }

        public Client(Bank Bank,string FirstName, string LastName, ulong Account, SqlMoney Amount): base(Bank, Account, Amount)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
        }

        public Client(Bank Bank ,string FirstName, string LastName, bool isVIP, ulong Account, SqlMoney Amount) : this(Bank, FirstName, LastName, Account, Amount)
        {
            this.isVIP = isVIP;
        }

        public void MakeVIP()
        {
            isVIP = !isVIP;
        }

        public void Edit(string FName, string LName)
        {
            FirstName = FName;
            LastName = LName;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
