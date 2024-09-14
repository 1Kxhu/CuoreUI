using CuoreUI.Components;
using CuoreUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiResizeGrip : UserControl
    {
        Point lastMousePoint = new Point(-1, -1);

        Form privateTargetForm;
        public Form TargetForm
        {
            get
            {
                return privateTargetForm;
            }
            set
            {
                privateTargetForm = value;
            }
        }

        private bool privateGripTexture = true;
        public bool GripTexture
        {
            get
            {
                return privateGripTexture;
            }
            set
            {
                privateGripTexture = value;

                if (value == true)
                {
                    BackgroundImage = Resources.gripTexture;
                }
                else
                {
                    BackgroundImage = null;
                }

                Invalidate();
            }
        }

        Timer dragTimer = new Timer();

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private const int VK_LBUTTON = 0x01;

        public cuiResizeGrip()
        {
            InitializeComponent();
            BackgroundImageLayout = ImageLayout.Center;
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            dragTimer.Tick += DragTimer_Tick;
            Size = new Size(24,24);
            Cursor = Cursors.SizeNWSE;
        }

        private void DragTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TargetForm != null)
                {
                    bool isLeftButtonDown = (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
                    if (isLeftButtonDown == false)
                    {
                        dragTimer.Stop();
                        return;
                    }

                    if (lastMousePoint == new Point(-1, -1))
                    {
                        lastMousePoint = Cursor.Position;
                    }

                    Point currentMousePoint = Cursor.Position;
                    Point mouseDelta = GetDelta(currentMousePoint, lastMousePoint);
                    lastMousePoint = currentMousePoint;
                    TargetForm.Size = Size.Subtract(TargetForm.Size, (Size)mouseDelta);

            
                }
            }
            catch (NullReferenceException)
            {
                
            }
        }

        private static Point GetDelta(Point p1, Point p2)
        {
            double deltaX = p2.X - p1.X;
            double deltaY = p2.Y - p1.Y;

            return new Point((int)deltaX, (int)deltaY);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            lastMousePoint = new Point(-1, -1);

            dragTimer.Interval = (int)(1000 / (double)CuoreUI.Drawing.GetHighestRefreshRate());
            dragTimer.Stop();
            dragTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (TargetForm != null)
            {
                Location = new Point(TargetForm.Size.Width - Width, TargetForm.Size.Height - Height);
            }
        }
    }
}
