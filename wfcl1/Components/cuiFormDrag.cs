using System.ComponentModel;
using System.Windows.Forms;

namespace CuoreUI
{
    public partial class cuiFormDrag : Component
    {
        private Form targetForm;
        private bool isDragging;
        private int offsetX;
        private int offsetY;

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
                    targetForm.MouseUp -= MouseUp;
                }

                targetForm = value;

                if (targetForm != null)
                {
                    targetForm.MouseDown += MouseDown;
                    targetForm.MouseMove += MouseMove;
                    targetForm.MouseUp += MouseUp;
                }
            }
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            offsetX = e.X;
            offsetY = e.Y;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                targetForm.Left = Cursor.Position.X - offsetX;
                targetForm.Top = Cursor.Position.Y - offsetY;
            }
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}
