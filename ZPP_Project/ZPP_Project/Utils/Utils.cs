using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ZPP_Project
{
    public static class Utils
    {
        public static string SanitizeHtml(string inputHtml)
        {
            if (String.IsNullOrEmpty(inputHtml))
                return String.Empty;
            var regex = new Regex(
                "(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)",
                RegexOptions.Singleline | RegexOptions.IgnoreCase
                );
            return regex.Replace(inputHtml.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "), "");
        }

        public static string GetDisplayName<Type>(string member)
        {
            var type = typeof(Type);
            var memInfo = type.GetMember(member);
            if (memInfo == null || memInfo.Length == 0)
                return member;
            var attributes = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
            if (attributes == null || attributes.Length == 0)
                return member;
            var displayAttribute = attributes[0] as System.ComponentModel.DataAnnotations.DisplayAttribute;
            if (displayAttribute == null)
                return member;
            return displayAttribute.Name;
        }
    }
}