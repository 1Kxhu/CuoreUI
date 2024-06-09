using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI
{
    public partial class cuiFormDrag : Component
    {
        private Form targetForm;
        private Point previousMousePosition;

        private int privateDragFrequency = 0;

        [Description("Leave at 0 for default value.")]
        public int DragFrequency
        {
            get
            {
                return privateDragFrequency;
            }
            set
            {
                privateDragFrequency = value;
            }
        }

        public cuiFormDrag(IContainer container)
        {
            container.Add(this);
        }

        public Form TargetForm
        {
            get
            {
                return targetForm;
            }
            set
            {
                if (targetForm != null)
                {
                    targetForm.MouseDown -= MouseDown;
                    targetForm.MouseMove -= MouseMove;
                }

                targetForm = value;

                if (targetForm != null)
                {
                    targetForm.MouseDown += MouseDown;
                    targetForm.MouseMove += MouseMove;
                }
            }
        }

        private async void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point currentMousePosition = Cursor.Position;
                int deltaX = currentMousePosition.X - previousMousePosition.X;
                int deltaY = currentMousePosition.Y - previousMousePosition.Y;

                targetForm.Left += deltaX;
                targetForm.Top += deltaY;

                previousMousePosition = currentMousePosition;

                if (DragFrequency < 1)
                {
                    await Task.Delay(1000 / Helper.Win32.GetRefreshRate());
                }
                {
                    await Task.Delay(DragFrequency);
                }
            }
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            previousMousePosition = Cursor.Position;
        }

    }
}
