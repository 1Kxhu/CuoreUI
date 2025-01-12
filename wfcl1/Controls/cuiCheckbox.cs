﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(CheckBox))]
    public partial class cuiCheckbox : Control
    {
        public cuiCheckbox()
        {
            InitializeComponent();

            Size = new Size(16, 16);

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MinimumSize = new Size(16, 16);

            Content = this.Name;
        }

        public event EventHandler CheckedChanged;

        private bool privateChecked = false;
        [Description("Whether the switch is on or off.")]
        public bool Checked
        {
            get
            {
                return privateChecked;
            }
            set
            {
                if (value != privateChecked)
                {
                    privateChecked = value;
                    Invalidate();
                    CheckedChanged?.Invoke(this, EventArgs.Empty);
                }

            }
        }

        private Color privateCheckedForeground = CuoreUI.Drawing.PrimaryColor;
        [Description("The checked foreground.")]
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

        private Color privateUncheckedForeground = Color.FromArgb(34, 34, 34);
        [Description("The unchecked foreground.")]
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

        private bool privateOutlineStyle = true;
        [Description("The style of the outline.")]
        public bool OutlineStyle
        {
            get
            {
                return privateOutlineStyle;
            }
            set
            {
                privateOutlineStyle = value;
                Invalidate();
            }
        }

        private Color privateOutlineColor = Color.FromArgb(34, 34, 34);
        [Description("The color of the outline.")]
        public Color UncheckedOutlineColor
        {
            get
            {
                return privateOutlineColor;
            }
            set
            {
                privateOutlineColor = value;
                Invalidate();
            }
        }

        private Color privateCheckedOutlineColor = CuoreUI.Drawing.PrimaryColor;
        [Description("The color of the checked outline.")]
        public Color CheckedOutlineColor
        {
            get
            {
                return privateCheckedOutlineColor;
            }
            set
            {
                privateCheckedOutlineColor = value;
                Invalidate();
            }
        }

        private float privateOutlineThickness = 1f;
        [Description("The thickness of the outline.")]
        public float OutlineThickness
        {
            get
            {
                return privateOutlineThickness;
            }
            set
            {
                privateOutlineThickness = value;
                Invalidate();
            }
        }

        private string privateContent;
        public string Content
        {
            get
            {
                return privateContent;
            }
            set
            {
                privateContent = value;
                Refresh();
            }
        }

        public float CheckmarkThickness
        {
            get
            {
                return ((Math.Min(Width, Height) / 20f) + (OutlineThickness + 1)) * 0.5f;
            }
        }

        int privateRounding = 5;
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

        Color SymbolColor = Color.FromArgb(64, 255, 255, 255);
        [Description("The color of the symbol when NOT checked.")]
        public Color UncheckedSymbolColor
        {
            get
            {
                return SymbolColor;
            }
            set
            {
                SymbolColor = value;
                Invalidate();
            }
        }

        Color privateCheckedSymbolColor = Color.White;
        [Description("The color of the symbol when checked.")]
        public Color CheckedSymbolColor
        {
            get
            {
                return privateCheckedSymbolColor;
            }
            set
            {
                privateCheckedSymbolColor = value;
                Invalidate();
            }
        }

        private bool privateShowSymbols = true;
        public bool ShowSymbols
        {
            get
            {
                return privateShowSymbols;
            }
            set
            {
                privateShowSymbols = value;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Checked = !Checked;
            base.OnMouseClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            symbolsOffset = new Point(0, 1);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF squareClientRectangle = new RectangleF((OutlineThickness * 0.5f) + 0.6f, (OutlineThickness * 0.5f) + 0.6f, Height - OutlineThickness - 1.2f, Height - OutlineThickness - 1.2f);

            using (GraphicsPath roundBackgroundInside = Helper.RoundRect(squareClientRectangle, (int)(Rounding - OutlineThickness - 0.6f)))
            using (GraphicsPath roundBackground = Helper.RoundRect(squareClientRectangle, Rounding))
            {
                float thumbDim = Height - (int)(OutlineThickness * 2);
                RectangleF thumbRect = new RectangleF(squareClientRectangle.X + OutlineThickness, squareClientRectangle.Y + OutlineThickness, thumbDim - OutlineThickness - 1.2f, thumbDim - OutlineThickness - 1.2f);
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

                    using (Pen outlinePen = new Pen(UncheckedOutlineColor, OutlineThickness))
                    {
                        e.Graphics.DrawPath(outlinePen, roundBackground);
                    }
                }

                thumbRect.Inflate(0.4f, 0.4f);

                RectangleF textRect = thumbRect;

                if (ShowSymbols)
                {
                    if (Checked)
                    {
                        thumbRect.Offset(0.5f, -0.33f);
                        thumbRect.Inflate(0.25f, 0.25f);
                        using (Pen checkmarkPen = new Pen(CheckedSymbolColor, CheckmarkThickness))
                        using (GraphicsPath checkmark = Helper.Checkmark(thumbRect, symbolsOffset))
                        {
                            checkmarkPen.StartCap = LineCap.Round;
                            checkmarkPen.EndCap = LineCap.Round;
                            e.Graphics.DrawPath(checkmarkPen, checkmark);
                        }
                    }
                    else
                    {
                        RectangleF tempRectF = thumbRect;
                        tempRectF.Inflate(-(int)(Height / 6.2f), -(int)(Height / 6.2f));
                        tempRectF.Offset(0, -2.2f);
                        using (Pen checkmarkPen = new Pen(UncheckedSymbolColor, CheckmarkThickness))
                        using (GraphicsPath crossmark = Helper.Crossmark(tempRectF, symbolsOffset))
                        {
                            checkmarkPen.StartCap = LineCap.Round;
                            checkmarkPen.EndCap = LineCap.Round;
                            e.Graphics.DrawPath(checkmarkPen, crossmark);
                        }
                    }
                }

                using (SolidBrush brush = new SolidBrush(ForeColor))
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    textRect.Offset(textRect.Width + 4, 0.5f);
                    textRect.Width = Width;
                    e.Graphics.DrawString(Content, Font, brush, textRect);
                }
            }

            base.OnPaint(e);
        }
    }
}
