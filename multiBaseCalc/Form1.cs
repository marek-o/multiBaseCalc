using System;
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
        private Model model;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "0";
            UpdateBaseLabel();

            model = new Model();
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

        private void UpdateBaseLabel()
        {
            labelBase.Text = string.Format("base {0}", @base);
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

                firstNumber = model.StringToDouble(editedNumber.ToString(), @base);
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

                    secondNumber = model.StringToDouble(editedNumber.ToString(), @base);
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
                    label1.Text = model.DoubleToString(result, @base);

                    operation = '\0';
                }
            }

            //if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.OemMinus)
        }
    }
}
