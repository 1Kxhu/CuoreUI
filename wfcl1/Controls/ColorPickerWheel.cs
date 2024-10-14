using CuoreUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(ColorDialog))]
    public partial class ColorPickerWheel : cuiPictureBox
    {
        Point lastClickPosition = new Point(-8, -8);

        public ColorPickerWheel()
        {
            Content = Resources.all_colours;
        }

        public void UpdatePos()
        {
            lastClickPosition = PointToClient(Cursor.Position);
            Refresh();
        }

        Pen whitepen = new Pen(Color.White, 1);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int size = 8;

            RectangleF circleRect = new RectangleF(lastClickPosition.X - size, lastClickPosition.Y - size, size*2, size*2);

            e.Graphics.DrawEllipse(whitepen, circleRect);
        }
    }
}
