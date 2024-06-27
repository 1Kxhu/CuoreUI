
using CuoreUI.Components.cuiFormRounderV2Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CuoreUI.Components
{
    public partial class cuiFormRounder : Component
    {
        public Form RoundedForm;
        public Form FakeForm = new FakeForm();

        public Form GetFakeForm()
        {
            return FakeForm;
        }

        internal Form GetRoundedForm()
        {
            return RoundedForm;
        }

        private Form privateTargetForm;
        public Form TargetForm
        {
            get
            {
                return privateTargetForm;
            }
            set
            {
                privateTargetForm = value;
                if (privateTargetForm != null)
                {
                    TargetForm.Load += TargetForm_Load;
                    TargetForm.Resize += TargetForm_Resize;
                    TargetForm.LocationChanged += TargetForm_LocationChanged;
                    TargetForm.TextChanged += TargetForm_TextChanged;
                    TargetForm.FormClosing += TargetForm_FormClosing;
                    TargetForm.VisibleChanged += TargetForm_VisibleChanged;

                    FakeForm.Activated += FakeForm_Activated;
                    FakeForm.FormClosing += TargetForm_FormClosing;
                }
            }
        }

        private void TargetForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
                RoundedForm.Visible = TargetForm.Visible;
        }

        private void TargetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel == false)
            {
                RoundedForm.Dispose();
                FakeForm.Dispose();
                TargetForm.Dispose();
                Environment.Exit(0);
            }
        }

        private void FakeForm_Activated(object sender, EventArgs e)
        {
            if (TargetForm != null && RoundedForm != null)
            {
                if (RoundedForm.WindowState == FormWindowState.Minimized)
                {
                    TargetForm.WindowState = FormWindowState.Normal;
                }
                else
                {
                    RoundedForm.Focus();
                    TargetForm.Focus();
                }
            }
        }

        private void TargetForm_TextChanged(object sender, EventArgs e)
        {
            FakeForm.Text = TargetForm.Text;
        }

        Point pointsubstract(Point p1, Point p2)
        {
            int subx = p1.X - p2.X;
            int suby = p1.Y - p2.Y;
            return new Point(subx, suby);
        }

        private int privateRounding = 8;
        public int Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                privateRounding = value;
                RoundedForm?.Invalidate();
            }
        }

        private void TargetForm_LocationChanged(object sender, EventArgs e)
        {
            if (RoundedForm != null && TargetForm != null)
            {
                RoundedForm.Location = pointsubstract(TargetForm.Location, new Point(Rounding, Rounding));
                FakeForm.Location = TargetForm.Location;
                if (new Random().Next(0, 6) == 3)
                {
                    FakeForm_Activated(sender, e);
                }
            }
        }

        private Color privateOutlineColor = Color.FromArgb(30, 0, 0, 0);
        public Color OutlineColor
        {
            get
            {
                return privateOutlineColor;
            }
            set
            {
                privateOutlineColor = value;
                RoundedForm?.Invalidate();
            }
        }

        private Color privateBackColor = Color.FromArgb(10, 10, 10);
        public Color BackColor
        {
            get
            {
                return privateBackColor;
            }
            set
            {
                privateBackColor = value;
                RoundedForm?.Invalidate();
            }
        }

        private async void TargetForm_Load(object sender, EventArgs e)
        {
            TargetForm.Opacity = 0;
            TargetForm.ShowInTaskbar = false;
            TargetForm.FormBorderStyle = FormBorderStyle.None;

            FakeForm.ShowInTaskbar = true;
            FakeForm.Opacity = 0;

            RoundedForm = new RoundedForm(BackColor, OutlineColor);

            RoundedForm.Show();
            FakeForm.Show();

            TargetForm.Opacity = 1;

            RoundedForm.Activated += FakeForm_Activated;
            TargetForm_LocationChanged(this, EventArgs.Empty);
            TargetForm_Resize(this, EventArgs.Empty);

            Timer tempTimer = new Timer
            {
                Interval = 1
            };
            tempTimer.Tick += ((a1, a2) =>
            {
                TargetForm_LocationChanged(this, EventArgs.Empty);
                TargetForm_Resize(this, EventArgs.Empty);
            });
            tempTimer.Start();

            await Task.Delay(300);
            tempTimer.Stop();
            tempTimer.Dispose();

            Timer bitmapTimer = new Timer
            {
                Interval = 100
            };
            Bitmap tempBitmap = new Bitmap(TargetForm.Width, TargetForm.Height);
            bitmapTimer.Tick += ((a1, a2) =>
            {
                SharedVariables.rounding = Rounding;

                if (TargetForm.WindowState == FormWindowState.Minimized)
                {
                    FakeForm.WindowState = FormWindowState.Minimized;
                    RoundedForm.WindowState = FormWindowState.Minimized;
                    TargetForm.WindowState = FormWindowState.Minimized;
                    return;
                }
                if (TargetForm.Visible && TargetForm.WindowState == FormWindowState.Minimized)
                {
                    try
                    {
                        using (Graphics g = Graphics.FromImage(tempBitmap))
                        {
                            TargetForm.DrawToBitmap(tempBitmap, new Rectangle(0, 0, TargetForm.Width, TargetForm.Height));
                        }

                        if (SharedVariables.FakeBitmap != null)
                        {
                            SharedVariables.FakeBitmap.Dispose();
                        }
                        SharedVariables.FakeBitmap = (Bitmap)tempBitmap.Clone();

                        FakeForm.Invoke((MethodInvoker)delegate
                        {
                            FakeForm.ResumeLayout(false);
                        });
                    }
                    catch (Exception)
                    {
                        // uhhh yeahhhhh but atleast the app won't crash
                    }
                    finally
                    {
                        tempBitmap.Dispose();
                    }
                }

                // 30 mb
                GC.AddMemoryPressure(30000000);
            });
            bitmapTimer.Start();


        }

        Size addsize(Size s1, Size s2)
        {
            int sizex = s1.Width + s2.Width;
            int sizey = s1.Height + s2.Height;
            return new Size(sizex, sizey);
        }

        private void TargetForm_Resize(object sender, EventArgs e)
        {
            if (RoundedForm != null && TargetForm != null)
            {
                int r2 = Rounding * 2;
                RoundedForm.Size = addsize(TargetForm.Size, new Size(r2 + 1, r2 + 1));
                FakeForm.Size = TargetForm.Size;
            }
        }
    }
}