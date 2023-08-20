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

        private static double StringToInt(string s, int @base)
        {
            if (s.Length == 0)
            {
                return 0;
            }

            double output = 0;
            bool negative = false;

            if (s[0] == '-')
            {
                negative = true;
                s = s.Remove(0, 1);
            }

            for (int i = 0; i < s.Length; ++i)
            {
                output *= @base;
                var d = CharToInt(s[i], @base);
                if (d < 0) throw new Exception("invalid character");
                output += d;
            }

            if (negative)
            {
                output = -output;
            }

            return output;
        }

        private static string IntToString(double i, int @base)
        {
            if (@base < 2 || @base > 36)
            {
                throw new ArgumentException("base");
            }

            StringBuilder output = new StringBuilder();

            //FIXME what if i is over 2^52?
            i = Math.Floor(i);

            bool negative = false;
            if (i < 0)
            {
                negative = true;
                i = -i;
            }

            while (i > 0)
            {
                int d = (int)(i % @base);
                i = Math.Floor(i / @base);
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

        public static string DoubleToString(double i, int @base, int maxDigitCount)
        {
            if (i == double.PositiveInfinity) return "[+inf]";
            if (i == double.NegativeInfinity) return "[-inf]";
            if (double.IsNaN(i)) return "[NaN]";

            bool negative = i < 0;
            i = Math.Abs(i);
            double whole = Math.Floor(i);
            double frac = i % 1;

            string minus = negative ? "-" : "";

            //FIXME check for overflow?
            string strWhole = IntToString(whole, @base);

            string strFrac = FractionToString(frac, @base);

            var sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", minus, strWhole);

            bool hasDecimalPoint = false;
            if (!strFrac.Equals("0"))
            {
                hasDecimalPoint = true;
                sb.AppendFormat(".{0}", strFrac);
            }

            int digitCount = sb.Length;
            digitCount -= negative ? 1 : 0;
            digitCount -= hasDecimalPoint ? 1 : 0;

            if (digitCount <= maxDigitCount)
            {
                //no rounding needed
                return sb.ToString();
            }

            if (strWhole.Length <= maxDigitCount)
            {
                //FIXME what about rounding 99999999.999, after rounding length increases!

                //rounding needed
                var strFracTruncated = strFrac.Substring(0, maxDigitCount - strWhole.Length);
                var strFracRest = strFrac.Substring(maxDigitCount - strWhole.Length);
                var firstRestDigit = CharToInt(strFracRest[0], @base); //FIXME check if exists

                if (firstRestDigit >= @base / 2) //FIXME negative //FIXME odd bases
                {
                    //increment
                    //incrementing done by hand to avoid floating point errors

                    strFracTruncated = IncrementString(strFracTruncated, @base, out var stringWasResized);
                    if (stringWasResized)
                    {
                        strFracTruncated = strFracTruncated.Substring(1);
                        strWhole = IncrementString(strWhole, @base, out _);
                    }
                }

                sb.Clear();
                sb.AppendFormat("{0}{1}.{2}", minus, strWhole, strFracTruncated);
                return sb.ToString().TrimEnd('0').TrimEnd('.');
            }

            //overflow, cannot round to reduce
            return new string('#', maxDigitCount);
        }

        private static string IncrementString(string i, int @base, out bool stringWasResized)
        {
            var sb = new StringBuilder(i);
            stringWasResized = false;

            int carry = 1;
            for (int j = sb.Length - 1; j >= 0; j--)
            {
                var digit = CharToInt(sb[j], @base) + carry;
                var strDigit = IntToString(digit, @base);

                carry = (strDigit.Length > 1) ? 1 : 0;
                sb[j] = strDigit[strDigit.Length - 1];
            }

            if (carry > 0)
            {
                sb.Insert(0, '1');
                stringWasResized = true;
            }

            return sb.ToString();
        }
    }
}
