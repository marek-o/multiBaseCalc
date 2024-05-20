using System.Windows.Forms;

namespace multiBaseCalc
{
    /// <summary>
    /// Button which doesn't perform Click when Enter is pressed,
    /// instead Enter is passed to my program.
    /// </summary>
    public class ButtonNoEnter : Button
    {
        public int X { get; set; }
        public int Y { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter
                || keyData == Keys.Left
                || keyData == Keys.Right
                || keyData == Keys.Up
                || keyData == Keys.Down
                )
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
