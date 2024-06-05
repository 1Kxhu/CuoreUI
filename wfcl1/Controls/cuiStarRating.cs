using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
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
        private Color privateStarColor = Color.MediumSlateBlue;
        private int privateStarBorderSize = 1;

        // Public properties to access private variables

        public int StarCount
        {
            get
            {
                return privateStarCount;
            }
            set
            {
                privateStarCount = value;
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
                GraphicsPath starPath = Helper.Star(starLeft + starWidth / 2, this.Height / 2, starWidth / 2, starWidth / 3.8f, 5);

                if ((i + 1) * 2 <= Rating)
                {
                    e.Graphics.FillPath(new SolidBrush(StarColor), starPath);
                }
                else if (i * 2 + 1 == Rating)
                {
                    e.Graphics.FillPath(new SolidBrush(StarColor), starPath);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), starRect);
                }

                using (Pen starBorderPen = new Pen(StarColor, StarBorderSize))
                {
                    e.Graphics.DrawPath(starBorderPen, starPath);
                }
            }
        }
    }
}
