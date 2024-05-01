using System.Drawing;
using System.Drawing.Drawing2D;

namespace CuoreUI
{
    // change private to public to allow external use of this because why not
    public class Helper
    {
        public static GraphicsPath RoundRect(int x, int y, int width, int height, int borderRadius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rectangle = new Rectangle(x, y, width, height);

            int diameter = borderRadius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(rectangle.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter - 1;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter - 1;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        public static GraphicsPath RoundRect(Rectangle rectangle, int borderRadius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = borderRadius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(rectangle.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter - 1;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter - 1;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        public static GraphicsPath RoundRect(RectangleF rectangle, int borderRadius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = borderRadius * 2;
            Size size = new Size(diameter, diameter);
            RectangleF arc = new RectangleF(rectangle.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter - 1;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter - 1;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }


    }
}
