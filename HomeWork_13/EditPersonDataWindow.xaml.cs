using System;
using System.Windows;
using PPBank;

namespace HomeWork_13
{
    enum UIType
    {
        EditClient,
        EditEntity,
        EditEmployee
    }


    /// <summary>
    /// Логика взаимодействия для EditPersonDataWindow.xaml
    /// </summary>
    public partial class EditPersonDataWindow : Window
    {
        UIType currentType;
        object person;
        public event Action<object> EditEnded;

        public EditPersonDataWindow(object person)
        {
            InitializeComponent();
            this.person = person;
            
            SetEditMode(person);
            ShowUI(currentType);
        }


        private void SetEditMode(object obj)
        {
            if(obj is AClient)
            {
                if(obj is Client)
                {
                    currentType = UIType.EditClient;
                }
                else if(obj is Entity)
                {
                    currentType = UIType.EditEntity;
                }
            }
            else if(obj is Employee)
            {
                currentType = UIType.EditEmployee;
            }

        }

        private void ShowUI(UIType uiType)
        {
            switch(uiType)
            {
                case UIType.EditClient:
                    ShowClientUI(Visibility.Visible);

                    ShowEntityUI(Visibility.Hidden);
                    ShowEmployeeUI(Visibility.Hidden);
                    break;
                case UIType.EditEntity:
                    ShowEntityUI(Visibility.Visible);

                    ShowClientUI(Visibility.Hidden);
                    ShowEmployeeUI(Visibility.Hidden);
                    break;
                case UIType.EditEmployee:
                    ShowEmployeeUI(Visibility.Visible);

                    ShowClientUI(Visibility.Hidden);
                    ShowEntityUI(Visibility.Hidden);
                    break;
            }
        }

        private void ShowClientUI(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
            {
                var client = person as Client;

                CFName_tb.Text = client.FirstName;
                CLName_tb.Text = client.LastName;
            }

            EditClient_gb.Visibility = visibility;
        }

        private void ShowEntityUI(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
            {
                var entity = person as Entity;
                Name_tb.Text = entity.Name;
            }

            EditEntity_gb.Visibility = visibility;
        }

        private void ShowEmployeeUI(Visibility visibility)
        {
            if(visibility == Visibility.Visible)
            {
                var empl = person as Employee;
                EFName_tb.Text = empl.FirstName;
                ELName_tb.Text = empl.LastName;
                Phone_mtb.Text = empl.Phone.ToString();
            }
                            
            EditEmployee_gb.Visibility = visibility;
        }

        private void EditClient(object client)
        {
            var fName = CFName_tb.Text;
            var lName = CLName_tb.Text;

            if(string.IsNullOrEmpty(fName) || string.IsNullOrEmpty(lName))
            {
                MessageBox.Show("Поля заполнены некорретно.");
                return;
            }

            (client as Client).Edit(fName, lName);
            EditEnded?.Invoke(client);
            this.Close();
        }

        private void EditEntity(object entity)
        {
            var Name = Name_tb.Text;

            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Поле заполнено некорретно.");
                return;
            }

            (entity as Entity).Edit(Name);
            EditEnded?.Invoke(entity);
            this.Close();
        }

        private void EditEmployee(object empl)
        {
            var fName = EFName_tb.Text;
            var lName = ELName_tb.Text;
            var phoneText = Phone_mtb.Text;
            var phone = GetInt(phoneText);

            if (string.IsNullOrEmpty(fName) || string.IsNullOrEmpty(lName) || phone.ToString().Length < 11)
            {
                MessageBox.Show("Поля заполнены некорретно.");
                return;
            }

            (empl as Employee).Edit(fName, lName, phone);
            EditEnded?.Invoke(empl);
            this.Close();
        }
        private long GetInt(string str)
        {
            string res = "";
            foreach (var simbol in str)
            {
                if (char.IsDigit(simbol)) res += simbol;
            }

            return Convert.ToInt64(res);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch(currentType)
            {
                case UIType.EditClient:
                    EditClient(person);
                    break;
                case UIType.EditEntity:
                    EditEntity(person);
                    break;
                case UIType.EditEmployee:
                    EditEmployee(person);
                    break;
            }
        }
    }
}
