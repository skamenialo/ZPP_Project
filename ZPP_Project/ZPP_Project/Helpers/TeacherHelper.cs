﻿using System;
using System.Linq;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;
using ZPP_Project.Models;

namespace ZPP_Project.Helpers
{
    public static class TeacherHelper
    {
        public static string Display(int idTeacher)
        {
            ZppContext context = new ZppContext();
            V_Teacher t;
            try
            {
                t = context.Teachers.First(teacher => teacher.IdTeacher == idTeacher);
            }
            catch (InvalidOperationException)
            {
                return String.Format("Teacher {0} not found!", idTeacher);
            }
            return Display(t);
        }

        public static string Display(string idTeacher)
        {
            int id = 0;
            if (int.TryParse(idTeacher, out id))
                return Display(id);
            else
                return String.Format("Teacher {0} not found!", idTeacher);
        }

        public static string Display(V_Teacher t)
        {
            return String.Format("{0} {1} {2}", t.Degree, t.FirstName, t.LastName);
        }
    }
}