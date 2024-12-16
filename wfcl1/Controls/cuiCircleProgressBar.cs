﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(ProgressBar))]
    public partial class cuiCircleProgressBar : UserControl
    {
        public cuiCircleProgressBar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private int privateBorderWidth = 12;
        public int BorderWidth
        {
            get
            {
                return privateBorderWidth;
            }
            set
            {
                privateBorderWidth = value;
                Invalidate();
            }
        }

        private int privateProgressValue = 50;
        public int ProgressValue
        {
            get
            {
                return privateProgressValue;
            }
            set
            {
                privateProgressValue = value;
                Invalidate();
            }
        }

        private int privateMinimumValue = 0;
        public int MinimumValue
        {
            get
            {
                return privateMinimumValue;
            }
            set
            {
                privateMinimumValue = value;
                Invalidate();
            }
        }

        private int privateMaximumValue = 100;
        public int MaximumValue
        {
            get
            {
                return privateMaximumValue;
            }
            set
            {
                privateMaximumValue = value;
                Invalidate();
            }
        }

        private Color privateNormalColor = Color.FromArgb(34, 34, 34);
        public Color NormalColor
        {
            get
            {
                return privateNormalColor;
            }
            set
            {
                privateNormalColor = value;
                Invalidate();
            }
        }

        private Color privateProgressColor = CuoreUI.Drawing.PrimaryColor;
        public Color ProgressColor
        {
            get
            {
                return privateProgressColor;
            }
            set
            {
                privateProgressColor = value;
                Invalidate();
            }
        }

        private bool privateRoundedEnds = true;
        public bool RoundedEnds
        {
            get
            {
                return privateRoundedEnds;
            }
            set
            {
                privateRoundedEnds = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float percent;
            try
            {
                percent = (float)(ProgressValue - MinimumValue) / (MaximumValue - MinimumValue) * 100;
            }
            catch
            {
                return;
            }
            
            int circleWidth = Width - BorderWidth - 1;
            int circleHeight = Height - BorderWidth - 1;
            int borderHalf = BorderWidth / 2;

            MinimumSize = new Size(BorderWidth * 2, BorderWidth * 2);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(NormalColor, BorderWidth))
            {
                path.AddArc(new Rectangle(borderHalf, borderHalf, circleWidth, circleHeight), (percent * 3.6f) - 92, 360 - (percent * 3.6f));
                e.Graphics.DrawPath(pen, path);
            }

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(ProgressColor, BorderWidth))
            {
                if (RoundedEnds)
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                }

                path.AddArc(new Rectangle(borderHalf, borderHalf, circleWidth, circleHeight), -92, percent * 3.6f);
                e.Graphics.DrawPath(pen, path);
            }

            base.OnPaint(e);
        }
    }
}
