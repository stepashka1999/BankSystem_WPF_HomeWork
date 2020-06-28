using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PPBank;

namespace HomeWork_13
{
    /// <summary>
    /// Логика взаимодействия для TransactToWindow.xaml
    /// </summary>
    public partial class TransactToWindow : Window
    {
        bool To;
        AClient Client;
        Bank Bank;
        Label info;
        public TransactToWindow(object bank, object client, bool To, Label lbl)
        {
            InitializeComponent();
            Client = (client as AClient);
            Bank = (bank as Bank);
            this.To = To;
            info = lbl;

            var clients = Bank.Clients.Where(x => x != Client).ToList();
            clients_cb.ItemsSource = clients;
            if (To)
            {
                helper_lbl.Content = "Выберите клиента, которому хотите перевести деньги:";
                operation_btn.Content = "Перевести";
            }
            else
            {
                helper_lbl.Content = "Выберите клиента, со счета которого хотите перевести деньги себе:";
                operation_btn.Content = "Запросить";
            }
        }

        private void operation_btn_Click(object sender, RoutedEventArgs e)
        {
            AClient secondClient = (clients_cb.SelectedItem as AClient);

            int sum;

            if (clients_cb.SelectedItem == null)
            {
                MessageBox.Show("Клиент не выбран.");
                return;
            }

            if (String.IsNullOrEmpty(Sum_tb.Text))
            {
                MessageBox.Show("Сумма не введена или введена неверно.");
                return;
            }

            if (int.TryParse(Sum_tb.Text, out sum))
            {
                if(sum <= 0) MessageBox.Show("Сумма не введена или введена неверно.");
                else
                {
                    if (To)
                    {
                        Client.SendMoneyTo(secondClient, sum);
                    }
                    else
                    {
                        Client.RequestMoneyFrom(secondClient, sum);
                    }
                }
            }
            else
            {
                MessageBox.Show("Сумма не введена или введена неверно.");
                return;
            }

            info.Content = Client.Info;

            this.Close();
        }
    }
}
