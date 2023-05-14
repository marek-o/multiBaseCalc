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
        private char operation = '\0';
        private double firstNumber = 0.0;

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
            if (k >= '0' && k <= '9' || k >= 'a' && k <= 'z' || k >= 'A' && k <= 'Z')
            {
                if (BaseConverter.CharToInt(k, @base) >= 0)
                {
                    editedNumber.Append(char.ToLower(k));
                    DisplayEditedNumber();
                }
            }

            if (k == '.' || k == ',')
            {
                editedNumber.Append('.');
                DisplayEditedNumber();
            }

            if (k == (int)Keys.Escape)
            {
                editedNumber.Clear();
                DisplayEditedNumber();
            }

            if (k == (int)Keys.Back)
            {
                if (editedNumber.Length >= 1)
                {
                    editedNumber.Remove(editedNumber.Length - 1, 1);
                }
                DisplayEditedNumber();
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
