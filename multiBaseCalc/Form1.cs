﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multiBaseCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "0";
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private int @base = 10;


        private StringBuilder editedNumber = new StringBuilder();
        private char operation = '\0';
        private double firstNumber = 0.0;

        private void UpdateEditedNumber()
        {
            if (editedNumber.Length == 0)
            {
                label1.Text = "0";
            }
            else
            {
                label1.Text = editedNumber.ToString();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char k = e.KeyChar;

            if (k >= '0' && k <= '9' || k >= 'a' && k <= 'z' || k >= 'A' && k <= 'Z')
            {
                char kLower = char.ToLower(k);
                int d = 0;

                if (kLower <= '9') d = kLower - '0';
                else d = kLower - 'a' + 10;

                if (d < @base)
                {
                    editedNumber.Append(kLower);
                    UpdateEditedNumber();
                }
            }

            if (k == '.' || k == ',')
            {
                editedNumber.Append('.');
                UpdateEditedNumber();
            }

            if (k == (int)Keys.Escape)
            {
                editedNumber.Clear();
                UpdateEditedNumber();
            }

            if (k == (int)Keys.Back)
            {
                if (editedNumber.Length >= 1)
                {
                    editedNumber.Remove(editedNumber.Length - 1, 1);
                }
                UpdateEditedNumber();
            }

            if (k == '*' || k == '/' || k == '+' || k == '-')
            {
                operation = k;

                firstNumber = StringToDouble(editedNumber.ToString(), @base);
                //if (!double.TryParse(editedNumber.ToString(), out firstNumber))//
                //{
                //    firstNumber = 0.0;
                //}
                editedNumber.Clear();

                //DEBUG
                Text = string.Format("{0} {1}", firstNumber, operation.ToString());
            }

            if (k == '=' || k == (int)Keys.Enter)
            {
                if (operation != 0)
                {
                    double secondNumber;

                    secondNumber = StringToDouble(editedNumber.ToString(), @base);
                    //if (!double.TryParse(editedNumber.ToString(), out secondNumber))//
                    //{
                    //    secondNumber = 0.0;
                    //}
                    editedNumber.Clear();

                    double result = 0.0;
                    if (operation == '+')
                    {
                        result = firstNumber + secondNumber;
                    }
                    else if (operation == '-')
                    {
                        result = firstNumber - secondNumber;
                    }
                    else if (operation == '*')
                    {
                        result = firstNumber * secondNumber;
                    }
                    else if (operation == '/')
                    {
                        result = firstNumber / secondNumber;
                    }


                    label1.Text = result.ToString();//
                    //FIXME make floating point
                    //label1.Text = IntToString((int)result, @base);
                    label1.Text = DoubleToString(result, @base);

                    operation = '\0';
                }
            }

            //if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.OemMinus)
        }

        public double StringToDouble(string s, int @base)
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

        private int CharToInt(char k, int @base)
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

        private int StringToInt(string s, int @base)
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

        private string IntToString(int i, int @base)
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

        private string FractionToString(double i, int @base)
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

        public string DoubleToString(double i, int @base)
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
