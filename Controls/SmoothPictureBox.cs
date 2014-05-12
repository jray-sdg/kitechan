using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Kitechan
{
    public partial class SmoothPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pe.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            base.OnPaint(pe);
        }
    }
}
