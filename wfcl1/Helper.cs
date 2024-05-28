﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CuoreUI
{
    public static class Helper
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

        public static GraphicsPath Checkmark(Rectangle area)
        {
            GraphicsPath path = new GraphicsPath();

            Point[] points = new Point[]
            {
            new Point(area.Left + (int)(area.Width * 0.25), area.Top + (int)(area.Height * 0.5)),
            new Point(area.Left + (int)(area.Width * 0.45), area.Top + (int)(area.Height * 0.7)),
            new Point(area.Right - (int)(area.Width * 0.3), area.Top + (int)(area.Height * 0.3))
            };

            path.AddLines(points);

            return path;
        }

        public static GraphicsPath Crossmark(Rectangle rect)
        {
            Rectangle area = rect;
            int WidthBeforeScale = area.Width;
            area.Width = (int)Math.Round(area.Width * 0.7f, 0);
            area.Height = area.Width;

            int WidthAfterScale = area.Width;
            int WidthDifference = WidthBeforeScale - WidthAfterScale;

            area.Offset(WidthDifference / 2, 1 + (WidthDifference / 2));

            GraphicsPath path = new GraphicsPath();

            Point[] points = new Point[]
            {
            new Point(area.Left, area.Top),
            new Point(area.Right, area.Bottom)
            };

            path.AddLines(points);

            GraphicsPath path2 = new GraphicsPath();

            Point[] points2 = new Point[]
            {
            new Point(area.Left, area.Bottom),
            new Point(area.Right, area.Top)
            };

            path2.AddLines(points2);

            path.AddPath(path2, false);

            return path;
        }

        public static GraphicsPath Plus(Rectangle rect)
        {
            Rectangle area = rect;
            int widthBeforeScale = area.Width;
            area.Width = (int)Math.Round(area.Width * 0.7f, 0);
            area.Height = area.Width;

            int widthAfterScale = area.Width;
            int widthDifference = widthBeforeScale - widthAfterScale;

            area.Offset(widthDifference / 2, 1 + (widthDifference / 2));

            GraphicsPath path = new GraphicsPath();

            Point[] horizontalPoints = new Point[]
{
        new Point(area.Left, area.Top + (area.Height / 2)),
        new Point(area.Right, area.Top + (area.Height / 2))
};

            path.AddLines(horizontalPoints);

            GraphicsPath path2 = new GraphicsPath();

            Point[] verticalPoints = new Point[]
{
        new Point(area.Left + (area.Width / 2), area.Top),
        new Point(area.Left + (area.Width / 2), area.Bottom)
};

            path2.AddLines(verticalPoints);
            path.AddPath(path2, false);

            return path;
        }

        private static int arrowThicknessHardcoded = 2;

        public static GraphicsPath LeftArrow(Rectangle rect)
        {
            rect.Height = rect.Width;
            rect.Width = rect.Width / 2;

            GraphicsPath path = new GraphicsPath();
            path.AddLine(rect.Right, rect.Top, rect.Left, rect.Bottom / 2);
            path.AddLine(rect.Left, rect.Bottom / 2, rect.Right, rect.Bottom);

            Matrix translateMatrix = new Matrix();
            translateMatrix.Translate(3 + (arrowThicknessHardcoded * 2), 4 + (arrowThicknessHardcoded + rect.Height / 2));
            translateMatrix.Scale(1, 0.9f);
            translateMatrix.Scale(0.6f, 0.6f);
            path.Transform(translateMatrix);

            return path;
        }

        public static GraphicsPath RightArrow(Rectangle rect)
        {
            rect.Height = rect.Width;
            rect.Width = rect.Width / 2;

            rect.Offset(rect.Width - arrowThicknessHardcoded, 0);

            GraphicsPath path = new GraphicsPath();
            path.AddLine(rect.Left, rect.Top, rect.Right, rect.Bottom / 2);
            path.AddLine(rect.Right, rect.Bottom / 2, rect.Left, rect.Bottom);

            Matrix translateMatrix = new Matrix();
            translateMatrix.Translate(13 + (-arrowThicknessHardcoded * 2), 4 + (arrowThicknessHardcoded + rect.Height / 2));
            translateMatrix.Scale(1, 0.9f);
            translateMatrix.Scale(0.6f, 0.6f);
            path.Transform(translateMatrix);

            return path;
        }

        public static void CopyProperties(this object source, object destination)
        {
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");

            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        public static class Win32
        {
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
