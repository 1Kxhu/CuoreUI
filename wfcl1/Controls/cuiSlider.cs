using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [DefaultEvent("ValueChanged")]
    public partial class cuiSlider : UserControl
    {
        public cuiSlider()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
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

                    float tempX = (value / (float)(MaxValue - MinValue) * Width) * 1;
                    tempX = tempX - (Height / 2 + OutlineThickness * 2);
                    tempX = Math.Max(0, tempX);
                    tempX = Math.Min(tempX, Width - Height);
                    thumbX = tempX;

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

        private float privatethumbX = 2;
        float thumbX
        {
            get
            {
                return privatethumbX;
            }
            set
            {
                privatethumbX = value;
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

            float ratio = Height - (OutlineThickness * 2);
            RectangleF thumbRectangle = new RectangleF(thumbX + OutlineThickness, OutlineThickness, ratio, ratio);
            thumbRectangle.Inflate(-(Height / 10), -(Height / 10));

            if (DesignStyle == Styles.Partial)
            {
                thumbRectangle.Inflate(-OutlineThickness, -OutlineThickness);
            }

            if (DesignStyle == Styles.Full)
            {
                GraphicsPath rounbackground = Helper.RoundRect(modifiedCR, (int)((Height / 2) - OutlineThickness));
                e.Graphics.FillPath(new SolidBrush(BackgroundColor), rounbackground);
                e.Graphics.DrawPath(new Pen(OutlineColor, OutlineThickness), rounbackground);
            }
            else if (DesignStyle == Styles.Partial)
            {
                Rectangle moddedCR = modifiedCR;
                moddedCR.Height = (int)(OutlineThickness * 2);
                moddedCR.Y = (Height / 2) - (int)OutlineThickness;

                moddedCR.Inflate(-(int)(OutlineThickness * 2), 0);
                moddedCR.Inflate(-(int)(thumbRectangle.Width / 2), 0);

                GraphicsPath rounbackground = Helper.RoundRect(moddedCR, moddedCR.Height / 2);
                e.Graphics.FillPath(new SolidBrush(OutlineColor), rounbackground);
            }


            GraphicsPath thumbPath = Helper.RoundRect(thumbRectangle, (int)(thumbRectangle.Height / 2));
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
                if (e.X >= Width - (OutlineThickness * 5) - (Height / 10))
                {
                    Value = MaxValue;
                }
                else if (e.X <= OutlineThickness)
                {
                    Value = MinValue;
                }
                else
                {
                    Value = ((float)(e.X - ((OutlineThickness - (Height / 10)))) / (Width - ((OutlineThickness - (Height / 10)))) * (MaxValue - MinValue));
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
