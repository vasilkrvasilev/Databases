USE Bank
GO

--01. Create Persons and Accounts and 
--stored procedure that selects the full names of all persons
CREATE TABLE Persons (
  PersonID int IDENTITY,
  FirstName nvarchar(20) NOT NULL,
  LastName nvarchar(20) NOT NULL,
  SSN varchar(10) NOT NULL,
  CONSTRAINT PK_Persons PRIMARY KEY(PersonID)
)
GO

CREATE TABLE Accounts (
  AccountID int IDENTITY,
  PersonID int,
  Balance money,
  CONSTRAINT PK_Accounts PRIMARY KEY(AccountID),
  CONSTRAINT FK_Accounts_Persons FOREIGN KEY(PersonID)
  REFERENCES Persons(PersonID)
)
GO

INSERT INTO Persons(FirstName, LastName, SSN)
VALUES ('Ivan', 'Ivanov', '1111111111')
INSERT INTO Persons(FirstName, LastName, SSN)
VALUES ('Sam', 'Jones', '2222222222')

INSERT INTO Accounts(PersonID, Balance)
VALUES (1, 1000)
INSERT INTO Accounts(PersonID, Balance)
VALUES (1, 5000)
INSERT INTO Accounts(PersonID, Balance)
VALUES (2, 3000)
GO

CREATE PROC dbo.usp_SelectFullName
AS
  SELECT CONCAT(FirstName, ' ', LastName) 
  FROM Persons
GO

EXEC dbo.usp_SelectFullName
GO

--02. Create stored procedure that selects all persons with more money than given number
CREATE PROC usp_SelectAccountsBiggerThen(@minBalance money = 1)
AS
  SELECT p.FirstName, p.LastName, p.SSN
  FROM Accounts a
    JOIN Persons p ON a.PersonID = p.PersonID
  GROUP BY p.FirstName, p.LastName, p.SSN
  HAVING Balance > @minBalance
  ORDER BY Balance
GO

EXEC usp_SelectAccountsBiggerThen 4000

EXEC usp_SelectAccountsBiggerThen
GO

--03. Create function to update balance with interest rate
CREATE FUNCTION ufn_UpdateBalance(@currentBalance money, @interestRate float, @months int)
  RETURNS money
AS
BEGIN
  DECLARE @periodInterestRate float
  SET @periodInterestRate = @interestRate * (@months / 12)
  SET @currentBalance = @currentBalance * (1 + @periodInterestRate)
  RETURN @currentBalance
END
GO

SELECT Balance, dbo.ufn_UpdateBalance(Balance, 3.4, 6) FROM Accounts
GO

--04. Create stored procedure to update given account with interest rate
CREATE PROC usp_UpdateAccount(@accountId int, @interestRate float)
AS
  UPDATE Accounts
  SET Balance = dbo.ufn_UpdateBalance(Balance, @interestRate, 1)
  WHERE AccountID = @accountId
GO

EXEC usp_UpdateAccount 1, 3.4
GO

--05. Create stored procedure for withdraw and deposit
CREATE PROC usp_WithdrawMoney(@accountId int, @ammount money)
AS
  UPDATE Accounts
  SET Balance = Balance - @ammount
  WHERE AccountID = @accountId
GO

BEGIN TRAN
EXEC usp_WithdrawMoney 1, 500
ROLLBACK TRAN
GO

CREATE PROC usp_DepositMoney(@accountId int, @ammount money)
AS
  UPDATE Accounts
  SET Balance = Balance + @ammount
  WHERE AccountID = @accountId
GO

BEGIN TRAN
EXEC usp_DepositMoney 1, 500
ROLLBACK TRAN
GO

--06. Create Logs and trigger to Accounts balance
CREATE TABLE Logs (
  LogID int IDENTITY,
  AccountID int,
  OldBalance money,
  NewBalance money,
  CONSTRAINT PK_Logs PRIMARY KEY(LogID),
  CONSTRAINT FK_Logs_Accounts FOREIGN KEY(AccountID)
  REFERENCES Accounts(AccountID)
)
GO

CREATE TRIGGER tr_BalanceUpdate ON Accounts FOR UPDATE
AS
  BEGIN
	DECLARE @accountId int
	SET @accountId = (SELECT AccountID FROM inserted)
	DECLARE @oldBalance money
	SET @oldBalance = (SELECT Balance FROM deleted)
	DECLARE @newBalance money
	SET @newBalance = (SELECT Balance FROM inserted)
    INSERT INTO Logs(AccountID, OldBalance, NewBalance)
	VALUES (@accountId, @oldBalance, @newBalance)
  END
GO

EXEC usp_DepositMoney 1, 500
GO

--07. Find names comprised of given set of letters
USE TelerikAcademy
GO

CREATE FUNCTION ufn_CheckName(@letters nvarchar(26), @name nvarchar(50))
  RETURNS bit
AS
BEGIN
  DECLARE @contains bit
  SET @contains = 1
  DECLARE @position int
  SET @position = 0
  DECLARE @current varchar(1)
  SET @current = SUBSTRING(@name, @position, 1)
  WHILE (@position < LEN(@name))
    BEGIN
	  IF (CHARINDEX(@current, @name) < 0)
	    BEGIN
		  SET @contains = 0
		END
	  SET @position = @position + 1
	  SET @current = SUBSTRING(@name, @position, 1)
	END
  RETURN @contains
END
GO

CREATE PROC usp_SelectNames(@letters nvarchar(26))
AS
  DECLARE @word TABLE(Word nvarchar(50))
  INSERT INTO @word 
    SELECT e.FirstName, e.MiddleName, e.LastName, t.Name
    FROM Employees e, Towns t
  SELECT Word FROM @word
  WHERE 1 = dbo.ufn_CheckName(@letters, Word)
GO

EXEC usp_SelectNames 'abc'
GO

--08. Create cursor
DECLARE empCursor CURSOR READ_ONLY FOR
  SELECT e.FirstName, e.LastName, a.TownID 
  FROM Employees e
    JOIN Addresses a ON e.AddressID = a.AddressID

  OPEN empCursor
  DECLARE @firstName nvarchar(50), @lastName nvarchar(50), @prevoiusFirstName nvarchar(50), 
    @previousLastName nvarchar(50), @previousId int, @currentId int
  FETCH NEXT FROM empCursor INTO @firstName, @lastName, @currentId

  WHILE @@FETCH_STATUS = 0
    BEGIN
      SET @prevoiusFirstName = @firstName
	  SET @previousLastName = @lastName
      SET @previousId = @currentId
	  FETCH NEXT FROM empCursor INTO @firstName, @lastName, @currentId
	  IF (@previousId = @currentId)
	    BEGIN
          PRINT @prevoiusFirstName + ' ' + @previousLastName + '-' + @firstName + ' ' + @lastName
	    END
    END

  CLOSE empCursor
DEALLOCATE empCursor
GO

--09. Create cursor that find all employees from each town
DECLARE empCursor CURSOR READ_ONLY FOR
  SELECT CONCAT(e.FirstName, ' ', e.LastName), t.Name 
  FROM Employees e
    JOIN Addresses a ON e.AddressID = a.AddressID
    JOIN Towns t ON a.TownID = t.TownID
  GROUP BY Name, CONCAT(e.FirstName, ' ', e.LastName)

  OPEN empCursor
  DECLARE @town nvarchar(20), @prevoiusTown nvarchar(20), 
    @name nvarchar(100), @record nvarchar(4000)
  FETCH NEXT FROM empCursor INTO @name, @town
  SET @record = @town + ' -> ' + @name
  WHILE @@FETCH_STATUS = 0
    BEGIN
      SET @prevoiusTown = @town
	  FETCH NEXT FROM empCursor INTO @name, @town
	  IF (@prevoiusTown = @town)
	    BEGIN
          SET @record = @record + ', ' + @name
	    END
	  ELSE
	    BEGIN
	      PRINT @record
		  SET @record = @town + ' -> ' + @name
	    END
    END

  CLOSE empCursor
DEALLOCATE empCursor
GO

--10. Create function strConcat
DECLARE @name nvarchar(MAX);
SET @name = N'';
SELECT @name+=e.FirstName+N','
FROM Employees e
SELECT LEFT(@name,LEN(@name)-1);
GO

-- Remove the aggregate and assembly if they're there
IF OBJECT_ID('dbo.concat') IS NOT NULL DROP Aggregate concat 
GO 

IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'concat_assembly') 
       DROP assembly concat_assembly; 
GO      

CREATE Assembly concat_assembly 
   AUTHORIZATION dbo 
   FROM 'concat.dll' 
   WITH PERMISSION_SET = SAFE; 
GO 

CREATE AGGREGATE dbo.concat ( 

    @Value NVARCHAR(MAX) 
  , @Delimiter NVARCHAR(4000) 

) RETURNS NVARCHAR(MAX) 
EXTERNAL Name concat_assembly.concat; 
GO  

SELECT dbo.concat(FirstName + ' ' + LastName, ', ')
FROM Employees
    WHERE FirstName IS NOT NULL AND LastName IS NOT NULL
GO     