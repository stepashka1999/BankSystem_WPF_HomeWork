using PPBank.SQL_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBank
{
    public static class BankDB
    {
        public static Client GetLastClient(SqlConnectionStringBuilder stringBuilder, Bank bank)
        {
            var sqlQuery = @"SELECT TOP 1 *
                             FROM Clients
                             ORDER BY ID DESC";
            Client lastClient = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while(reader.Read())
                {
                    lastClient = ConvertSqlData.ToClient(reader, bank);
                }
            }

            return lastClient;
        }
        public static Client GetClientById(SqlConnectionStringBuilder stringBuilder, int id, Bank bank)
        {
            var sqlQuery = $@"SELECT * FROM Clients
                             WHERE Id = {id}";

            Client currentClient = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while(reader.Read())
                {
                    currentClient = ConvertSqlData.ToClient(reader, bank);
                }
            }

            return currentClient;
        }

        public static Entity GetLastEntity(SqlConnectionStringBuilder stringBuilder, Bank bank)
        {
            var sqlQuery = @"SELECT TOP 1 *
                             FROM Entitys
                             ORDER BY ID DESC";

            Entity lastEntity = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    lastEntity = ConvertSqlData.ToEntity(reader, bank);
                }
            }

            return lastEntity;
        }
        public static Entity GetEntityById(SqlConnectionStringBuilder stringBuilder, int id, Bank bank)
        {
            var sqlQuery = $@"SELECT * FROM Entitys
                              WHERE Id = {id}";

            Entity currentEntity = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    currentEntity = ConvertSqlData.ToEntity(reader, bank);
                }
            }

            return currentEntity;
        }

        public static Employee GetLastEmployee(SqlConnectionStringBuilder stringBuilder, ADepartament departament)
        {
            var sqlQuery = @"SELECT TOP 1 *
                             FROM Employees
                             ORDER BY ID DESC";

            Employee lastEmployee = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    lastEmployee = ConvertSqlData.ToEmployee(reader, departament);
                }
            }

            return lastEmployee;
        }
        public static Employee GetEmployeeById(SqlConnectionStringBuilder stringBuilder, int id, ADepartament departament)
        {
            var sqlQuery = $@"SELECT * FROM Employees
                             WHERE Id = {id}";

            Employee lastEmployee = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    lastEmployee = ConvertSqlData.ToEmployee(reader, departament);
                }
            }

            return lastEmployee;
        }

        public static Credit GetLastCreditByHolder(SqlConnectionStringBuilder stringBuilder, AClient holder)
        {
            int isEntity = 0;

            if (holder is Entity) isEntity = 1;
            else if (holder is Client) isEntity = 0;

            var sqlQuery = $@"SELECT TOP 1 *
                              FROM Credits
                              WHERE HolderId = {holder.Id} and isEntity = {isEntity}
                              ORDER BY ID DESC";
            Credit lastCredit = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    lastCredit = ConvertSqlData.ToCredit(reader, holder);
                }
            }

            return lastCredit;
        }

        public static Deposit GetLastDerpositByHolder(SqlConnectionStringBuilder stringBuilder, AClient holder)
        {
            int isEntity = 0;

            if (holder is Entity) isEntity = 1;
            else if (holder is Client) isEntity = 0;

            var sqlQuery = $@"SELECT TOP 1 *
                              FROM Deposits
                              WHERE HolderId = {holder.Id} and isEntity = {isEntity}
                              ORDER BY ID DESC";
            Deposit lastDeposit = null;

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    lastDeposit = ConvertSqlData.ToDeposit(reader, holder);
                }
            }

            return lastDeposit;
        }

        public static IEnumerable<Credit> GetCreditsByHolder(SqlConnectionStringBuilder stringBuilder, AClient holder)
        {
            var sqlQuery = $@"SELECT * FROM Credits
                             WHERE HolderId = {holder.Id}";

            List<Credit> credits = new List<Credit>();

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while(reader.Read())
                {
                    var credit = ConvertSqlData.ToCredit(reader, holder);

                    credits.Add(credit);
                }
            }

            return credits;
        }
        public static IEnumerable<Deposit> GetDepositsByHolder(SqlConnectionStringBuilder stringBuilder, AClient holder)
        {
            var sqlQuery = $@"SELECT * FROM Deposits
                             WHERE HolderId = {holder.Id}";

            List<Deposit> deposits = new List<Deposit>();

            using (var connector = new SqlConnector(stringBuilder))
            {
                var reader = connector.GetData(sqlQuery);
                while (reader.Read())
                {
                    var deposit = ConvertSqlData.ToDeposit(reader, holder);

                    deposits.Add(deposit);
                }
            }

            return deposits;
        }

        public static void DeleteClient(SqlConnectionStringBuilder stringBuilder, Client client)
        {
            DeleteClient(stringBuilder, client.Id);
        }
        public static void DeleteClient(SqlConnectionStringBuilder stringBuilder, int clientId)
        {
            var sqlQuery = new string[]
            {
                $@"DELETE FROM Clients WHERE Id = {clientId}",
                $@"DELETE FROM Credits WHERE HolderId = {clientId} and isEntity = 0",
                $@"DELETE FROM Deposits WHERE HolderId = {clientId} and isEntity = 0"
            };

            using (var connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommands(sqlQuery);
            }
        }

        public static void DeleteEntity(SqlConnectionStringBuilder stringBuilder, Entity entity)
        {
            DeleteEntity(stringBuilder, entity.Id);
        }
        public static void DeleteEntity(SqlConnectionStringBuilder stringBuilder, int entityId)
        {
            var sqlQuery = new string[]
            {
                $@"DELETE FROM Entitys WHERE Id = {entityId}",
                $@"DELETE FROM Credits WHERE HolderId = {entityId} and isEntity = 1",
                $@"DELETE FROM Deposits WHERE HolderId = {entityId} and isEntity = 1"
            };

            using (var connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommands(sqlQuery);
            }
        }

        public static void DeleteCredit(SqlConnectionStringBuilder stringBuilder, Credit credit)
        {
            DeleteCredit(stringBuilder, credit.Id);
        }
        public static void DeleteCredit(SqlConnectionStringBuilder stringBuilder, int creditId)
        {
            var sqlQuery = $@"DELETE FROM Credits
                              WHERE Id = {creditId}";

            using (var connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }

        public static void DeleteDeposit(SqlConnectionStringBuilder stringBuilder, Deposit dep)
        {
            DeleteDeposit(stringBuilder, dep.Id);
        }
        public static void DeleteDeposit(SqlConnectionStringBuilder stringBuilder, int depositId)
        {
            var sqlQuery = $@"DELETE FROM Deposits
                              WHERE Id = {depositId}";

            using (var connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }

        public static void DeleteEmployee(SqlConnectionStringBuilder stringBuilder, Employee empl)
        {
            DeleteEmployee(stringBuilder, empl.Id);
        }
        public static void DeleteEmployee(SqlConnectionStringBuilder stringBuilder, int employeeId)
        {
            var sqlQuery = $@"DELETE FROM Employees
                              WHERE Id = {employeeId}";

            using (var connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }

        public static void AddClient(SqlConnectionStringBuilder stringBuilder, string fName, string lName, bool isVip, long account, decimal amount)
        {
            var sqlQuery = $@"INSERT INTO Clients(FName, LName, IsVip, Account, Amount)
                             VALUES(N'{fName}', N'{lName}', {Convert.ToInt32(isVip)}, {account}, {amount})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }
        public static void AddClient(SqlConnectionStringBuilder stringBuilder, Client client)
        {
            var sqlQuery = $@"INSERT INTO Clients(FName, LName, IsVip, Account, Amount)
                             VALUES(N'{client.FirstName}', N'{client.LastName}', {Convert.ToInt32(client.isVIP)}, {client.Account}, {client.Amount})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }

        public static void AddEntity(SqlConnectionStringBuilder stringBuilder, Entity entity)
        {
            var sqlQuery = $@"INSERT INTO Entitys(EntityName, Account, Amount)
                             VALUES(N'{entity.Name}', {entity.Account}, {entity.Amount})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }
        public static void AddEntity(SqlConnectionStringBuilder stringBuilder, string name, long account, decimal amount)
        {
            var sqlQuery = $@"INSERT INTO Entitys(EntityName, Account, Amount)
                             VALUES(N'{name}', {account}, {amount})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }

        public static void AddEmployee(SqlConnectionStringBuilder stringBuilder, string fName, string lName, long phone, int depId)
        {
            var sqlQuery = $@"INSERT INTO Employees(FName, LName, Phone, DepartamentId)
                             VALUES(N'{fName}', N'{lName}', {phone}, {depId})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }

        }
        public static void AddEmployee(SqlConnectionStringBuilder stringBuilder, Employee employee)
        {
            int depId = 1;

            if (employee.Departament is Departament) depId = 1;
            else if (employee.Departament is VIPDepartament) depId = 2;
            else if (employee.Departament is LegalDepartament) depId = 3;

            AddEmployee(stringBuilder, employee, depId);
        }
        public static void AddEmployee(SqlConnectionStringBuilder stringBuilder, Employee employee, int depId)
        {
            var sqlQuery = $@"INSERT INTO Employees(FName, LName, Phone, DepartamentId)
                             VALUES(N'{employee.FirstName}', N'{employee.LastName}', {employee.Phone}, {depId})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }

        public static void AddCredit(SqlConnectionStringBuilder stringBuilder, AClient holder, decimal amount, int month)
        {
            int isEntity = 0;
            if (holder is Entity) isEntity = 1;
            else if (holder is Client) isEntity = 0;

            var sqlQuery = $@"INCERT INTO Credits(HolderId, Amount, CreditMonth, isEntity)
                              VALUES ({holder.Id}, @amount, {month}, {isEntity})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                var con = connector.GetConnection();
                var command = new SqlCommand(sqlQuery, con);
                command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Money)).Value = amount;
                command.ExecuteNonQuery();
            }
        }
        public static void AddDeposit(SqlConnectionStringBuilder stringBuilder, AClient holder, decimal amount, int month)
        {
            int isEntity = 0;
            if (holder is Entity) isEntity = 1;
            else if (holder is Client) isEntity = 0;

            var sqlQuery = $@"INCERT INTO Deposits(HolderId, Amount, CreditMonth, isEntity)
                              VALUES ({holder.Id}, @amount, {month}, {isEntity})";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                var con = connector.GetConnection();
                var command = new SqlCommand(sqlQuery, con);
                command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Money)).Value = amount;
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateClient(SqlConnectionStringBuilder stringBuilder, Client client)
        {
            var sqlQuery = $@"
                      UPDATE Clients
                      SET FName = N'{client.FirstName}', LName = N'{client.LastName}', IsVip = {Convert.ToInt32(client.isVIP)}, Amount = @amount
                      WHERE Id = {client.Id}";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                try
                {
                    var con = connector.GetConnection();
                    var command = new SqlCommand(sqlQuery, con);
                    command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Money)).Value = client.Amount;
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    throw e;
                }               
            }
        }
        public static void UpdateCredit(SqlConnectionStringBuilder stringBuilder, Credit credit)
        {
            var sqlQuery = $@"UPDATE Credits
                              SET Amount = @amount
                              WHERE Id = {credit.Id}";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                try
                {
                    var con = connector.GetConnection();
                    var command = new SqlCommand(sqlQuery, con);
                    command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Money)).Value = credit.Amount;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public static void UpdateDeposit(SqlConnectionStringBuilder stringBuilder, Deposit deposit)
        {
            var sqlQuery = $@"UPDATE Deposits
                              SET Amount = @amount
                              WHERE Id = {deposit.Id}";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                try
                {
                    var con = connector.GetConnection();
                    var command = new SqlCommand(sqlQuery, con);
                    command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Money)).Value = deposit.Amount;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public static void UpdateEntity(SqlConnectionStringBuilder stringBuilder, Entity entity)
        {
            var sqlQuery = $@"UPDATE Entitys
                              SET EntityName = N'{entity.Name}', Amount = @amount
                              WHERE Id = {entity.Id}";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                try
                {
                    var command = new SqlCommand(sqlQuery);
                    command.Parameters.Add(new SqlParameter("@amount", SqlDbType.Money)).Value = entity.Amount;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public static void UpdateEmployee(SqlConnectionStringBuilder stringBuilder, Employee employee)
        {
            int depId = 0;

            if (employee.Departament is Departament) depId = 1;
            else if (employee.Departament is VIPDepartament) depId = 2;
            else if (employee.Departament is LegalDepartament) depId = 3;

            var sqlQuery = $@"UPDATE Employees
                              SET FName = N'{employee.FirstName}', LName = N'{employee.LastName}', Phone = {employee.Phone}, DepartamentId = {depId}
                              WHERE Id = {employee.Id}";

            using (SqlConnector connector = new SqlConnector(stringBuilder))
            {
                connector.ExecuteCommand(sqlQuery);
            }
        }
        
        public static void UpdateAllClients(SqlConnectionStringBuilder stringBuilder, IEnumerable<AClient> clients)
        {
            foreach(var client in clients)
            {
                UpdateClient(stringBuilder, (Client)client);
            }
        }
        public static void UpdateAllCredits(SqlConnectionStringBuilder stringBuilder, IEnumerable<Credit> credits)
        {
            foreach (var credit in credits)
            {
                UpdateCredit(stringBuilder, credit);
            }
        }
        public static void UpdateAllDeposits(SqlConnectionStringBuilder stringBuilder, IEnumerable<Deposit> deposits)
        {
            foreach (var deposit in deposits)
            {
                UpdateDeposit(stringBuilder, deposit);
            }
        }
        public static void UpdateAllEntitys(SqlConnectionStringBuilder stringBuilder, IEnumerable<AClient> entitys)
        {
            foreach (var entity in entitys)
            {
                UpdateEntity(stringBuilder, (Entity)entity);
            }
        }
        public static void UpdateAllEmployees(SqlConnectionStringBuilder stringBuilder, IEnumerable<Employee> employees)
        {
            foreach (var employee in employees)
            {
                UpdateEmployee(stringBuilder, employee);
            }
        }


        public static void UpdateAll(SqlConnectionStringBuilder stringBuilder, Bank bank)
        {
            var clients = bank.Clients.Where(x => x is Client);
            var credits = bank.Credits;
            var deposits = bank.Deposits;
            var entitys = bank.Clients.Where(x => x is Entity);
            var employees = bank.Employees;

            UpdateAllClients(stringBuilder, clients);
            UpdateAllCredits(stringBuilder, credits);
            UpdateAllDeposits(stringBuilder, deposits);
            UpdateAllEntitys(stringBuilder, entitys);
            UpdateAllEmployees(stringBuilder, employees);
        }
    }
}
