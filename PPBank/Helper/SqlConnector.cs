using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PPBank.SQL_DB
{
    class SqlConnector:IDisposable
    {
        string ConnectionString;// = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = MSSQLLocal_TestDb; Integrated Security = True; Pooling=False";
        SqlConnection Connection;
        public ConnectionState State => Connection.State;

        public SqlConnector(SqlConnectionStringBuilder connectionString)
        {
            ConnectionString = connectionString.ConnectionString;
            Open();
        }

        public SqlConnection GetConnection()
        {
            return Connection;
        }

        private void Open()
        {
            try
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Выполнить запрос
        /// </summary>
        /// <param name="sqlQuery">запрос</param>
        /// <returns>Читалка</returns>
        public SqlDataReader GetData(string sqlQuery)
        {
            var command = new SqlCommand(sqlQuery, Connection);

            return command.ExecuteReader();
        }

        /// <summary>
        /// Выполнить запрос
        /// </summary>
        /// <param name="sqlQuery">запрос</param>
        public void ExecuteCommand(string sqlQuery)
        {
            try
            {
                var command = new SqlCommand(sqlQuery, Connection);
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }           
        }

        /// <summary>
        /// Выполнить запросы
        /// </summary>
        /// <param name="sqlQuerys">Запросы</param>
        public void ExecuteCommands(string[] sqlQuerys)
        {
            foreach(var query in sqlQuerys)
            {
                ExecuteCommand(query);
            }
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
