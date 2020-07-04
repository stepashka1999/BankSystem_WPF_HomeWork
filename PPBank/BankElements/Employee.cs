using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public class Employee: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; private set; }

        private string firstName;
        public string FirstName {get => firstName; private set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }

        private string lastName;
        public string LastName { get => lastName; private set { lastName = value; OnPropertyChanged(nameof(LastName)); } }

        private long phone;
        public long Phone { get => phone; private set { phone = value; OnPropertyChanged(nameof(Phone)); } }
        public ADepartament Departament { get; private set; }

        public string Info
        {
            get => $"First Name: {FirstName}\nLast Name: {LastName}\n" +
                   $"Phone: {Phone}\n" +
                   $"Departament: {Departament.Name}";
        }

        public Employee(ADepartament departament, string FirstName, string LastName, long Phone, int id)
        {
            Id = id;
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

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
