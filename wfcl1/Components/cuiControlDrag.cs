﻿using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI
{
    [ToolboxBitmap(typeof(Button))]
    public partial class cuiControlDrag : Component
    {
        private Control targetControl;
        private Point previousMousePosition;

        private int privateDragFrequency = 4;
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

        public cuiControlDrag(IContainer container)
        {
            container.Add(this);
        }

        public Control TargetControl
        {
            get
            {
                return targetControl;
            }
            set
            {
                if (targetControl != null)
                {
                    targetControl.MouseDown -= MouseDown;
                    targetControl.MouseMove -= MouseMove;
                }

                targetControl = value;

                if (targetControl != null)
                {
                    targetControl.MouseDown += MouseDown;
                    targetControl.MouseMove += MouseMove;
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

                if (targetControl.Parent != null && targetControl.Parent is Form controlParent)
                {
                    controlParent.Left += deltaX;
                    controlParent.Top += deltaY;
                }

                previousMousePosition = currentMousePosition;

                await Task.Delay(DragFrequency);
            }
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            previousMousePosition = Cursor.Position;
        }

    }
}
