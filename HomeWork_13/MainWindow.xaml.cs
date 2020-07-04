﻿using System;
using System.Windows;
using System.Windows.Controls;
using PPBank;

namespace HomeWork_13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bank bank;
        public MainWindow()
        {
            InitializeComponent();
            bank = new Bank("Pis' Pis' Bank", Dispatcher);
            DataContext = bank;
            bank.FillTreeViewClients(tv_Clients);
            bank.FillTreeViewEmployees(tv_Employees);
            bank.SetTransactionList(Transactions_lb);

            ClientButtons(Visibility.Hidden);
            CloseCreditDeposit(Visibility.Hidden);

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = (sender as TreeView).SelectedItem;
            if (item is AClient)
            {
                if (item is Client) ClientButtons(Visibility.Visible);
                else OrganisationButtons(Visibility.Visible);

                Info_lbl.Content = (item as AClient).Info;
                Credits.ItemsSource = (item as AClient).Credits;
                Deposits.ItemsSource = (item as AClient).Deposits;
            }
            else if (item is Employee)
            {
                ClientButtons(Visibility.Hidden);
                CloseCreditDeposit(Visibility.Hidden);
                OrganisationButtons(Visibility.Hidden);

                Credits.ItemsSource = null;
                Deposits.ItemsSource = null;
                Info_lbl.Content = (item as Employee).Info;
            }
            else
            {
                ClientButtons(Visibility.Hidden);
                CloseCreditDeposit(Visibility.Hidden);
                OrganisationButtons(Visibility.Hidden);
            }
        }

        private void Make_transact_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;
            var client = (item as AClient);

            var window = new TransactToWindow(bank, client, true, Info_lbl);
            window.Show();

            Info_lbl.Content = client.Info;
        }

        private void ClientButtons(Visibility visibility)
        {
            Make_transact_btn.Visibility = visibility;
            request_transact_btn.Visibility = visibility;
            Vip_btn.Visibility = visibility;
            Open_credit_btn.Visibility = visibility;
            Open_deposit_btn.Visibility = visibility;
        }

        private void OrganisationButtons(Visibility visibility)
        {
            Vip_btn.Visibility = Visibility.Hidden;
            Make_transact_btn.Visibility = visibility;
            request_transact_btn.Visibility = visibility;
            Open_credit_btn.Visibility = visibility;
            Open_deposit_btn.Visibility = visibility;
        }

        private void CloseCreditDeposit(Visibility visibility)
        {
            Close_Credit_btn.Visibility = visibility;
            Close_Deposit_btn.Visibility = visibility;
        }

        private void request_transact_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;
            var client = (item as AClient);

            var window = new TransactToWindow(bank, client, false, Info_lbl);
            window.Show();

            Info_lbl.Content = client.Info;
        }

        private void Vip_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;
            var client = (item as Client);

            client.MakeVIP();
            Info_lbl.Content = client.Info;
            tv_Clients.Items.Clear();
            bank.FillTreeViewClients(tv_Clients);
        }

        private void Open_credit_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;
            var client = (item as AClient);

            var window = new OpenCredits_Deposits(client, true, Info_lbl);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Credits.Items.Refresh();
            Deposits.Items.Refresh();

            (sender as OpenCredits_Deposits).Closed -= Window_Closed;
        }

        private void Open_deposit_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;
            var client = (item as Client);

            var window = new OpenCredits_Deposits(client, false, Info_lbl);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Close_Credit_btn_Click(object sender, RoutedEventArgs e)
        {
            var clietn = tv_Clients.SelectedItem as AClient;

            var credit = (Credits.SelectedItem as Credit);
            if(credit != null && clietn!= null)
            {
                clietn.CloseCredit(credit);
                Credits.Items.Refresh();
            }

            Info_lbl.Content = clietn.Info;
        }

        private void Close_Deposit_btn_Click(object sender, RoutedEventArgs e)
        {
            var clietn = tv_Clients.SelectedItem as AClient;

            var deposit = (Deposits.SelectedItem as Deposit);
            if (deposit != null && clietn != null)
            {
                clietn.CloseDeposit(deposit);
                Deposits.Items.Refresh();
            }

            Info_lbl.Content = clietn.Info;
        }

        private void Deposits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CloseCreditDeposit(Visibility.Visible);
        }

        private void makePayment_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;

            bank.MakePaymentTask();

            if(item != null)
            {
                var client = (item as AClient);
                Info_lbl.Content = client.Info;
            }
            
       
            Credits.Items.Refresh();
            Deposits.Items.Refresh();
        }

        private void AddClient_btn_Click(object sender, RoutedEventArgs e)
        {
            var addWind = new AddClient_Window(bank);
            
            addWind.ClientCreated += ClientCreated;
            addWind.EmployeeCreated += EmployeeCreated;
            addWind.EntityCreated += EntityCreated;

            addWind.Show();
        }

        private void EntityCreated(Entity entity)
        {
            bank.AddClient(entity);
            var entitys = bank.GetClients(x => x is Entity);
            (tv_Clients.Items[2] as TreeViewItem).ItemsSource = entitys;

            var strokePtrn = $"{entity.Name}\n теперь сотрудничает с нами!";
            Transactions_lb.Items.Add(strokePtrn);
        }

        private void ClientCreated(Client client)
        {
            bank.AddClient(client);
            
            if(client.isVIP)
            {
                var clients = bank.GetClients(x => (x is Client) && (x as Client).isVIP == true);
                (tv_Clients.Items[1] as TreeViewItem).ItemsSource = clients;
            }
            else
            {
                var clients = bank.GetClients(x => (x is Client) && (x as Client).isVIP == false);
                (tv_Clients.Items[0] as TreeViewItem).ItemsSource = clients;
            }

            var strokePtrn = $"{client.FirstName} {client.LastName}\n теперь с нами!";
            Transactions_lb.Items.Add(strokePtrn);
        }

        private void EmployeeCreated(Employee empl)
        {
            bank.AddEmployee(empl, tv_Employees);

            var strokePtrn = $"{empl.FirstName} {empl.LastName}\n теперь наш сотрудник!";
            Transactions_lb.Items.Add(strokePtrn);
        }

        private void DeleteClient_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;

            if (item is AClient)
            {
                DeleteAClient(item as AClient);
            }
            else if(item is Employee)
            {
                DeleteEmployee(item as Employee);
            }
        }

        private void DeleteAClient(AClient item)
        {
            bank.DeleteClient(item as AClient);

            if (item is Client)
            {
                if ((item as Client).isVIP)
                {
                    var clients = bank.GetClients(x => (x is Client) && (x as Client).isVIP == true);
                    (tv_Clients.Items[1] as TreeViewItem).ItemsSource = clients;
                }
                else
                {
                    var clients = bank.GetClients(x => (x is Client) && (x as Client).isVIP == false);
                    (tv_Clients.Items[0] as TreeViewItem).ItemsSource = clients;
                }
            }
            else if (item is Entity)
            {
                var entitys = bank.GetClients(x => x is Entity);
                (tv_Clients.Items[2] as TreeViewItem).ItemsSource = entitys;
            }
        }

        private void DeleteEmployee(Employee empl)
        {
            bank.DeleteEmployee(empl, tv_Employees);
        }

        private void Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = tv_Clients.SelectedItem;
            
            if (item == null)
            {
                item = tv_Employees.SelectedItem;
            }
            
            if (item is AClient || item is Employee)
            {
                var editWind = new EditPersonDataWindow(item);
                editWind.EditEnded += EditWind_EditEnded;
                editWind.Show();
            }
        }

        private void EditWind_EditEnded(object item)
        {
            if (item is Client)
            {
                if ((item as Client).isVIP)
                {
                    var clients = bank.GetClients(x => (x is Client) && (x as Client).isVIP == true);
                    (tv_Clients.Items[1] as TreeViewItem).ItemsSource = clients;
                }
                else
                {
                    var clients = bank.GetClients(x => (x is Client) && (x as Client).isVIP == false);
                    (tv_Clients.Items[0] as TreeViewItem).ItemsSource = clients;
                }
            }
            else if (item is Entity)
            {
                var entitys = bank.GetClients(x => x is Entity);
                (tv_Clients.Items[2] as TreeViewItem).ItemsSource = entitys;
            }

        }
    }
}
