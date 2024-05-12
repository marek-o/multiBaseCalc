using System.Drawing;
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.LightGray);
            TextRenderer.DrawText(e.Graphics, Text, Font, new Rectangle(0, 0, Width, Height), Color.Black, Color.Transparent, TextFormatFlags.Right);

            using (Pen p = new Pen(Color.Black, 2.0f * DeviceDpi / 96.0f))
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
