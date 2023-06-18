
if exists (select 1 from sys.databases where [name]='Eduverse')
begin
drop database Eduverse
end
create database Eduverse
go

USE  EDUVERSE
GO

--connection String
--scaffold-dbcontext "data source=areyhant;initial catalog=Eduverse;integrated security=true;trustservercertificate=false;trusted_connection=true;encrypt=false;" "Microsoft.EntityFrameworkCore.SqlServer" -outputDir "DBModels" -force

if exists(select 1 from sys.objects where [object_id]=OBJECT_ID('smtpMailCredentials'))
begin
drop table smtpMailCredentials
end
create table smtpMailCredentials(smtpMailCredentialsId int identity primary KEY,[role] varchar(50) UNIQUE NOT NULL,[emailId] varchar(100) NOT NULL,[password] varchar(50) NOT NULL,[port] int,[server] varchar(50) )
insert into smtpMailCredentials values('OTP-GENERATION','eduverse1802@gmail.com','gdqnynxshtlxjmoi',587,'smtp.gmail.com')


select * from smtpMailCredentials


if exists(select 1 from sys.objects where [object_id]=OBJECT_ID('Temp_OTPs'))
begin
drop table Temp_Otps
end

create table Temp_Otps(Id varchar(100) primary key ,Method varchar(40),Otp decimal(6,0),GeneratedTimeStamp date )

select * from Temp_Otps