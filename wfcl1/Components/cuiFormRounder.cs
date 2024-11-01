using CuoreUI.Components.cuiFormRounderV2Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
                TargetForm.Activated += TargetForm_Activated;
                TargetForm.ResizeEnd += (_, __) =>
                {
                    UpdateRoundedFormBitmap();
                };

                FakeForm.Activated += FakeForm_Activated;
                FakeForm.FormClosing += TargetForm_FormClosing;
            }
        }

        bool targetFormActivating = false;

        private void TargetForm_Activated(object sender, EventArgs e)
        {
            if (shouldCloseDown)
            {
                return;
            }

            if (!targetFormActivating)
            {
                targetFormActivating = true;
                FakeForm_Activated(sender, e);
            }
        }

        private void TargetForm_BackColorChanged(object sender, EventArgs e)
        {
            if (shouldCloseDown)
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
            if (shouldCloseDown)
            {
                return;
            }

            if (!DesignMode && roundedFormObj != null && !wasFormClosingCalled)
            {
                roundedFormObj.Visible = TargetForm.Visible;
                roundedFormObj.Tag = TargetForm.Opacity;
                roundedFormObj.InvalidateNextDrawCall = true;
            }
        }

        private bool shouldCloseDown = false;
        private bool wasFormClosingCalled = false;

        private void TargetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // causes the other parts of this code to not run
            // that stops any exceptions in the code trying to access disposed or null stuff 
            shouldCloseDown = true;
            if (!wasFormClosingCalled && TargetForm != null)
            {
                wasFormClosingCalled = true;

                // unattach events
                TargetForm.Load -= TargetForm_Load;
                TargetForm.Resize -= TargetForm_Resize;
                TargetForm.LocationChanged -= TargetForm_LocationChanged;
                TargetForm.TextChanged -= TargetForm_TextChanged;
                TargetForm.FormClosing -= TargetForm_FormClosing;
                TargetForm.VisibleChanged -= TargetForm_VisibleChanged;
                TargetForm.BackColorChanged -= TargetForm_BackColorChanged;

                // clean controls from targetform
                TargetForm.Controls.Clear();

                // send close message to all forms
                Helper.Win32.SendMessage(FakeForm.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);
                Helper.Win32.SendMessage(TargetForm.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);
                Helper.Win32.SendMessage(roundedFormObj.Handle, 0x0010, IntPtr.Zero, IntPtr.Zero);

                (FakeForm as FakeForm).CloseFakeForm();
            }
        }

        bool updated = true;

        public void FakeForm_Activated(object sender, EventArgs e)
        {
            if (shouldCloseDown || wasFormClosingCalled || TargetForm == null || TargetForm.IsDisposed)
            {
                return;
            }

            if (!DesignMode && TargetForm != null && roundedFormObj != null)
            {
                try
                {
                    // may crash if roundedFormObject is disposed or null
                    roundedFormObj.Tag = TargetForm.Opacity;
                }
                catch
                {
                    // ComboBoxDropDown raises an exception here
                    // but we can just not care about this, since it's opacity is ALWAYS 100%
                }

                updated = false;
                roundedFormObj.InvalidateNextDrawCall = true;

                // https://github.com/1Kxhu/CuoreUI/issues/11 fix #3
                if (roundedFormObj.WindowState != FormWindowState.Minimized)
                {
                    if (!wasFormClosingCalled && !shouldCloseDown)
                    {
                        if (TargetForm.WindowState != FormWindowState.Minimized)
                        {
                            SetWindowPos(roundedFormObj.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                            SetWindowPos(TargetForm.Handle, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                            updated = true;

                        }


                        TargetForm.BringToFront();
                    }
                }
            }

            targetFormActivating = false;
        }

        private void TargetForm_TextChanged(object sender, EventArgs e)
        {
            if (shouldCloseDown)
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
                if (shouldCloseDown)
                {
                    return;
                }
                if (roundedFormObj != null)
                {
                    roundedFormObj.Rounding = value;
                    roundedFormObj?.Invalidate();
                }
            }
        }

        private async void TargetForm_LocationChanged(object sender, EventArgs e)
        {
            if (shouldCloseDown)
            {
                return;
            }

            // update Location with a 2,2 offset caused by RoundedForm in mind
            if (!DesignMode && roundedFormObj != null && TargetForm != null)
            {
                roundedFormObj.Location = PointSubtract(TargetForm.Location, new Point(2, 2));

                // https://github.com/1Kxhu/CuoreUI/issues/11 fix #2
                if (TargetForm.WindowState == FormWindowState.Minimized)
                {
                    roundedFormObj.Hide();
                }
                else
                {
                    await Task.Delay(1000 / Drawing.GetHighestRefreshRate());
                    roundedFormObj.Show();
                }

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
                if (shouldCloseDown)
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

        internal void UpdateRoundedFormBitmap()
        {
            if (DesignMode || TargetForm == null || roundedFormObj == null)
            {
                return;
            }
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private void TargetForm_Load(object sender, EventArgs e)
        {
            // initialize rounding
            FakeForm_Activated(sender, e);

            TargetForm.FormBorderStyle = FormBorderStyle.None;
            FakeForm.Opacity = 0;

            roundedFormObj = new RoundedForm(TargetForm.BackColor, OutlineColor, ref privateRounding);
            TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));

            roundedFormObj.Show();
            FakeForm.Show();

            roundedFormObj.Activated += FakeForm_Activated;
            TargetForm_LocationChanged(this, EventArgs.Empty);
            TargetForm_Resize(this, EventArgs.Empty);

            // Drawing.FrameDrawn is called every 1000/hz milliseconds
            // where hz stands for the maximum refresh rate recorded from all display devices
            Drawing.TenFramesDrawn += (_, __) =>
            {

                if (roundedFormObj != null && shouldCloseDown == false)
                {
                    try // https://github.com/1Kxhu/CuoreUI/issues/11 fix #1
                    {
                        // this is the part that MAY raise an exception from ComboBoxDropDown
                        roundedFormObj.Tag = TargetForm.Opacity;
                        roundedFormObj.InvalidateNextDrawCall = true;
                    }
                    catch
                    {
                        // ComboBoxDropDown raises an exception here
                        // but we can just not care about this, since it's opacity is ALWAYS 100%
                    }
                }
                else
                {
                    // either roundedFormObj is null or "stop" is true
                    // stop is true when the form had announced it wants to close (see TargetForm_FormClosing)
                    Dispose();
                }
            };
        }

        FormWindowState lastState;

        private void TargetForm_Resize(object sender, EventArgs e)
        {
            if (roundedFormObj != null && TargetForm != null)
            {
                // If windowstate has changed, set said windowstate value to all the other forms
                if (TargetForm.WindowState != lastState)
                {
                    lastState = TargetForm.WindowState;

                    FakeForm.WindowState = TargetForm.WindowState;
                    roundedFormObj.WindowState = TargetForm.WindowState;
                }

                if (TargetForm.WindowState != FormWindowState.Minimized)
                {
                    // Related to how RoundedForm is drawn
                    // Updates rounding if needed, too
                    roundedFormObj.Size = Size.Add(TargetForm.Size, new Size(4, 4));
                    FakeForm.Size = TargetForm.Size;

                    roundedFormObj.InvalidateNextDrawCall = true;

                    if (TargetForm.WindowState == FormWindowState.Normal)
                    {
                        TargetForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, TargetForm.Width, TargetForm.Height, (int)(Rounding * 2f), (int)(Rounding * 2f)));
                    }
                    else
                    {
                        TargetForm.Region = new Region(TargetForm.ClientRectangle);
                    }
                }
                targetFormActivating = false;
            }
        }
    }
}
