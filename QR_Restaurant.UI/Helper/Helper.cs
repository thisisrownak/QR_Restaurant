using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Helper
{
    public class Helper
    {
        public static string ToTitleCase(string str)
        {
            if (str.Contains(" "))
            {
                string[] strings = str.Split(" ");
                str = String.Empty;
                for (int i = 0; i < strings.Length ; i++)
                {
                    strings[i] = strings[i].Substring(0, 1).ToUpper() + strings[i].Substring(1, strings[i].Length -1).ToLower();
                    str += $"{strings[i]} ";
                }
                return str;
            }
            return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length -1).ToLower();
        }
    }
}
