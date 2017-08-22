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
  SELECT IdUser, UserType, Active, Banned
  FROM Users;
GO
-- V_Students --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Students')
    DROP VIEW V_Students
GO
CREATE VIEW V_Students AS
  SELECT u.IdUser, u.UserType, s.LastName, s.FirstName, s.Address, s.Email
  FROM Users u
  INNER JOIN Students s ON u.IdUser = s.IdUser;
GO
-- V_Companies --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Companies')
    DROP VIEW V_Companies
GO
CREATE VIEW V_Companies AS
  SELECT IdUser, Name, Address, Email, Website, Description
  FROM Companies;
GO
-- V_Teachers --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Teachers')
    DROP VIEW V_Teachers
GO
CREATE VIEW V_Teachers AS
  SELECT u.IdUser, u.UserType, t.LastName, t.FirstName, t.Address, t.Email
  FROM Users u
  INNER JOIN Teachers t ON u.IdUser = t.IdUser;
GO
-- V_Courses --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Courses')
    DROP VIEW V_Courses
GO
CREATE VIEW V_Courses AS
  SELECT IdCourse, IdTeacher, IdCompany, Name, Lectures, State, Description, DateStart, DateEnd
  FROM Courses;
GO
-- V_Groups --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Groups')
    DROP VIEW V_Groups
GO
CREATE VIEW V_Groups AS
  SELECT IdGroup, IdStudent, IdCourse
  FROM Groups;
GO
-- V_Lectures --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Lectures')
    DROP VIEW V_Lectures
GO
CREATE VIEW V_Lectures AS
  SELECT IdLecture, IdCourse, IdTeacher
  FROM Lectures;
GO
-- V_Attendance --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Attendance')
    DROP VIEW V_Attendance
GO
CREATE VIEW V_Attendance AS
  SELECT IdLecture, IdStudent, Attended
  FROM Attendance;
GO
-- V_Grades --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Grades')
    DROP VIEW V_Grades
GO
CREATE VIEW V_Grades AS
  SELECT IdGrade, IdStudent, IdCourse, Grade, Date, IdTeacher, Comment
  FROM Grades;
GO
-- V_Comments --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Comments')
    DROP VIEW V_Comments
GO
CREATE VIEW V_Comments AS
  SELECT IdComment, IdStudent, IdCourse, Date, Content, State
  FROM Comments;
GO