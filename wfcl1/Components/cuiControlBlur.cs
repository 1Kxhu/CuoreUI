using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    public partial class cuiControlBlur : Component
    {
        private Bitmap cachedBitmap;
        public cuiControlBlur(IContainer container)
        {
            container.Add(this);
        }

        private Control targetControl;
        public Control TargetControl
        {
            get
            {
                return targetControl;
            }
            set
            {
                if (targetControl != null)
                {
                    targetControl.Paint -= TargetControl_Paint;
                    targetControl.Invalidated -= TargetControl_Invalidated;
                }
                targetControl = value;
                if (targetControl != null)
                {
                    targetControl.Paint += TargetControl_Paint;
                    targetControl.Invalidated += TargetControl_Invalidated;
                }
                cachedBitmap?.Dispose();
                cachedBitmap = null;
                targetControl?.Invalidate();
            }
        }

        private double privateSigma = 1.1;
        private double Sigma
        {
            get
            {
                return privateSigma;
            }
            set
            {
                if (value > 0)
                {
                    privateSigma = value;
                }
                cachedBitmap?.Dispose();
                cachedBitmap = null;
                targetControl?.Invalidate();
            }
        }

        private int privateBlurAmount = 5;
        public int BlurAmount
        {
            get
            {
                return privateBlurAmount;
            }
            set
            {
                if (value > 0)
                {
                    privateBlurAmount = value;
                }
                cachedBitmap?.Dispose();
                cachedBitmap = null;
                targetControl?.Refresh();
            }
        }

        private void TargetControl_Invalidated(object sender, InvalidateEventArgs e)
        {
            cachedBitmap?.Dispose();
            cachedBitmap = null;
        }

        private void TargetControl_Paint(object sender, PaintEventArgs e)
        {
            targetControl?.SuspendLayout();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(targetControl.ClientRectangle);

            using (Region clipRegion = new Region(path))
            {
                e.Graphics.Clip = clipRegion;

                if (cachedBitmap == null)
                {
                    cachedBitmap = new Bitmap(targetControl.Width, targetControl.Height);
                    using (Graphics g = Graphics.FromImage(cachedBitmap))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        g.Clear(Color.Transparent);
                        targetControl.DrawToBitmap(cachedBitmap, new Rectangle(0, 0, targetControl.Width, targetControl.Height));
                        GaussianBlur.Apply(ref cachedBitmap, BlurAmount);
                    }
                }

                e.Graphics.DrawImage(cachedBitmap, targetControl.ClientRectangle);
            }
            targetControl?.ResumeLayout(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cachedBitmap?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
