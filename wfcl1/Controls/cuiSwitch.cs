using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiSwitch : UserControl
    {
        public cuiSwitch()
        {
            InitializeComponent();
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

        private Color privateBackground = Color.FromArgb(222, 222, 222);
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

        private Color privateUncheckedForeground = Color.DarkGray;
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

            int thumbDim = Height - 6;
            RectangleF thumbRect = new RectangleF(3, 3, thumbDim, thumbDim);

            if (Checked)
            {
                thumbRect.X = Width - 3 - thumbDim;
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
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Checked = !Checked;
        }
    }
}
