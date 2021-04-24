using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ShadeMsg.Security
{
    class DataValidation
    {
        public static bool CheckLenght(string data, int minLenght, int maxLenght)
        {
            return ((data.Length > minLenght) && (data.Length < maxLenght));
        }

        public static bool RegexCheck(string data, string regex)
        {
            return new Regex(regex).IsMatch(data);
        }
    }
}
