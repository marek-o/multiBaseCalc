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
        private char operation = '+';
        private double firstNumber = 0.0; //or result
        private double secondNumber = 0.0;
 
        private CalculationState state = CalculationState.Result;

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
            if ((k == '[' || k == ']')
                &&
                  (state == CalculationState.Result
                || state == CalculationState.EnteringFirst))
            {
                if (state == CalculationState.EnteringFirst)
                {
                    firstNumber = CommitEditedNumber();
                    state = CalculationState.Result;
                }

                int dir = k == ']' ? 1 : -1;
                @base = Math.Max(2, Math.Min(36, @base + dir));
                UpdateBaseLabel();

                view.SetNumber(BaseConverter.DoubleToString(firstNumber, @base));
            }

            if (k >= '0' && k <= '9' || k >= 'a' && k <= 'z' || k >= 'A' && k <= 'Z')
            {
                if (BaseConverter.CharToInt(k, @base) >= 0)
                {
                    if (state == CalculationState.Result)
                    {
                        state = CalculationState.EnteringFirst;
                    }
                    else if (state == CalculationState.EnteringOperation)
                    {
                        state = CalculationState.EnteringSecond;
                    }

                    editedNumber.Append(char.ToLower(k));
                    DisplayEditedNumber();
                }
            }

            if (k == '.' || k == ',')
            {
                if (!editedNumber.ToString().Any(i => i == '.' || i == ','))
                {
                    if (state == CalculationState.Result)
                    {
                        state = CalculationState.EnteringFirst;
                    }
                    else if (state == CalculationState.EnteringOperation)
                    {
                        state = CalculationState.EnteringSecond;
                    }

                    editedNumber.Append('.');
                    DisplayEditedNumber();
                }
            }

            if (k == (int)Keys.Escape)
            {
                editedNumber.Clear();
                DisplayEditedNumber();

                firstNumber = 0.0;
                secondNumber = 0.0;

                operation = '+';

                state = CalculationState.Result;
            }

            if (k == (int)Keys.Back)
            {
                if (state == CalculationState.EnteringFirst
                    || state == CalculationState.EnteringSecond)
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
                if (state == CalculationState.EnteringFirst)
                {
                    firstNumber = CommitEditedNumber();
                }
                else if (state == CalculationState.EnteringSecond)
                {
                    secondNumber = CommitEditedNumber();

                    firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                    view.SetNumber(BaseConverter.DoubleToString(firstNumber, @base));
                }

                state = CalculationState.EnteringOperation;
                operation = k;

                //DEBUG
                //Text = string.Format("{0} {1}", firstNumber, operation.ToString());
            }

            if (k == '=' || k == (int)Keys.Enter)
            {
                if (state == CalculationState.Result)
                {
                    //repeat
                }
                else if (state == CalculationState.EnteringFirst)
                {
                    //variant
                    firstNumber = CommitEditedNumber();
                }
                else if (state == CalculationState.EnteringOperation)
                {
                    //with itself
                    secondNumber = firstNumber;
                }
                else if (state == CalculationState.EnteringSecond)
                {
                    //normal calculation
                    secondNumber = CommitEditedNumber();
                }

                firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                view.SetNumber(BaseConverter.DoubleToString(firstNumber, @base));
                state = CalculationState.Result;
            }
        }

        private double CommitEditedNumber()
        {
            double res = BaseConverter.StringToDouble(editedNumber.ToString(), @base);
            editedNumber.Clear();
            return res;
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
