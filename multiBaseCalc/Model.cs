using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace multiBaseCalc
{
    public class Model
    {
        private Form1 view = null;

        private int @base = 10;

        private StringBuilder editedNumber = new StringBuilder();
        private char operation = '\0';
        private double firstNumber = 0.0;

        public void SetView(Form1 view)
        {
            if (this.view != null)
            {
                throw new InvalidOperationException();
            }

            this.view = view;
            this.view.KeyPressed += View_KeyPressed;

            view.SetNumber("0");
            UpdateBaseLabel();
        }

        private void View_KeyPressed(char k)
        {
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

                firstNumber = BaseConverter.StringToDouble(editedNumber.ToString(), @base);
                editedNumber.Clear();

                //DEBUG
                //Text = string.Format("{0} {1}", firstNumber, operation.ToString());
            }

            if (k == '=' || k == (int)Keys.Enter)
            {
                if (operation != 0)
                {
                    double secondNumber;

                    secondNumber = BaseConverter.StringToDouble(editedNumber.ToString(), @base);
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

                    view.SetNumber(BaseConverter.DoubleToString(result, @base));

                    operation = '\0';
                }
            }
        }

        private void UpdateEditedNumber()
        {
            if (editedNumber.Length == 0)
            {
                view.SetNumber("0");
            }
            else
            {
                view.SetNumber(editedNumber.ToString());
            }
        }

        private void UpdateBaseLabel()
        {
            view.SetBaseLabel(string.Format("base {0}", @base));
        }
    }
}
