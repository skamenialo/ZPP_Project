using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Helpers
{
    public class StudentHelper
    {
        public static string Display(int idStudent)
        {
            ZppContext context = new ZppContext();
            V_Student s;
            try
            {
                s = context.Students.First(student => student.IdStudent == idStudent);
            }
            catch (InvalidOperationException)
            {
                return String.Format("Teacher {0} not found!", idStudent);
            }
            return Display(s);
        }

        public static string Display(V_Student s)
        {
            return String.Format("{0} {1}", s.FirstName, s.LastName);
        }
    }
}