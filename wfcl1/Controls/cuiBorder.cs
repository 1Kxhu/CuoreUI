using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI
{
    public partial class cuiBorder : UserControl
    {
        private Color privatePanelColor = Color.MediumSlateBlue;
        public Color PanelColor
        {
            get
            {
                return privatePanelColor;
            }
            set
            {
                privatePanelColor = value;
                Invalidate();
            }
        }

        public cuiBorder()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
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

            GraphicsPath roundBackground = Helper.RoundRect(ClientRectangle, Rounding);
            using (SolidBrush brush = new SolidBrush(PanelColor))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }
            base.OnPaint(e);

        }
    }
}
