
using CuoreUI.Components.cuiFormRounderV2Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CuoreUI.Components
{
    public partial class cuiFormRounderV2 : Component
    {
        private Form RoundedForm;
        private Form FakeForm = new cuiFormRounderV2Resources.FakeForm();

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

                    FakeForm.Activated += FakeForm_Activated;
                }
            }
        }

        private void FakeForm_Activated(object sender, EventArgs e)
        {
            if (TargetForm != null && RoundedForm != null)
            {
                RoundedForm.Focus();
                TargetForm.Focus();
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
                if (RoundedForm != null)
                {
                    RoundedForm.Invalidate();
                }
            }
        }

        private void TargetForm_LocationChanged(object sender, EventArgs e)
        {
            if (RoundedForm != null && TargetForm != null)
            {
                RoundedForm.Location = pointsubstract(TargetForm.Location, new Point(Rounding, Rounding));
                FakeForm.Location = TargetForm.Location;
            }
        }

        private Color privateOutlineColor = Color.White;
        public Color OutlineColor
        {
            get
            {
                return privateOutlineColor;
            }
            set
            {
                privateOutlineColor = value;
            }
        }

        private async void TargetForm_Load(object sender, EventArgs e)
        {
            RoundedForm = new cuiFormRounderV2Resources.RoundedForm(TargetForm.BackColor, OutlineColor);
            RoundedForm.Activated += FakeForm_Activated;
            RoundedForm.Show();

            FakeForm.ShowInTaskbar = true;
            FakeForm.Opacity = 0;
            FakeForm.Show();

            TargetForm.ShowInTaskbar = false;
            TargetForm.FormBorderStyle = FormBorderStyle.None;

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
            await Task.Delay(1000);
            tempTimer.Stop();
            tempTimer.Dispose();

            Timer bitmapTimer = new Timer
            {
                Interval = 32
            };
            Bitmap tempBitmap = new Bitmap(TargetForm.Width, TargetForm.Height);
            bitmapTimer.Tick += ((a1, a2) =>
            {
                SharedVariables.rounding = Rounding;

                using (Graphics g = Graphics.FromImage(tempBitmap))
                {
                    TargetForm.DrawToBitmap(tempBitmap, new Rectangle(0, 0, TargetForm.Width, TargetForm.Height));
                }

                SharedVariables.FakeBitmap = (Bitmap)tempBitmap.Clone();
                FakeForm.Invoke((MethodInvoker)delegate {
                    FakeForm.Refresh();
                });

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
                RoundedForm.Size = addsize(TargetForm.Size, new Size(r2, r2));
                FakeForm.Size = TargetForm.Size;
            }
        }
    }
}