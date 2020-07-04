using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public class Client : AClient
    {

        private string firstName;
        public string FirstName { get => firstName; private set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }

        private string lastName;
        public string LastName { get => lastName; private set { lastName = value; OnPropertyChanged(nameof(LastName)); } }

        private bool isVip;
        public bool isVIP { get => isVip; private set { isVip = value; OnPropertyChanged(nameof(isVIP)); } }

        public override string Info { get => $"First Name: {FirstName}\nLast Name: {LastName}\n" +
                                             $"Account: {Account}\nAmount: {Amount}\n" +
                                             $"Status: {(isVIP? "VIP" : "Default")}\n" +
                                             $"Credits: {Credits.Count}\nDeposits: {Deposits.Count}\n" +
                                             $"Credit History: {CreditHistory}"; }

        public Client(Bank Bank,string FirstName, string LastName, long Account, decimal Amount, int id): base(Bank, Account, Amount, id)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
        }

        public Client(Bank Bank ,string FirstName, string LastName, bool isVIP, long Account, decimal Amount, int id) : this(Bank, FirstName, LastName, Account, Amount, id)
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
