using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI
{
    public partial class cuiBorder : UserControl
    {
        public Color PanelColor
        {
            get;
            set;
        } = Color.FromArgb(9, 125, 255);

        public cuiBorder()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            GraphicsPath roundBackground = Helper.RoundRect(ClientRectangle, 17);
            using (SolidBrush brush = new SolidBrush(PanelColor))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }
        }
    }
}
