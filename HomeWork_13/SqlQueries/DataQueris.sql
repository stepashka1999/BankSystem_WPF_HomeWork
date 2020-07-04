SELECT * FROM Clients

SELECT * FROM Credits
WHERE HolderId = 1

SELECT * FROM Deposits
WHERE HolderId = 1

SELECT * FROM Employees
WHERE DepartamentId = 1

-- Поулчение последнего элемента
SELECT TOP 1 *
FROM Clients
ORDER BY ID DESC

-- Изменение данных в таблице
UPDATE Clients
SET FName = N'Василий'
WHERE id = 1

-- Добавление данных
INSERT INTO Clients(FName, LName, IsVip, Account, Amount)
VALUES(N'Григорий', N'Лепс', 0, 1234567812345678, 100000000)

-- Удаление данных
DELETE FROM Clients
WHERE Id = 13

UPDATE Clients 
SET FName = N'Василий', LName = N'Пупкин', IsVip = 0, Amount = 100000 
WHERE Id = 1

SELECT TOP 1 *
FROM Credits
WHERE HolderId = 1
ORDER BY ID DESC