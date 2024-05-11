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
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char k = e.KeyChar;

            if (KeyTranslation.TryGetValue(k, out Key key))
            {
                KeyPressed(key);
            }
        }

        public static Dictionary<char, Key> KeyTranslation = new Dictionary<char, Key>(
            new KeyValuePair<char, Key>[]{
                new KeyValuePair<char, Key>('0', Key.D0),
                new KeyValuePair<char, Key>('1', Key.D1),
                new KeyValuePair<char, Key>('2', Key.D2),
                new KeyValuePair<char, Key>('3', Key.D3),
                new KeyValuePair<char, Key>('4', Key.D4),
                new KeyValuePair<char, Key>('5', Key.D5),
                new KeyValuePair<char, Key>('6', Key.D6),
                new KeyValuePair<char, Key>('7', Key.D7),
                new KeyValuePair<char, Key>('8', Key.D8),
                new KeyValuePair<char, Key>('9', Key.D9),
                new KeyValuePair<char, Key>('a', Key.A),
                new KeyValuePair<char, Key>('b', Key.B),
                new KeyValuePair<char, Key>('c', Key.C),
                new KeyValuePair<char, Key>('d', Key.D),
                new KeyValuePair<char, Key>('e', Key.E),
                new KeyValuePair<char, Key>('f', Key.F),
                new KeyValuePair<char, Key>('g', Key.G),
                new KeyValuePair<char, Key>('h', Key.H),
                new KeyValuePair<char, Key>('i', Key.I),
                new KeyValuePair<char, Key>('j', Key.J),
                new KeyValuePair<char, Key>('k', Key.K),
                new KeyValuePair<char, Key>('l', Key.L),
                new KeyValuePair<char, Key>('m', Key.M),
                new KeyValuePair<char, Key>('n', Key.N),
                new KeyValuePair<char, Key>('o', Key.O),
                new KeyValuePair<char, Key>('p', Key.P),
                new KeyValuePair<char, Key>('q', Key.Q),
                new KeyValuePair<char, Key>('r', Key.R),
                new KeyValuePair<char, Key>('s', Key.S),
                new KeyValuePair<char, Key>('t', Key.T),
                new KeyValuePair<char, Key>('u', Key.U),
                new KeyValuePair<char, Key>('v', Key.V),
                new KeyValuePair<char, Key>('w', Key.W),
                new KeyValuePair<char, Key>('x', Key.X),
                new KeyValuePair<char, Key>('y', Key.Y),
                new KeyValuePair<char, Key>('z', Key.Z),
                new KeyValuePair<char, Key>(',', Key.Period),
                new KeyValuePair<char, Key>('.', Key.Period),
                new KeyValuePair<char, Key>((char)Keys.Back, Key.Backspace),
                new KeyValuePair<char, Key>((char)Keys.Escape, Key.Escape),
                new KeyValuePair<char, Key>('=', Key.Equals),
                new KeyValuePair<char, Key>((char)Keys.Enter, Key.Equals),
                new KeyValuePair<char, Key>('[', Key.DecrementBase),
                new KeyValuePair<char, Key>(']', Key.IncrementBase),
                new KeyValuePair<char, Key>('+', Key.Add),
                new KeyValuePair<char, Key>('-', Key.Subtract),
                new KeyValuePair<char, Key>('*', Key.Multiply),
                new KeyValuePair<char, Key>('/', Key.Divide),
                new KeyValuePair<char, Key>('!', Key.Sqrt),
                new KeyValuePair<char, Key>('@', Key.PiConstant),
                new KeyValuePair<char, Key>('#', Key.EConstant),
                new KeyValuePair<char, Key>('$', Key.Cos),
            }
            );
    }
}
