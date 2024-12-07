using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("ValueChanged")]
    public partial class cuiSliderVertical : UserControl
    {
        public cuiSliderVertical()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private bool privateUpsideDown = false;
        public bool UpsideDown
        {
            get
            {
                return privateUpsideDown;
            }
            set
            {
                privateUpsideDown = value;
                Refresh();
            }
        }

        private float privateValue = 100;
        private float privateMinValue = 0;
        private float privateMaxValue = 100;

        public float Value
        {
            get
            {
                return privateValue;
            }
            set
            {
                if (value >= privateMinValue && value <= privateMaxValue)
                {
                    privateValue = (int)value;

                    float tempY = Height - ((value / (float)(MaxValue - MinValue) * Height));
                    tempY = tempY - (Width / 2 + OutlineThickness * 2);
                    tempY = Math.Max(0, tempY);
                    tempY = Math.Min(tempY, Height - Width);
                    thumbY = tempY;

                    Refresh();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ValueChanged;

        public float MinValue
        {
            get
            {
                return privateMinValue;
            }
            set
            {
                if (value < privateMaxValue && value <= privateValue)
                {
                    privateMinValue = value;
                    Refresh();
                }
            }
        }

        public float MaxValue
        {
            get
            {
                return privateMaxValue;
            }
            set
            {
                if (value > privateMinValue && value >= privateValue)
                {
                    privateMaxValue = value;
                    Refresh();
                }
            }
        }

        private float privateOutlineThickness = 1.6f;
        public float OutlineThickness
        {
            get
            {
                return privateOutlineThickness;
            }
            set
            {
                privateOutlineThickness = value;
                Refresh();
            }
        }

        private float privatethumbY = 2;
        float thumbY
        {
            get
            {
                return privatethumbY;
            }
            set
            {
                privatethumbY = value;
                Refresh();
            }
        }

        private Color privateOutlineColor = Color.FromArgb(34, 34, 34);
        public Color OutlineColor
        {
            get
            {
                return privateOutlineColor;
            }
            set
            {
                privateOutlineColor = value;
                Refresh();
            }
        }

        private Color privateThumbColor = CuoreUI.Drawing.PrimaryColor;
        public Color ThumbColor
        {
            get
            {
                return privateThumbColor;
            }
            set
            {
                privateThumbColor = value;
                Refresh();
            }
        }

        private Color privateBackgroundColor = Color.FromArgb(10, 10, 10);
        public Color BackgroundColor
        {
            get
            {
                return privateBackgroundColor;
            }
            set
            {
                privateBackgroundColor = value;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Inflate(-1, -1);

            float ratio = Width - (OutlineThickness * 2);
            RectangleF thumbRectangle = new RectangleF(OutlineThickness, thumbY + OutlineThickness, ratio, ratio);
            thumbRectangle.Inflate(-(Width / 10), -(Width / 10));

            if (DesignStyle == Styles.Partial)
            {
                thumbRectangle.Inflate(-OutlineThickness, -OutlineThickness);
            }

            if (DesignStyle == Styles.Full)
            {
                GraphicsPath roundBackground = Helper.RoundRect(modifiedCR, (int)((Width / 2) - OutlineThickness));
                e.Graphics.FillPath(new SolidBrush(BackgroundColor), roundBackground);
                e.Graphics.DrawPath(new Pen(OutlineColor, OutlineThickness), roundBackground);
            }
            else if (DesignStyle == Styles.Partial)
            {
                Rectangle moddedCR = modifiedCR;
                moddedCR.Width = (int)(OutlineThickness * 2);
                moddedCR.X = (Width / 2) - (int)OutlineThickness;

                moddedCR.Inflate(0, -(int)(OutlineThickness * 2));
                moddedCR.Inflate(0, -(int)(thumbRectangle.Height / 2));

                GraphicsPath roundBackground = Helper.RoundRect(moddedCR, moddedCR.Width / 2);
                e.Graphics.FillPath(new SolidBrush(OutlineColor), roundBackground);
            }

            if (UpsideDown)
            {
                thumbRectangle.Y = Height - thumbRectangle.Bottom - OutlineThickness - thumbRectangle.Height * 0.125f + (0.1f * Width) - 2.5f;
            }
            else
            {
                thumbRectangle.Y = thumbRectangle.Top - OutlineThickness + thumbRectangle.Height * 0.125f - (0.1f * Width) - 5;
            }

            if (DesignStyle == Styles.Full)
            {
                if (UpsideDown)
                {
                    thumbRectangle.Y += 4f;
                }
                else
                {
                    thumbRectangle.Y += 7;
                }
            }

            GraphicsPath thumbPath = Helper.RoundRect(thumbRectangle, (int)(thumbRectangle.Width / 2));
            e.Graphics.FillPath(new SolidBrush(ThumbColor), thumbPath);
        }

        protected override void OnResize(EventArgs e)
        {
            Value = Value;
            base.OnResize(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OnMouseMove(new MouseEventArgs(MouseButtons.Left, 1, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y, 0));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                if (UpsideDown)
                {
                    if (e.Y <= OutlineThickness)
                    {
                        Value = MinValue;
                    }
                    else if (e.Y >= Height - OutlineThickness)
                    {
                        Value = MaxValue;
                    }
                    else
                    {
                        Value = MinValue + ((float)(e.Y - OutlineThickness) / (Height - OutlineThickness) * (MaxValue - MinValue));
                    }
                }
                else
                {
                    if (e.Y <= OutlineThickness)
                    {
                        Value = MaxValue;
                    }
                    else if (e.Y >= Height - OutlineThickness)
                    {
                        Value = MinValue;
                    }
                    else
                    {
                        Value = MaxValue - ((float)(e.Y - OutlineThickness) / (Height - OutlineThickness) * (MaxValue - MinValue));
                    }
                }
            }
        }
        public enum Styles
        {
            Full,
            Partial
        }

        public Styles privateDesignStyle = Styles.Partial;
        public Styles DesignStyle
        {
            get
            {
                return privateDesignStyle;
            }
            set
            {
                privateDesignStyle = value;
                Refresh();
            }
        }
    }
}
