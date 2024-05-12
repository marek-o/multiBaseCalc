using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace multiBaseCalc
{
    public partial class DisplayControl : UserControl
    {
        public DisplayControl()
        {
            SetStyle(ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        private string digitText = "";
        private string periodText = "";
        private string separatorText = "";

        public void SetContent(string text, int separatorGroupSize)
        {
            digitText = text.Replace(".", "");

            if (text.Length > 0 && text[0] == '[')
            {
                periodText = "";
                separatorText = "";
                return;
            }

            int periodPos = text.IndexOf('.');
            if (periodPos == -1)
            {
                periodPos = text.Length;
            }

            char[] periodTextArray = Enumerable.Repeat(' ', digitText.Length * 2).ToArray();
            int outPeriodPos = periodPos * 2 - 1;
            periodTextArray[outPeriodPos] = '.';
            periodText = new string(periodTextArray);

            char[] separatorTextArray = Enumerable.Repeat(' ', digitText.Length * 2).ToArray();
            int i = outPeriodPos - separatorGroupSize * 2;
            while (i >= (digitText[0] == '-' ? 2 : 0))
            {
                separatorTextArray[i] = '.';
                i -= separatorGroupSize * 2;
            }
            i = outPeriodPos + separatorGroupSize * 2;
            while (i < separatorTextArray.Length)
            {
                separatorTextArray[i] = '.';
                i += separatorGroupSize * 2;
            }

            separatorText = new string(separatorTextArray);

            Invalidate();
        }

        private int DpiScale(int x)
        {
            return (int)(x * DeviceDpi / 96.0f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.LightGray);
            TextRenderer.DrawText(e.Graphics, digitText, Font, new Rectangle(0, 0, Width, Height), Color.Black, Color.Transparent, TextFormatFlags.Right);
            using (Font halfFont = new Font(Font.Name, Font.Size / 2))
            {
                TextRenderer.DrawText(e.Graphics, periodText, halfFont, new Rectangle(DpiScale(0), DpiScale(30), Width, Height), Color.Black, Color.Transparent, TextFormatFlags.Right);
                TextRenderer.DrawText(e.Graphics, separatorText, halfFont, new Rectangle(DpiScale(0), DpiScale(-10), Width, Height), Color.Black, Color.Transparent, TextFormatFlags.Right);
            }

            using (Pen p = new Pen(Color.Black, DpiScale(2)))
            {
                e.Graphics.DrawLine(p, 0, 0, Width - 1, 0);
                e.Graphics.DrawLine(p, 0, 0, 0, Height - 1);
                p.Color = Color.White;
                e.Graphics.DrawLine(p, Width - 1, 0, Width - 1, Height - 1);
                e.Graphics.DrawLine(p, 0, Height - 1, Width - 1, Height - 1);
            }
        }
    }
}
