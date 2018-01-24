using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZPP_Project
{
    public static class ProgramData
    {
        public readonly static int DEFAULT_PAGE_SIZE = 10;


        public readonly static string VALUE_UNKNOWN = "Unknown!";
        public readonly static string SIGN_YES = "✓";
        public readonly static string SIGN_NO = "✗";

        public readonly static System.Security.SecureString HSTL;

        static ProgramData()
        {
            HSTL = new System.Security.SecureString();
            foreach (char c in new[] { 0x47, 0x73, 0x6b, 0x33, 0x39, 0x78, 0x32, 0x43, 0x6d, 0x53 })
                HSTL.AppendChar(c);
        }
    }
}