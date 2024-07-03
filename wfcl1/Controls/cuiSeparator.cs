using System.Drawing;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiSeparator : UserControl
    {
        public cuiSeparator()
        {
            InitializeComponent();
            ForeColor = Color.FromArgb(34, 34, 34);
        }

        private int privateSeparatorMargin = 8;
        public int SeparatorMargin
        {
            get
            {
                return privateSeparatorMargin;
            }
            set
            {
                privateSeparatorMargin = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int halfY = Height / 2;
            Rectangle lineRect = new Rectangle(SeparatorMargin, halfY, Width - (SeparatorMargin * 2), 1);
            using (Pen pen = new Pen(ForeColor))
            {
                e.Graphics.DrawRectangle(pen, lineRect);
            }
            base.OnPaint(e);
        }
    }
}
