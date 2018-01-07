DROP VIEW V_UserLogin;
DROP VIEW V_UserInfo;
DROP VIEW V_Student;
DROP VIEW V_StudentInfo;
DROP VIEW V_Company;
DROP VIEW V_CompanyInfo;
DROP VIEW V_Teacher;
DROP VIEW V_TeacherInfo;
DROP VIEW V_Course;
DROP VIEW V_Group;
DROP VIEW V_Lecture;
DROP VIEW V_Attendance;
DROP VIEW V_Grade;
DROP VIEW V_Comment;

-- V_UserLogin --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_UserLogin')
    DROP VIEW V_UserLogin
GO
CREATE VIEW V_UserLogin AS
  SELECT IdUser, Login, PasswordHash
  FROM Users;
  GO
-- V_UserInfo --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_UserInfo')
    DROP VIEW V_UserInfo
GO
CREATE VIEW V_UserInfo AS
  SELECT IdUser, UserType, Login, Active, Banned
  FROM Users;
GO
-- V_Student --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Student')
    DROP VIEW V_Student
GO
CREATE VIEW V_Student AS
  SELECT s.IdStudent, u.IdUser, u.UserType, s.LastName, s.FirstName, s.Address, u.Email
  FROM Students s
  INNER JOIN Users u ON u.IdUser = s.IdUser;
GO
-- V_StudentInfo --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_StudentInfo')
    DROP VIEW V_StudentInfo
GO
CREATE VIEW V_StudentInfo AS
  SELECT IdStudent, LastName, FirstName, Address
  FROM Students;
GO
-- V_StudentData --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_StudentData')
    DROP VIEW V_StudentData
GO
CREATE VIEW V_StudentData AS
  SELECT IdStudent, IdUser, LastName, FirstName, Address
  FROM Students;
GO
-- V_Company --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Company')
    DROP VIEW V_Company
GO
CREATE VIEW V_Company AS
  SELECT c.IdCompany, u.IdUser, u.UserType, c.Name, c.Address, u.Email as EmailUser, c.Email as EmailCompany, c.Website, c.Description
  FROM Companies c
  INNER JOIN Users u ON u.IdUser = c.IdUser;
GO
-- V_CompanyInfo --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_CompanyInfo')
    DROP VIEW V_CompanyInfo
GO
CREATE VIEW V_CompanyInfo AS
  SELECT IdCompany, Name, Address, Email, Website, Description
  FROM Companies
GO
-- V_CompanyData --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_CompanyData')
    DROP VIEW V_CompanyData
GO
CREATE VIEW V_CompanyData AS
  SELECT IdCompany, IdUser, Name, Address, Email
  FROM Companies;
GO
-- V_Teacher --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Teacher')
    DROP VIEW V_Teacher
GO
CREATE VIEW V_Teacher AS
  SELECT t.IdTeacher, u.IdUser, u.UserType, t.IdCompany, t.LastName, t.FirstName, t.Address, u.Email, t.Degree, t.Website, t.Description
  FROM Teachers t
  INNER JOIN Users u ON u.IdUser = t.IdUser;
GO
-- V_TeacherInfo --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_TeacherInfo')
    DROP VIEW V_TeacherInfo
GO
CREATE VIEW V_TeacherInfo AS
  SELECT IdTeacher, IdCompany, LastName, FirstName, Address, Degree, Website, Description
  FROM Teachers;
GO
-- V_TeacherData --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_TeacherData')
    DROP VIEW V_TeacherData
GO
CREATE VIEW V_TeacherData AS
  SELECT IdTeacher, IdUser, IdCompany, LastName, FirstName, Address
  FROM Teachers;
GO
-- V_Course --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Course')
    DROP VIEW V_Course
GO
CREATE VIEW V_Course AS
  SELECT IdCourse, IdTeacher, IdCompany, Name, Lectures, State, Description, DateStart, DateEnd
  FROM Courses;
GO
-- V_Group --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Group')
    DROP VIEW V_Group
GO
CREATE VIEW V_Group AS
  SELECT IdGroup, IdStudent, IdCourse
  FROM Groups;
GO
-- V_Lecture --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Lecture')
    DROP VIEW V_Lecture
GO
CREATE VIEW V_Lecture AS
  SELECT IdLecture, IdCourse, IdTeacher
  FROM Lectures;
GO
-- V_Attendance --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Attendance')
    DROP VIEW V_Attendance
GO
CREATE VIEW V_Attendance AS
  SELECT IdAttendance, IdLecture, IdStudent, Attended
  FROM Attendance;
GO
-- V_Grade --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Grade')
    DROP VIEW V_Grade
GO
CREATE VIEW V_Grade AS
  SELECT IdGrade, IdStudent, IdCourse, Grade, Date, IdTeacher, Comment
  FROM Grades;
GO
-- V_Comment --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Comment')
    DROP VIEW V_Comment
GO
CREATE VIEW V_Comment AS
  SELECT IdComment, IdStudent, IdCourse, Date, Content, State
  FROM Comments;
GO