USE TelerikAcademy
GO
--04. Find all about deparments
SELECT * FROM Departments

--05. Find all department names
SELECT Name FROM Departments

--06. Find salary of each employee
SELECT EmployeeId, Salary FROM Employees

--07. Find full name of each employee
SELECT CONCAT(FirstName, ' ', MiddleName, ' ', LastName) AS FullName 
FROM Employees

--08. Find Email Addresses
SELECT CONCAT(FirstName, '.', LastName, '@telerik.com') AS FullEmailAddress 
FROM Employees

--09. Find different salaries
SELECT DISTINCT Salary FROM Employees 
ORDER BY Salary DESC

--10. Find all Sales Representative
SELECT * FROM Employees 
WHERE JobTitle = 'Sales Representative'

--11. Find all first names starting with SA
SELECT FirstName FROM Employees 
WHERE FirstName LIKE 'SA%'

--12. Find all last names containing ei
SELECT LastName FROM Employees 
WHERE LastName LIKE '%ei%'

--13. Find all employees with salary between 20000 and 30000
SELECT EmployeeID, Salary FROM Employees 
WHERE Salary BETWEEN 20000 AND 30000

--14. Find name of employees with salary 25000, 14000, 12500 or 23600
SELECT CONCAT(FirstName, ' ', MiddleName, ' ', LastName) AS FullName, Salary 
FROM Employees
WHERE Salary IN (25000, 14000, 12500, 23600)

--15. Find name of employees without manager
SELECT CONCAT(FirstName, ' ', MiddleName, ' ', LastName) AS FullName 
FROM Employees
WHERE ManagerID IS NULL

--16. Find name of employees with salary bigger than 50000
SELECT CONCAT(FirstName, ' ', MiddleName, ' ', LastName) AS FullName, Salary 
FROM Employees
WHERE Salary > 50000 ORDER BY Salary DESC

--17. Find name of employees with top 5 salaries
SELECT TOP 5 CONCAT(FirstName, ' ', MiddleName, ' ', LastName) AS FullName, Salary 
FROM Employees
ORDER BY Salary DESC

--18. Join Employees with Addresses with Towns
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName, 
	a.AddressText, t.Name
FROM Employees e
	JOIN Addresses a ON e.AddressID = a.AddressID
	JOIN Towns t ON a.TownID = t.TownID

--19. Join Employees with Addresses (equijoins)
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName, a.AddressText
FROM Employees e, Addresses a
WHERE e.AddressID = a.AddressID

--20. Join Employees with Employees
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS EmployeeFullName, 
	CONCAT(m.FirstName, ' ', m.MiddleName, ' ', m.LastName) AS ManagerFullName
FROM Employees e
	JOIN Employees m ON e.ManagerID = m.EmployeeID
--check
--SELECT * FROM Employees WHERE FirstName = 'Roberto'
--SELECT * FROM Employees WHERE FirstName = 'Ovidiu'

--21. Join Employees with Employees with Addresses
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS EmployeeFullName, 
	CONCAT(m.FirstName, ' ', m.MiddleName, ' ', m.LastName) AS ManagerFullName, a.AddressText
FROM Employees e
	JOIN Employees m ON e.ManagerID = m.EmployeeID
	JOIN Addresses a ON e.AddressID = a.AddressID

--22. Union tables
SELECT Name FROM Departments
UNION
SELECT Name FROM Towns

--23. Right and left join
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS EmployeeFullName, 
	CONCAT(m.FirstName, ' ', m.MiddleName, ' ', m.LastName) AS ManagerFullName
FROM Employees e
	RIGHT JOIN Employees m ON e.ManagerID = m.EmployeeID

SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS EmployeeFullName, 
	CONCAT(m.FirstName, ' ', m.MiddleName, ' ', m.LastName) AS ManagerFullName
FROM Employees e
	LEFT JOIN Employees m ON e.ManagerID = m.EmployeeID

--24. Find name of employees in Sales and Finance hired between 1995 and 2000
SELECT CONCAT(e.FirstName, ' ', e.MiddleName, ' ', e.LastName) AS FullName, 
	e.HireDate AS HireDate, d.Name AS Department
FROM Employees e
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE d.Name IN ('Sales', 'Finance') 
	AND (e.HireDate > '1/1/1995' AND e.HireDate < '12/31/2000')