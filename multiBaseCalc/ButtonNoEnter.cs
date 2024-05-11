using System.Windows.Forms;

namespace multiBaseCalc
{
    /// <summary>
    /// Button which doesn't perform Click when Enter is pressed,
    /// instead Enter is passed to my program.
    /// </summary>
    public class ButtonNoEnter : Button
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
