﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiScrollbar : Control
    {
        private int thumbHeight;
        private ScrollableControl targetControl;
        private ScrollMessageFilter messageFilter;

        private bool isThumbHovered = false;
        private bool isThumbPressed = false;
        private int thumbPosition = 0;
        private int initialMouseY = 0;
        private int initialThumbPosition = 0;

        public int ThumbHeight
        {
            get
            {
                return thumbHeight;
            }
            set
            {
                thumbHeight = value;
                Invalidate();
            }
        }

        public ScrollableControl TargetControl
        {
            get
            {
                return targetControl;
            }
            set
            {
                if (targetControl != value)
                {
                    if (targetControl != null)
                    {
                        targetControl.Scroll -= TargetControl_Scroll;
                        targetControl.Resize -= TargetControl_Resize;
                        if (messageFilter != null)
                        {
                            messageFilter.ReleaseHandle();
                        }
                    }

                    targetControl = value;

                    if (targetControl != null)
                    {
                        targetControl.Scroll += TargetControl_Scroll;
                        targetControl.Resize += TargetControl_Resize;
                        messageFilter = new ScrollMessageFilter(this);
                        messageFilter.AssignHandle(targetControl.Handle);
                        BindToTargetControl();
                    }
                }
            }
        }

        private void BindToTargetControl()
        {
            if (targetControl != null)
            {
                this.Height = targetControl.Height;
                this.Location = new Point(targetControl.Right - this.Width, targetControl.Top);
                this.BringToFront();
                UpdateThumbPosition();
            }
        }

        public cuiScrollbar()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.thumbHeight = 50;
            MinimumSize = new Size(20, 50);
        }

        private void TargetControl_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateThumbPosition();
            Refresh();
        }

        private void TargetControl_Resize(object sender, EventArgs e)
        {
            BindToTargetControl();
            Refresh();
        }

        private void UpdateThumbPosition()
        {
            if (targetControl != null)
            {
                int min = targetControl.VerticalScroll.Minimum;
                int max = targetControl.VerticalScroll.Maximum - targetControl.VerticalScroll.LargeChange + 1;
                int value = targetControl.VerticalScroll.Value;

                float ratio = (float)value / max;
                thumbPosition = (int)((Height - thumbHeight) * ratio);
                Invalidate();
            }
        }

        private int privateRounding = 5;
        public int Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                privateRounding = value;
                Invalidate();
            }
        }

        private Color privateBackground = Color.Transparent;
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

        private Color privateThumbColor = Drawing.PrimaryColor;
        public Color ThumbColor
        {
            get
            {
                return privateThumbColor;
            }
            set
            {
                privateThumbColor = value;
                Invalidate();
            }
        }

        private Color privateHoveredThumbColor = Drawing.TranslucentPrimaryColor;
        public Color HoveredThumbColor
        {
            get
            {
                return privateHoveredThumbColor;
            }
            set
            {
                privateHoveredThumbColor = value;
                Invalidate();
            }
        }

        private Color privatePressedThumbColor = Drawing.TranslucentPrimaryColor;
        public Color PressedThumbColor
        {
            get
            {
                return privatePressedThumbColor;
            }
            set
            {
                privatePressedThumbColor = value;
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (targetControl != null)
            {
                if (Location != new Point(targetControl.Right - this.Width, targetControl.Top))
                {
                    Location = new Point(targetControl.Right - this.Width, targetControl.Top);
                }

                targetControl.AutoScroll = true;
                Height = targetControl.Height;
            }

            e.Graphics.Clear(BackColor);

            Rectangle modifiedCR = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            GraphicsPath bgPath = Helper.RoundRect(modifiedCR, Rounding);
            e.Graphics.FillPath(new SolidBrush(Background), bgPath);

            Rectangle thumbRect = new Rectangle(0, thumbPosition, Width - 1, thumbHeight - 1);
            GraphicsPath thumbPath = Helper.RoundRect(thumbRect, Rounding);
            e.Graphics.FillPath(
                  isThumbPressed ? new SolidBrush(PressedThumbColor)
                : isThumbHovered ? new SolidBrush(HoveredThumbColor)
                : new SolidBrush(ThumbColor), thumbPath);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Rectangle thumbRect = new Rectangle(0, thumbPosition, Width, thumbHeight);
                if (thumbRect.Contains(e.Location))
                {
                    isThumbPressed = true;
                    Capture = true;
                    initialMouseY = e.Y;
                    initialThumbPosition = thumbPosition;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isThumbPressed = false;
                Capture = false;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isThumbPressed)
            {
                int deltaY = e.Y - initialMouseY;
                int newThumbPosition = initialThumbPosition + deltaY;
                newThumbPosition = Math.Max(0, Math.Min(Height - thumbHeight, newThumbPosition));
                float ratio = (float)newThumbPosition / (Height - thumbHeight);

                if (targetControl != null)
                {
                    int max = targetControl.VerticalScroll.Maximum - targetControl.VerticalScroll.LargeChange + 1;
                    targetControl.VerticalScroll.Value = (int)(max * ratio);
                }

                thumbPosition = newThumbPosition;
                Refresh();
            }
            else
            {
                Rectangle thumbRect = new Rectangle(0, thumbPosition, Width, thumbHeight);
                bool hovered = thumbRect.Contains(e.Location);

                if (hovered != isThumbHovered)
                {
                    isThumbHovered = hovered;
                    Invalidate();
                }
            }
        }

        private class ScrollMessageFilter : NativeWindow
        {
            private cuiScrollbar scrollbar;

            public ScrollMessageFilter(cuiScrollbar scrollbar)
            {
                this.scrollbar = scrollbar;
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_VSCROLL = 0x0115;
                const int WM_MOUSEWHEEL = 0x020A;

                if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL)
                {
                    scrollbar.UpdateThumbPosition();
                }

                base.WndProc(ref m);

                scrollbar.Refresh();
            }
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.Style |= 1;
                return createParams;
            }
        }
    }
}
