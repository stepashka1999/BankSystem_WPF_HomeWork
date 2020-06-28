using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank.Helper
{
    public static class ExtensionMethods
    {

        static public void AddToBank(this Client Client, Bank bank)
        {
            bank.AddClient(Client);
        }

        static public void DeleteFromBank(this Client Client, Bank bank)
        {
            bank.DeleteClient(Client);
        }


        static public void AddToBank(this Employee empl, Bank bank)
        {
            bank.AddEmployee(empl);
        }

        static public void DeleteFromBank(this Employee empl, Bank bank)
        {
            bank.DeleteEmployee(empl);
        }
    }
}
