/*
This is an initial script to be executed once during the system setup.
*/
USE master
GO
CREATE DATABASE WebAppTemplate_DB
GO
USE WebAppTemplate_DB
GO

/*Create table*/
CREATE TABLE MST_User(
	UserId INT CONSTRAINT PK_MST_User PRIMARY KEY IDENTITY(1,1),
	UserName VARCHAR(20) CONSTRAINT UQ_MST_User_UserName UNIQUE NOT NULL ,
	FullName VARCHAR(100) NOT NULL,
	Password VARCHAR(20) NOT NULL,
	Role VARCHAR(10) NOT NULL,
	IsActive BIT CONSTRAINT DF_MST_User_IsActive DEFAULT 1 NOT NULL
);
GO

--Insert an Admin user into MST_User table
INSERT INTO MST_User
(UserName,FullName,Password,Role,IsActive)

SELECT 'admin' AS 'UserName', 'Mr Admin Admin' AS 'FullName', 'pass123' AS 'Password', 'admin' AS 'Role', 1 AS 'IsActive'
GO
