using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiPictureBox : UserControl
    {
        public cuiPictureBox()
        {
            InitializeComponent();
        }

        Image cachedImage = null;
        private Image privateContent = null;
        public Image Content
        {
            get
            {
                return privateContent;
            }
            set
            {
                if (value != null)
                {
                    privateContent = value;
                    cachedImage = value;
                    Image transformedImage = TransformImage(cachedImage);
                    cachedImageBrush = new TextureBrush(transformedImage);
                    transformedImage.Dispose();
                    cachedImageBrush.WrapMode = WrapMode.Clamp;
                }
                else
                {
                    privateContent = null;
                }
                Invalidate();
            }
        }

        private void DiposeIfPossible(ref Image target)
        {
            try
            {
                target.Dispose();
                GC.Collect();
            }
            catch {; }
            target = null;
        }

        private void DiposeIfPossible(ref TextureBrush target)
        {
            try
            {
                target.Image.Dispose();
                target.Dispose();
                GC.Collect();
            }
            catch { }
            target = null;
        }

        private int privateCornerRadius = 8;
        public int CornerRadius
        {
            get
            {
                return privateCornerRadius;
            }
            set
            {
                privateCornerRadius = value;
                Invalidate();
            }
        }

        private TextureBrush cachedImageBrush = null;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (privateContent == null || cachedImage == null || cachedImageBrush == null)
            {
                DiposeIfPossible(ref privateContent);
                DiposeIfPossible(ref cachedImage);
                DiposeIfPossible(ref cachedImageBrush);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle clipRectangle = ClientRectangle;
            clipRectangle.Inflate(-1, -1);

            if (CornerRadius == 0)
            {
                GraphicsPath clipGraphicsPath = Helper.RoundRect(clipRectangle, 1);

                if (Content != null && cachedImageBrush != null && cachedImage != null)
                {
                    if (privateContent == null || cachedImage == null || cachedImageBrush == null)
                    {
                        return;
                    }
                    e.Graphics.FillPath(cachedImageBrush, clipGraphicsPath);
                }
            }
            else
            {
                GraphicsPath clipGraphicsPath = Helper.RoundRect(clipRectangle, CornerRadius);

                if (Content != null && cachedImageBrush != null && cachedImage != null)
                {
                    if (privateContent == null || cachedImage == null || cachedImageBrush == null)
                    {
                        return;
                    }
                    e.Graphics.FillPath(cachedImageBrush, clipGraphicsPath);
                }
            }
        }

        public Image TransformImage(Image inputImage)
        {

            int width = ClientRectangle.Width;
            int height = ClientRectangle.Height;

            var newImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.TranslateTransform(width / 2, height / 2);
                g.ScaleTransform((float)width / inputImage.Width, (float)height / inputImage.Height);
                g.TranslateTransform(-inputImage.Width / 2, -inputImage.Height / 2);
                g.DrawImage(inputImage, Point.Empty);
                g.Dispose();
            }

            cachedImage = newImage;
            return newImage;
        }

        private void cuiPictureBox_Resize(object sender, EventArgs e)
        {
            if (Content != null)
            {
                Content = Content;
            }
        }
    }
}
