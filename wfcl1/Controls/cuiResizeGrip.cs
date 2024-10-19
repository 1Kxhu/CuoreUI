using CuoreUI.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(Form))]

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

        private Color privateGripColor = Color.White;
        public Color GripColor
        {
            get
            {
                return privateGripColor;
            }
            set
            {
                privateGripColor = value;

                if (GripTexture == false)
                {
                    return;
                }

                if (TintedGrip != null)
                {
                    TintedGrip.Dispose();
                }

                TintedGrip = Drawing.Imaging.TintBitmap(Resources.grip, value);

                Refresh();
            }
        }

        Image TintedGrip = Resources.grip; // previously null, caused an error when added at first

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
                Refresh();
            }
        }

        Timer dragTimer = new Timer();

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private const int VK_LBUTTON = 0x01;

        public cuiResizeGrip()
        {
            InitializeComponent();

            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            dragTimer.Tick += DragTimer_Tick;
            Size = new Size(24, 24);
            Cursor = Cursors.SizeNWSE;

            Timer refreshTimer = new Timer();
            refreshTimer.Interval = 1000;
            refreshTimer.Start();
            refreshTimer.Tick += (e, s) => {
                dragTimer.Interval = 1000 / CuoreUI.Drawing.GetHighestRefreshRate();
            };
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

            dragTimer.Interval = 1000 / CuoreUI.Drawing.GetHighestRefreshRate();
            dragTimer.Stop();
            dragTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Size != new Size(21, 21))
            {
                Size = new Size(21, 21);
            }

            if (GripTexture || TintedGrip != null)
            {
                e.Graphics.DrawImage(TintedGrip, ClientRectangle);
            }

            if (TargetForm != null)
            {
                Location = new Point(TargetForm.Size.Width - Width, TargetForm.Size.Height - Height);
            }
        }
    }
}
