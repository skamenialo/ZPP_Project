using System;
using System.Collections.Generic;
using ZPP_Project.States;

namespace ZPP_Project.Models
{
    public class Model //TODO : DataContext
    {
        public int ID;
        public string Name;
    }

    public class UserModel : Model
    {
        public UserState State;
        public string LastName;
        public string NickName;
        public string Email;

        public bool SetPassword(string password)
        {
            //TODO ustawianie hasła (sprawdzić zaimplementowane domyślnie Modele)
            throw new NotImplementedException();
        }

        public bool CheckPassword(string password)
        {
            //TODO sprawdzanie hasla (sprawdzić zaimplementowane domyślnie Modele)
            throw new NotImplementedException();
        }
    }

    public class CompanyModel : UserModel
    {
        public IList<CourseModel> Courses;
        public IList<LecturerModel> Lecturers;
    }

    public class LecturerModel : UserModel
    {
        public CompanyModel Company;
        public IList<LectureModel> Lectures;
    }

    public class StudentModel : UserModel
    {
        public IList<StudentInfo> Courses;
    }

    public class StudentInfo
    {
        public StudentModel Student;
        public CourseModel Course;
        public StudentState State;
        public int Rate;
        public string Comment;
    }

    public class DesctiptionModel : Model
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string Description;
    }

    public class CourseModel : DesctiptionModel
    {
        public CompanyModel Company;
        public LecturerModel Leader;
        public CourseState State;
        public IList<LectureModel> Lectures;
        public IList<StudentInfo> Students;

        public IList<LectureModel> GetAttendance(int studentID)
        {
            //TODO pobranie obecności dla konkretnego studenta
            throw new NotImplementedException();
        }
    }

    public class LectureModel : DesctiptionModel
    {
        public CompanyModel Company;
        public LecturerModel Lecturer;
        public LectureState State;
        public IList<StudentInfo> Attendance;
    }
}