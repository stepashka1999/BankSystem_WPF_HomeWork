using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public abstract class ADepartament
    {
        private protected Bank Bank { get; set; }
        public string Name { get; private set; }
        public List<Employee> Employees { get; private set; }
        public virtual List<AClient> Clients { get; private protected set; } //Клиенты с которыми работает департамент
       
        public void AddEmployee(Employee empl)
        {            
            Employees.Add(empl);
        }
        public void AddNewEmployee(Employee employee)
        {
            Employees.Add(employee);
            Bank.AddEmployee(employee);
        }
        public void RemoveEmployee(Employee empl)
        {
            Employees.Remove(empl);
        }
        public void DeleteEmployee(Employee empl)
        {
            Employees.Remove(empl);
            Bank.DeleteEmployee(empl);
        }

        //public void Fill()
        //{
        //    Random rnd = new Random();
        //    for(int i = 0; i < 10; i++)
        //    {
        //        int fh_phone = rnd.Next(8_000_00, 9_000_00);
        //        int sh_phone = rnd.Next(10_00_00, 100_00_00);
        //        long phone = (long)fh_phone * 10_00_00 + sh_phone;

        //        Employee empl = new Employee(this, $"FNameE_{i}", $"LNameE_{i}", phone);
        //        Employees.Add(empl);
        //    }
        //}

        public ADepartament(Bank Bank, string Name)
        {
            this.Bank = Bank;
            this.Name = Name;
            Employees = new List<Employee>();
        }
    }
}
