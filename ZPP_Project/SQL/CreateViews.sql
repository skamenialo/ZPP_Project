-- V_Courses --
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
        WHERE TABLE_NAME = 'V_Courses')
    DROP VIEW V_Courses
GO
CREATE VIEW V_Courses AS
  SELECT IdCourse, IdTeacher, IdCompany, Name, Lectures, State, Description, DateStart, DateEnd
  FROM Courses;

  GO
