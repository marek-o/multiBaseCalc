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
        private IView view = null;

        private int @base = 10;

        private StringBuilder editedNumber = new StringBuilder();
        private double lastResult = 0.0;
        private char operation = '\0';
        private double firstNumber = 0.0;
        private bool resultMode = false;

        public void SetView(IView view)
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
            if (k == '[' || k == ']')
            {
                double num;
                if (resultMode)
                {
                    num = lastResult;
                }
                else
                {
                    num = BaseConverter.StringToDouble(editedNumber.ToString(), @base);
                }

                int dir = k == ']' ? 1 : -1;
                @base = Math.Max(2, Math.Min(36, @base + dir));
                UpdateBaseLabel();

                var newStr = BaseConverter.DoubleToString(num, @base);
                view.SetNumber(newStr);
                editedNumber.Clear();
                editedNumber.Append(newStr);
                
                lastResult = num;
                resultMode = true;
            }

            if (k >= '0' && k <= '9' || k >= 'a' && k <= 'z' || k >= 'A' && k <= 'Z')
            {
                if (BaseConverter.CharToInt(k, @base) >= 0)
                {
                    editedNumber.Append(char.ToLower(k));
                    DisplayEditedNumber();

                    resultMode = false;
                }
            }

            if (k == '.' || k == ',')
            {
                if (!editedNumber.ToString().Any(i => i == '.' || i == ','))
                {
                    editedNumber.Append('.');
                    DisplayEditedNumber();

                    resultMode = false;
                }
            }

            if (k == (int)Keys.Escape)
            {
                editedNumber.Clear();
                DisplayEditedNumber();
                lastResult = 0.0;
                resultMode = true;
            }

            if (k == (int)Keys.Back)
            {
                if (!resultMode)
                {
                    if (editedNumber.Length >= 1)
                    {
                        editedNumber.Remove(editedNumber.Length - 1, 1);
                    }
                    DisplayEditedNumber();
                }
            }

            if (k == '*' || k == '/' || k == '+' || k == '-')
            {
                if (operation.Equals('\0'))
                {
                    if (resultMode)
                    {
                        firstNumber = lastResult;
                    }
                    else
                    {
                        firstNumber = BaseConverter.StringToDouble(editedNumber.ToString(), @base);
                    }
                    editedNumber.Clear();

                    resultMode = false;
                }

                operation = k;

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

                    double result = PerformOperation(operation, firstNumber, secondNumber);

                    view.SetNumber(BaseConverter.DoubleToString(result, @base));

                    lastResult = result;
                    resultMode = true;

                    operation = '\0';
                }
                else
                {
                    //else set resultMode to true?
                }
            }
        }

        private double PerformOperation(char operation, double lhs, double rhs)
        {
            if (operation == '+') return lhs + rhs;
            if (operation == '-') return lhs - rhs;
            if (operation == '*') return lhs * rhs;
            if (operation == '/') return lhs / rhs;

            throw new ArgumentException("invalid operation");
        } 

        private void DisplayEditedNumber()
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
