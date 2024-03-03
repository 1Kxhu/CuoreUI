using CuoreUI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace wfcl1
{
    public partial class cuiProgressBarVertical : cuiProgressBarHorizontal
    {
        public cuiProgressBarVertical()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Bitmap tempBitmap = new Bitmap(ClientSize.Width * 2, ClientSize.Height * 2);
            using (Graphics tempGraphics = Graphics.FromImage(tempBitmap))
            {
                tempGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                tempGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                tempGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                tempGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                GraphicsPath roundBackground = Helper.RoundRect(new Rectangle(0, 0, ClientSize.Width * 2, ClientSize.Height * 2), 17);

                float filledPercent = (float)Value / MaxValue;
                float foreHeight = ClientRectangle.Height * filledPercent * 2;
                RectangleF foreHalf = new RectangleF(0, 0, ClientRectangle.Width * 2, ClientRectangle.Height * 2);
                RectangleF client = new RectangleF(0, foreHeight, ClientRectangle.Width * 2, ClientRectangle.Height * 2 - foreHeight);
                tempGraphics.SetClip(roundBackground);

                using (SolidBrush brush = new SolidBrush(Foreground))
                {
                    tempGraphics.FillRectangle(brush, foreHalf);
                }

                using (SolidBrush brush = new SolidBrush(Background))
                {
                    tempGraphics.FillRectangle(brush, client);
                }
            }

            if (Flipped)
            {
                tempBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            e.Graphics.DrawImage(tempBitmap, ClientRectangle);

            tempBitmap.Dispose();
        }
    }
}
