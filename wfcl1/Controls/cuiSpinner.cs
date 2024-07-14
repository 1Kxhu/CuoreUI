using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CuoreUI.Drawing;

namespace CuoreUI.Controls
{
    public partial class cuiSpinner : UserControl
    {
        Timer designerExclusiveRotationTimer = new Timer();

        private float privateRotateSpeed = 2;
        public float RotateSpeed
        {
            get
            {
                return privateRotateSpeed;
            }
            set
            {
                privateRotateSpeed = value;
                Invalidate();
            }
        }

        public bool RotateEnabled = true;

        public cuiSpinner()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Rotation = 0;
        }

        bool designerRotating = false;

        private async void DesignerRotationLogic()
        {
            if (designerRotating == false)
            {
                designerRotating = true;
                if (DesignMode)
                {
                    while (true)
                    {
                        if (!DesignMode)
                        {
                            break;
                        }

                        await universalRotateLogic();
                    }
                }
            }
            designerRotating = false;
        }

        Drawing.TimeDeltaInstance tdi = new TimeDeltaInstance();

        async Task universalRotateLogic()
        {
            int refreshrate = Drawing.GetHighestRefreshRate();
            Rotation += (RotateSpeed / 2) * tdi.TimeDelta;

            if (Rotation > 359)
            {
                Rotation -= 360;
            }
            if (Rotation < 359)
            {
                Rotation += 360;
            }

            if (DesignMode)
                await Task.Delay(1000 / refreshrate);
        }

        private Color privateArcColor = Color.Coral;
        public Color ArcColor
        {
            get
            {
                return privateArcColor;
            }
            set
            {
                privateArcColor = value;
                Invalidate();
            }
        }

        private Color privateRingColor = Color.FromArgb(34, 34, 34);
        public Color RingColor
        {
            get
            {
                return privateRingColor;
            }
            set
            {
                privateRingColor = value;
                Invalidate();
            }
        }

        private float privateRotation = 0;
        public float Rotation
        {
            get
            {
                return privateRotation;
            }
            set
            {
                if (value >= 360)
                {
                    value -= 360;
                }
                privateRotation = value;
                Refresh();
            }
        }

        float ArcSize = 5;
        float ArcDegrees = 90;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!designerRotating && DesignMode)
            {
                DesignerRotationLogic();
            }

            if (Rotation > 720)
            {
                Rotation = 0;
            }

            if (DesignMode && designerExclusiveRotationTimer.Enabled == false)
            {
                designerExclusiveRotationTimer.Start();
            }
            else if (!DesignMode && designerExclusiveRotationTimer.Enabled == true)
            {
                designerExclusiveRotationTimer.Stop();
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float SpinnerThickness = ArcSize * 2f;

            RectangleF ClientConsideringArcSize = ClientRectangle;
            ClientConsideringArcSize.Width = Math.Min(ClientConsideringArcSize.Width, ClientConsideringArcSize.Height);
            ClientConsideringArcSize.Width = Math.Max(SpinnerThickness * 2 + (ArcSize * 2), ClientConsideringArcSize.Width);
            ClientConsideringArcSize.Height = ClientConsideringArcSize.Width;
            ClientConsideringArcSize.Inflate(-SpinnerThickness, -SpinnerThickness);

            Pen RingPen = new Pen(RingColor, SpinnerThickness);
            e.Graphics.DrawArc(RingPen, ClientConsideringArcSize, 0, 360);

            Pen ArcPen = new Pen(ArcColor, SpinnerThickness);
            ArcPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            ArcPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            e.Graphics.DrawArc(ArcPen, ClientConsideringArcSize, Rotation, ArcDegrees);

        }

        private void cuiSpinner_Load(object sender, EventArgs e)
        {
            Rotation = 0;

            if (!DesignMode)
            {
                FrameDrawn += (f, s) =>
                {
                    _ = universalRotateLogic();
                };
            }
        }
    }
}
