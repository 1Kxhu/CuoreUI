using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static CuoreUI.Helper.Win32;

namespace CuoreUI.Components.cuiFormRounderV2Resources
{
    public partial class RoundedForm : Form
    {
        private Color privateBackgroundColor = Color.White;
        private Color privateBorderColor = Color.White;
        private Bitmap backImage;
        private Graphics backGraphics;

        private bool privateInvalidateNextDrawCall = false;
        public bool InvalidateNextDrawCall
        {
            get
            {
                return privateInvalidateNextDrawCall;
            }
            set
            {
                if (value == true)
                {
                    DrawForm(null, EventArgs.Empty);
                    privateInvalidateNextDrawCall = false;
                }
            }
        }

        public Color BackgroundColor
        {
            get => privateBackgroundColor;
            set
            {
                privateBackgroundColor = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get => privateBorderColor;
            set
            {
                privateBorderColor = value;
                Invalidate();
            }
        }

        public RoundedForm(Color init_backgroundColor, Color init_borderColor, bool show = true)
        {
            Visible = show;
            InitializeComponent();
            SetStyles();
            BackgroundColor = init_backgroundColor;
            BorderColor = init_borderColor;
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        public void DrawForm(object sender, EventArgs e)
        {
            SuspendLayout();
            try
            {
                if (backImage == null || backImage.Size != Size || InvalidateNextDrawCall)
                {
                    InvalidateNextDrawCall = false;
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

                Rectangle gradientRectangle = new Rectangle(0, 0, Width - 2, Height - 2);
                GraphicsPath roundedRectangle = Helper.RoundRect(gradientRectangle, Stored.rounding);

                Rectangle subtractRectangle = gradientRectangle;
                subtractRectangle.Offset(1, 1);
                subtractRectangle.Inflate(-1, -1);

                Rectangle fillinoutlineRectangle = subtractRectangle;
                fillinoutlineRectangle.Offset(-1, -1);

                GraphicsPath subtractPath = Helper.RoundRect(subtractRectangle, Stored.rounding);
                GraphicsPath fillinoutlinePath = Helper.RoundRect(fillinoutlineRectangle, Stored.rounding);

                using (SolidBrush brush = new SolidBrush(BackgroundColor))
                using (Pen pen = new Pen(BorderColor))
                {
                    backGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    backGraphics.DrawPath(new Pen(BackgroundColor, 1.6f), fillinoutlinePath);
                    backGraphics.FillPath(brush, roundedRectangle);
                    backGraphics.DrawPath(pen, roundedRectangle);
                    backGraphics.SmoothingMode = SmoothingMode.None;
                }

                double normalisedOpacity = 1;

                double.TryParse(Tag.ToString(), out normalisedOpacity);
                normalisedOpacity *= 255;

                byte opacity = Convert.ToByte(normalisedOpacity);

                PerPixelAlphaBlend.SetBitmap(backImage, opacity, Left, Top, Handle);

            }
            finally
            {
                ResumeLayout();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!DesignMode)
            {
                var cp = NativeMethods.GetWindowLong(Handle, NativeMethods.GWL_EXSTYLE);
                cp |= NativeMethods.WS_EX_LAYERED;
                NativeMethods.SetWindowLong(Handle, NativeMethods.GWL_EXSTYLE, cp);
            }
            DrawForm(this, EventArgs.Empty);
        }

        private void RoundedForm_PaddingChanged(object sender, EventArgs e)
        {
            DrawForm(this, e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            DrawForm(this, e);
        }
    }
}
