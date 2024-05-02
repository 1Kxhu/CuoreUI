using System;
using System.Drawing;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiSpinner : UserControl
    {
        Timer updateTimer = new Timer();
        Timer rotationTimer = new Timer();

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

        private float FloatLimitHalf = float.MaxValue / 2;

        public cuiSpinner()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            backsiteSmoothRotation = RotateSpeed;
            Rotation = 0;

            updateTimer.Interval = 500 / Helper.Win32.GetRefreshRate();
            updateTimer.Tick += (obj, args) =>
            {
                Invalidate();
            };

            updateTimer.Start();

            rotationTimer.Interval = 4;
            rotationTimer.Tick += (obj, args) =>
            {
                // weird workaround because of interpolating Rotation (check SmoothRotation variable)
                if (Rotation >= FloatLimitHalf)
                {
                    Rotation = 0;
                }
                if (RotateEnabled)
                {
                    Rotation += RotateSpeed;
                }
            };

            rotationTimer.Start();
        }

        private Color privateArcColor = Color.White;
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

        private float privateRotation = 0;
        public float Rotation
        {
            get
            {
                return privateRotation;
            }
            set
            {
                backsiteSmoothRotation = value;
                privateRotation = value;
                Invalidate();
            }
        }

        private float backsiteSmoothRotation = 0;
        private float SmoothRotation
        {
            get
            {
                // interpolate or whatever it's called to see the spinner spinning smoothly on all refresh rates
                backsiteSmoothRotation = ((backsiteSmoothRotation * 14) + Rotation) / 15;
                if (backsiteSmoothRotation > FloatLimitHalf)
                {
                    backsiteSmoothRotation = 0;
                    Rotation = 0;
                }

                if (backsiteSmoothRotation > 360 && Rotation > 360)
                {
                    Rotation = 360 - backsiteSmoothRotation;
                }
                return backsiteSmoothRotation;
            }
        }

        float ArcSize = 5;
        float ArcDegrees = 90;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float SpinnerThickness = ArcSize * 2f;

            RectangleF ClientConsideringArcSize = ClientRectangle;
            ClientConsideringArcSize.Width = Math.Min(ClientConsideringArcSize.Width, ClientConsideringArcSize.Height);
            ClientConsideringArcSize.Width = Math.Max(SpinnerThickness * 2 + (ArcSize * 2), ClientConsideringArcSize.Width);
            ClientConsideringArcSize.Height = ClientConsideringArcSize.Width;
            ClientConsideringArcSize.Inflate(-SpinnerThickness, -SpinnerThickness);

            Pen ArcPen = new Pen(ArcColor, SpinnerThickness);
            ArcPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            ArcPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            e.Graphics.DrawArc(ArcPen, ClientConsideringArcSize, SmoothRotation, ArcDegrees);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Rotation = 0;
            backsiteSmoothRotation = 0;
        }
    }
}
