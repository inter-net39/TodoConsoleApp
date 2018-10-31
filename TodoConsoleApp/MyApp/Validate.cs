using System;
using System.Text.RegularExpressions;

namespace MyApp
{
    public static class Validate
    {
        /// <summary>
        ///     dd/mm/yyyy,dd-mm-yyyy or dd.mm.yyyy
        /// </summary>
        private static Regex _regexDate = new Regex(@"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$");
        private static Regex _regexInteger = new Regex(@"^(0|[1-9][0-9]*)$");

        public static bool MatchDate(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Match match = _regexDate.Match(text);
                if (match.Success)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool MatchBool(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (text.ToLower() == "true" || text.ToLower() == "false")
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool MatchInteger(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Match match = _regexInteger.Match(text);
                if (match.Success)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        
    }
}