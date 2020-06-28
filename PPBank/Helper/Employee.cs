using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public class Employee
    {
        public string FirstName {get; private set;}
        public string LastName { get; private set; }
        public long Phone { get; private set; }
        public ADepartament Departament { get; private set; }

        public string Info
        {
            get => $"First Name: {FirstName}\nLast Name: {LastName}\n" +
                   $"Phone: {Phone}\n" +
                   $"SimpleDepartament: {Departament.Name}";
        }

        public Employee(ADepartament departament, string FirstName, string LastName, long Phone)
        {
            Departament = departament;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
        }

        public void Edit(string FName, string LName, long Phone)
        {
            FirstName = FName;
            LastName = LName;
            this.Phone = Phone;
        }

        public void ChangeDepartament(ADepartament dep)
        {
            Departament.RemoveEmployee(this);
            Departament = dep;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

    }
}
