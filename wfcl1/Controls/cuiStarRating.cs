﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiStarRating : Control
    {
        public cuiStarRating()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Size = new Size(150, 28);
        }

        private int privateStarCount = 5;
        private int privateRating = 2;
        private Color privateStarColor = Color.Coral;
        private int privateStarBorderSize = 1;

        public int StarCount
        {
            get
            {
                return privateStarCount;
            }
            set
            {
                privateStarCount = value;
                Invalidate();
            }
        }

        public int Rating
        {
            get
            {
                return privateRating;
            }
            set
            {
                privateRating = value;
                Invalidate();
            }
        }

        public Color StarColor
        {
            get
            {
                return privateStarColor;
            }
            set
            {
                privateStarColor = value;
                Invalidate();
            }
        }

        public int StarBorderSize
        {
            get
            {
                return privateStarBorderSize;
            }
            set
            {
                privateStarBorderSize = value;
                Invalidate();
            }
        }

        private bool privateAllowUserInteraction = true;
        public bool AllowUserInteraction
        {
            get
            {
                return privateAllowUserInteraction;
            }
            set
            {
                privateAllowUserInteraction = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            int starWidth = Height - 2;
            int spacing = starWidth / 5;

            for (int i = 0; i < StarCount; i++)
            {
                int starLeft = i * (starWidth + spacing);
                Rectangle starRect = new Rectangle(starLeft, 0, starWidth, this.Height);
                starRect.Offset(starWidth / 2, 0);
                starRect.Inflate(-StarBorderSize, -StarBorderSize);
                starRect.Offset(StarBorderSize / 2, StarBorderSize / 2);

                GraphicsPath starPath = Helper.Star(starLeft + starWidth / 2, Height / 2, starWidth / 2, starWidth / 3.8f, 5);

                if ((i + 1) * 2 <= Rating)
                {
                    e.Graphics.FillPath(new SolidBrush(StarColor), starPath);
                }
                else if (i * 2 + 1 == Rating)
                {
                    e.Graphics.FillPath(new SolidBrush(StarColor), starPath);

                    starRect.Inflate(StarBorderSize, StarBorderSize);
                    starRect.Offset(-(StarBorderSize / 2), -(StarBorderSize / 2));
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), starRect);
                }

                using (Pen starBorderPen = new Pen(StarColor, StarBorderSize / 2f))
                {
                    e.Graphics.DrawPath(starBorderPen, starPath);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (AllowUserInteraction == false)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                int starWidth = Height - 2;
                int spacing = starWidth / 5;
                int starCount = 5;

                int mouseX = e.X + 5;

                if (mouseX < 0)
                {
                    Rating = 0;
                }
                else if (mouseX > starCount * (starWidth + spacing))
                {
                    Rating = 10;
                }
                else
                {
                    int starClicked = (mouseX - spacing) / (starWidth + spacing);
                    float remainder = (mouseX - spacing) % (starWidth + spacing);

                    if (remainder > starWidth / 2)
                    {
                        Rating = (starClicked + 1) * 2;
                    }
                    else
                    {
                        Rating = starClicked * 2 + 1;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            OnMouseMove(e);
        }
    }
}
