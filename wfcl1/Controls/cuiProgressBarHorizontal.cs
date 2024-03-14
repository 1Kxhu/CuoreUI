﻿using CuoreUI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace wfcl1
{
    public partial class cuiProgressBarHorizontal : UserControl
    {
        public cuiProgressBarHorizontal()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private bool privateRoundedCorners = true;
        public bool RoundedCorners
        {
            get
            {
                return privateRoundedCorners;
            }
            set
            {
                privateRoundedCorners = value;
                Invalidate();
            }
        }

        private int privateValue = 50;
        public int Value
        {
            get
            {
                return privateValue;
            }
            set
            {
                if (value > MaxValue)
                {
                    throw new Exception("Value cannot be more than the MaxValue");
                }
                else
                {
                    privateValue = value;
                }
                Invalidate();
            }
        }

        private int privateMaxValue = 100;
        public int MaxValue
        {
            get
            {
                return privateMaxValue;
            }
            set
            {
                if (value < privateValue)
                {
                    throw new Exception("MaxValue cannot be less than the Value");
                }
                else
                {
                    privateMaxValue = value;
                }
                Invalidate();
            }
        }

        private bool privateFlipped = false;
        public bool Flipped
        {
            get
            {
                return privateFlipped;
            }
            set
            {
                privateFlipped = value;
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

        private Color privateForeground = Color.FromArgb(50, 240, 117);
        public Color Foreground
        {
            get
            {
                return privateForeground;
            }
            set
            {
                privateForeground = value;
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


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Bitmap tempBitmap = new Bitmap(ClientSize.Width * 2, ClientSize.Height * 2);
            using (Graphics tempGraphics = Graphics.FromImage(tempBitmap))
            {
                tempGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                tempGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                tempGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                tempGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                GraphicsPath roundBackground = Helper.RoundRect(new Rectangle(0, 0, ClientSize.Width * 2, ClientSize.Height * 2), Rounding * 2);
                tempGraphics.SetClip(roundBackground);

                float filledPercent = (float)Value / MaxValue;
                float foreWidth = ClientRectangle.Width * filledPercent * 2;
                RectangleF foreHalf = new RectangleF(0, 0, foreWidth, ClientRectangle.Height * 2 + 1);
                RectangleF client = new RectangleF(foreWidth - Rounding, 0, ClientRectangle.Width * 2 - foreWidth + (Rounding * 2), ClientRectangle.Height * 2);


                using (SolidBrush brush = new SolidBrush(Background))
                {
                    tempGraphics.FillRectangle(brush, client);
                }

                if (RoundedCorners)
                {
                    GraphicsPath graphicsPath = Helper.RoundRect(foreHalf, Rounding);

                    using (SolidBrush brush = new SolidBrush(Foreground))
                    {
                        tempGraphics.FillPath(brush, graphicsPath);
                    }
                }
                else
                {

                    using (SolidBrush brush = new SolidBrush(Foreground))
                    {
                        tempGraphics.FillRectangle(brush, foreHalf);
                    }
                }

            }

            if (Flipped)
            {
                tempBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }


            e.Graphics.DrawImage(tempBitmap, ClientRectangle);

            tempBitmap.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}
