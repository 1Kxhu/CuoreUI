﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiButton : UserControl
    {
        public cuiButton()
        {
            InitializeComponent();
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

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
