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
    public interface IView
    {
        public void SetNumber(string s, int separatorGroupSize);
        public void SetBaseLabel(string s);
        public event Action<Key> KeyPressed;
    }

    public partial class Form1 : Form, IView
    {
        private float relativeDpi = 1.0f;
        private int Scale(int x)
        {
            return (int)(x * relativeDpi);
        }

        private List<CalculatorButton> buttons = new List<CalculatorButton>();

        public Form1()
        {
            InitializeComponent();

            relativeDpi = DeviceDpi / 96.0f;

            buttons.Add(new CalculatorButton(3, 2, Key.D7, "7"));
            buttons.Add(new CalculatorButton(4, 2, Key.D8, "8"));
            buttons.Add(new CalculatorButton(5, 2, Key.D9, "9"));
            buttons.Add(new CalculatorButton(3, 3, Key.D4, "4"));
            buttons.Add(new CalculatorButton(4, 3, Key.D5, "5"));
            buttons.Add(new CalculatorButton(5, 3, Key.D6, "6"));
            buttons.Add(new CalculatorButton(3, 4, Key.D1, "1"));
            buttons.Add(new CalculatorButton(4, 4, Key.D2, "2"));
            buttons.Add(new CalculatorButton(5, 4, Key.D3, "3"));
            buttons.Add(new CalculatorButton(3, 5, Key.D0, "0"));
            buttons.Add(new CalculatorButton(4, 5, Key.Period, "."));
            buttons.Add(new CalculatorButton(5, 5, Key.Equals, "="));

            buttons.Add(new CalculatorButton(6, 2, Key.Divide, "/"));
            buttons.Add(new CalculatorButton(6, 3, Key.Multiply, "*"));
            buttons.Add(new CalculatorButton(6, 4, Key.Subtract, "-"));
            buttons.Add(new CalculatorButton(6, 5, Key.Add, "+"));

            buttons.Add(new CalculatorButton(5, 0, Key.Escape, "C"));
            buttons.Add(new CalculatorButton(6, 0, Key.Backspace, "⌫"));

            buttons.Add(new CalculatorButton(1, 1, Key.Base2, "BIN\nF5"));
            buttons.Add(new CalculatorButton(2, 1, Key.Base8, "OCT\nF6"));
            buttons.Add(new CalculatorButton(3, 1, Key.Base10, "DEC\nF7"));
            buttons.Add(new CalculatorButton(4, 1, Key.Base16, "HEX\nF8"));
            buttons.Add(new CalculatorButton(5, 1, Key.DecrementBase, "base↓\n["));
            buttons.Add(new CalculatorButton(6, 1, Key.IncrementBase, "base↑\n]"));

            buttons.Add(new CalculatorButton(0, 2, Key.Inverse, "1/x\nShift+Q"));
            buttons.Add(new CalculatorButton(0, 3, Key.Square, "x^2\nShift+W"));
            buttons.Add(new CalculatorButton(0, 4, Key.Log10, "log10(x)\nShift+E"));
            buttons.Add(new CalculatorButton(0, 5, Key.Ln, "ln(x)\nShift+R"));

            buttons.Add(new CalculatorButton(1, 2, Key.Exp, "e^x\nShift+T"));
            buttons.Add(new CalculatorButton(1, 3, Key.Sin, "sin\nShift+Y"));
            buttons.Add(new CalculatorButton(1, 4, Key.Cos, "cos\nShift+U"));
            buttons.Add(new CalculatorButton(1, 5, Key.Tan, "tan\nShift+I"));

            buttons.Add(new CalculatorButton(2, 2, Key.Sqrt, "√\nShift+1"));
            buttons.Add(new CalculatorButton(2, 3, Key.PiConstant, "π\nShift+2"));
            buttons.Add(new CalculatorButton(2, 4, Key.EConstant, "e\nShift+3"));

            buttons.Add(new CalculatorButton(0, 0, Key.Power, "x^y\nShift+O"));
            buttons.Add(new CalculatorButton(1, 0, Key.NthRoot, "y√x\nShift+P"));
            buttons.Add(new CalculatorButton(0, 1, Key.NthLog, "log_y(x)\nShift+A"));

            buttons.Add(new CalculatorButton(4, 0, Key.Negate, "+/-\nShift+S"));

            buttons.Add(new CalculatorButton(2, 0, Key.ParenOpen, "("));
            buttons.Add(new CalculatorButton(3, 0, Key.ParenClose, ")"));

            foreach (var i in buttons)
            {
                i.Button = new ButtonNoEnter();
                i.Button.Location = new Point(Scale(i.X * 70 + 10), Scale(i.Y * 60 + 150));
                i.Button.Size = new Size(Scale(60), Scale(50));
                float fontSize = i.Text.Length == 1 ? 20f : 10f;
                i.Button.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point);

                i.Button.Text = i.Text;
                i.Button.KeyDown += Form1_KeyDown;
                i.Button.Click += (object sender, EventArgs args) => { KeyPressed(i.Key); };

                Controls.Add(i.Button);
            }
        }

        public void SetNumber(string s, int separatorGroupSize)
        {
            display.SetContent(s, separatorGroupSize);
        }

        public void SetBaseLabel(string s)
        {
            labelBase.Text = s;
        }

        public event Action<Key> KeyPressed = delegate { };

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (InputKeyToKey.TryGetValue(e.KeyData, out Key key))
            {
                KeyPressed(key);
            }
        }

        public static Dictionary<Keys, Key> InputKeyToKey = new Dictionary<Keys, Key>(
            new KeyValuePair<Keys, Key>[]{
                new KeyValuePair<Keys, Key>(Keys.D0, Key.D0),
                new KeyValuePair<Keys, Key>(Keys.D1, Key.D1),
                new KeyValuePair<Keys, Key>(Keys.D2, Key.D2),
                new KeyValuePair<Keys, Key>(Keys.D3, Key.D3),
                new KeyValuePair<Keys, Key>(Keys.D4, Key.D4),
                new KeyValuePair<Keys, Key>(Keys.D5, Key.D5),
                new KeyValuePair<Keys, Key>(Keys.D6, Key.D6),
                new KeyValuePair<Keys, Key>(Keys.D7, Key.D7),
                new KeyValuePair<Keys, Key>(Keys.D8, Key.D8),
                new KeyValuePair<Keys, Key>(Keys.D9, Key.D9),
                new KeyValuePair<Keys, Key>(Keys.A, Key.A),
                new KeyValuePair<Keys, Key>(Keys.B, Key.B),
                new KeyValuePair<Keys, Key>(Keys.C, Key.C),
                new KeyValuePair<Keys, Key>(Keys.D, Key.D),
                new KeyValuePair<Keys, Key>(Keys.E, Key.E),
                new KeyValuePair<Keys, Key>(Keys.F, Key.F),
                new KeyValuePair<Keys, Key>(Keys.G, Key.G),
                new KeyValuePair<Keys, Key>(Keys.H, Key.H),
                new KeyValuePair<Keys, Key>(Keys.I, Key.I),
                new KeyValuePair<Keys, Key>(Keys.J, Key.J),
                new KeyValuePair<Keys, Key>(Keys.K, Key.K),
                new KeyValuePair<Keys, Key>(Keys.L, Key.L),
                new KeyValuePair<Keys, Key>(Keys.M, Key.M),
                new KeyValuePair<Keys, Key>(Keys.N, Key.N),
                new KeyValuePair<Keys, Key>(Keys.O, Key.O),
                new KeyValuePair<Keys, Key>(Keys.P, Key.P),
                new KeyValuePair<Keys, Key>(Keys.Q, Key.Q),
                new KeyValuePair<Keys, Key>(Keys.R, Key.R),
                new KeyValuePair<Keys, Key>(Keys.S, Key.S),
                new KeyValuePair<Keys, Key>(Keys.T, Key.T),
                new KeyValuePair<Keys, Key>(Keys.U, Key.U),
                new KeyValuePair<Keys, Key>(Keys.V, Key.V),
                new KeyValuePair<Keys, Key>(Keys.W, Key.W),
                new KeyValuePair<Keys, Key>(Keys.X, Key.X),
                new KeyValuePair<Keys, Key>(Keys.Y, Key.Y),
                new KeyValuePair<Keys, Key>(Keys.Z, Key.Z),
                new KeyValuePair<Keys, Key>(Keys.Oemcomma, Key.Period),
                new KeyValuePair<Keys, Key>(Keys.OemPeriod, Key.Period),
                new KeyValuePair<Keys, Key>(Keys.Back, Key.Backspace),
                new KeyValuePair<Keys, Key>(Keys.Escape, Key.Escape),
                new KeyValuePair<Keys, Key>(Keys.Oemplus, Key.Equals),
                new KeyValuePair<Keys, Key>(Keys.Enter, Key.Equals),
                new KeyValuePair<Keys, Key>(Keys.OemOpenBrackets, Key.DecrementBase),
                new KeyValuePair<Keys, Key>(Keys.OemCloseBrackets, Key.IncrementBase),
                new KeyValuePair<Keys, Key>(Keys.F5, Key.Base2),
                new KeyValuePair<Keys, Key>(Keys.F6, Key.Base8),
                new KeyValuePair<Keys, Key>(Keys.F7, Key.Base10),
                new KeyValuePair<Keys, Key>(Keys.F8, Key.Base16),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D9, Key.ParenOpen),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D0, Key.ParenClose),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.Oemplus, Key.Add),
                new KeyValuePair<Keys, Key>(Keys.OemMinus, Key.Subtract),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D8, Key.Multiply),
                new KeyValuePair<Keys, Key>(Keys.OemQuestion, Key.Divide),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.S, Key.Negate),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.Q, Key.Inverse),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.W, Key.Square),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.O, Key.Power),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D1, Key.Sqrt),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.P, Key.NthRoot),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.E, Key.Log10),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.A, Key.NthLog),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.R, Key.Ln),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.T, Key.Exp),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D2, Key.PiConstant),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D3, Key.EConstant),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.Y, Key.Sin),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.U, Key.Cos),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.I, Key.Tan),
            }
            );
    }
}
