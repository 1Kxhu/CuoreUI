using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(DateTimePicker))]
    public partial class cuiDateTimePicker : DateTimePicker
    {
        public cuiDateTimePicker()
        {
            InitializeComponent();
            Size = new Size(200, 21);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.FixedHeight, true);
        }

        private Padding privateRounding = new Padding(2);
        public Padding Rounding
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

        private Color privateNormalOutline = Color.FromArgb(60, 255, 255, 255);
        public Color Outline
        {
            get
            {
                return privateNormalOutline;
            }
            set
            {
                privateNormalOutline = value;
                Invalidate();
            }
        }

        private Color privateBackground = Color.Black;
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


        private Color privateArrowColor = Color.White;
        public Color ArrowColor
        {
            get
            {
                return privateArrowColor;
            }
            set
            {
                privateArrowColor = value;
                Invalidate();
            }
        }

        private Color privateTextColor = Color.White;
        public Color TextColor
        {
            get
            {
                return privateTextColor;
            }
            set
            {
                privateTextColor = value;
                Invalidate();
            }
        }

        string dayName
        {
            get
            {
                string day = Value.ToString("dddd");

                if (day.Length > 0)
                {
                    day = day.First().ToString().ToUpper() + day.Substring(1);
                }

                return day;
            }
        }

        string dateDetails
        {
            get
            {
                return Value.Day + ", " + Value.ToString("MMM") + ", " + Value.Year;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Background);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Offset(-1, -1);
            modifiedCR.Inflate(-1, -1);
            modifiedCR.Width += 1;
            modifiedCR.Height += 1;


            GraphicsPath roundBackground = Helper.RoundRect(modifiedCR, Rounding);

            e.Graphics.FillPath(new SolidBrush(Background), roundBackground);
            e.Graphics.DrawPath(new Pen(Outline), roundBackground);

            Rectangle arrowRectangle = modifiedCR;
            arrowRectangle.Width = arrowRectangle.Height;
            arrowRectangle.X = modifiedCR.Width - arrowRectangle.Width;

            arrowRectangle.Inflate(-5, -7); //magic values (fixed height so i'm allowed to do this :))) )
            arrowRectangle.Offset(1, 0);

            GraphicsPath arrow = Helper.DownArrow(arrowRectangle);

            e.Graphics.FillPath(new SolidBrush(ArrowColor), arrow);

            RectangleF textRectangle = modifiedCR;
            textRectangle.Y = modifiedCR.Height / 2 - Font.Height / 2 + 1.05f;
            textRectangle.X = Font.Height / 4 + 1;

            void drawStringAndAdd(string stringValue, bool bold = false)
            {
                Font tempFont = Font;
                if (bold)
                {
                    tempFont = new Font(Font, FontStyle.Bold);
                }

                e.Graphics.DrawString(stringValue, tempFont, new SolidBrush(TextColor), textRectangle);
                textRectangle.X += e.Graphics.MeasureString(stringValue, tempFont).Width;


            }

            drawStringAndAdd(dayName, true);
            drawStringAndAdd(dateDetails);

            base.OnPaint(e);
        }
    }
}
