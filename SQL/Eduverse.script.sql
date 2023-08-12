-- Check if the database 'Eduverse' exists and drop it if it does
USE MASTER
GO

IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = 'Eduverse')
BEGIN
    DROP DATABASE Eduverse;
END
GO
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
    [Role] VARCHAR(50) CHECK (UPPER([Role]) IN ('ADMIN', 'STREAMER', 'SELF')),
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
[RoleId] BIGINT IDENTITY(1,1) PRIMARY KEY,
EduverseID VARCHAR(50) NOT NULL CONSTRAINT NN_FK_CREDERNTIALROLES_CREDENTIALS REFERENCES Credentials(EduverseId),
[Role] VARCHAR(50) NOT NULL,
constraint UNIQUE_EDUVERSEROLE_CREDENTIALS UNIQUE([Role],EduverseID) 

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
IF(UPPER(@ROLE)='ADMIN')
BEGIN
INSERT INTO EduverseRoles values(@EduverseId,'ADMIN'),
(@EduverseId,'STREAMER'),(@EduverseId,'SELF')
END 
ELSE IF(UPPER(@ROLE)='STREAMER')
BEGIN
INSERT INTO EduverseRoles VALUES
(@EduverseId,'STREAMER'),(@EduverseId,'SELF')
END 
ELSE IF(UPPER(@ROLE)='SELF')
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

    IF EXISTS (SELECT 1 FROM Credentials WHERE UPPER(emailId)=UPPER(@EMAIL) OR UPPER(@PHONENO)=UPPER(phoneNumber))
	BEGIN 
	RETURN -1;
	END
	IF(UPPER(@role)<>'ADMIN' AND UPPER(@ROLE)='SELF')
	BEGIN
    INSERT INTO Credentials
    VALUES ('EDUVERSE_STU_' + CAST(NEXT VALUE FOR eduverseKey AS VARCHAR(50)), @NAME, @EMAIL, @PHONENO, @PASSWORD, 'SELF');
	END
	IF(UPPER(@role)<>'ADMIN' AND UPPER(@ROLE)='STREAMER')
	BEGIN
    INSERT INTO Credentials
    VALUES ('EDUVERSE_INST_' + CAST(NEXT VALUE FOR eduverseKey AS VARCHAR(50)), @NAME, @EMAIL, @PHONENO, @PASSWORD, 'STREAMER');
	END

	IF EXISTS (SELECT 1 FROM Credentials WHERE phoneNumber=@PHONENO AND EMAILID=@EMAIL)
	BEGIN
	DECLARE @EDUVERSEID VARCHAR(50) ;
	SELECT @EDUVERSEID=EduverseID FROM Credentials WHERE phoneNumber=@PHONENO AND EMAILID=@EMAIL
	EXEC ASSIGNROLE @EDUVERSEID
	END
END

GO





INSERT INTO  Credentials  values('EDU_ADM_70248','Arihant Jain','eduverse1802@gmail.com',7649006403,'Akansha@1802','ADMIN') 
SELECT * FROM Credentials
GO

BEGIN

	DECLARE @EDUVERSEADMINID VARCHAR(50)
	SELECT @EDUVERSEADMINID=EduverseID FROM Credentials WHERE phoneNumber=7649006403 AND EMAILID='eduverse1802@gmail.com'
	PRINT @EDUVERSEADMINID
	EXEC AssignRole @EDUVERSEADMINID

	
END





IF EXISTS(SELECT * FROM sys.tables where [name]='Genre' )
DROP TABLE Genre
GO

IF EXISTS(SELECT * FROM sys.tables where [name]='Subgenre' )
DROP TABLE Subgenre
GO

-- Create the Genre table
CREATE TABLE Genre (
  GenreID INT IDENTITY(1,1) PRIMARY KEY,
  GenreName VARCHAR(255) NOT NULL
);

-- Create the Subgenre table
CREATE TABLE Subgenre (
  SubgenreID INT IDENTITY(1,1) PRIMARY KEY,
  SubgenreName VARCHAR(255) NOT NULL,
  GenreID INT,
  FOREIGN KEY (GenreID) REFERENCES Genre(GenreID)
);

-- Insert sample data into the Genre table
INSERT INTO Genre (GenreName)
VALUES
  ('Academic Subjects'),
  ('Skill Development'),
  ('Test Preparation'),
  ('Career Guidance'),
  ('Online Courses'),
  ('E-Learning for Kids'),
  ('Educational Resources for Teachers'),
  ('Health and Wellness Education');

-- Insert sample data into the Subgenre table
INSERT INTO Subgenre (SubgenreName, GenreID)
VALUES
  ('Algebra', 1),
  ('Geometry', 1),
  ('Calculus', 1),
  ('Statistics', 1),
  ('Mathematical Modeling', 1),
  ('Web Development', 2),
  ('Graphic Design', 2),
  ('Photography', 2),
  ('Writing', 2),
  ('SAT', 3),
  ('ACT', 3),
  ('GRE', 3),
  ('GMAT', 3),
  ('IELTS', 3),
  ('Entrepreneurship', 4),
  ('Financial Literacy', 4),
  ('Job Interview Skills', 4),
  ('Resume Writing', 4),
  ('Personal Finance', 5),
  ('Digital Marketing', 5),
  ('Photography', 5),
  ('Creative Writing', 5),
  ('STEM Education for Kids', 6),
  ('Early Literacy', 6),
  ('Arts and Crafts', 6),
  ('Coding for Children', 6),
  ('Classroom Management Strategies', 7),
  ('Differentiated Instruction', 7),
  ('Special Education Resources', 7),
  ('Technology Integration in the Classroom', 7),
  ('Nutrition and Healthy Eating', 8),
  ('Yoga and Meditation', 8),
  ('Mental Health Awareness', 8),
  ('Stress Management', 8),
  ('Fitness Workouts and Exercises', 8);



IF EXISTS(SELECT * FROM sys.tables where [name]='FieldsOfStudy' )
DROP TABLE FieldsOfStudy
GO

IF EXISTS(SELECT * FROM sys.tables where [name]='Subjects' )
DROP TABLE Subjects
GO


  -- Table: Fields of Study
CREATE TABLE FieldsOfStudy (
  FieldID INT IDENTITY(1,1) PRIMARY KEY,
  FieldName VARCHAR(255) NOT NULL
);

-- Table: Subjects
CREATE TABLE Subjects (
  SubjectID INT IDENTITY(1,1) PRIMARY KEY,
  SubjectName VARCHAR(255) NOT NULL,
  FieldID INT,
  FOREIGN KEY (FieldID) REFERENCES FieldsOfStudy(FieldID)
);

-- Inserting sample data into FieldsOfStudy table
INSERT INTO FieldsOfStudy (FieldName)
VALUES
  ('Arts and Humanities'),
  ('Social Sciences'),
  ('Sciences'),
  ('Business and Management'),
  ('Engineering and Technology'),
  ('Health Sciences'),
  ('Education'),
  ('Communication and Media Studies'),
  ('Computer Science and Information Technology'),
  ('Foreign Languages'),
  ('Environmental Studies');



-- Inserting sample data into Subjects table
INSERT INTO Subjects (SubjectName, FieldID)
VALUES
  -- Arts and Humanities
  ('English Literature', 1),
  ('World Literature', 1),
  ('Creative Writing', 1),
  ('Poetry', 1),
  ('Comparative Literature', 1),
  ('Shakespeare Studies', 1),
  ('Cultural Studies', 1),
  ('Art History', 1),
  ('Music History and Theory', 1),
  ('Theatre Arts and Drama', 1),

  -- Social Sciences
  ('Psychology', 2),
  ('Sociology', 2),
  ('Anthropology', 2),
  ('Political Science', 2),
  ('Economics', 2),
  ('Geography', 2),
  ('History', 2),
  ('Social Work', 2),
  ('Gender Studies', 2),
  ('International Relations', 2),

  -- Sciences
  ('Biology', 3),
  ('Chemistry', 3),
  ('Physics', 3),
  ('Mathematics', 3),
  ('Environmental Science', 3),
  ('Geology', 3),
  ('Astronomy', 3),
  ('Statistics', 3),
  ('Genetics', 3),
  ('Microbiology', 3),

  -- Business and Management
  ('Marketing', 4),
  ('Finance', 4),
  ('Accounting', 4),
  ('Entrepreneurship', 4),
  ('Business Administration', 4),
  ('Human Resource Management', 4),
  ('Operations Management', 4),
  ('Business Ethics', 4),
  ('Organizational Behavior', 4),
  ('Supply Chain Management', 4),

  -- Engineering and Technology
  ('Computer Science', 5),
  ('Electrical Engineering', 5),
  ('Mechanical Engineering', 5),
  ('Civil Engineering', 5),
  ('Chemical Engineering', 5),
  ('Information Technology', 5),
  ('Aerospace Engineering', 5),
  ('Robotics', 5),
  ('Materials Science', 5),
  ('Software Engineering', 5),

  -- Health Sciences
  ('Nursing', 6),
  ('Medicine', 6),
  ('Pharmacy', 6),
  ('Public Health', 6),
  ('Anatomy and Physiology', 6),
  ('Nutrition', 6),
  ('Epidemiology', 6),
  ('Health Informatics', 6),
  ('Health Policy', 6),
  ('Medical Research', 6),

  -- Education
  ('Education Psychology', 7),
  ('Curriculum and Instruction', 7),
  ('Special Education', 7),
  ('Educational Leadership', 7),
  ('Early Childhood Education', 7),
  ('Educational Technology', 7),
  ('Classroom Management', 7),
  ('Assessment and Evaluation', 7),
  ('Literacy Education', 7),
  ('Multicultural Education', 7),

  -- Communication and Media Studies
  ('Journalism', 8),
  ('Public Relations', 8),
  ('Broadcasting', 8),
  ('Advertising', 8),
  ('Media Ethics', 8),
  ('Intercultural Communication', 8),
  ('Digital Media Production', 8),
  ('Media Effects', 8),
  ('Media and Society', 8),
  ('Media Writing', 8),

  -- Computer Science and Information Technology
  ('Programming', 9),
  ('Database Management', 9),
  ('Artificial Intelligence', 9),
  ('Data Science', 9),
  ('Web Development', 9),
  ('Cybersecurity', 9),
  ('Network Administration', 9),
  ('Software Testing', 9),
  ('Computer Graphics', 9),
  ('Operating Systems', 9),

  -- Foreign Languages
  ('Spanish Language and Culture', 10),
  ('French Language and Culture', 10),
  ('German Language and Culture', 10),
  ('Mandarin Chinese Language and Culture', 10),
  ('Japanese Language and Culture', 10),
  ('Italian Language and Culture', 10),
  ('Arabic Language and Culture', 10),
  ('Linguistics', 10),

  -- Environmental Studies
  ('Environmental Policy and Law', 11),
  ('Environmental Science', 11),
  ('Sustainability', 11),
  ('Conservation Biology', 11),
  ('Environmental Ethics', 11),
  ('Renewable Energy Systems', 11),
  ('Ecology and Ecosystems', 11),
  ('Climate Change and Adaptation', 11),
  ('Environmental Impact Assessment', 11);


IF EXISTS(SELECT * FROM sys.tables where [name]='Streams' )
DROP TABLE Streams
GO


CREATE TABLE Streams(
StreamerId BIGINT IDENTITY , EduverseId VARCHAR(50) NOT NULL CONSTRAINT FK_CREDENTIALS_Streamer  REFERENCES CREDENTIALS([eduverseId]),
StreamerName VARCHAR(100) NOT NULL,StreamerType int references Subgenre(SubgenreID) not null,[Public] TINYINT CONSTRAINT SCOPE_Streamer_CHECK CHECK([Public] IN (1,2)),
[image] varbinary(max) , StreamerDescription varchar(200) ,
paid tinyint  check( paid in (0,1)) not null , price money not null
)  

-- Inserting sample data into Streams table
INSERT INTO Streams (EduverseId, StreamerName, StreamerType, [Public], [image], StreamerDescription, paid, price)
VALUES
  -- Educational Courses
  ('EDU_ADM_70248', 'MathMaster', 1, 1, NULL, 'Streaming expert-level math courses.', 1, 49.99),
  ('EDU_ADM_70248', 'Language Learner', 1, 1, NULL, 'Streaming language learning courses.', 1, 29.99),
  ('EDU_ADM_70248', 'Science Spectacle', 1, 1, NULL, 'Streaming captivating science courses.', 1, 39.99),
  ('EDU_ADM_70248', 'History Haven', 1, 1, NULL, 'Streaming immersive history courses.', 1, 34.99),
  ('EDU_ADM_70248', 'Art Appreciator', 1, 1, NULL, 'Streaming courses on art history and appreciation.', 1, 44.99),

  -- Quizzes
  ('EDU_ADM_70248', 'Brain Buster Quiz', 2, 1, NULL, 'Streaming challenging brain teaser quizzes.', 0, 0.00),
  ('EDU_ADM_70248', 'Language Trivia', 2, 1, NULL, 'Streaming language trivia quizzes.', 0, 0.00),
  ('EDU_ADM_70248', 'Science Quiz Mania', 2, 1, NULL, 'Streaming fun and educational science quizzes.', 1, 19.99),
  ('EDU_ADM_70248', 'History Explorer', 2, 1, NULL, 'Streaming quizzes on historical events and figures.', 1, 24.99),
  ('EDU_ADM_70248', 'Art Quiz Quest', 2, 1, NULL, 'Streaming quizzes to test your knowledge of art and artists.', 1, 29.99),

  -- Trending Discussions
  ('EDU_ADM_70248', 'Hot Topic Debates', 3, 2, NULL, 'Streaming trending debates on hot educational topics.', 1, 14.99),
  ('EDU_ADM_70248', 'Language Exchange', 3, 2, NULL, 'Streaming language exchange discussions.', 0, 0.00),
  ('EDU_ADM_70248', 'Science Talks', 3, 2, NULL, 'Streaming insightful discussions on scientific discoveries.', 1, 9.99),
  ('EDU_ADM_70248', 'History Mysteries', 3, 2, NULL, 'Streaming intriguing discussions on historical mysteries.', 1, 14.99),
  ('EDU_ADM_70248', 'Artistic Inspiration', 3, 2, NULL, 'Streaming discussions to inspire and appreciate art.', 0, 0.00),

  -- Other Educational Content
  ('EDU_ADM_70248', 'TEDx Inspire', 4, 1, NULL, 'Streaming inspiring TEDx talks on various educational topics.', 1, 9.99),
  ('EDU_ADM_70248', 'Documentary Hub', 4, 1, NULL, 'Streaming captivating educational documentaries.', 1, 19.99),
  ('EDU_ADM_70248', 'Edutainment Fun', 4, 1, NULL, 'Streaming educational content with a touch of entertainment.', 1, 14.99),
  ('EDU_ADM_70248', 'Skill Showcase', 4, 1, NULL, 'Streaming showcases of unique educational skills.', 0, 0.00),
  ('EDU_ADM_70248', 'Science Experiments', 4, 1, NULL, 'Streaming exciting science experiments and demonstrations.', 1, 24.99);


  IF EXISTS(SELECT * FROM sys.tables where [name]='Notes' )
DROP TABLE Notes
GO
create Table Notes(notesId bigint identity(1,1) primary key, title varchar(max),body varchar(max), titleStyle varchar(max),bodyStyle varchar(max),userId VARCHAR(50) references credentials(eduverseId) ,isPrivate bit)
select * from Notes