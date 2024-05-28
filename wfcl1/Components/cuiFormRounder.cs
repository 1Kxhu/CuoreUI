using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    public partial class cuiFormRounder : Component
    {
        private Timer drawTimer = new Timer();

        private Form targetForm;
        public Form TargetForm
        {
            get
            {
                return targetForm;
            }
            set
            {
                targetForm = value;
                if (!DesignMode)
                {
                    DrawForm(null, null);
                    targetForm.Resize += OnPaint;
                    targetForm.Paint += OnPaint;
                    drawTimer.Interval = UpdateFrequency;
                    drawTimer.Tick += DrawForm;
                    drawTimer.Start();
                }
            }
        }

        private int privateUpdateFrequency = 32;
        public int UpdateFrequency
        {
            get
            {
                return privateUpdateFrequency;
            }
            set
            {
                privateUpdateFrequency = value;
            }
        }

        private int privateSizableOffset = 32;
        public int SizableOffset
        {
            get
            {
                return privateSizableOffset;
            }
            set
            {
                privateSizableOffset = value;
            }
        }

        private int privateRounding = 10;
        public int Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                privateRounding = value;
            }
        }

        private Color privateBorderColor = Color.White;
        public Color BorderColor
        {
            get
            {
                return privateBorderColor;
            }
            set
            {
                privateBorderColor = value;
                OnPaint(null, null);
            }
        }

        private void OnPaint(object sender, EventArgs e)
        {
            DrawForm(null, null);
        }

        private Bitmap backImage; private Graphics backGraphics;
        private void DrawForm(object pSender, EventArgs pE)
        {
            drawTimer.Interval = UpdateFrequency;
            ModifyFormStyles();

            if (targetForm != null)
            {
                int tempSizableXOffset = Rounding - 1;
                int tempSizableOffset = SizableOffset;
                if (targetForm.FormBorderStyle == FormBorderStyle.None)
                {
                    tempSizableOffset = 0;
                    tempSizableXOffset = 0;
                }

                if (backImage == null || backImage.Size != targetForm.Size)
                {
                    backImage?.Dispose();
                    backImage = new Bitmap(targetForm.Width, targetForm.Height);
                    backGraphics?.Dispose();
                    backGraphics = Graphics.FromImage(backImage);
                    backGraphics.SmoothingMode = SmoothingMode.None;
                }
                else
                {
                    backGraphics.Clear(Color.Transparent);
                }

                Rectangle gradientRectangle = new Rectangle(0, SizableOffset, targetForm.Width - 1, targetForm.Height - 1 - tempSizableOffset);
                GraphicsPath roundedRectangle = Helper.RoundRect(gradientRectangle, Rounding);

                using (SolidBrush brush = new SolidBrush(targetForm.BackColor))
                using (Pen pen = new Pen(BorderColor))
                {
                    backGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    backGraphics.FillPath(brush, roundedRectangle);
                    backGraphics.DrawPath(pen, roundedRectangle);
                    backGraphics.SmoothingMode = SmoothingMode.None;

                    foreach (Control ctrl in targetForm.Controls)
                    {
                        Point loc = ctrl.Location;
                        loc.Y += SizableOffset;
                        loc.X += tempSizableXOffset;
                        Rectangle rect = new Rectangle(loc, ctrl.Size);
                        ctrl.DrawToBitmap(backImage, rect);
                    }
                }

                PerPixelAlphaBlend.SetBitmap(backImage, targetForm.Left, targetForm.Top, targetForm.Handle);
            }
        }

        private void ModifyFormStyles()
        {
            if (targetForm != null && !DesignMode)
            {
                var cp = NativeMethods.GetWindowLong(targetForm.Handle, NativeMethods.GWL_EXSTYLE);
                cp |= NativeMethods.WS_EX_LAYERED;
                NativeMethods.SetWindowLong(targetForm.Handle, NativeMethods.GWL_EXSTYLE, cp);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                drawTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    internal static class NativeMethods
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

        internal const int LWA_ALPHA = 0x2;
    }

    internal static class PerPixelAlphaBlend
    {
        public static void SetBitmap(Bitmap bitmap, int left, int top, IntPtr handle)
        {
            SetBitmap(bitmap, 255, left, top, handle);
        }

        public unsafe static void SetBitmap(Bitmap bitmap, byte opacity, int left, int top, IntPtr handle)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");


            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBitmap = Win32.SelectObject(memDc, hBitmap);

                Win32.Size size = new Win32.Size(bitmap.Width, bitmap.Height);
                Win32.Point topPos = new Win32.Point(left, top);
                Win32.Point pointSource = new Win32.Point(0, 0);
                Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION
                {
                    BlendOp = Win32.AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = opacity,
                    AlphaFormat = Win32.AC_SRC_ALPHA
                };

                Win32.UpdateLayeredWindow(handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);

            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBitmap);
                    Win32.DeleteObject(hBitmap);
                }

                Win32.DeleteDC(memDc);
            }
        }
    }

    internal static class Win32
    {
        public enum Bool
        {
            False = 0,
            True
        };


        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(Int32 x, Int32 y)
            {
                this.x = x;
                this.y = y;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 cx, Int32 cy)
            {
                this.cx = cx;
                this.cy = cy;
            }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }


        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;


        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);


    }
}
