using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Components.cuiFormRounderV2Resources
{
    public partial class RoundedForm : Form
    {
        private Color privateBackgroundColor = Color.White;
        private Color privateBorderColor = Color.White;

        public Color BackgroundColor
        {
            get
            {
                return privateBackgroundColor;
            }
            set
            {
                privateBackgroundColor = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get
            {
                return privateBorderColor;
            }
            set
            {
                privateBorderColor = value;
                Invalidate();
            }
        }

        private int privateRounding = 8;
        public int Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                privateRounding = value;
                Invalidate();
            }
        }

        public RoundedForm(Color init_backgroundColor, Color init_borderColor)
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            BackgroundColor = init_backgroundColor;
            BorderColor = init_borderColor;
        }

        public RoundedForm(Color init_backgroundColor, Color init_borderColor, bool show)
        {
            Visible = show;
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            BackgroundColor = init_backgroundColor;
            BorderColor = init_borderColor;
            
        }

        private Bitmap backImage; // Declare a member variable to hold the back image
        private Graphics backGraphics; // Declare a member variable to hold the graphics object

        private void DrawForm(object pSender, EventArgs pE)
        {
            try
            {

                if (this != null)
                {

                    if (backImage == null || backImage.Size != Size)
                    {
                        backImage?.Dispose();
                        backImage = new Bitmap(Width, Height);
                        backGraphics?.Dispose();
                        backGraphics = Graphics.FromImage(backImage);
                        backGraphics.SmoothingMode = SmoothingMode.None;
                    }
                    else
                    {
                        backGraphics.Clear(Color.Transparent);
                    }

                    Rectangle gradientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    GraphicsPath roundedRectangle = Helper.RoundRect(gradientRectangle, Rounding);

                    using (SolidBrush brush = new SolidBrush(BackgroundColor))
                    using (Pen pen = new Pen(BorderColor))
                    {
                        backGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                        backGraphics.FillPath(brush, roundedRectangle);
                        backGraphics.DrawPath(pen, roundedRectangle);
                        backGraphics.SmoothingMode = SmoothingMode.None;
                    }

                    if (this != null && backImage != null && backGraphics != null)
                    {
                        PerPixelAlphaBlend.SetBitmap(backImage, Left, Top, Handle);
                    }
                }
            }
            catch
            {
                // refer to cuiFormRounderV2 why I'm doing this
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {

                if (this != null && !DesignMode)
                {
                    var cp = NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_EXSTYLE);
                    cp |= NativeMethods.WS_EX_LAYERED;
                    NativeMethods.SetWindowLong(Handle, NativeMethods.GWL_EXSTYLE, cp);
                }
                DrawForm(this, EventArgs.Empty);
            }
            catch
            {

            }
        }
    }
}
