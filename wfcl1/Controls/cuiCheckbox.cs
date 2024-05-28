using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiCheckbox : cuiSwitch
    {

        Timer unlockedColorTimer = new Timer();
        Timer fixedTimeColorTimer = new Timer();

        public cuiCheckbox()
        {
            InitializeComponent();

            Size = new Size(24, 24);

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MinimumSize = new Size(24, 24);

            if (Checked)
            {
                ThumbRenderColor = CheckedForeground;
            }
            else
            {
                ThumbRenderColor = UncheckedForeground;
            }
            fixedTimeThumbRenderColor = ThumbRenderColor;

            //timers intervals
            unlockedColorTimer.Interval = 1000 / Helper.Win32.GetRefreshRate();
            fixedTimeColorTimer.Interval = 1;

            //locked fps timer
            fixedTimeColorTimer.Tick += (s, e) =>
            {
                if (UncheckedForeground == CheckedForeground)
                {
                    return;
                }
                if (Checked)
                {
                    int R = ((fixedTimeThumbRenderColor.R * 1) + CheckedForeground.R * 9) / 10;
                    int G = ((fixedTimeThumbRenderColor.G * 1) + CheckedForeground.G * 9) / 10;
                    int B = ((fixedTimeThumbRenderColor.B * 1) + CheckedForeground.B * 9) / 10;
                    fixedTimeThumbRenderColor = Color.FromArgb(R, G, B);
                }
                else
                {
                    int R = ((fixedTimeThumbRenderColor.R * 1) + UncheckedForeground.R * 9) / 10;
                    int G = ((fixedTimeThumbRenderColor.G * 1) + UncheckedForeground.G * 9) / 10;
                    int B = ((fixedTimeThumbRenderColor.B * 1) + UncheckedForeground.B * 9) / 10;
                    fixedTimeThumbRenderColor = Color.FromArgb(R, G, B);
                }

                //MessageBox.Show("a");
            };
            fixedTimeColorTimer.Start();

            //unlocked fps timer
            unlockedColorTimer.Tick += (s, e) =>
            {
                if (UncheckedForeground == CheckedForeground)
                {
                    return;
                }
                int R = ((ThumbRenderColor.R * 4) + fixedTimeThumbRenderColor.R) / 5;
                int G = ((ThumbRenderColor.G * 4) + fixedTimeThumbRenderColor.G) / 5;
                int B = ((ThumbRenderColor.B * 4) + fixedTimeThumbRenderColor.B) / 5;
                ThumbRenderColor = Color.FromArgb(R, G, B);
                Refresh();
                //MessageBox.Show("b");
            };
            unlockedColorTimer.Start();
        }

        private int privateThumbOffset = 1;
        public int ThumbOffset
        {
            get
            {
                return privateThumbOffset;
            }
            set
            {
                privateThumbOffset = value;
                Invalidate();
            }
        }

        private Color privateUncheckedForeground = Color.Black;
        new public Color UncheckedForeground
        {
            get
            {
                return privateUncheckedForeground;
            }
            set
            {
                privateUncheckedForeground = value;
                Invalidate();
            }
        }

        public Color CheckmarkColor { get; set; } = Color.White;
        public int CheckmarkThickness { get; set; } = 2;

        private Color fixedTimeThumbRenderColor;
        public Color ThumbRenderColor;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle squareClientRectangle = new Rectangle(0, 0, Height, Height);
            GraphicsPath roundBackground = Helper.RoundRect(squareClientRectangle, Height / 2);

            using (SolidBrush brush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }

            int thumbDim = Height - privateThumbOffset * 2;
            Rectangle thumbRect = new Rectangle(privateThumbOffset, privateThumbOffset, thumbDim, thumbDim);
            GraphicsPath roundThumbRect = Helper.RoundRect(thumbRect, (Height / 2) - privateThumbOffset);

            using (SolidBrush brush = new SolidBrush(ThumbRenderColor))
            {
                e.Graphics.FillPath(brush, roundThumbRect);
            }

            if (Checked)
            {
                Pen checkmarkPen = new Pen(CheckmarkColor, CheckmarkThickness);
                checkmarkPen.StartCap = LineCap.Round;
                checkmarkPen.EndCap = LineCap.Round;
                GraphicsPath checkmark = Helper.Checkmark(thumbRect);
                e.Graphics.DrawPath(checkmarkPen, checkmark);
            }
        }
    }
}
