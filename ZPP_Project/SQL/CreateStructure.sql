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
DROP TABLE Users;
DROP TABLE SL_UserType;

-- ## CREATE ## --

-- SL_UserType --
CREATE TABLE SL_UserType(
  IdUserType INTEGER,
  Name VARCHAR(64) NOT NULL,
  CONSTRAINT SL_UserType_PK PRIMARY KEY (IdUserType)
);

-- Users --
CREATE TABLE Users(
  IdUser INTEGER,
  UserType INTEGER NOT NULL,
  Active BIT NOT NULL,
  Banned BIT,
  Login VARCHAR(64) NOT NULL,
  PasswordHash VARCHAR(256) NOT NULL,
  CONSTRAINT User_PK PRIMARY KEY (IdUser),
  CONSTRAINT User_UserType_FK FOREIGN KEY (UserType) REFERENCES SL_UserType(IdUserType),
  CONSTRAINT User_Login_UQ UNIQUE (Login),
);


-- Students --
CREATE TABLE Students(
  IdUser INTEGER NOT NULL,
  LastName VARCHAR(256) NOT NULL,
  FirstName VARCHAR(256) NOT NULL,
  Address VARCHAR(512) NOT NULL,
  Email VARCHAR(256) NOT NULL,
  CONSTRAINT Student_IdUser_FK FOREIGN KEY (IdUser) REFERENCES Users(IdUser),
  CONSTRAINT Student_IdUser_UQ UNIQUE (IdUser),
);

-- Companies --
CREATE TABLE Companies(
  IdUser INTEGER NOT NULL,
  Name VARCHAR(256) NOT NULL,
  Address VARCHAR(512) NOT NULL,
  Email VARCHAR(256) NOT NULL,
  Website VARCHAR(256),
  Description VARCHAR(4096),
  CONSTRAINT Company_IdUser_FK FOREIGN KEY (IdUser) REFERENCES Users(IdUser),
  CONSTRAINT Company_IdUser_UQ UNIQUE (IdUser)
);

-- Teachers --
CREATE TABLE Teachers(
  IdUser INTEGER NOT NULL,
  IdCompany INTEGER NOT NULL,
  LastName VARCHAR(256) NOT NULL,
  FirstName VARCHAR(256) NOT NULL,
  Address VARCHAR(512) NOT NULL,
  Email VARCHAR(256) NOT NULL,
  Degree VARCHAR(128),
  Website VARCHAR(256),
  Description VARCHAR(4096),
  CONSTRAINT Teacher_IdUser_FK FOREIGN KEY (IdUser) REFERENCES Users(IdUser),
  CONSTRAINT Teacher_IdUser_UQ UNIQUE (IdUser),
  CONSTRAINT Teacher_IdCompany_FK FOREIGN KEY (IdCompany) REFERENCES Companies(IdUser)
);

-- SL_CourseStates --
CREATE TABLE SL_CourseStates(
  IdState INTEGER,
  Name VARCHAR(64) NOT NULL,
  CONSTRAINT SL_CourseStates_PK PRIMARY KEY (IdState)
);

-- Courses --
CREATE TABLE Courses(
  IdCourse INTEGER,
  IdTeacher INTEGER NOT NULL,
  IdCompany INTEGER NOT NULL,
  Name VARCHAR(256) NOT NULL,
  Lectures Integer NOT NULL,
  State INTEGER NOT NULL,
  Description VARCHAR(4096),
  DateStart DATE,
  DateEnd DATE,
  CONSTRAINT Course_PK PRIMARY KEY (IdCourse),
  CONSTRAINT Course_IdTeacher_FK FOREIGN KEY (IdTeacher) REFERENCES Teachers(IdUser),
  CONSTRAINT Course_IdCompany_FK FOREIGN KEY (IdCompany) REFERENCES Companies(IdUser),
  CONSTRAINT Course_State_FK FOREIGN KEY (State) REFERENCES SL_CourseStates(IdState)
);

-- Groups --
CREATE TABLE Groups(
  IdGroup INTEGER,
  IdStudent INTEGER NOT NULL,
  IdCourse INTEGER NOT NULL,
  CONSTRAINT Group_PK PRIMARY KEY (IdGroup),
  CONSTRAINT Group_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdUser),
  CONSTRAINT Group_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse)
);

-- Lectures --
CREATE TABLE Lectures(
  IdLecture INTEGER,
  IdCourse INTEGER NOT NULL,
  IdTeacher INTEGER NOT NULL,
  CONSTRAINT Lecture_PK PRIMARY KEY (IdLecture),
  CONSTRAINT Lecture_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse),
  CONSTRAINT Lecture_IdTeacher_FK FOREIGN KEY (IdTeacher) REFERENCES Teachers(IdUser)
);

-- Attendance --
CREATE TABLE Attendance(
  IdLecture INTEGER NOT NULL,
  IdStudent INTEGER NOT NULL,
  Attended BIT NOT NULL,
  CONSTRAINT Attendance_IdLecture_FK FOREIGN KEY (IdLecture) REFERENCES Lectures(IdLecture),
  CONSTRAINT Attendance_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdUser),
  CONSTRAINT Attendance_UQ UNIQUE (IdLecture, IdStudent)
);

-- Grades --
CREATE TABLE Grades(
  IdGrade INTEGER,
  IdStudent INTEGER NOT NULL,
  IdCourse INTEGER NOT NULL,
  Grade NUMERIC(2,1) NOT NULL,
  Date DATE NOT NULL,
  IdTeacher INTEGER NOT NULL,
  Comment VARCHAR(256),
  CONSTRAINT Grades_PK PRIMARY KEY (IdGrade),
  CONSTRAINT Grades_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdUser),
  CONSTRAINT Grades_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse),
  CONSTRAINT Grades_IdTeacher_FK FOREIGN KEY (IdTeacher) REFERENCES Teachers(IdUser)
);

-- SL_CommentState --
CREATE TABLE SL_CommentState(
  IdState INTEGER,
  Name VARCHAR(64) NOT NULL,
  CONSTRAINT SL_CommentState_PK PRIMARY KEY (IdState)
);

-- Comments --
CREATE TABLE Comments(
  IdComment INTEGER,
  IdStudent INTEGER NOT NULL,
  IdCourse INTEGER NOT NULL,
  Date DATE NOT NULL,
  Content VARCHAR(1024) NOT NULL,
  State INTEGER NOT NULL,
  CONSTRAINT Comments_PK PRIMARY KEY (IdComment),
  CONSTRAINT Comments_IdStudent_FK FOREIGN KEY (IdStudent) REFERENCES Students(IdUser),
  CONSTRAINT Comments_IdCourse_FK FOREIGN KEY (IdCourse) REFERENCES Courses(IdCourse),
  CONSTRAINT Comments_StateFK FOREIGN KEY (State) REFERENCES SL_CommentState(IdState)
);