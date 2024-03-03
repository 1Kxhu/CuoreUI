using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace wfcl1
{
    public partial class cuiLabel : UserControl
    {
        public cuiLabel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
        }

        private string privateContent = "Your text here!";
        public string Content
        {
            get
            {
                return Regex.Escape(privateContent);
            }
            set
            {
                privateContent = Regex.Unescape(value);
            }
        } 

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            StringFormat stringFormat = new StringFormat();

            switch (HorizontalAlignment)
            {
                case HorizontalAlignments.Left:
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case HorizontalAlignments.Center:
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignments.Right:
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }

            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(privateContent, Font, brush, ClientRectangle, stringFormat);
            }
        }

        private HorizontalAlignments privateHorizontalAlignment = HorizontalAlignments.Center;

        public HorizontalAlignments HorizontalAlignment
        {
            get
            {
                return privateHorizontalAlignment;
            }
            set
            {
                privateHorizontalAlignment = value;
                Invalidate();
            }
        }

        public enum HorizontalAlignments
        {
            Left,
            Center,
            Right
        }
    }
}
