USE TelerikAcademy
GO
--01. Find names and salaries of employees with minimal salary
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName, 
	e.Salary AS Salary
FROM Employees e
WHERE Salary = 
	(SELECT MIN(Salary) FROM Employees)

--02. Find names and salaries of employees with up to 1.1*minimal salary
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName, 
	e.Salary AS Salary
FROM Employees e
WHERE Salary BETWEEN 
	(SELECT MIN(Salary) FROM Employees) AND 
	(SELECT 1.1 * MIN(Salary) FROM Employees)

--03. Find names and salaries of employees with up to 1.1*minimal salary
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName, 
	e.Salary, d.Name
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE e.Salary = 
	(SELECT MIN(Salary) FROM Employees
	WHERE DepartmentID = d.DepartmentID)
ORDER BY d.DepartmentID

--04. Find average salary in department 1
SELECT AVG(Salary) AS AverageSalary 
FROM Employees
WHERE DepartmentID = 1

--05. Find average salary in department Sales
SELECT AVG(Salary) AS AverageSalary
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE d.Name = 'Sales'

--06. Find number of employees in department Sales
SELECT COUNT(*) AS NumberEmployees
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE d.Name = 'Sales'

--07. Find number of employees with manager
SELECT COUNT(*) AS NumberEmployees
FROM Employees e 
WHERE e.ManagerID IS NOT NULL

--08. Find number of employees without manager
SELECT COUNT(*) AS NumberEmployees
FROM Employees e 
WHERE e.ManagerID IS NULL

--09. Find average salary for all department
SELECT d.Name AS DepartnentName, AVG(Salary) AS AverageSalary
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
GROUP BY d.Name

--10. Find number of employees for all department and each town
SELECT d.Name AS DepartnentTownName, COUNT(*) AS NumberEmployees
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
GROUP BY d.Name
UNION
SELECT t.Name, COUNT(*) AS NumberEmployees
FROM Employees e 
	JOIN Addresses a ON e.AddressID = a.AddressID
	JOIN Towns t ON a.TownID = t.TownID
GROUP BY t.Name

--11. Find all managers with 5 employees
SELECT CONCAT(m.FirstName, ' ', m.LastName) AS FullName
FROM Employees e 
	JOIN Employees m ON e.ManagerID = m.EmployeeID
GROUP BY CONCAT(m.FirstName, ' ', m.LastName)
HAVING COUNT(*) = 5

--12. Find all employees and their managers
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS EmployeeFullName, 
	CASE WHEN e.ManagerID IS NULL
		THEN '(no manager)'
		ELSE CONCAT(m.FirstName, ' ', m.MiddleName, ' ', m.LastName)
	END AS ManagerFullName
FROM Employees e
	LEFT JOIN Employees m ON e.ManagerID = m.EmployeeID

--13. Find all employees with last name of 5 letters
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS EmployeeFullName,
	e.LastName AS LastName
FROM Employees e
WHERE LEN(e.LastName) = 5

--14. Find current time in format "day.month.year hour:minutes:seconds:milliseconds"
SELECT CONCAT(DAY(GETDATE()), '.', MONTH(GETDATE()), '.', YEAR(GETDATE()), ' ',
	CONVERT(TIME, GETDATE(), 114)) AS CurrentTime

--15. Create table Users
CREATE TABLE Users (
	UserID int IDENTITY,
	UserName nvarchar(100) UNIQUE NOT NULL,
	UserPassword nvarchar(100),
	FullName nvarchar(100) NOT NULL,
	LastLoginTime DATETIME,
	CONSTRAINT PK_Users PRIMARY KEY(UserID),
	CONSTRAINT UC_UserName UNIQUE (UserName),
	CONSTRAINT LC_UserPassword CHECK (LEN(UserPassword) > 5)
)
GO

--16. Create table Users
CREATE VIEW [Today Logged Users] AS
	SELECT UserName FROM Users
	WHERE DAY(LastLoginTime) = DAY(GETDATE())
GO

--17. Create table Groups
CREATE TABLE Groups (
	GroupID int IDENTITY,
	Name nvarchar(100) UNIQUE NOT NULL,
	CONSTRAINT PK_Groups PRIMARY KEY(GroupID),
	CONSTRAINT UC_Name UNIQUE (Name)
)
GO

--18. Add foreign key
ALTER TABLE Users 
ADD GroupID int
ALTER TABLE Users
ADD CONSTRAINT FK_Users_Groups
	FOREIGN KEY (GroupID)
	REFERENCES Groups(GroupID)
GO

--19. Insert data
INSERT INTO Groups(Name)
VALUES ('Group')
INSERT INTO Groups(Name)
VALUES ('ToDelete')

INSERT INTO Users(UserName, UserPassword, FullName, LastLoginTime, GroupID)
VALUES ('user', '111111', 'S T', GETDATE(), 1)
INSERT INTO Users(UserName, UserPassword, FullName, LastLoginTime, GroupID)
VALUES ('other', '333333', 'T S', GETDATE(), 1)
GO

--20. Update data
UPDATE Groups
SET Name = 'Company'
WHERE GroupID = 1

UPDATE Users
SET FullName = 'Sam Thomas'
WHERE UserID = 1
GO

--21. Delete data
DELETE FROM Groups WHERE GroupID = 2

DELETE FROM Users WHERE UserID = 2
GO

--22. Insert data
INSERT INTO Users(UserName, UserPassword, FullName, LastLoginTime, GroupID)
	SELECT LEFT(FirstName, 1) + LOWER(LastName), LEFT(FirstName, 1) + LOWER(LastName),
		FirstName + ' ' + LastName, NULL, NULL
	FROM Employees
GO

--23. Update data
UPDATE Users
SET UserPassword = NULL
WHERE LastLoginTime < '10/3/2010'
GO

--24. Delete data
DELETE FROM Users WHERE UserPassword = NULL
GO

--25. Find average salary for all department and all their job titles
SELECT d.Name AS DepartnentName, e.JobTitle AS JobTitle, AVG(Salary) AS AverageSalary
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
GROUP BY d.Name, e.JobTitle
ORDER BY d.Name

--26. Find average salary for all department and all their job titles
SELECT d.Name AS DepartnentName, e.JobTitle AS JobTitle, MIN(Salary) AS MinimalSalary,
	CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName
FROM Employees e 
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
GROUP BY d.Name, e.JobTitle, CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName)
ORDER BY d.Name

--27. Find town with max employees
SELECT TOP 1 t.Name AS TownName, COUNT(*) AS EmployeesNumber
FROM Employees e 
	JOIN Addresses a ON e.AddressID = a.AddressID
	JOIN Towns t ON a.TownID = t.TownID
GROUP BY t.Name
ORDER BY EmployeesNumber DESC

--28. Find average salary for all department and all their job titles
SELECT t.*, COUNT(e.EmployeeID) as ManagerCount
FROM Towns t
	JOIN Addresses a ON a.TownID = t.TownId
	JOIN Employees e ON e.AddressID = a.AddressID 
WHERE e.EmployeeID IN
	(SELECT DISTINCT m.EmployeeID 
	FROM Employees m
		JOIN Employees e ON m.EmployeeID = e.ManagerID)
GROUP BY t.Name, t.TownID

--29.
CREATE TABLE [dbo].[WorkHours](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [EmployeeId] [int] NOT NULL,
        [Date] [date] NOT NULL,
        [Task] [nvarchar](80) NOT NULL,
        [Hours] [float] NOT NULL,
        [Comments] [nvarchar] (300) NOT NULL,
 CONSTRAINT [PK_WorkHours] PRIMARY KEY CLUSTERED
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[WorkHours]  WITH CHECK ADD  CONSTRAINT [FK_WorkHours_Employees] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO
 
ALTER TABLE [dbo].[WorkHours] CHECK CONSTRAINT [FK_WorkHours_Employees]
GO
 
----------------------------------------------------------------------------------------------------
INSERT INTO WorkHours
VALUES (5, '12-05-2012', 'Added new product', 2.30, 'The new product will be developed')
 
INSERT INTO WorkHours
VALUES (12, '10-25-2011', 'Found a new bug', 2.30, 'Found a bug in an existing project')
 
INSERT INTO WorkHours
VALUES (181, '7-7-2012', 'Fixed a fe bugs', 2.30, 'Fixed some bugs before the release')
 
UPDATE WorkHours
SET EmployeeId = 141
WHERE Id = 3
 
UPDATE WorkHours
SET EmployeeId = 13
WHERE EmployeeId = 10
 
DELETE FROM WorkHours
WHERE EmployeeId = 5
 
DELETE FROM WorkHours
WHERE Date = '10-24-2011'
 
--------------------------------------------------------------------------------------------------
 
CREATE TABLE [dbo].[WorkHoursLogs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [OldEmployeeId] [int],
        [OldDate] [date],
        [OldTask] [nvarchar](80),
        [OldHours] [float],
        [OldComments] [nvarchar] (300),
        [NewEmployeeId] [int],
        [NewDate] [date],
        [NewTask] [nvarchar](80),
        [NewHours] [float],
        [NewComments] [nvarchar] (300),
        [Command] [nvarchar] (30),
 CONSTRAINT [PK_WorkHoursLogs] PRIMARY KEY CLUSTERED
(
        [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
 
----------------------------------------------------------------------------------------------------
 
USE [TelerikAcademy]
GO
/****** Object:  Trigger [dbo].[WorkHoursInsert]    Script Date: 24.12.2012 ?. 16:00:28 ?. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE TRIGGER [dbo].[WorkHoursInsert] ON [dbo].[WorkHours]
AFTER INSERT
AS
DECLARE @newEmployeeId int, @newDate date, @newTask nvarchar (80),
        @newHours float, @newComment nvarchar (max)
 
SELECT @newEmployeeId = i.EmployeeId, @newDate = i.Date,
        @newTask = i.Task, @newHours = i.Hours, @newComment = i.Comments
FROM [dbo].[WorkHours] AS p INNER JOIN inserted i
ON p.Id = i.Id
 
INSERT INTO WorkHoursLogs (NewEmployeeId, NewDate, NewTask,
        NewHours, NewComments, Command)
VALUES (@newEmployeeId, @newDate, @newTask, @newHours, @newComment, 'Insert')
 
GO

USE [TelerikAcademy]
GO
/****** Object:  Trigger [dbo].[WorkHoursUpdate]    Script Date: 25.12.2012 ?. 12:21:00 ?. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE TRIGGER [dbo].[WorkHoursUpdate] ON [dbo].[WorkHours]
AFTER UPDATE
AS
DECLARE @newEmployeeId int, @newDate date, @newTask nvarchar (80),
        @newHours float, @newComment nvarchar (300),
        @oldEmployeeId int, @oldDate date, @oldTask nvarchar (80),
        @oldHours float, @oldComment nvarchar (300)
 
SELECT @oldEmployeeId = i.EmployeeId, @oldDate = i.Date,
        @oldTask = i.Task, @oldHours = i.Hours, @oldComment = i.Comments
FROM [dbo].[WorkHours] AS p INNER JOIN deleted i
ON p.Id = i.Id
 
SELECT @newEmployeeId = i.EmployeeId, @newDate = i.Date,
        @newTask = i.Task, @newHours = i.Hours, @newComment = i.Comments
FROM [dbo].[WorkHours] AS p INNER JOIN inserted i
ON p.Id = i.Id
 
INSERT INTO WorkHoursLogs (OldEmployeeId, OldDate, OldTask,
        OldHours, OldComments, NewEmployeeId, NewDate, NewTask,
        NewHours, NewComments, Command)
VALUES (@oldEmployeeId, @oldDate, @oldTask, @oldHours, @oldComment,
                @newEmployeeId, @newDate, @newTask, @newHours, @newComment,
                'Update')
GO              
               
USE [TelerikAcademy]
GO
/****** Object:  Trigger [dbo].[WorkHoursDelete]    Script Date: 25.12.2012 ?. 12:42:59 ?. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE TRIGGER [dbo].[WorkHoursDelete] ON [dbo].[WorkHours]
AFTER DELETE
AS
DECLARE @oldEmployeeId int, @oldDate date, @oldTask nvarchar (80),
        @oldHours float, @oldComment nvarchar (300)
 
SELECT @oldEmployeeId = i.EmployeeId, @oldDate = i.Date,
           @oldTask = i.Task, @oldHours = i.Hours, @oldComment = i.Comments
FROM deleted i
 
INSERT INTO WorkHoursLogs (OldEmployeeId, OldDate, OldTask,
        OldHours, OldComments, Command)
VALUES (@oldEmployeeId, @oldDate, @oldTask, @oldHours, @oldComment,
                'Delete')
GO

--30. Start a database transaction, delete all employees from the 'Sales' department
--along with all dependent records from the pother tables.
--At the end rollback the transaction. */
USE TelerikAcademy
GO
 
BEGIN TRAN DeleteEmployees
 
DECLARE @id int, @managerId int
SET @id = (SELECT DepartmentID FROM Departments
WHERE Name = 'Sales')
SET @managerId = (SELECT ManagerID
FROM Departments WHERE DepartmentID = @id)
 
DELETE FROM Employees
WHERE DepartmentID = @id --AND EmployeeID != @managerId
 
ROLLBACK TRAN DeleteEmployees 

--31. Start a database transaction and drop the table EmployeesProjects.
--Now how you could restore back the lost table data? */
USE TelerikAcademy
GO
 
BEGIN TRAN
 
DROP TABLE EmployeesProjects
 
ROLLBACK TRAN
 
--32. Find how to use temporary tables in SQL Server. Using temporary tables backup all records
--from EmployeesProjects and restore them back after dropping and re-creating the table */

USE TelerikAcademy
GO

BEGIN TRAN
 
CREATE TABLE #EmployeesProjectsTemp(
        EmployeeID int NOT NULL,
        ProjectID int NOT NULL,
        PRIMARY KEY (EmployeeID, ProjectID)
)
 
GO
 
INSERT INTO #EmployeesProjectsTemp (EmployeeID, ProjectID)
SELECT EmployeeID, ProjectID FROM EmployeesProjects

SELECT * FROM #EmployeesProjectsTemp
 
DROP TABLE EmployeesProjects
 
GO
 
CREATE TABLE EmployeesProjects(
        EmployeeID int NOT NULL,
        ProjectID int NOT NULL,
        PRIMARY KEY (EmployeeID, ProjectID)
)
 
GO
 
INSERT INTO EmployeesProjects (EmployeeID, ProjectID)
SELECT EmployeeID, ProjectID FROM #EmployeesProjectsTemp
 
SELECT EmployeeID, ProjectID FROM EmployeesProjects