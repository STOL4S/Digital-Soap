using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSoap
{
    public static class StringExtensions
    {
        public static bool IsLowercase(this string _Input)
        {
            List<string> LowercaseLetters = new List<string>()
            {
                "a", "b", "c", "d", "e", "f", "g", "h",
                "i", "j", "k", "l", "m", "n", "o", "p",
                "q", "r", "s", "t", "u", "v", "w", "x",
                "y", "z"
            };

            for (int i = 0; i < LowercaseLetters.Count; i++)
            {
                if (_Input == LowercaseLetters[i])
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsUppercase(this string _Input)
        {
            List<string> UppercaseLetters = new List<string>()
            {
                "A", "B", "C", "D", "E", "F", "G", "H",
                "I", "J", "K", "L", "M", "N", "O", "P",
                "Q", "R", "S", "T", "U", "V", "W", "X",
                "Y", "Z"
            };

            for (int i = 0; i < UppercaseLetters.Count; i++)
            {
                if (_Input == UppercaseLetters[i])
                {
                    return true;
                }
            }

            return false;
        }

        //CAPITALIZES THE FIRST LETTER OF THE STRING
        public static string CapitalizeFirst(this string _Input)
        {
            int InitialLength = _Input.Length;
            string FP = _Input.Substring(0, 1).ToUpper();
            FP += _Input.Substring(1, InitialLength - 1);

            return FP;
        }
    }
}
