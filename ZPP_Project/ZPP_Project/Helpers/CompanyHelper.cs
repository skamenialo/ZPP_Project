using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZPP_Project.DataAccess;
using ZPP_Project.EntityDataModel;

namespace ZPP_Project.Helpers
{
    public static class CompanyHelper
    {
        public static string Display(int idCompany)
        {
            ZppContext context = new ZppContext();
            V_Company c;
            try
            {
                c = context.Companies.First(company => company.IdCompany == idCompany);
            }
            catch (InvalidOperationException)
            {
                return String.Format("Company {0} not found!", idCompany);
            }
            return c.Name;
        }

        public static string Display(V_Company c)
        {
            return c.Name;
        }
    }
}