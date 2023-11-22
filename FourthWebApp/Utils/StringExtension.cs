using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FourthWebApp.Utils
{
    public static class StringExtension
    {
        public static string Left(this String input, int length)
        {
            return (input.Length <= length) ? input : input.Substring(0, length) + "...";
        }


        public static string ToProperCase(this String input)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(input.ToLower());
        }


        public static string ExtractHtmlInnerText(this String input, string replaceWith)
        {
            try
            {
                Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);
                string resultText = regex.Replace(input, replaceWith).Trim();
                return resultText;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string SafeSubstring(this string orig, int length)
        {
            return orig.Substring(0, orig.Length >= length ? length : orig.Length);
        }
    }
}