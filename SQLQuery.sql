IF  NOT EXISTS (SELECT schema_name FROM information_schema.schemata WHERE schema_name = 'Mydatabase')
CREATE DATABASE [Mydatabase]
GO

USE Mydatabase
GO

------------------------------------------------------------------------------------------------
--1.Скрипт создания таблицы клиентов Clients, в которой будет хранится следующая информация: 
--наименование клиента, его адрес, дата добавления в таблицу. Адрес клиента может быть не известен.
CREATE TABLE Clients
(
	-- Ключевое слово IDENTITY задает начальное значение и устанавливает авто инкремент.
	-- По умолчанию значение первой ячейки равно 1 и с каждой новой записью увеличивается на 1.
	ClientId smallint IDENTITY NOT NULL PRIMARY KEY,
	Name Varchar(20) NOT NULL,
	Addres Varchar(20) NULL,
	Date_Added date NOT NULL
)
GO -- Конец пакета инструкций.


INSERT INTO Clients
(Name, Addres, Date_Added)
VALUES
('Vladimir', 'Moscow', '02/01/1970'),
('OAO BelarusBank', 'Minsk', '03/02/1970'),
('Sergei', NULL, '05/01/1970'),
('OAO MUSICART', 'Gomel', '03/06/1970'),
('Kirill', 'Gomel', '03/06/1970'),
('Misha', 'Minsk', '03/02/1970'),
('Vasya', 'Minsk', '03/02/1970');
GO


---------------------------------------------------------------------------------------------------
--2.Скрипт создания таблицы счетов клиентов Accounts, в которой будет хранится следующая информация: 
--номер счета, валюта счета, дата открытия, дата закрытия, текущий остаток. Текущий остаток и 
--дата закрытия известны не сразу.
CREATE TABLE Accounts
(
	Account_NO Varchar(40) NOT NULL PRIMARY KEY,
	ClientId smallint NOT NULL FOREIGN KEY REFERENCES Clients(ClientId),
	Account_Corrency Varchar(20) NOT NULL,
	Opening_Date date NOT NULL,
	Closing_Date date NULL,
	Current_Balance float NULL
)
GO


INSERT INTO Accounts
(Account_NO, ClientId, Account_Corrency, Opening_Date, Closing_Date, Current_Balance)
VALUES
('44662399486', '1', 'RUB', '04/01/1980', '01/01/1988', 100000),
('88847363570', '1', 'EUR', '04/01/1980', '01/01/1988', NULL),
('32344456753', '1', 'RUB', '04/01/1980', NULL,         270000),

('67584344348', '2', 'USD', '01/02/1980', '01/01/1988', 5000),
('43OAO344443', '2', 'USD', '02/02/1980', NULL,         3200),
('22987934857', '2', 'USD', '03/02/1980', NULL,         10000),
('27758839948', '2', 'USD', '04/02/1980', '01/01/1988', 1000),

('39988277874', '3', 'EUR', '01/03/1980', NULL, 32000),
('38895927675', '3', 'EUR', '02/03/1980', NULL, 20000),

('55455362283', '4', 'RUB', '01/04/1980', '01/01/1988', 1000000),
('88594363738', '4', 'USD', '02/04/1980', '01/01/1988', NULL),

('85030394749', '5', 'USD', '01/05/1980', '01/01/1988', 180000);
GO






SELECT * FROM Clients

SELECT * FROM Accounts 
ORDER BY ClientId


---------------------------------------------------------------------------------------------------
--3.Необходимо написать запрос, который возвращает список клиентов, у которых есть счета.
SELECT Clients.ClientId, Name, Addres, Date_Added FROM 
			  Clients	  -- Левая таблица 
			  INNER JOIN  -- Оператор объединения.
			  (SELECT Accounts.ClientId FROM Accounts
			   GROUP BY Accounts.ClientId) as temp	   -- Правая таблица
ON Clients.ClientId = temp.ClientId;		  -- Условие объединения при котором значения в сравниваемых ячейках должны совпадать. 





---------------------------------------------------------------------------------------------------
--4.Необходимо написать запрос, который возвращает список клиентов, у которых все счета закрыты.
SELECT Clients.ClientId, Name, Addres, Date_Added FROM 
			  Clients	 
			  INNER JOIN 
			  (SELECT Accounts.ClientId from Accounts
			   where Accounts.ClientId not in (select Accounts.ClientId from Accounts
				                			   where Accounts.Closing_Date is null 
								               group by Accounts.ClientId)
               group by Accounts.ClientId) as temp	          
ON Clients.ClientId = temp.ClientId;   






------------------------------------------------------------------------------------------------------------
--5.Необходимо написать запрос, который возвращает список счетов клиентов, у которых в наименовании есть ОАО.
SELECT Account_NO, Accounts.ClientId, Account_Corrency, Opening_Date, Closing_Date, Current_Balance FROM 
			  Accounts	 
			  INNER JOIN 
			  (SELECT Clients.ClientId from Clients
			   WHERE Name LIKE '%OAO%') as temp	          
ON Accounts.ClientId = temp.ClientId; 





---------------------------------------------------------------------------------------------------
--6.Необходимо написать запрос, который проставляет текущий остаток счета равным 0, если он был неизвестен.
UPDATE Accounts
SET Current_Balance = '0'
WHERE Current_Balance is NULL
GO


SELECT * FROM Accounts
ORDER BY ClientId


---------------------------------------------------------------------------------------------------
--7.Необходимо написать запрос, который возвращает список незакрытых счетов вместе с наименованием клиента, 
--кому принадлежит этот счет.
SELECT Account_NO, tb1.ClientId, Account_Corrency, Opening_Date, Closing_Date, Current_Balance, Clients.Name FROM
			 (SELECT * FROM Accounts
			  WHERE Accounts.Closing_Date is NULL) as tb1
			 LEFT JOIN
			 Clients
ON tb1.ClientId = Clients.ClientId
ORDER BY tb1.ClientId;











DROP TABLE Accounts
DROP TABLE Clients


----------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------
--8.Имеется две таблицы T1 и T2. Каждая имеет по одному столбцу F1.
--В таблице T1 хранится 10 строк. В таблице T2 хранится 5 строк.
-- __________	    	 _________
--| T1.F1    |			| T2.F1   |
--|__________|          |_________|
--|	1        |          | 1       |
--|	2        |          | 3       |
--|	3        |          | 6       |
--|	4        |          | 7       |
--|	5        |          | 8       |
--|	6        |          | 11      |
--|	7        |          | 15      |
--|	8        |          |_________|
--|	9        |
--| 10       |
--|__________|
--
--Необходимо написать запрос, который вернет только те значения столбца F1, которые есть в обеих таблицах.

CREATE TABLE T1
(
	F1 Varchar(20) NOT NULL,
)
GO

INSERT INTO T1
VALUES
(1),
(2),
(3),
(4),
(5),
(6),
(7),
(8),
(9),
(10);
GO

CREATE TABLE T2
(
	F1 Varchar(20) NOT NULL,
)

INSERT INTO T2
VALUES
(1),
(3),
(6),
(7),
(8),
(11),
(15);
GO

SELECT * FROM T1
SELECT * FROM T2


-- Производим выборку всех данных из объеденения таблиц T1 и T2 
-- по связующим полям T1.F1 и T2.F1
SELECT * FROM 
			  T1	  
			  INNER JOIN
			  T2	  
ON T1.F1 = T2.F1;		
GO






DROP TABLE T1
DROP TABLE T2