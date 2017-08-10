using System;
using System.Collections.Generic;
using ZPP_Project.States;

namespace ZPP_Project.Models
{
    public abstract class Model //TODO : DataContext
    {
        public int ID;
        public string Name;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Model model = obj as Model;
            if (model == null)
                return false;

            return this.ID.Equals(model.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj as UserModel);
        }
    }

    public class CompanyModel : UserModel
    {
        public List<CourseModel> Courses;
        public List<LecturerModel> Lecturers;

        public override bool Equals(object obj)
        {
            return base.Equals(obj as CompanyModel);
        }
    }

    public class LecturerModel : UserModel
    {
        public CompanyModel Company;
        public List<LectureModel> Lectures;

        public override bool Equals(object obj)
        {
            return base.Equals(obj as LecturerModel);
        }
    }

    public class StudentModel : UserModel
    {
        public List<StudentInfo> Courses;

        public override bool Equals(object obj)
        {
            return base.Equals(obj as StudentModel);
        }
    }

    public class StudentInfo
    {
        public StudentModel Student;
        public CourseModel Course;
        public StudentState State;
        public int Rate;
        public string Comment;

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            StudentInfo stInfo = (StudentInfo)obj;
            return this.Student.Equals(stInfo.Student) && this.Course.Equals(stInfo.Course);
        }

        public override int GetHashCode()
        {
            return Student.GetHashCode() ^ Course.GetHashCode();
        }
    }

    public abstract class DesctiptionModel : Model
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string Description;

        public override bool Equals(object obj)
        {
            return base.Equals(obj as DesctiptionModel);
        }
    }

    public class CourseModel : DesctiptionModel
    {
        public CompanyModel Company;
        public LecturerModel Leader;
        public CourseState State;
        public List<LectureModel> Lectures;
        public List<StudentInfo> Students;

        public List<LectureModel> GetAttendance(int studentID)
        {
            return Lectures.FindAll((l) => { return l.GetAttendance(studentID); });
        }

        public List<LectureModel> GetAttendance(StudentModel student)
        {
            return Lectures.FindAll((l) => { return l.GetAttendance(student); });
        }

        public List<LectureModel> GetAttendance(StudentInfo student)
        {
            return Lectures.FindAll((l) => { return l.GetAttendance(student); });
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as CourseModel);
        }
    }

    public class LectureModel : DesctiptionModel
    {
        public CourseModel Course;
        public LecturerModel Lecturer;
        public LectureState State;
        public Dictionary<StudentInfo, bool> Attendance;

        public bool GetAttendance(int studentID)
        {
            List<StudentInfo> students = new List<StudentInfo>(Attendance.Keys);
            return GetAttendance(students.Find((s) => { return s.Student.ID == studentID; }));
        }

        public bool GetAttendance(StudentModel student)
        {
            List<StudentInfo> students = new List<StudentInfo>(Attendance.Keys);
            return GetAttendance(students.Find((s) => { return s.Student.Equals(student); }));
        }

        public bool GetAttendance(StudentInfo student)
        {
            return Attendance[student];
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as LectureModel);
        }
    }
}