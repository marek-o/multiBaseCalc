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
        public void SetNumber(string s);
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

            buttons.Add(new CalculatorButton(2, 0, Key.D7, "7"));
            buttons.Add(new CalculatorButton(3, 0, Key.D8, "8"));
            buttons.Add(new CalculatorButton(4, 0, Key.D9, "9"));
            buttons.Add(new CalculatorButton(2, 1, Key.D4, "4"));
            buttons.Add(new CalculatorButton(3, 1, Key.D5, "5"));
            buttons.Add(new CalculatorButton(4, 1, Key.D6, "6"));
            buttons.Add(new CalculatorButton(2, 2, Key.D1, "1"));
            buttons.Add(new CalculatorButton(3, 2, Key.D2, "2"));
            buttons.Add(new CalculatorButton(4, 2, Key.D3, "3"));
            buttons.Add(new CalculatorButton(2, 3, Key.D0, "0"));
            buttons.Add(new CalculatorButton(3, 3, Key.Period, "."));
            buttons.Add(new CalculatorButton(4, 3, Key.Equals, "="));

            buttons.Add(new CalculatorButton(5, 0, Key.Divide, "/"));
            buttons.Add(new CalculatorButton(5, 1, Key.Multiply, "*"));
            buttons.Add(new CalculatorButton(5, 2, Key.Subtract, "-"));
            buttons.Add(new CalculatorButton(5, 3, Key.Add, "+"));

            buttons.Add(new CalculatorButton(4, -2, Key.Escape, "C"));
            buttons.Add(new CalculatorButton(5, -2, Key.Backspace, "⌫"));

            buttons.Add(new CalculatorButton(0, -1, Key.Base2, "BIN\nF5"));
            buttons.Add(new CalculatorButton(1, -1, Key.Base8, "OCT\nF6"));
            buttons.Add(new CalculatorButton(2, -1, Key.Base10, "DEC\nF7"));
            buttons.Add(new CalculatorButton(3, -1, Key.Base16, "HEX\nF8"));
            buttons.Add(new CalculatorButton(4, -1, Key.DecrementBase, "base↓\n["));
            buttons.Add(new CalculatorButton(5, -1, Key.IncrementBase, "base↑\n]"));

            buttons.Add(new CalculatorButton(1, 0, Key.Sqrt, "√\nShift+1"));
            buttons.Add(new CalculatorButton(1, 1, Key.PiConstant, "π\nShift+2"));
            buttons.Add(new CalculatorButton(1, 2, Key.EConstant, "e\nShift+3"));
            buttons.Add(new CalculatorButton(1, 3, Key.Cos, "cos\nShift+4"));

            foreach (var i in buttons)
            {
                i.Button = new ButtonNoEnter();
                i.Button.Location = new Point(Scale(i.X * 70 + 10), Scale(i.Y * 60 + 210));
                i.Button.Size = new Size(Scale(60), Scale(50));
                float fontSize = i.Text.Length == 1 ? 20f : 10f;
                i.Button.Font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point);

                i.Button.Text = i.Text;
                i.Button.KeyDown += Form1_KeyDown;
                i.Button.Click += (object sender, EventArgs args) => { KeyPressed(i.Key); };

                Controls.Add(i.Button);
            }
        }

        public void SetNumber(string s)
        {
            display.Text = s;
            display.Invalidate();
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
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.Oemplus, Key.Add),
                new KeyValuePair<Keys, Key>(Keys.OemMinus, Key.Subtract),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D8, Key.Multiply),
                new KeyValuePair<Keys, Key>(Keys.OemQuestion, Key.Divide),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D1, Key.Sqrt),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D2, Key.PiConstant),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D3, Key.EConstant),
                new KeyValuePair<Keys, Key>(Keys.Shift | Keys.D4, Key.Cos),
            }
            );
    }
}
