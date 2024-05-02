using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

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

        public static class Win32
        {
            // Define the necessary structures and Win32 API functions
            [StructLayout(LayoutKind.Sequential)]
            public struct DEVMODE
            {
                private const int CCHDEVICENAME = 32;
                private const int CCHFORMNAME = 32;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
                public string dmDeviceName;
                public short dmSpecVersion;
                public short dmDriverVersion;
                public short dmSize;
                public short dmDriverExtra;
                public int dmFields;
                public int dmPositionX;
                public int dmPositionY;
                public int dmDisplayOrientation;
                public int dmDisplayFixedOutput;
                public short dmColor;
                public short dmDuplex;
                public short dmYResolution;
                public short dmTTOption;
                public short dmCollate;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
                public string dmFormName;
                public short dmLogPixels;
                public int dmBitsPerPel;
                public int dmPelsWidth;
                public int dmPelsHeight;
                public int dmDisplayFlags;
                public int dmDisplayFrequency;
                public int dmICMMethod;
                public int dmICMIntent;
                public int dmMediaType;
                public int dmDitherType;
                public int dmReserved1;
                public int dmReserved2;
                public int dmPanningWidth;
                public int dmPanningHeight;
            }

            [DllImport("user32.dll")]
            public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

            public static int GetRefreshRate()
            {
                DEVMODE devMode = new DEVMODE();
                devMode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

                if (EnumDisplaySettings(null, -1, ref devMode))
                {
                    return devMode.dmDisplayFrequency;
                }
                else
                {
                    return 60;
                }
            }
        }
    }
}
