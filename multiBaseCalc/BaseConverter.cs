using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multiBaseCalc
{
    public class BaseConverter
    {
        public static double StringToDouble(string s, int @base)
        {
            var substrings = s.Split(new char[] { '.', ',' }, 2);
            if (substrings.Length < 2)
            {
                return StringToInt(s, @base);
            }

            bool negative = false;
            if (substrings[0].StartsWith('-'))
            {
                negative = true;
                substrings[0] = substrings[0].Substring(1);
            }

            int whole = StringToInt(substrings[0], @base);
            int frac = StringToInt(substrings[1], @base);
            double fracD = Math.Pow(@base, substrings[1].Length);

            return (whole + (frac / fracD)) * (negative ? -1 : 1);
        }

        public static int CharToInt(char k, int @base)
        {
            char kLower = char.ToLower(k);
            int d = 0;

            if (kLower <= '9') d = kLower - '0';
            else d = kLower - 'a' + 10;

            if (d < @base)
            {
                return d;
            }

            return -1;//FIXME
        }

        private static int StringToInt(string s, int @base)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            int output = 0;
            bool negative = false;

            if (s[0] == '-')
            {
                negative = true;
                s = s.Remove(0, 1);//FIXME?
            }

            for (int i = 0; i < s.Length; ++i)
            {
                output *= @base;
                int d = CharToInt(s[i], @base);
                output += d;
            }

            if (negative)
            {
                output = -output;
            }

            return output;
        }

        private static string IntToString(int i, int @base)
        {
            string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

            if (@base < 2 || @base > 36)
            {
                throw new ArgumentException("base");
            }

            StringBuilder output = new StringBuilder();

            bool negative = false;
            if (i < 0)
            {
                negative = true;
                i = -i;
            }

            while (i > 0)
            {
                int d = i % @base;
                i /= @base;
                output.Append(digits[d]);
            }

            if (output.Length == 0)
            {
                output.Append('0');
            }

            if (negative)
            {
                output.Append('-');
            }

            return new string(output.ToString().Reverse().ToArray());
        }

        private static string FractionToString(double i, int @base)
        {
            string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

            if (@base < 2 || @base > 36)
            {
                throw new ArgumentException("base");
            }

            StringBuilder output = new StringBuilder();

            if (i < 0)
            {
                throw new ArgumentException("negative number not supported");
                //negative = true;
                //i = -i;
            }

            while (i > 0) //FIXME rounding errors, max iteration count
            {
                i *= @base;
                int d = (int)Math.Floor(i);
                output.Append(digits[d]);
                i -= d;
            }

            if (output.Length == 0)
            {
                output.Append('0');
            }

            return output.ToString();
        }

        public static string DoubleToString(double i, int @base)
        {
            bool negative = i < 0;
            i = Math.Abs(i);
            double whole = Math.Floor(i);
            double frac = i % 1; //FIXME negative

            string minus = negative ? "-" : "";

            string strWhole = IntToString((int)whole, @base);

            string strFrac = FractionToString(frac, @base);

            return string.Format("{0}{1}.{2}", minus, strWhole, strFrac);
            //FIXME
            //return IntToString((int)i, @base);
        }
    }
}
