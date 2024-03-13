using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace CuoreUI.Controls
{
    public partial class cuiCheckbox : cuiSwitch
    {
        public cuiCheckbox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
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
                if (value > 0)
                {
                    if (value > (ClientRectangle.Height / 2))
                    {
                        privateRounding = ClientRectangle.Height / 2;
                        Rounding = privateRounding;
                    }
                    else
                    {
                        privateRounding = value;
                    }
                }
                else
                {
                    throw new Exception("Rounding cannot be less than 1");
                }
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF squareClientRectangle = new Rectangle(0, 0, ClientRectangle.Height, ClientRectangle.Height);
            GraphicsPath roundBackground = Helper.RoundRect(squareClientRectangle, Rounding);

            using (SolidBrush brush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }

            int thumbDim = Height - 6;
            RectangleF thumbRect = new RectangleF(3, 3, thumbDim, thumbDim);
            GraphicsPath roundThumbRect = Helper.RoundRect(thumbRect, Rounding);

            if (Checked)
            {
                using (SolidBrush brush = new SolidBrush(CheckedForeground))
                {
                    e.Graphics.FillPath(brush, roundThumbRect);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(UncheckedForeground))
                {
                    e.Graphics.FillPath(brush, roundThumbRect);
                }
            }
        }
    }
}
