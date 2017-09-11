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
-- V_Student --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Student')
    DROP VIEW V_Student
GO
CREATE VIEW V_Student AS
  SELECT u.IdUser, u.UserType, s.LastName, s.FirstName, s.Address, u.Email
  FROM Users u
  INNER JOIN Students s ON u.IdUser = s.IdUser;
GO
-- V_Company --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Company')
    DROP VIEW V_Company
GO
CREATE VIEW V_Company AS
  SELECT IdUser, Name, Address, Email, Website, Description
  FROM Companies;
GO
-- V_Teacher --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Teacher')
    DROP VIEW V_Teacher
GO
CREATE VIEW V_Teacher AS
  SELECT u.IdUser, u.UserType, t.IdCompany, t.LastName, t.FirstName, t.Address, u.Email, t.Degree, t.Website, t.Description
  FROM Users u
  INNER JOIN Teachers t ON u.IdUser = t.IdUser;
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
  SELECT IdLecture, IdStudent, Attended
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