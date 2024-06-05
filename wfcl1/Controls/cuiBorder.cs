using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiBorder : Control
    {
        private Color privatePanelColor = Color.MediumSlateBlue;
        public Color PanelColor
        {
            get
            {
                return privatePanelColor;
            }
            set
            {
                privatePanelColor = value;
                Invalidate();
            }
        }

        public cuiBorder()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }


        private Padding privateRounding = new Padding(8, 8, 8, 8);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Inflate(-1, -1);

            GraphicsPath roundBackground = Helper.RoundRect(modifiedCR, Rounding);
            using (SolidBrush brush = new SolidBrush(PanelColor))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }
        }
    }
}
