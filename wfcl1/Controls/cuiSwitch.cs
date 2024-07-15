﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CuoreUI.Drawing;

namespace CuoreUI.Controls
{
    [DefaultEvent("Click")]
    public partial class cuiSwitch : UserControl
    {
        public cuiSwitch()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if (DesignMode)
            {
                privateBackground = BackColor;
            }
        }

        private double elapsedTime = 0;

        const int Duration = 350;
        double xDistance = 2;
        float startX = 0;

        bool animationFinished = true;

        public async Task AnimateThumbLocation()
        {
            if (animating)
            {
                animationsInQueue++;
                return;
            }
            animating = true;

            startX = thumbX;

            if (Checked)
            {
                targetX = Width - 3.5f - (Height - 7);
            }
            else
            {
                targetX = OutlineThickness + 1;
            }

            xDistance = -(startX - targetX);

            DateTime animationStartTime = DateTime.Now;
            double durationRatio = Duration / 1000.0;

            animationFinished = false;
            elapsedTime = 0;

            DateTime lastFrameTime = DateTime.Now;

            EmergencySetLocation(Duration);

            while (true)
            {
                DateTime rightnow = DateTime.Now;
                double elapsedMilliseconds = (rightnow - lastFrameTime).TotalMilliseconds;
                lastFrameTime = rightnow;

                elapsedTime += (elapsedMilliseconds / Duration);

                if (elapsedTime >= Duration || animationFinished || animationsInQueue > 0)
                {
                    if (animationsInQueue > 0)
                    {
                        animationsInQueue--;
                        _ = AnimateThumbLocation();
                    }

                    thumbX = (int)targetX;
                    animating = false;
                    animationFinished = false;
                    elapsedTime = 0;
                    Refresh();
                    return;
                }

                double quad = CuoreUI.Drawing.EasingFunctions.FromEasingType(EasingTypes.SextOut, elapsedTime, Duration / (double)1000) * durationRatio;
                //MessageBox.Show($"quad: {quad}\n elapsedTime: {elapsedTime}\n durationRatio: {durationRatio}");
                thumbX = (int)(startX + (int)(xDistance * quad));
                Refresh();

                await Task.Delay(1000 / Drawing.GetHighestRefreshRate());

                if (!Checked)
                {
                    targetX = OutlineThickness * 2;
                }
            }
        }

        int animationsInQueue = 0;

        private async void EmergencySetLocation(int duration)
        {
            await Task.Delay(duration);
            thumbX = (int)targetX;
            animationFinished = true;
            Refresh();
        }

        private bool privateChecked = false;
        [Description("Whether the switch is on or off.")]
        public bool Checked
        {
            get
            {
                return privateChecked;
            }
            set
            {
                if (!animating)
                {
                    privateChecked = value;
                    CheckedChanged?.Invoke(this, EventArgs.Empty);
                    _ = AnimateThumbLocation();
                }
                Invalidate();
            }
        }

        private Color privateBackground = Color.Black;
        [Description("The rounded background for the switch.")]
        public Color Background
        {
            get
            {
                return privateBackground;
            }
            set
            {
                privateBackground = value;
                Invalidate();
            }
        }

        private Color privateCheckedForeground = CuoreUI.Drawing.PrimaryColor;
        [Description("The checked foreground.")]
        public Color CheckedForeground
        {
            get
            {
                return privateCheckedForeground;
            }
            set
            {
                privateCheckedForeground = value;
                Invalidate();
            }
        }

        private Color privateUncheckedForeground = Color.FromArgb(34, 34, 34);
        [Description("The unchecked foreground.")]
        public Color UncheckedForeground
        {
            get
            {
                return privateUncheckedForeground;
            }
            set
            {
                privateUncheckedForeground = value;
                Invalidate();
            }
        }

        public bool OutlineStyle { get; set; } = true;
        public Color OutlineColor { get; set; } = Color.FromArgb(34, 34, 34);
        public Color CheckedOutlineColor { get; set; } = CuoreUI.Drawing.PrimaryColor;
        public float OutlineThickness { get; set; } = 1.6f;

        private int thumbX = 2;

        private bool animating = false;

        float targetX = 2;
        RectangleF thumbRect;

        private bool privateShowSymbols = true;
        public bool ShowSymbols
        {
            get
            {
                return privateShowSymbols;
            }
            set
            {
                privateShowSymbols = value;
                Invalidate();
            }
        }

        public event EventHandler CheckedChanged;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (animating == false)
            {
                if (Checked)
                {
                    thumbX = (int)(Width - 3.5f - (Height - 7));
                }
                else
                {
                    thumbX = (int)(OutlineThickness * 2);
                }
            }
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int Rounding;
            try
            {
                Rounding = (Height / 2) - 1;
            }
            catch
            {
                Rounding = 1;
            }

            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Inflate(-1, -1);

            modifiedCR.Inflate(-(int)OutlineThickness, -(int)OutlineThickness);
            int temporaryRounding = Rounding - (int)OutlineThickness;

            GraphicsPath roundBackground = Helper.RoundRect(modifiedCR, temporaryRounding);

            using (SolidBrush brush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(brush, roundBackground);
            }

            int thumbDim = Height - 7;
            thumbRect = new RectangleF(thumbX, 3, thumbDim, thumbDim);
            thumbRect.Offset(0.5f, 0.5f);
            thumbRect.Inflate(-(int)(OutlineThickness), -(int)(OutlineThickness));

            Rectangle temporaryThumbRect = thumbRectangleInt;
            temporaryThumbRect.Offset(1, 0);

            temporaryThumbRect.Height = temporaryThumbRect.Width;

            using (Pen graphicsPen = new Pen(Background, Height / 10))
            {
                graphicsPen.StartCap = LineCap.Round;
                graphicsPen.EndCap = LineCap.Round;

                if (Checked)
                {
                    using (SolidBrush brush = new SolidBrush(CheckedForeground))
                    {
                        e.Graphics.FillEllipse(brush, thumbRect);
                    }

                    using (Pen outlinePen = new Pen(CheckedOutlineColor, OutlineThickness))
                    {
                        e.Graphics.DrawPath(outlinePen, roundBackground);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(UncheckedForeground))
                    {
                        e.Graphics.FillEllipse(brush, thumbRect);
                    }

                    using (Pen outlinePen = new Pen(OutlineColor, OutlineThickness))
                    {
                        e.Graphics.DrawPath(outlinePen, roundBackground);
                    }
                }

                if (ShowSymbols)
                {
                    if (Checked)
                    {
                        temporaryThumbRect.Offset(0, 1);
                        e.Graphics.DrawPath(graphicsPen, Helper.Checkmark(temporaryThumbRect));
                    }
                    else
                    {
                        temporaryThumbRect.Inflate(-(int)(Height / 6.2f), -(int)(Height / 6.2f));

                        e.Graphics.DrawPath(graphicsPen, Helper.Crossmark(temporaryThumbRect));
                    }
                }
            }

        }

        Rectangle thumbRectangleInt
        {
            get
            {
                return new Rectangle((int)thumbRect.X, (int)thumbRect.Y, (int)thumbRect.Width, (int)thumbRect.Height);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            thumbX = 3;
            if (Width > 0)
            {
                int thumbDim = Height - 7;
                thumbX = Math.Min(Width - 5 - thumbDim, 3);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (animating == false)
            {
                Checked = !Checked;
            }
        }

        private void cuiSwitch_Load(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
