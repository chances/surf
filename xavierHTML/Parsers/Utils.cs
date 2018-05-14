using System;

namespace xavierHTML.Parsers
{
    public class Utils
    {
        public static bool IsLatinLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        
        public static bool IsArabicNumeral(char c)
        {
            return (c >= '0' && c <= '9');
        }
    }
}
