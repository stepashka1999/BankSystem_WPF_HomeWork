-- Creating Clients table
CREATE TABLE [dbo].[Clients]
(
	Id INT NOT NULL IDENTITY,
	FName NVARCHAR(50) NOT NULL,
	LName NVARCHAR(50) NOT NULL,
	IsVip BIT NOT NULL, -- I hope that "bool"
	Account INT NOT NULL,
	Amount MONEY NOT NULL,
	CreditGistory_Id int NOT NULL
)

-- Creating Entitys table
CREATE TABLE [dbo].[Entitys]
(
	Id INT NOT NULL IDENTITY,
	EntityName NVARCHAR(50) NOT NULL,
	Account INT NOT NULL,
	Amount MONEY NOT NULL
)

-- Creating Credits table
CREATE TABLE dbo.Credits
(
	Id INT NOT NULL IDENTITY,
	HolderId INT NOT NULL,
	Amount MONEY NOT NULL,
	CreditPercent INT NOT NULL,
	CreditMonth INT NOT NULL
)

-- Creating Deposit table
CREATE TABLE dbo.Deposits
(
	Id INT NOT NULL IDENTITY,
	HolderId INT NOT NULL,
	DepositPercent INT NOT NULL,
	DepositMonth INT NOT NULL
)

-- Creating CreditHistory table
CREATE TABLE dbo.CreditHistory
(
	ClientId INT NOT NULL,
	HistoryStatus INT NOT NULL
)

-- Creating Employees table
CREATE TABLE dbo.Employees
(
	Id INT NOT NULL IDENTITY,
	FName NVARCHAR(50) NOT NULL,
	LName NVARCHAR(50) NOT NULL,
	Phone INT NOT NULL,
	DepartamentId INT NOT NULL
)