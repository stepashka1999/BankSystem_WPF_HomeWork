using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_13
{
    public class Entity:AClient
    {
        public string Name { get; private set; }
        public override string Info { get => $"Name: {Name}\nAccount: {Account}\nAmount: {Amount}\nCredits: {Credits.Count}\nDeposits: {Deposits.Count}\nCredit History: {CreditHistory}"; }

        public Entity(Bank bank, string Name, ulong Account, SqlMoney Amount): base(bank,Account, Amount)
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
