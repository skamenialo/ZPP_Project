-- ## DROP ## --
DROP TABLE Comments;
DROP TABLE SL_CommentState;
DROP TABLE Grades;
DROP TABLE Attendance;
DROP TABLE Lectures;
DROP TABLE Groups;
DROP TABLE Courses;
DROP TABLE SL_CourseStates;
DROP TABLE Teachers;
DROP TABLE Companies;
DROP TABLE Students;
DROP TABLE EF_UserLogins;
DROP TABLE EF_UserClaims;
DROP TABLE EF_UserRoles;
DROP TABLE Users;
DROP TABLE SL_UserType;

-- ## CREATE ## --

-- SL_UserType --
CREATE TABLE SL_UserType(					--known as Roles in Identity.EntityFramework
  IdUserType INTEGER,						--known as Id in Identity.EntityFramework
  Name VARCHAR(64) NOT NULL,
  CONSTRAINT SL_UserType_PK PRIMARY KEY CLUSTERED (IdUserType ASC)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX SL_UserType_Name_IX
    ON SL_UserType(Name ASC);
	
-- Users --
CREATE TABLE Users(
  IdUser INTEGER IDENTITY(1,1) NOT NULL,	--known as Id in Identity.EntityFramework
  UserType INTEGER NOT NULL,
  Active BIT NOT NULL,						--known as EmailConfirmed in Identity.EntityFramework
  Banned BIT NOT NULL,
  Login VARCHAR(64) NOT NULL,				--known as UserName in Identity.EntityFramework
  PasswordHash VARCHAR(256) NOT NULL,
  Email VARCHAR(256) NOT NULL,
  --needed by Identity.EntityFramework
  EF_SecurityStamp VARCHAR(256),
  EF_PhoneNumber VARCHAR(128),
  EF_PhoneNumberConfirmed BIT NOT NULL,
  EF_TwoFactorEnabled BIT NOT NULL,
  EF_LockoutEnabled BIT NOT NULL,
  EF_LockoutEndDateUtc DATETIME,
  EF_AccessFailedCount INTEGER NOT NULL, 
  CONSTRAINT User_PK PRIMARY KEY (IdUser),
  CONSTRAINT User_UserType_FK FOREIGN KEY (UserType) REFERENCES SL_UserType(IdUserType),
  CONSTRAINT User_Login_UQ UNIQUE (Login),
);

-- Students --
CREATE TABLE Students(
  IdStudent INTEGER IDENTITY(1,1) NOT NULL,
  IdUser INTEGER NOT NULL,
  LastName VARCHAR(256) NOT NULL,
  FirstName VARCHAR(256) NOT NULL,
  Address VARCHAR(512) NOT NULL,
  CONSTRAINT Student_PK PRIMARY KEY (IdStudent),
  CONSTRAINT Student_IdUser_FK FOREIGN KEY (IdUser) REFERENCES Users(IdUser),
  CONSTRAINT Student_IdUser_UQ UNIQUE (IdUser),
);

-- Companies --
CREATE TABLE Companies(
  IdCompany INTEGER IDENTITY(1,1) NOT NULL,
  IdUser INTEGER NOT NULL,
  Name VARCHAR(256) NOT NULL,
  Address VARCHAR(512) NOT NULL,
  Email VARCHAR(256) NOT NULL,		--contact email
  Website VARCHAR(256),
  Description VARCHAR(4096),
  CONSTRAINT Company_PK PRIMARY KEY (IdCompany),
  CONSTRAINT Company_IdUser_FK FOREIGN KEY (IdUser) REFERENCES Users(IdUser),
  CONSTRAINT Company_IdUser_UQ UNIQUE (IdUser)
);

-- Teachers --
CREATE TABLE Teachers(
  IdTeacher INTEGER IDENTITY(1,1) NOT NULL,
  IdUser INTEGER NOT NULL,
  IdCompany INTEGER NOT NULL,
  LastName VARCHAR(256) NOT NULL,
  FirstName VARCHAR(256) NOT NULL,
  Address VARCHAR(512) NOT NULL,
  Degree VARCHAR(128),
  Website VARCHAR(256),
  Description VARCHAR(4096),
  CONSTRAINT Teacher_PK PRIMARY KEY (IdTeacher),
  CONSTRAINT Teacher_IdUser_FK FOREIGN KEY (IdUser) REFERENCES Users(IdUser),
  CONSTRAINT Teacher_IdUser_UQ UNIQUE (IdUser),
  CONSTRAINT Teacher_IdCompany_FK FOREIGN KEY (IdCompany) REFERENCES Companies(IdCompany)
);

-- SL_CourseStates --
CREATE TABLE SL_CourseStates(
  IdState INTEGER,
  Name VARCHAR(64) NOT NULL,
  CONSTRAINT SL_CourseStates_PK PRIMARY KEY (IdState)
);

-- Courses --
CREATE TABLE Courses(
  IdCourse INTEGER IDENTITY(1,1) NOT NULL,
  IdTeacher INTEGER NOT NULL,
  IdCompany INTEGER NOT NULL,
  Name VARCHAR(256) NOT NULL,
  Lectures Integer NOT NULL,
  State INTEGER NOT NULL,
  Description VARCHAR(4096),
  DateStart DATE,
  DateEnd DATE,
  CONSTRAINT Course_PK PRIMARY KEY (IdCourse),
  CONSTRAINT Course_IdTeacher_FK FOREIGN KEY (IdTeacher) REFERENCES Teachers(IdTeacher),
  CONSTRAINT Course_IdCompany_FK FOREIGN KEY (IdCompany) REFERENCES Companies(IdCompany),
  CONSTRAINT Course_State_FK FOREIGN KEY (State) REFERENCES SL_CourseStates(IdState)
);

-- Groups --
CREATE TABLE Groups(
  IdGroup INTEGER IDENTITY(1,1) NOT NULL,
  IdStudent INTEGER NOT NULL,
  IdCourse INTEGER NOT NULL,
  CONSTRAINT Group_PK PRIMARY KEY (IdGroup),
  CONSTRAINT Group_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdStudent),
  CONSTRAINT Group_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse)
);

-- Lectures --
CREATE TABLE Lectures(
  IdLecture INTEGER IDENTITY(1,1) NOT NULL,
  IdCourse INTEGER NOT NULL,
  IdTeacher INTEGER NOT NULL,
  LecuteDate DATE,
  CONSTRAINT Lecture_PK PRIMARY KEY (IdLecture),
  CONSTRAINT Lecture_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse),
  CONSTRAINT Lecture_IdTeacher_FK FOREIGN KEY (IdTeacher) REFERENCES Teachers(IdTeacher)
);

-- Attendance --
CREATE TABLE Attendance(
  IdAttendance INTEGER IDENTITY(1,1) NOT NULL,
  IdLecture INTEGER NOT NULL,
  IdStudent INTEGER NOT NULL,
  Attended BIT NOT NULL,
  CONSTRAINT Attendance_PK PRIMARY KEY (IdAttendance),
  CONSTRAINT Attendance_IdLecture_FK FOREIGN KEY (IdLecture) REFERENCES Lectures(IdLecture),
  CONSTRAINT Attendance_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdStudent),
  CONSTRAINT Attendance_UQ UNIQUE (IdLecture, IdStudent)
);

-- Grades --
CREATE TABLE Grades(
  IdGrade INTEGER IDENTITY(1,1) NOT NULL,
  IdStudent INTEGER NOT NULL,
  IdCourse INTEGER NOT NULL,
  Grade NUMERIC(2,1) NOT NULL,
  Date DATE NOT NULL,
  IdTeacher INTEGER NOT NULL,
  Comment VARCHAR(256),
  CONSTRAINT Grades_PK PRIMARY KEY (IdGrade),
  CONSTRAINT Grades_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdStudent),
  CONSTRAINT Grades_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse),
  CONSTRAINT Grades_IdTeacher_FK FOREIGN KEY (IdTeacher) REFERENCES Teachers(IdTeacher)
);

-- SL_CommentState --
CREATE TABLE SL_CommentState(
  IdState INTEGER,
  Name VARCHAR(64) NOT NULL,
  CONSTRAINT SL_CommentState_PK PRIMARY KEY (IdState)
);

-- Comments --
CREATE TABLE Comments(
  IdComment INTEGER IDENTITY(1,1) NOT NULL,
  IdStudent INTEGER NOT NULL,
  IdCourse INTEGER NOT NULL,
  Date DATE NOT NULL,
  Content VARCHAR(1024) NOT NULL,
  State INTEGER NOT NULL,
  CONSTRAINT Comments_PK PRIMARY KEY (IdComment),
  CONSTRAINT Comments_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdStudent),
  CONSTRAINT Comments_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse),
  CONSTRAINT Comments_StateFK FOREIGN KEY (State) REFERENCES SL_CommentState(IdState)
);

--needed by Identity.EntityFramework--

CREATE TABLE EF_UserRoles(
    UserId INTEGER NOT NULL,
    RoleId INTEGER NOT NULL,
    CONSTRAINT EF_UserRoles_PK PRIMARY KEY CLUSTERED (UserId ASC, RoleId ASC),
    CONSTRAINT EF_UserRoles_RoleId_FK FOREIGN KEY (RoleId) REFERENCES SL_UserType(IdUserType) ON DELETE CASCADE,
    CONSTRAINT EF_UserRoles_UserId_FK FOREIGN KEY (UserId) REFERENCES Users(IdUser) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX EF_UserRoles_UserId_IX
    ON EF_UserRoles(UserId ASC);
GO
CREATE NONCLUSTERED INDEX EF_UserRoles_RoleId_IX
    ON EF_UserRoles(RoleId ASC);

CREATE TABLE EF_UserClaims(
    Id INTEGER IDENTITY (1, 1) NOT NULL,
    UserId INTEGER NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT EF_UserClaims_PK PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT EF_UserClaims_UserId_FK FOREIGN KEY (UserId) REFERENCES Users(IdUser) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX EF_UserClaims_UserId_IX
    ON EF_UserClaims(UserId ASC);

CREATE TABLE EF_UserLogins(
    UserId INTEGER NOT NULL,
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey NVARCHAR(128) NOT NULL,
    CONSTRAINT EF_UserLogins_PK PRIMARY KEY CLUSTERED (UserId ASC, LoginProvider ASC, ProviderKey ASC),
    CONSTRAINT EF_UserLogins_UserId_FK FOREIGN KEY (UserId) REFERENCES Users(IdUser) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX EF_UserLogins_UserId_IX
    ON EF_UserLogins(UserId ASC);
