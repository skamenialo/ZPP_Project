using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZPP_Project.Models
{
    public class GenericViewModel
    {
        public string Title { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// Text to be displayed on the button; use null to hide button
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Href that button should lead to
        /// </summary>
        public string ButtonHref { get; set; }
    }
}