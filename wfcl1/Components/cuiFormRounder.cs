using CuoreUI.Components.cuiFormRounderV2Resources;
using CuoreUI.Components.Forms.cuiFormRounderV2Resources;
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

        public RoundedForm roundedFormObj;
        public Form FakeForm { get; internal set; } = new FakeForm();

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
            if (stop)
            {
                return;
            }
            if (!DesignMode)
            {
                FakeForm.BackColor = TargetForm.BackColor;
            }
        }

        private void TargetForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                roundedFormObj.Visible = TargetForm.Visible;
                roundedFormObj.Tag = TargetForm.Opacity;
                roundedFormObj.InvalidateNextDrawCall = true;
            }
        }

        private bool stop = false;
        private bool sent = false;

        private void TargetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!sent)
            {
                sent = true;
                stop = true;

                TargetForm.Load -= TargetForm_Load;
                TargetForm.Resize -= TargetForm_Resize;
                TargetForm.LocationChanged -= TargetForm_LocationChanged;
                TargetForm.TextChanged -= TargetForm_TextChanged;
                TargetForm.FormClosing -= TargetForm_FormClosing;
                TargetForm.VisibleChanged -= TargetForm_VisibleChanged;
                TargetForm.BackColorChanged -= TargetForm_BackColorChanged;

                TargetForm.Controls.Clear();

                Helper.Win32.SendMessage(FakeForm.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);
                Helper.Win32.SendMessage(TargetForm.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);
                Helper.Win32.SendMessage(roundedFormObj.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);

            }
        }

        bool updated = true;

        public void FakeForm_Activated(object sender, EventArgs e)
        {
            if (stop || sent || TargetForm == null || TargetForm.IsDisposed)
            {
                return;
            }

            if (!DesignMode && TargetForm != null && roundedFormObj != null)
            {
                try
                {
                    roundedFormObj.Tag = TargetForm.Opacity;
                }
                catch
                {
                    // comboboxdropdown raises an exception here, but ofc we know its always 100% opacity
                }

                updated = false;
                roundedFormObj.InvalidateNextDrawCall = true;

                FakeForm.Icon = TargetForm.Icon;
                if (roundedFormObj.WindowState == FormWindowState.Minimized)
                {
                    TargetForm.WindowState = FormWindowState.Normal;
                }
                else
                {
                    if (!sent && !stop)
                    {

                        if (TargetForm.WindowState == FormWindowState.Normal && !updated)
                        {
                            SetWindowPos(roundedFormObj.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                            SetWindowPos(TargetForm.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                            updated = true;
                        }
                    }
                }
            }
        }

        private void TargetForm_TextChanged(object sender, EventArgs e)
        {
            if (stop)
            {
                return;
            }
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
                if (stop)
                {
                    return;
                }
                roundedFormObj?.Invalidate();
            }
        }

        private void TargetForm_LocationChanged(object sender, EventArgs e)
        {
            if (stop)
            {
                return;
            }
            if (!DesignMode && roundedFormObj != null && TargetForm != null)
            {
                roundedFormObj.Location = PointSubtract(TargetForm.Location, new Point(2, 2));
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
                if (stop)
                {
                    return;
                }
                roundedFormObj?.Invalidate();
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

            roundedFormObj = new RoundedForm(TargetForm.BackColor, OutlineColor);
            TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));

            roundedFormObj.Show();
            FakeForm.Show();

            TargetForm.Opacity = 1;

            roundedFormObj.Activated += FakeForm_Activated;
            TargetForm_LocationChanged(this, EventArgs.Empty);
            TargetForm_Resize(this, EventArgs.Empty);

            TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));

            Timer miscTimer = new Timer { Interval = 1000 };
            miscTimer.Tick += (a1, a2) =>
            {
                if (!DesignMode && !stop && TargetForm != null)
                {
                    if (TargetForm.WindowState == FormWindowState.Minimized)
                    {
                        FakeForm.WindowState = FormWindowState.Minimized;
                        roundedFormObj.WindowState = FormWindowState.Minimized;
                        TargetForm.WindowState = FormWindowState.Minimized;
                        return;
                    }

                    //FakeForm_Activated(sender, e);
                }
            };
            miscTimer.Start();

            Drawing.FrameDrawn += (e2, s2) =>
            {
                if (roundedFormObj != null && stop == false)
                {
                    try
                    {

                        roundedFormObj.Tag = TargetForm.Opacity;
                        roundedFormObj.InvalidateNextDrawCall = true;
                    }
                    catch
                    {
                        // comboboxdropdown raises an exception here, but ofc we know its always 100% opacity
                    }
                }
                else
                {
                    Dispose();
                }
            };
        }

        private void TargetForm_Resize(object sender, EventArgs e)
        {
            if (roundedFormObj != null && TargetForm != null)
            {
                roundedFormObj.Size = Size.Add(TargetForm.Size, new Size(4, 4));
                FakeForm.Size = TargetForm.Size;
                roundedFormObj.InvalidateNextDrawCall = true;
                TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));
            }
        }
    }
}
