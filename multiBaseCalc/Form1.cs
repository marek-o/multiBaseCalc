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
    public partial class Form1 : Form
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

        public event Action<char> KeyPressed = delegate { };

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char k = e.KeyChar;
            KeyPressed(k);
        }
    }
}
