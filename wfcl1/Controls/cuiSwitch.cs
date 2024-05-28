using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [DefaultEvent("Click")]
    public partial class cuiSwitch : UserControl
    {
        Timer timer = new Timer();

        public cuiSwitch()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            privateSmoothThumbX = thumbX;

            timer.Interval = 10;
            timer.Tick += (e, s) =>
            {
                privateSmoothThumbX = ((privateSmoothThumbX * 14) + thumbX) / 15;
                Refresh();
            };
            timer.Start();
        }

        private bool privateChecked = false;
        public bool Checked
        {
            get
            {
                return privateChecked;
            }
            set
            {
                privateChecked = value;
                Invalidate();
            }
        }

        private Color privateBackground = Color.Black;
        public Color Background
        {
            get
            {
                return privateBackground;
            }
            set
            {
                privateBackground = value;
                Invalidate();
            }
        }

        private Color privateCheckedForeground = Color.MediumSlateBlue;
        public Color CheckedForeground
        {
            get
            {
                return privateCheckedForeground;
            }
            set
            {
                privateCheckedForeground = value;
                Invalidate();
            }
        }

        private Color privateUncheckedForeground = Color.FromArgb(34, 34, 34);
        public Color UncheckedForeground
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

        public bool OutlineStyle { get; set; } = true;
        public Color OutlineColor { get; set; } = Color.FromArgb(34, 34, 34);
        public float OutlineThickness { get; set; } = 1;

        private int privateSmoothThumbX;
        private int thumbX;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int Rounding;
            try
            {
                Rounding = (Height / 2) - 1;
            }
            catch
            {
                Rounding = 1;
            }
            GraphicsPath roundBackground = Helper.RoundRect(ClientRectangle, Rounding);

            using (SolidBrush brush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }

            int thumbDim = Height - 7;
            RectangleF thumbRect = new RectangleF(thumbX, 3, thumbDim, thumbDim);

            if (Checked)
            {
                thumbRect.X = Width - 5 - thumbDim;
                using (SolidBrush brush = new SolidBrush(CheckedForeground))
                {
                    e.Graphics.FillEllipse(brush, thumbRect);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(UncheckedForeground))
                {
                    e.Graphics.FillEllipse(brush, thumbRect);
                }
            }

            using (Pen outlinePen = new Pen(OutlineColor, OutlineThickness))
            {
                e.Graphics.DrawPath(outlinePen, roundBackground);
            }

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            thumbX = 3;
            if (Width > 0)
            {
                int thumbDim = Height - 7;
                thumbX = Math.Min(Width - 5 - thumbDim, 3);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Checked = !Checked;
        }
    }
}
