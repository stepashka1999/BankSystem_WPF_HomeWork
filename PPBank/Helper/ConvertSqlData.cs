using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PPBank
{
    public static class ConvertSqlData
    {
        /// <summary>
        /// Конвертирует данные Сотрудника
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="departament"></param>
        /// <returns></returns>
        public static Employee ToEmployee(SqlDataReader reader, ADepartament departament)
        {
            try
            {
                var id = (int)reader[0];
                var fName = (string)reader[1];
                var lName = (string)reader[2];
                var phone = (long)reader[3];

                return new Employee(departament, fName, lName, phone, id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Конвертирует данные в кредит
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="holder"></param>
        /// <returns></returns>
        public static Credit ToCredit(SqlDataReader reader, AClient holder)
        {
            try
            {
                int id = (int)reader[0];
                var Amount = (decimal)reader[2];
                var Month = (int)reader[3];

                return new Credit(holder, Amount, id, Month);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Конвертирует данные в депозит
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="holder"></param>
        /// <returns></returns>
        public static Deposit ToDeposit(SqlDataReader reader, AClient holder)
        {
            try
            {
                int id = (int)reader[0];
                var Amount = (decimal)reader[2];
                var Month = (int)reader[3];

                return new Deposit(holder, Amount, id, Month);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Конвертирует данные в организацию
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        public static Entity ToEntity(SqlDataReader reader, Bank bank)
        {
            try
            {

                var id = (int)reader[0];
                var name = (string)reader[1];
                var Aaccount = (long)reader[2];
                var Amount = (decimal)reader[3];

                return new Entity(bank, name, Aaccount, Amount, id);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Конвертирует данные в клиента
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        public static Client ToClient(SqlDataReader reader, Bank bank)
        {
            try
            {
                var id = (int)reader[0];
                var fName = (string)reader[1];
                var lName = (string)reader[2];
                var isVip = (bool)reader[3];
                var Account = (long)reader[4];
                var Ammount = (decimal)reader[5];
                
                return new Client(bank, fName, lName, isVip, Account, Ammount, id);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }
    }
}
