using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZPP_Project.Helpers
{
    public static class ZPPHtmlHelper
    {
        public static MvcHtmlString RenderColapseIcon(string regionName)
        {
            MvcHtmlString result = new MvcHtmlString("<span class=\"fa fa-angle-right visibility-toggle-button\" data-toggle=\"collapse\" id=\"" + regionName + "_toggle\" data-target=\"#" + regionName + "\"></span>");
            return result;
        }
    }
}