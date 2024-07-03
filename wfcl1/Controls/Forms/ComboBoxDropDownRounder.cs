using CuoreUI.Components.cuiFormRounderV2Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CuoreUI.Components
{
    public partial class ComboBoxDropDownRounder : Component
    {
        public Form RoundedForm;

        public ComboBoxDropDownRounder()
        {
            if (DesignMode)
                throw new Exception("Not meant for other use than cuiComboBox");
        }

        public Form GetRoundedForm()
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
                    //TargetForm.HandleCreated += TargetForm_HandleCreated;
                    TargetForm.Load += TargetForm_Load;
                    TargetForm.Resize += TargetForm_Resize;
                    TargetForm.LocationChanged += TargetForm_LocationChanged;
                    TargetForm.FormClosing += TargetForm_FormClosing;

                }
            }
        }

        private void TargetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel == false)
            {
                RoundedForm.Dispose();
                TargetForm.Dispose();
                Environment.Exit(0);
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

            RoundedForm = new RoundedForm(BackColor, OutlineColor, false);
            RoundedForm.TopMost = true;
            RoundedForm.SendToBack();

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

            await Task.Delay(1000);
            tempTimer.Stop();
            tempTimer.Dispose();
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
            }
        }
    }
}