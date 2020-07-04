-- Creating Clients table
CREATE TABLE [dbo].[Clients]
(
	Id INT NOT NULL IDENTITY,
	FName NVARCHAR(50) NOT NULL,
	LName NVARCHAR(50) NOT NULL,
	IsVip BIT NOT NULL, -- I hope that "bool"
	Account BIGINT NOT NULL,
	Amount MONEY NOT NULL
)

DROP TABLE Clients

-- Creating Entitys table
CREATE TABLE [dbo].[Entitys]
(
	Id INT NOT NULL IDENTITY,
	EntityName NVARCHAR(50) NOT NULL,
	Account BIGINT NOT NULL,
	Amount MONEY NOT NULL
)

DROP TABLE Entitys

-- Creating Credits table
CREATE TABLE dbo.Credits
(
	Id INT NOT NULL IDENTITY,
	HolderId INT NOT NULL,
	Amount MONEY NOT NULL,
	CreditMonth INT NOT NULL
)

ALTER TABLE dbo.Credits ADD isEntity BIT NULL

DROP TABLE Credits

-- Creating Deposit table
CREATE TABLE dbo.Deposits
(
	Id INT NOT NULL IDENTITY,
	HolderId INT NOT NULL,
	Amount MONEY NOT NULL,
	DepositMonth INT NOT NULL
)

ALTER TABLE dbo.Deposits ADD isEntity BIT NULL

DROP TABLE Deposits

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
	Phone BIGINT NOT NULL,
	DepartamentId INT NOT NULL
)

DROP TABLE Employees

DROP TABLE CreditHistory