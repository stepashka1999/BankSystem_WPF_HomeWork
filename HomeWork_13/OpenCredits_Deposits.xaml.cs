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
    /// Логика взаимодействия для OpenCredits_Deposits.xaml
    /// </summary>
    public partial class OpenCredits_Deposits : Window
    {
        bool isCredit;
        AClient Client;
        Label info;
        public OpenCredits_Deposits(object holder, bool credit, Label label)
        {
            InitializeComponent();
            Client = (holder as AClient);
            isCredit = credit;
            info = label;

            Configurate();
        }

        private void Configurate()
        {
            Month_slider.Minimum = 1;
            Month_slider.Value = 1;

            if (isCredit) Month_slider.Maximum = 60;
            else Month_slider.Maximum = 36;

            if (isCredit)
            {
                Sum_slider.Minimum = 100_000;
                Sum_slider.Maximum = 100_000_000;
                Sum_slider.Value = 100_000;
            }
            else
            {
                Sum_slider.Minimum = 1_000;
                Sum_slider.Maximum = (int)Client.Amount;
                Sum_slider.Value = 1_000;
            }

            if (isCredit) Percent_lbl.Content += ((int)Client.CreditHistory).ToString() + "%";
            else Percent_lbl.Content += "8%";

            int payment = (int)Sum_slider.Value / (int)Month_slider.Value;
            Payment_lbl.Content = $"Ежемесячный платеж: {payment}";
        }

        private void Open_btn_Click(object sender, RoutedEventArgs e)
        {
            int Amount = (int)Sum_slider.Value;
            int Month = (int)Month_slider.Value;
            
            if(isCredit)
            {
                Credit credit = new Credit(Client, Amount, Month);
                Client.OpenCredit(credit);
            }
            else
            {
                Deposit deposit = new Deposit(Client, Amount, Month);
                Client.OpenDeposit(deposit);
            }

            info.Content = Client.Info;

            this.Close();
        }

        private void Sum_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int payment = (int)Sum_slider.Value / (int)Month_slider.Value;
            Payment_lbl.Content = $"Ежемесячный платеж: {payment}"; 
        }

        private void Month_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int payment = (int)Sum_slider.Value / (int)Month_slider.Value;
            Payment_lbl.Content = $"Ежемесячный платеж: {payment}";
        }
    }
}
