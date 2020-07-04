using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public class Entity:AClient
    {
        private string name;
        public string Name { get => name; private set { name = value; OnPropertyChanged(nameof(Name)); } }
        public override string Info { get => $"Name: {Name}\nAccount: {Account}\nAmount: {Amount}\nCredits: {Credits.Count}\nDeposits: {Deposits.Count}\nCredit History: {CreditHistory}"; }

        public Entity(Bank bank, string Name, long Account, decimal Amount, int id): base(bank,Account, Amount, id)
        {
            this.Name = Name;
        }


        public void Edit(string Name)
        {
            this.Name = Name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
