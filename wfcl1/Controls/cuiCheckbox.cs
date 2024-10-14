using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(CheckBox))]
    public partial class cuiCheckbox : cuiSwitch
    {
        public cuiCheckbox()
        {
            InitializeComponent();

            Size = new Size(24, 24);

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MinimumSize = new Size(24, 24);
        }

        private float privateCheckmarkThickness;

        public float CheckmarkThickness
        {
            get
            {
                return privateCheckmarkThickness;
            }
            set
            {
                privateCheckmarkThickness = value;
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
                privateRounding = value;
                Invalidate();
            }
        }

        public Point symbolsOffset = new Point(0, 1);

        protected override void OnPaint(PaintEventArgs e)
        {
            symbolsOffset = new Point(0, 1);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int RoundingNormalized = Math.Min(Rounding, (int)((Height/2)-1));

            RectangleF squareClientRectangle = new RectangleF(OutlineThickness + 0.6f, OutlineThickness + 0.6f, Height - (OutlineThickness * 2) - 1.2f, Height - (OutlineThickness * 2) - 1.2f);

            GraphicsPath roundBackgroundInside = Helper.RoundRect(squareClientRectangle, (int)(RoundingNormalized - OutlineThickness - 0.6f));
            GraphicsPath roundBackground = Helper.RoundRect(squareClientRectangle, (int)(RoundingNormalized));

            using (SolidBrush brush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }

            float thumbDim = Height - (int)(OutlineThickness * 2);
            RectangleF thumbRect = new RectangleF(squareClientRectangle.X + OutlineThickness, squareClientRectangle.Y + OutlineThickness, thumbDim - (OutlineThickness * 2) - 1.2f, thumbDim - (OutlineThickness * 2) - 1.2f);
            thumbRect.Inflate(-1.0f, -1.0f);

            if (Checked)
            {
                using (SolidBrush brush = new SolidBrush(CheckedForeground))
                {
                    e.Graphics.FillPath(brush, roundBackgroundInside);
                }

                using (Pen outlinePen = new Pen(CheckedOutlineColor, OutlineThickness))
                {
                    e.Graphics.DrawPath(outlinePen, roundBackground);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(UncheckedForeground))
                {
                    e.Graphics.FillPath(brush, roundBackgroundInside);
                }

                using (Pen outlinePen = new Pen(OutlineColor, OutlineThickness))
                {
                    e.Graphics.DrawPath(outlinePen, roundBackground);
                }


            }

            thumbRect.Inflate(0.4f, 0.4f);

            if (ShowSymbols)
            {
                if (Checked)
                {
                    Pen checkmarkPen = new Pen(Background, CheckmarkThickness);
                    checkmarkPen.StartCap = LineCap.Round;
                    checkmarkPen.EndCap = LineCap.Round;
                    GraphicsPath checkmark = Helper.Checkmark(thumbRect, symbolsOffset);
                    e.Graphics.DrawPath(checkmarkPen, checkmark);
                }
                else
                {
                    RectangleF tempRectF = thumbRect;
                    tempRectF.Inflate(-(int)(Height / 6.2f), -(int)(Height / 6.2f));
                    tempRectF.Offset(0, -2.2f);
                    Pen checkmarkPen = new Pen(Background, CheckmarkThickness);
                    checkmarkPen.StartCap = LineCap.Round;
                    checkmarkPen.EndCap = LineCap.Round;
                    GraphicsPath crossmark = Helper.Crossmark(tempRectF, symbolsOffset);
                    e.Graphics.DrawPath(checkmarkPen, crossmark);
                }
            }
        }
    }
}
