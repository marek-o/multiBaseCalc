using System.Windows.Forms;

namespace multiBaseCalc
{
    public class CalculatorButton
    {
        public ButtonNoEnter Button { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Key Key { get; set; }
        public string Text { get; set; }

        public CalculatorButton(int x, int y, Key key, string text)
        {
            X = x;
            Y = y;
            Key = key;
            Text = text;
        }
    }
}
