using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multiBaseCalc
{
    public class BaseConverter
    {
        private static string digits = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static double StringToDouble(string s, int @base)
        {
            //FIXME disallow of even entering second decimal point (earlier)
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

            var whole = StringToInt(substrings[0], @base);
            var frac = StringToInt(substrings[1], @base);
            double fracD = Math.Pow(@base, substrings[1].Length);

            return (whole + (frac / fracD)) * (negative ? -1 : 1);
        }

        public static long CharToInt(char k, int @base)
        {
            char kLower = char.ToLower(k);
            var d = 0;

            if (kLower <= '9') d = kLower - '0';
            else d = kLower - 'a' + 10;

            if (d >= 0 && d < @base)
            {
                return d;
            }

            return -1;
        }

        private static long StringToInt(string s, int @base)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            long output = 0;
            bool negative = false;

            if (s[0] == '-')
            {
                negative = true;
                s = s.Remove(0, 1);
            }

            try
            {
                for (int i = 0; i < s.Length; ++i)
                {
                    checked
                    {
                        output *= @base;
                        var d = CharToInt(s[i], @base);
                        if (d < 0) throw new Exception("invalid character");
                        output += d;
                    }
                }
            }
            catch (OverflowException)
            {
                output = long.MaxValue; //FIXME?
            }

            if (negative)
            {
                output = -output;
            }

            return output;
        }

        private static string IntToString(long i, int @base)
        {
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
                int d = (int)(i % @base);
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
            if (@base < 2 || @base > 36)
            {
                throw new ArgumentException("base");
            }

            StringBuilder output = new StringBuilder();

            if (i >= 1.0)
            {
                throw new ArgumentException("not a fraction");
            }

            if (i < 0)
            {
                throw new ArgumentException("negative number not supported");
            }

            int iteration = 0;

            //pass max iteration count as parameter?
            while (i > 0 && iteration < 1000) //FIXME rounding errors
            {
                i *= @base;
                int d = (int)Math.Floor(i);
                output.Append(digits[d]);
                i -= d;
                iteration++;
            }

            if (output.Length == 0)
            {
                output.Append('0');
            }

            return output.ToString();
        }

        public static string DoubleToString(double i, int @base)
        {
            //FIXME inf, nan?
            bool negative = i < 0;
            i = Math.Abs(i);
            double whole = Math.Floor(i);
            double frac = i % 1;

            string minus = negative ? "-" : "";

            //FIXME check for overflow?
            string strWhole = IntToString((long)whole, @base);

            string strFrac = FractionToString(frac, @base);

            var sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", minus, strWhole);

            if (!strFrac.Equals("0"))
            {
                sb.AppendFormat(".{0}", strFrac);
            }
            return sb.ToString();
        }
    }
}
