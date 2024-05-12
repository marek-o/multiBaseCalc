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

        private int maxNumberOfDigits;

        private int @base = 10;

        private StringBuilder editedNumber = new StringBuilder();
        private Key operation = Key.Add;
        private double firstNumber = 0.0; //or result
        private double secondNumber = 0.0;
 
        private CalculationState state = CalculationState.Result;

        public Model(int maxNumberOfDigits = 15)
        {
            this.maxNumberOfDigits = maxNumberOfDigits;
        }

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

        private void View_KeyPressed(Key k)
        {
            if ((k == Key.DecrementBase
                || k == Key.IncrementBase
                || k == Key.Base2
                || k == Key.Base8
                || k == Key.Base10
                || k == Key.Base16)
                &&
                  (state == CalculationState.Result
                || state == CalculationState.EnteringFirst))
            {
                if (state == CalculationState.EnteringFirst)
                {
                    firstNumber = CommitEditedNumber();
                    state = CalculationState.Result;
                }

                switch (k)
                {
                    case Key.IncrementBase:
                        @base = Math.Max(2, Math.Min(36, @base + 1)); break;
                    case Key.DecrementBase:
                        @base = Math.Max(2, Math.Min(36, @base - 1)); break;
                    case Key.Base2: @base = 2; break;
                    case Key.Base8: @base = 8; break;
                    case Key.Base10: @base = 10; break;
                    case Key.Base16: @base = 16; break;
                }

                UpdateBaseLabel();
                DisplayResult();
            }

            if (k >= Key.D0 && k <= Key.Z)
            {
                char ch = BaseConverter.KeyToChar(k, @base);
                if (ch != '\0')
                {
                    if (state == CalculationState.Result)
                    {
                        state = CalculationState.EnteringFirst;
                    }
                    else if (state == CalculationState.EnteringOperation)
                    {
                        state = CalculationState.EnteringSecond;
                    }

                    string editedNumberStr = editedNumber.ToString();
                    int editedDigitCount = editedNumberStr.Length;
                    if (editedNumberStr.Contains('.')) //FIXME DRY
                    {
                        editedDigitCount--;

                        if (editedNumberStr.StartsWith('.'))
                        {
                            //add implied "0"
                            editedDigitCount++;
                        }
                    }

                    if ((k != Key.D0 || editedNumber.Length > 0)
                        && editedDigitCount < maxNumberOfDigits)
                    {
                        editedNumber.Append(ch);
                    }
                    DisplayEditedNumber();
                }
            }

            if (k == Key.Period)
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

            if (k == Key.Escape)
            {
                editedNumber.Clear();
                DisplayEditedNumber();

                firstNumber = 0.0;
                secondNumber = 0.0;

                operation = Key.Add;

                state = CalculationState.Result;
            }

            if (k == Key.Backspace)
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

            if (k == Key.Add || k == Key.Subtract || k == Key.Multiply || k == Key.Divide)
            {
                if (state == CalculationState.EnteringFirst)
                {
                    firstNumber = CommitEditedNumber();
                }
                else if (state == CalculationState.EnteringSecond)
                {
                    secondNumber = CommitEditedNumber();

                    firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                    DisplayResult();
                }

                state = CalculationState.EnteringOperation;
                operation = k;
            }

            if (k == Key.Sqrt || k == Key.Cos)
            {
                if (state == CalculationState.EnteringFirst)
                {
                    firstNumber = CommitEditedNumber();
                }
                else if (state == CalculationState.EnteringSecond)
                {
                    secondNumber = CommitEditedNumber();

                    firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                }

                firstNumber = PerformOperation(k, firstNumber, 0.0);
                DisplayResult();

                state = CalculationState.Result;
            }

            if (k == Key.PiConstant || k == Key.EConstant)
            {
                if (state == CalculationState.Result)
                {
                    state = CalculationState.EnteringFirst;
                }
                else if (state == CalculationState.EnteringOperation)
                {
                    state = CalculationState.EnteringSecond;
                }

                editedNumber.Clear();
                editedNumber.Append(BaseConverter.DoubleToString(PerformOperation(k, 0.0, 0.0), @base, maxNumberOfDigits));
                DisplayEditedNumber();
            }

            if (k == Key.Equals)
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
                DisplayResult();
                state = CalculationState.Result;
            }

            //DEBUG
            //(view as Form1).Text = string.Format(
            //    "{0}{1} {2}{3} {4}{5}",
            //    state == CalculationState.EnteringFirst ? ">" : "", firstNumber,
            //    state == CalculationState.EnteringOperation ? ">" : "", operation.ToString(),
            //    state == CalculationState.EnteringSecond ? ">" : "", secondNumber);
        }

        private double CommitEditedNumber()
        {
            double res = BaseConverter.StringToDouble(editedNumber.ToString(), @base);
            editedNumber.Clear();
            return res;
        }

        private double PerformOperation(Key operation, double lhs, double rhs)
        {
            if (operation == Key.Add) return lhs + rhs;
            if (operation == Key.Subtract) return lhs - rhs;
            if (operation == Key.Multiply) return lhs * rhs;
            if (operation == Key.Divide) return lhs / rhs;
            if (operation == Key.Sqrt) return Math.Sqrt(lhs);
            if (operation == Key.PiConstant) return Math.PI;
            if (operation == Key.EConstant) return Math.E;
            if (operation == Key.Cos) return Math.Cos(lhs);

            throw new ArgumentException("invalid operation");
        } 

        private void DisplayResult()
        {
            view.SetNumber(BaseConverter.DoubleToString(firstNumber, @base, maxNumberOfDigits));
        }

        private void DisplayEditedNumber()
        {
            if (editedNumber.Length == 0)
            {
                view.SetNumber("0");
            }
            else if (editedNumber[0] == '.')
            {
                view.SetNumber("0" + editedNumber.ToString());
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
