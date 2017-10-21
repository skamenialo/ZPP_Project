using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZPP_Project.Helpers
{
    public static class HtmlHelperZpp
    {
        public static MvcHtmlString RenderColapseIcon(string regionName)
        {
            MvcHtmlString result = new MvcHtmlString("<i class=\"fa fa-angle-right\" aria-hidden=\"true\" id=\"" + regionName + "_toggle\" onclick=\"ToggleVisibilityState('" + regionName + "')\"></i>");
            //onclick="ToggleVisibilityState(regionName)"function() {$( "regionName" ).slideToggle( "slow"} )
            //<i class="fa fa-angle-down" aria-hidden="true"></i>ToggleVisibilityState(regionName)

            return result;
        }
    }
}