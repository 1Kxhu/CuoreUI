using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [DefaultEvent("Click")]
    public partial class cuiButton : UserControl
    {
        public cuiButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ForeColor = Color.White;
            Font = new Font("Microsoft Sans Serif", 9.75f);
        }

        private string privateContent = "Your text here!";
        public string Content
        {
            get
            {
                return privateContent;
            }
            set
            {
                privateContent = value;
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

        private Color privateNormalBackground = Color.MediumSlateBlue;
        public Color NormalBackground
        {
            get
            {
                return privateNormalBackground;
            }
            set
            {
                privateNormalBackground = value;
                Invalidate();
            }
        }

        private Color privateHoverBackground = Color.MediumSlateBlue;
        public Color HoverBackground
        {
            get
            {
                return privateHoverBackground;
            }
            set
            {
                privateHoverBackground = value;
                Invalidate();
            }
        }

        private Color privatePressedBackground = Color.MediumSlateBlue;
        public Color PressedBackground
        {
            get
            {
                return privatePressedBackground;
            }
            set
            {
                privatePressedBackground = value;
                Invalidate();
            }
        }
        // 1 - normal
        // 2 - hover
        // 3 - press
        private int state = 1;
        private SolidBrush privateBrush = new SolidBrush(Color.Black);
        StringFormat stringFormat = new StringFormat();

        protected override void OnPaint(PaintEventArgs e)
        {
            stringFormat.Alignment = StringAlignment.Center;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            GraphicsPath roundBackground = Helper.RoundRect(ClientRectangle, Rounding);

            Color renderedBackgroundColor;
            switch (state)
            {
                case 2:
                    renderedBackgroundColor = HoverBackground;
                    break;

                case 3:
                    renderedBackgroundColor = PressedBackground;
                    break;

                case 1:
                    renderedBackgroundColor = NormalBackground;
                    break;

                default:
                    renderedBackgroundColor = Color.Black;
                    break;
            }

            privateBrush.Color = renderedBackgroundColor;
            e.Graphics.FillPath(privateBrush, roundBackground);

            Rectangle textRectangle = ClientRectangle;
            int textY = (Height / 2) - (Font.Height / 2);
            textRectangle.Location = new Point(0, textY);
            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(privateContent, Font, brush, textRectangle, stringFormat);
            }

            base.OnPaint(e);
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (ClientRectangle.Contains(e.Location))
            {
                state = 2;
                Invalidate();
            }
            else
            {
                state = 1;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            state = 1;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            state = 2;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            state = 3;
            Invalidate();
        }

    }
}
