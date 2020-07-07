using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace HomeWork_13
{
    /// <summary>
    /// Логика взаимодействия для SetConnectionStringWindow.xaml
    /// </summary>
    public partial class SetConnectionStringWindow : Window
    {
        SqlConnectionStringBuilder connectionStringBuilder;
        public SetConnectionStringWindow(SqlConnectionStringBuilder connectionStringBuilder)
        {
            InitializeComponent();
            this.connectionStringBuilder = connectionStringBuilder;
        }

        //Connect
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(DataSource_tb.Text) || string.IsNullOrEmpty(InitCatalog_tb.Text))
            {
                MessageBox.Show("Какая-то из строк не заполнена.");
                return;
            }


            connectionStringBuilder.DataSource = @DataSource_tb.Text;
            connectionStringBuilder.InitialCatalog = InitCatalog_tb.Text;

            if (TestConnection() == false)
            {
                connectionStringBuilder.DataSource = "";
                connectionStringBuilder.InitialCatalog = "";
                return;
            }
            this.Close();
        }

        private bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                }

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);                
            }

            return false;
        }

        //Close
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
