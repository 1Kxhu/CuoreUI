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

        private Image privateBackgroundImage;
        public Image BackgroundImageOfTargetForm
        {
            get => privateBackgroundImage;
            set
            {
                privateBackgroundImage?.Dispose();
                privateBackgroundImage = value;
                DrawForm(null, EventArgs.Empty);
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

        public int Rounding = 8;

        public RoundedForm(Color init_backgroundColor, Color init_borderColor, ref int RoundValue, bool show = true)
        {
            Visible = show;
            InitializeComponent();
            SetStyles();
            BackgroundColor = init_backgroundColor;
            BorderColor = init_borderColor;
            Rounding = RoundValue;
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        public void DrawForm(object sender, EventArgs e)
        {
            if (stop)
            {
                return;
            }

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
                GraphicsPath roundedRectangle = Helper.RoundRect(gradientRectangle, Rounding);

                Rectangle subtractRectangle = gradientRectangle;
                subtractRectangle.Offset(1, 1);
                subtractRectangle.Inflate(-1, -1);

                Rectangle fillinoutlineRectangle = subtractRectangle;
                fillinoutlineRectangle.Offset(-1, -1);

                GraphicsPath subtractPath = Helper.RoundRect(subtractRectangle, Rounding);
                GraphicsPath fillinoutlinePath = Helper.RoundRect(fillinoutlineRectangle, Rounding);

                using (SolidBrush brush = new SolidBrush(BackgroundColor))
                using (Pen pen = new Pen(BorderColor))
                {
                    backGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    backGraphics.DrawPath(new Pen(BackgroundColor, 1.6f), fillinoutlinePath);

                    if (BackgroundImageOfTargetForm == null)
                    {
                        backGraphics.FillPath(brush, roundedRectangle);
                    }
                    else
                    {
                        try
                        {
                            backgroundImageTextureBrush = new TextureBrush(BackgroundImageOfTargetForm);
                            backgroundImageTextureBrush.WrapMode = WrapMode.Clamp;
                        }
                        catch
                        {

                        }
                        backGraphics.FillPath(backgroundImageTextureBrush, roundedRectangle); //experimental
                    }

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

        TextureBrush backgroundImageTextureBrush = null;

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

        private bool stop = false;

        internal void Stop()
        {
            stop = true;
            backImage?.Dispose();
            backGraphics?.Dispose();

            Dispose();
        }
    }
}
