--00. Create database PerformanceDB

USE master
GO

CREATE DATABASE PerformanceDB
GO

USE PerformanceDB
GO

--01. Create table Logs and populate 10 000 000 rows in it

CREATE TABLE Logs(
  LogId INT NOT NULL PRIMARY KEY IDENTITY,
  MessageText TEXT NOT NULL,
  SendDate DATE NOT NULL
)

DECLARE @Counter INT = 0
WHILE (SELECT COUNT(*) FROM Logs) < 10000000
BEGIN
  INSERT INTO Logs(MessageText, SendDate) 
  VALUES ('Text ' + CONVERT(TEXT, @Counter), GETDATE() + DAY(@Counter))
  SET @Counter = @Counter + 1
END

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache
SELECT COUNT(*) FROM Logs

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache
SELECT MessageText FROM Logs
WHERE SendDate > '31-Dec-2020' and SendDate < '1-Jan-2024'

--02. Add index by SendDate

CREATE INDEX IDX_Logs_SendDate ON Logs(SendDate)

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT MessageText FROM Logs
WHERE SendDate > '31-Dec-2020' and SendDate < '1-Jan-2024'

DROP INDEX IDX_Logs_SendDate ON Logs

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

--03. Add full-text index and search by indexed text column (left LIKE)

SELECT COUNT(*) FROM Logs
WHERE MessageText LIKE 'Text 999%'

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT LogId FROM Logs
WHERE MessageText LIKE 'Text 999%'

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT MessageText FROM Logs
WHERE MessageText LIKE 'Text 999%'

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

CREATE FULLTEXT CATALOG MessageTextCatalog
WITH ACCENT_SENSITIVITY = OFF

CREATE FULLTEXT INDEX ON Logs(MessageText)
KEY INDEX PK_Messages_MessageId
ON MessageTextCatalog
WITH CHANGE_TRACKING AUTO

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

SELECT * FROM Logs
WHERE CONTAINS(MessageText, '999')

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache

-- This is still slow
SELECT COUNT(*) FROM Logs
WHERE MessageText LIKE '999%'

DROP FULLTEXT INDEX ON Logs
DROP FULLTEXT CATALOG MessageTextCatalog

CHECKPOINT; DBCC DROPCLEANBUFFERS; -- Empty the SQL Server cache