using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
                    TintImage();
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

        private void TintImage()
        {
            if (privateContent == null)
                return;

            Bitmap tintedBitmap = new Bitmap(privateContent.Width, privateContent.Height);

            using (Graphics graphics = Graphics.FromImage(tintedBitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                float r = ImageTint.R / 255f;
                float g = ImageTint.G / 255f;
                float b = ImageTint.B / 255f;
                float a = ImageTint.A / 255f;

                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {r, 0, 0, 0, 0},
                    new float[] {0, g, 0, 0, 0},
                    new float[] {0, 0, b, 0, 0},
                    new float[] {0, 0, 0, a, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                graphics.DrawImage(privateContent, new Rectangle(0, 0, privateContent.Width, privateContent.Height),
                            0, 0, privateContent.Width, privateContent.Height,
                            GraphicsUnit.Pixel, imageAttributes);
            }

            cachedImageBrush = new TextureBrush(privateContent, WrapMode.Clamp);
            cachedImage = TransformImage(tintedBitmap);
        }

        private TextureBrush cachedImageBrush = null;

        private Color privateImageTint = Color.White;
        public Color ImageTint
        {
            get
            {
                return privateImageTint;
            }
            set
            {
                privateImageTint = value;
                TintImage();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (cachedImage == null)
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

            GraphicsPath clipGraphicsPath = Helper.RoundRect(clipRectangle, CornerRadius);

            if (cachedImage != null)
            {
                e.Graphics.SetClip(clipGraphicsPath);
                e.Graphics.DrawImage(cachedImage, clipRectangle);
            }
        }

        public Image TransformImage(Image inputImage)
        {
            int width = ClientRectangle.Width;
            int height = ClientRectangle.Height;

            var newImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(inputImage, new Rectangle(0, 0, width, height));
            }

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
