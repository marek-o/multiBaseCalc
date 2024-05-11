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
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        public void SetNumber(string s)
        {
            label1.Text = s;
        }

        public void SetBaseLabel(string s)
        {
            labelBase.Text = s;
        }

        public event Action<Key> KeyPressed = delegate { };

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyTranslation.TryGetValue(e.KeyData, out Key key))
            {
                KeyPressed(key);
            }
        }

        public static Dictionary<Keys, Key> KeyTranslation = new Dictionary<Keys, Key>(
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

        private void button1_Click(object sender, EventArgs e)
        {
            KeyPressed(Key.Add);
        }
    }
}
