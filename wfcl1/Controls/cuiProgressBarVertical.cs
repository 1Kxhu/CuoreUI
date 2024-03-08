using CuoreUI;
using System;
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
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Bitmap tempBitmap = new Bitmap(ClientSize.Width * 2, ClientSize.Height * 2);
            using (Graphics tempGraphics = Graphics.FromImage(tempBitmap))
            {
                tempGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                tempGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                tempGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                tempGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                float filledPercent = (float)Value / MaxValue;
                float foreHeight = ClientRectangle.Height * filledPercent * 2;
                RectangleF foreHalf = new RectangleF(0, 0, ClientRectangle.Width * 2, ClientRectangle.Height * 2);
                RectangleF client = new RectangleF(-5, foreHeight, ClientRectangle.Width * 2 + 7, ClientRectangle.Height * 2 + 1);
                int roundedclientHeight = (int)Math.Round((double)Height, 0);

                GraphicsPath roundBackground = Helper.RoundRect(new Rectangle(0, 0, ClientSize.Width * 2, ClientSize.Height * 2), Rounding);
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
                tempBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            }

            e.Graphics.DrawImage(tempBitmap, ClientRectangle);

            tempBitmap.Dispose();
        }
    }
}
