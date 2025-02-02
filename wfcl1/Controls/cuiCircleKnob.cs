﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("ValueChanged")]
    public partial class cuiCircleKnob : UserControl
    {
        private bool isDragging = false;

        public cuiCircleKnob()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            ForeColor = Color.FromArgb(128, 128, 128, 128);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            isDragging = true;
            UpdateValueFromMouse(e.Location);

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isDragging)
            {
                UpdateValueFromMouse(e.Location);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isDragging = false;

            base.OnMouseUp(e);
        }

        private void UpdateValueFromMouse(Point mousePosition)
        {
            float centerX = Width / 2;
            float centerY = Height / 2;
            float dx = mousePosition.X - centerX;
            float dy = mousePosition.Y - centerY;
            float angle = (float)(Math.Atan2(dy, dx) * (180 / Math.PI) + 90);

            if (angle < 0)
                angle += 360;

            Value = MinValue + (angle / 360f) * (MaxValue - MinValue);
        }

        private Color privateTrackColor = Color.FromArgb(64, 128, 128, 128);
        public Color TrackColor
        {
            get
            {
                return privateTrackColor;
            }
            set
            {
                privateTrackColor = value;
                Refresh();
            }
        }

        private float privateHalfTrackThickness = 0;
        private int privateTrackThickness = 2;
        public int TrackThickness
        {
            get
            {
                return privateTrackThickness;
            }
            set
            {
                privateTrackThickness = value;
                privateHalfTrackThickness = value / 2f;
                UpdateThumbRectangle();
                Refresh();
            }
        }

        private Color privateThumbColor = CuoreUI.Drawing.PrimaryColor;
        public Color ThumbColor
        {
            get
            {
                return privateThumbColor;
            }
            set
            {
                privateThumbColor = value;
                Refresh();
            }
        }

        private float privateValue = 100;
        private float privateMinValue = 0;
        private float privateMaxValue = 360;

        public event EventHandler ValueChanged;

        public float Value
        {
            get
            {
                return privateValue;
            }
            set
            {
                if (value >= privateMinValue && value <= privateMaxValue)
                {
                    bool isNewValue = value != privateValue;
                    privateValue = (int)value;
                    UpdateThumbRectangle();
                    Refresh();
                    if (isNewValue)
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public float MinValue
        {
            get
            {
                return privateMinValue;
            }
            set
            {
                if (value < privateMaxValue && value <= privateValue)
                {
                    privateMinValue = value;
                    UpdateThumbRectangle();
                    Refresh();
                }
            }
        }

        public float MaxValue
        {
            get
            {
                return privateMaxValue;
            }
            set
            {
                if (value > privateMinValue && value >= privateValue)
                {
                    privateMaxValue = value;
                    UpdateThumbRectangle();
                    Refresh();
                }
            }
        }

        private bool privateShowValueText = true;
        public bool ShowValueText
        {
            get
            {
                return privateShowValueText;
            }
            set
            {
                privateShowValueText = value;
                Refresh();
            }
        }

        public enum KnobStyles
        {
            Thumb,
            Arc
        }

        private KnobStyles privateKnobStyle = KnobStyles.Arc;
        public KnobStyles KnobStyle
        {
            get
            {
                return privateKnobStyle;
            }
            set
            {
                privateKnobStyle = value;
                Refresh();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateThumbRectangle();

            Width = Height;

            base.OnResize(e);
        }

        RectangleF thumbRectangle = RectangleF.Empty;
        void UpdateThumbRectangle()
        {
            float centerX = Width / 2;
            float centerY = Height / 2;
            float radius = (Math.Min(Width, Height) / 2) - privateHalfTrackThickness - (Height / 8);
            float angle = (Value - MinValue) / (MaxValue - MinValue) * 360f - 90f;
            double radians = angle * (Math.PI / 180);
            float thumbX = centerX + (int)(radius * Math.Cos(radians)) - Height / 8;
            float thumbY = centerY + (int)(radius * Math.Sin(radians)) - Height / 8;
            thumbRectangle = new RectangleF(thumbX, thumbY, Height / 4, Height / 4);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF modifiedCR = ClientRectangle;
            modifiedCR.Inflate(-1, -1);
            modifiedCR.Inflate(-privateHalfTrackThickness, -privateHalfTrackThickness);
            modifiedCR.Inflate(-Height / 8, -Height / 8);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen p = new Pen(TrackColor, privateTrackThickness))
            {
                e.Graphics.DrawEllipse(p, modifiedCR);
            }


            if (KnobStyle == KnobStyles.Thumb)
            {
                using (SolidBrush br = new SolidBrush(ThumbColor))
                {
                    e.Graphics.FillEllipse(br, thumbRectangle);
                }
            }
            else if (KnobStyle == KnobStyles.Arc)
            {
                float angle = (Value - MinValue) / (MaxValue - MinValue) * 360f;

                using (Pen p = new Pen(ThumbColor, privateTrackThickness))
                {
                    e.Graphics.DrawArc(p, modifiedCR, -90, angle);
                }
            }

            using (StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            using (SolidBrush br = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Value.ToString(), Font, br, modifiedCR, sf);
            }

            base.OnPaint(e);
        }
    }
}