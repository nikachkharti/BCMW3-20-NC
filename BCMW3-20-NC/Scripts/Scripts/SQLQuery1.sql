USE master
GO

CREATE DATABASE Class
GO

USE  Class
GO

CREATE TABLE Students
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FullName NVARCHAR(50) NOT NULL,
	Age INT NOT NULL
)

--ALTER TABLE Students
--ADD Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL

--DROP TABLE Students


SELECT*
FROM Students

INSERT INTO Students(FullName,Age)
VALUES
(N'??????? ??????',21),
(N'??????? ???????????',30)

UPDATE Students
SET FullName = N'??????? ??????'
WHERE Id = 2


DELETE FROM Students
WHERE Id = 3


INSERT INTO Students(FullName,Age)
VALUES(N'?????? ?????????',35)


--TRUNCATE TABLE dbo.Students

SELECT*
FROM Students
ORDER BY FullName ASC



SET IDENTITY_INSERT Students ON;
INSERT INTO Students (Id, FullName, Age)
VALUES (3, N'?????? ?????????', 35);
SET IDENTITY_INSERT Students OFF;