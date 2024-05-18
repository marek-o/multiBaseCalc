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

        private Stack<Tuple<double, Key>> operationStack = new Stack<Tuple<double, Key>>();

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

            view.SetNumber("0", SeparatorGroupSize());
            UpdateBaseLabel();
        }

        private void View_KeyPressed(Key k)
        {
            if (k == Key.Copy)
            {
                var str = "";
                if (state == CalculationState.Result)
                {
                    str = BaseConverter.DoubleToString(firstNumber, @base, maxNumberOfDigits);
                }
                else if (state == CalculationState.EnteringFirst)
                {
                    str = EditedNumberToDisplayable();
                }
                else if (state == CalculationState.EnteringOperation)
                {
                    str = BaseConverter.DoubleToString(firstNumber, @base, maxNumberOfDigits);
                }
                else if (state == CalculationState.EnteringSecond)
                {
                    str = EditedNumberToDisplayable();
                }
                view.SetClipboard(str);
            }

            if (k == Key.Paste)
            {
                var clip = view.GetClipboard();
                if (clip != null)
                {
                    if (state == CalculationState.Result)
                    {
                        state = CalculationState.EnteringFirst;
                    }
                    else if (state == CalculationState.EnteringOperation)
                    {
                        state = CalculationState.EnteringSecond;
                    }

                    clip = SanitizeClipboard(clip);
                    clip = DisplayableToEditedNumber(clip);
                    editedNumber.Clear();
                    editedNumber.Append(clip);
                    DisplayEditedNumber();
                }
            }

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
                operationStack.Clear();

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
                    if (editedNumber.ToString() == "-")
                    {
                        editedNumber.Clear();
                    }
                    DisplayEditedNumber();
                }
            }

            if (k == Key.ParenOpen)
            {
                if (state == CalculationState.EnteringOperation)
                {
                    //2*(
                    var item = new Tuple<double, Key>(firstNumber, operation);
                    operationStack.Push(item);
                    firstNumber = 0.0;
                    state = CalculationState.Result;
                }
                else if (state == CalculationState.Result)
                {
                    //2*((
                    //or 2*(_100_( FIXME after one argument operation this will be wrong
                    //only remember nesting
                    operationStack.Push(null);
                    firstNumber = 0.0;
                    state = CalculationState.Result;
                }
            }

            if (k == Key.ParenClose)
            {
                if (operationStack.Any())
                {
                    if (state == CalculationState.Result)
                    {
                        //2*()
                        //or 2*(_123_) result could be from one argument operation
                    }
                    else if (state == CalculationState.EnteringFirst)
                    {
                        //2*(3)
                        firstNumber = CommitEditedNumber();
                    }
                    else if (state == CalculationState.EnteringOperation)
                    {
                        //2*(3+)
                        //with itself
                        secondNumber = firstNumber;
                        firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                    }
                    else if (state == CalculationState.EnteringSecond)
                    {
                        //2*(3+4)
                        secondNumber = CommitEditedNumber();
                        firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                    }

                    DisplayResult();

                    editedNumber.Clear();
                    editedNumber.Append(BaseConverter.DoubleToString(firstNumber, @base, maxNumberOfDigits));

                    var item = operationStack.Pop();
                    if (item != null)
                    {
                        firstNumber = item.Item1;
                        operation = item.Item2;
                        state = CalculationState.EnteringSecond;
                    }
                    else
                    {
                        state = CalculationState.EnteringFirst;
                    }
                }
            }

            if (k == Key.Add
                || k == Key.Subtract
                || k == Key.Multiply
                || k == Key.Divide
                || k == Key.Power
                || k == Key.NthRoot
                || k == Key.NthLog
                )
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

            if (k == Key.Inverse
                || k == Key.Square
                || k == Key.Sqrt
                || k == Key.Log10
                || k == Key.Ln
                || k == Key.Exp
                || k == Key.Sin
                || k == Key.Cos
                || k == Key.Tan
                )
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

            if (k == Key.Negate)
            {
                if (state == CalculationState.EnteringFirst 
                    || state == CalculationState.EnteringSecond)
                {
                    if (editedNumber[0] == '-')
                    {
                        editedNumber.Remove(0, 1);
                    }
                    else
                    {
                        editedNumber.Insert(0, '-');
                    }
                    DisplayEditedNumber();
                }
                else if (state == CalculationState.Result)
                {
                    firstNumber = -firstNumber;
                    DisplayResult();
                }
            }

            if (k == Key.Equals)
            {
                if (operationStack.Any())
                {
                    while (operationStack.Any())
                    {
                        if (state == CalculationState.Result)
                        {   
                            //eg. during stack unwinding
                            //1+( _5_ =
                        }
                        else if (state == CalculationState.EnteringFirst)
                        {
                            // 1+(2+(3=
                            firstNumber = CommitEditedNumber();
                        }
                        else if (state == CalculationState.EnteringOperation)
                        {
                            //1+(2+=
                            //do "with itself"
                            secondNumber = firstNumber;
                            firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                        }
                        else if (state == CalculationState.EnteringSecond)
                        {
                            //1+(2+3=
                            secondNumber = CommitEditedNumber();
                            firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                        }

                        secondNumber = firstNumber;

                        var item = operationStack.Pop();
                        if (item != null)
                        {
                            firstNumber = item.Item1;
                            operation = item.Item2;
                            firstNumber = PerformOperation(operation, firstNumber, secondNumber);
                        }
                        else
                        {
                            firstNumber = secondNumber;
                        }

                        state = CalculationState.Result;
                    }

                    DisplayResult();
                }
                else
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
            }

            //DEBUG
            //(view as Form1).Text = string.Format(
            //    "{0}{1} {2}{3} {4}{5}",
            //    state == CalculationState.EnteringFirst ? ">" : "", firstNumber,
            //    state == CalculationState.EnteringOperation ? ">" : "", operation.ToString(),
            //    state == CalculationState.EnteringSecond ? ">" : "", secondNumber);
        }

        private string SanitizeClipboard(string s)
        {
            var sb = new StringBuilder();
            bool firstPeriod = true;
            for (int i = 0; i < s.Length; ++i)
            {
                if (BaseConverter.CharToInt(s[i], @base) != -1)
                {
                    sb.Append(char.ToLower(s[i]));
                }
                else if ((s[i] == ',' || s[i] == '.') && firstPeriod)
                {
                    sb.Append('.');
                    firstPeriod = false;
                }
                else if (s[i] == '-' && sb.Length == 0)
                {
                    sb.Append('-');
                }
            }

            return sb.ToString();
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
            if (operation == Key.Inverse) return 1.0 / lhs;
            if (operation == Key.Square) return lhs * lhs;
            if (operation == Key.Power)
            {
                if (lhs == 0.0 && rhs == 0.0) return double.NaN;
                return Math.Pow(lhs, rhs);
            }
            if (operation == Key.Sqrt) return Math.Sqrt(lhs);
            if (operation == Key.NthRoot)
            {
                if (rhs == 0.0) return double.NaN;
                return Math.Pow(lhs, 1 / rhs);
            }
            if (operation == Key.Log10) return Math.Log10(lhs);
            if (operation == Key.NthLog)
            {
                if (rhs <= 0.0 || rhs == 1.0) return double.NaN;
                return Math.Log(lhs) / Math.Log(rhs);
            }
            if (operation == Key.Ln) return Math.Log(lhs);
            if (operation == Key.Exp) return Math.Exp(lhs);
            if (operation == Key.PiConstant) return Math.PI;
            if (operation == Key.EConstant) return Math.E;
            if (operation == Key.Sin) return Math.Sin(lhs);
            if (operation == Key.Cos) return Math.Cos(lhs);
            if (operation == Key.Tan) return Math.Tan(lhs);

            throw new ArgumentException("invalid operation");
        } 

        private int SeparatorGroupSize()
        {
            return (@base == 10) ? 3 : 4;
        }

        private void DisplayResult()
        {
            view.SetNumber(BaseConverter.DoubleToString(firstNumber, @base, maxNumberOfDigits),
                SeparatorGroupSize());
        }

        private string EditedNumberToDisplayable()
        {
            if (editedNumber.Length == 0)
            {
                return "0";
            }
            else if (editedNumber[0] == '.')
            {
                return "0" + editedNumber.ToString();
            }
            else if (editedNumber[0] == '-' && editedNumber[1] == '.')
            {
                return "-0" + editedNumber.ToString().Substring(1);
            }
            else
            {
                return editedNumber.ToString();
            }
        }

        private string DisplayableToEditedNumber(string s)
        {
            s = s.TrimStart('0');

            if (s.StartsWith("-"))
            {
                s = "-" + s.Substring(1).TrimStart('0');
                if (s == "-")
                {
                    return "";
                }
            }

            return s;
        }

        private void DisplayEditedNumber()
        {
            view.SetNumber(EditedNumberToDisplayable(), SeparatorGroupSize());
        }

        private void UpdateBaseLabel()
        {
            view.SetBaseLabel(string.Format("base {0}", @base));
        }
    }
}
