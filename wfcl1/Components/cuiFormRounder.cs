using CuoreUI.Components.cuiFormRounderV2Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CuoreUI.Components
{
    public partial class cuiFormRounder : Component
    {
        public cuiFormRounder()
        {
        }

        public RoundedForm RoundedForm
        {
            get; private set;
        }
        public Form FakeForm { get; } = new FakeForm();

        private Form privateTargetForm;
        public Form TargetForm
        {
            get => privateTargetForm;
            set
            {
                privateTargetForm = value;
                if (privateTargetForm == null)
                    return;

                TargetForm.Load += TargetForm_Load;
                TargetForm.Resize += TargetForm_Resize;
                TargetForm.LocationChanged += TargetForm_LocationChanged;
                TargetForm.TextChanged += TargetForm_TextChanged;
                TargetForm.FormClosing += TargetForm_FormClosing;
                TargetForm.VisibleChanged += TargetForm_VisibleChanged;
                TargetForm.BackColorChanged += TargetForm_BackColorChanged;

                FakeForm.Activated += FakeForm_Activated;
                FakeForm.FormClosing += TargetForm_FormClosing;
            }
        }

        private void TargetForm_BackColorChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                FakeForm.BackColor = TargetForm.BackColor;
            }
        }

        private void TargetForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RoundedForm.Visible = TargetForm.Visible;
            }
        }

        private void TargetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DesignMode && !e.Cancel)
            {
                RoundedForm.Dispose();
                FakeForm.Dispose();
                TargetForm.Dispose();
                Environment.Exit(0);
            }
        }

        public void FakeForm_Activated(object sender, EventArgs e)
        {
            if (!DesignMode && TargetForm != null && RoundedForm != null)
            {
                FakeForm.Icon = TargetForm.Icon;
                if (RoundedForm.WindowState == FormWindowState.Minimized)
                {
                    TargetForm.WindowState = FormWindowState.Normal;
                }
                else
                {
                    if (TargetForm.WindowState == FormWindowState.Normal)
                    {
                        SetWindowPos(TargetForm.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                        SetWindowPos(RoundedForm.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

                        SetWindowPos(RoundedForm.Handle, TargetForm.Handle, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    }
                }
            }
        }

        private void TargetForm_TextChanged(object sender, EventArgs e)
        {
            FakeForm.Text = TargetForm.Text;
        }

        private static Point PointSubtract(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        private int privateRounding = 8;
        public int Rounding
        {
            get => privateRounding;
            set
            {
                privateRounding = value;
                Stored.rounding = value;
                RoundedForm?.Invalidate();
            }
        }

        private void TargetForm_LocationChanged(object sender, EventArgs e)
        {
            if (!DesignMode && RoundedForm != null && TargetForm != null)
            {
                RoundedForm.Location = PointSubtract(TargetForm.Location, new Point(2, 2));
                FakeForm.Location = TargetForm.Location;
            }
        }

        private Color privateOutlineColor = Color.FromArgb(30, 255, 255, 255);
        public Color OutlineColor
        {
            get => privateOutlineColor;
            set
            {
                privateOutlineColor = value;
                RoundedForm?.Invalidate();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010;
        private static readonly IntPtr HWND_TOP = new IntPtr(0);


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void TargetForm_Load(object sender, EventArgs e)
        {
            FakeForm_Activated(sender, e);

            TargetForm.Opacity = 0;
            TargetForm.ShowInTaskbar = false;
            TargetForm.FormBorderStyle = FormBorderStyle.None;

            FakeForm.ShowInTaskbar = true;
            FakeForm.Opacity = 0;

            RoundedForm = new RoundedForm(TargetForm.BackColor, OutlineColor);
            TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));

            RoundedForm.Show();
            FakeForm.Show();

            TargetForm.Opacity = 1;

            RoundedForm.Activated += FakeForm_Activated;
            TargetForm_LocationChanged(this, EventArgs.Empty);
            TargetForm_Resize(this, EventArgs.Empty);

            TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));

            Timer miscTimer = new Timer { Interval = 1000 };
            miscTimer.Tick += (a1, a2) =>
            {
                if (!DesignMode)
                {
                    if (TargetForm.WindowState == FormWindowState.Minimized)
                    {
                        FakeForm.WindowState = FormWindowState.Minimized;
                        RoundedForm.WindowState = FormWindowState.Minimized;
                        TargetForm.WindowState = FormWindowState.Minimized;
                        return;
                    }

                    FakeForm_Activated(sender, e);
                }
            };
            miscTimer.Start();
        }

        private void TargetForm_Resize(object sender, EventArgs e)
        {
            if (RoundedForm != null && TargetForm != null)
            {
                RoundedForm.Size = Size.Add(TargetForm.Size, new Size(4, 4));
                FakeForm.Size = TargetForm.Size;
                RoundedForm.InvalidateNextDrawCall = true;
                TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));
            }
        }
    }
}
