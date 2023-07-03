-- Check if the database 'Eduverse' exists and drop it if it does
USE MASTER
IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = 'Eduverse')
BEGIN
    DROP DATABASE Eduverse;
END

-- Create the database 'Eduverse'
CREATE DATABASE Eduverse;
GO

-- Use the 'Eduverse' database
USE Eduverse;
GO

-- Create the 'smtpMailCredentials' table
IF EXISTS (SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID('smtpMailCredentials'))
BEGIN
    DROP TABLE smtpMailCredentials;
END

CREATE TABLE smtpMailCredentials (
    smtpMailCredentialsId INT IDENTITY PRIMARY KEY,
    [role] VARCHAR(50) NOT NULL,
    [emailId] VARCHAR(100) NOT NULL,
    [password] VARCHAR(50) NOT NULL,
    [port] INT,
    [server] VARCHAR(50),
    CONSTRAINT UQ_Role UNIQUE ([role])
);

-- Insert a row into 'smtpMailCredentials' table
INSERT INTO smtpMailCredentials VALUES ('OTP-GENERATION', 'eduverse1802@gmail.com', 'gdqnynxshtlxjmoi', 587, 'smtp.gmail.com');

-- Select all records from 'smtpMailCredentials' table
SELECT * FROM smtpMailCredentials;

-- Create the 'Temp_OTPs' table
IF EXISTS (SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID('Temp_OTPs'))
BEGIN
    DROP TABLE Temp_OTPs;
END

CREATE TABLE Temp_OTPs (
    Id VARCHAR(100) PRIMARY KEY,
    Method VARCHAR(40),
    Otp DECIMAL(6, 0),
    GeneratedTimeStamp DATE
);

-- Check if the 'Credentials' table exists and drop it if it does
IF EXISTS (SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID('Credentials'))
BEGIN
    DROP TABLE Credentials;
END

-- Create the 'Credentials' table
CREATE TABLE Credentials (
    EduverseId VARCHAR(50) PRIMARY KEY,
    NAME VARCHAR(300) NOT NULL,
    emailId VARCHAR(300) NOT NULL UNIQUE,
    phoneNumber DECIMAL(10, 0) NOT NULL UNIQUE,
    [password] VARCHAR(100) NOT NULL,
    [Role] VARCHAR(50) CHECK (Lower([Role]) IN ('admin', 'institute', 'self')),
    CONSTRAINT CHK_Role CHECK (LEN([Role]) > 0),
    CONSTRAINT CHK_Email CHECK (emailId LIKE '%_@__%.%'),
    CONSTRAINT CHK_Password CHECK ([password] LIKE '%[A-Za-z0-9]%[!@#$%^&*()]%')
);


IF EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'eduverseKey')
drop sequence eduverseKey

-- Create the 'eduverseKey' sequence
CREATE SEQUENCE eduverseKey
AS BIGINT
START WITH 1000
NO CYCLE
CACHE 50;
GO

-- Create the 'EnteringCredentials' procedure
IF EXISTS(SELECT 1 FROM SYS.procedures WHERE name = 'EnteringCredentials')
DROP PROCEDURE EnteringCredentials

--connection String
--scaffold-dbcontext "data source=areyhant;initial catalog=Eduverse;integrated security=true;trustservercertificate=false;trusted_connection=true;encrypt=false;" "Microsoft.EntityFrameworkCore.SqlServer" -outputDir "DBModels" -force



IF  EXISTS(SELECT 1 FROM Sys.objects WHERE name = 'EduverseRoles')
DROP TABLE EDUVERSEROLES

IF NOT EXISTS(SELECT 1 FROM Sys.objects WHERE name = 'EduverseRoles')
BEGIN
CREATE TABLE EduverseRoles(
EduverseRoleId BIGINT IDENTITY(1,1) PRIMARY KEY,
EduverseID VARCHAR(50) NOT NULL CONSTRAINT NN_FK_CREDERNTIALROLES_CREDENTIALS REFERENCES Credentials(EduverseId),
EduverseRole VARCHAR(50) NOT NULL,
constraint UNIQUE_EDUVERSEROLE_CREDENTIALS UNIQUE(EduverseRole,EduverseID) 

)
END

GO


IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='AssignRole') 
DROP PROCEDURE AssignRole
GO
CREATE PROCEDURE AssignRole 
@EduverseId varchar(100)
AS
BEGIN 
DECLARE @ROLE VARCHAR(50);
SELECT @ROLE =[ROLE] FROM  Credentials WHERE EduverseID=@EduverseID
IF(LOWER(@ROLE)='admin')
BEGIN
INSERT INTO EduverseRoles values(@EduverseId,'ADMIN'),
(@EduverseId,'INSTITUTE'),(@EduverseId,'SELF')
END 
ELSE IF(LOWER(@ROLE)='institute')
BEGIN
INSERT INTO EduverseRoles VALUES
(@EduverseId,'INSTITUTE'),(@EduverseId,'SELF')
END 
ELSE IF(LOWER(@ROLE)='self')
BEGIN
INSERT INTO EduverseRoles VALUES
(@EduverseId,'SELF')
END 
END
GO

CREATE PROCEDURE EnteringCredentials
    @NAME VARCHAR(100),
    @EMAIL VARCHAR(100),
    @PHONENO DECIMAL(10, 0),
    @PASSWORD VARCHAR(100),
    @ROLE VARCHAR(100)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM Credentials WHERE LOWER(emailId)=LOWER(@EMAIL) OR LOWER(@PHONENO)=LOWER(phoneNumber))
	BEGIN 
	RETURN -1;
	END
	IF(LOWER(@role)<>'admin' AND LOWER(@ROLE)='self')
	BEGIN
    INSERT INTO Credentials
    VALUES ('EDUVERSE_STU_' + CAST(NEXT VALUE FOR eduverseKey AS VARCHAR(50)), @NAME, @EMAIL, @PHONENO, @PASSWORD, @ROLE);
	END
	IF(LOWER(@role)<>'admin' AND LOWER(@ROLE)='institute')
	BEGIN
    INSERT INTO Credentials
    VALUES ('EDUVERSE_INST_' + CAST(NEXT VALUE FOR eduverseKey AS VARCHAR(50)), @NAME, @EMAIL, @PHONENO, @PASSWORD, @ROLE);
	END

	IF EXISTS (SELECT 1 FROM Credentials WHERE phoneNumber=@PHONENO AND EMAILID=@EMAIL)
	BEGIN
	DECLARE @EDUVERSEID VARCHAR(50) ;
	SELECT @EDUVERSEID=EduverseID FROM Credentials WHERE phoneNumber=@PHONENO AND EMAILID=@EMAIL
	EXEC ASSIGNROLE @EDUVERSEID
	END
END;
GO


INSERT INTO  Credentials  values('EDU_ADM_70248','Arihant Jain','eduverse1802@gmail.com',7649006403,'Akansha@1802','ADMIN') 
SELECT * FROM Credentials
GO


BEGIN

	DECLARE @EDUVERSEADMINID VARCHAR(50)
	SELECT @EDUVERSEADMINID=EduverseID FROM Credentials WHERE phoneNumber=7649006403 AND EMAILID='eduverse1802@gmail.com'
	PRINT @EDUVERSEADMINID
	EXEC AssignRole @EDUVERSEADMINID

	select * from EduverseRoles
END