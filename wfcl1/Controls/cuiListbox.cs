using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiListbox : ListBox
    {
        Timer refreshTimer;

        public cuiListbox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            DrawMode = DrawMode.OwnerDrawFixed;
            BorderStyle = BorderStyle.None;
            ItemHeight = 34;
            ForeColor = Color.FromArgb(84, 84, 84);
            SelectionMode = SelectionMode.One;
            refreshTimer = new Timer();
            refreshTimer.Interval = 25;
            refreshTimer.Start();
            refreshTimer.Tick += RefreshTimer_Tick;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (DoubleBuffered)
            {
                Refresh();
            }
        }

        private int privateRounding = 8;
        public int Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                if (value > 0)
                {
                    if (value > (ClientRectangle.Height / 2))
                    {
                        privateRounding = ClientRectangle.Height / 2;
                        Rounding = privateRounding;
                    }
                    else
                    {
                        privateRounding = value;
                    }
                }
                else
                {
                    throw new Exception("Rounding cannot be less than 1");
                }
                Invalidate();
            }
        }

        private int privateItemRounding = 8;
        public int ItemRounding
        {
            get
            {
                return privateItemRounding;
            }
            set
            {
                if (value > 0)
                {
                    if (value > (ItemHeight / 2))
                    {
                        privateItemRounding = (ItemHeight / 2) + 1;
                        ItemRounding = privateItemRounding;
                    }
                    else
                    {
                        privateItemRounding = value;
                    }
                }
                else
                {
                    throw new Exception("ItemRounding cannot be greater than half of Item Height");
                }
                Invalidate();
            }
        }

        private Color privateBackgroundColor = Color.FromArgb(222, 222, 222);
        public Color BackgroundColor
        {
            get
            {
                return privateBackgroundColor;
            }
            set
            {
                privateBackgroundColor = value;
                Invalidate();
            }
        }

        private Color privateForegroundColor = Color.DimGray;
        public Color ForegroundColor
        {
            get
            {
                return privateForegroundColor;
            }
            set
            {
                privateForegroundColor = value;
                Invalidate();
            }
        }

        private Color privateItemSelectedBackgroundColor = Color.FromArgb(50, 240, 117);
        public Color ItemSelectedBackgroundColor
        {
            get
            {
                return privateItemSelectedBackgroundColor;
            }
            set
            {
                privateItemSelectedBackgroundColor = value;
                Invalidate();
            }
        }

        private Color privateSelectedForegroundColor = Color.Black;
        public Color SelectedForegroundColor
        {
            get
            {
                return privateSelectedForegroundColor;
            }
            set
            {
                privateSelectedForegroundColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SelectionMode = SelectionMode.One;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle cr = ClientRectangle;
            Rectangle backgroundRect = cr;
            backgroundRect.Inflate(5, 5);
            backgroundRect.Offset(-1, -1);
            cr.Inflate(-1, -1);

            g.FillRectangle(new SolidBrush(BackColor), backgroundRect);

            GraphicsPath path2 = Helper.RoundRect(cr, Rounding);

            using (Brush itemBrush = new SolidBrush(BackgroundColor))
            {
                g.FillPath(itemBrush, path2);
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle itemRect = GetItemRectangle(i);
                itemRect.Inflate(-4, -2);
                itemRect.Offset(0, 2);
                itemRect.Offset(0, -1);

                int yCenterString = itemRect.Y + (ItemHeight / 2) - (Font.Height) + 4;

                GraphicsPath path = Helper.RoundRect(itemRect, ItemRounding);

                if (SelectedIndex == i)
                {
                    using (Brush itemBrush = new SolidBrush(ItemSelectedBackgroundColor))
                    {
                        g.FillPath(itemBrush, path);
                    }

                    string itemText = Items[i].ToString();
                    using (Brush textBrush = new SolidBrush(SelectedForegroundColor))
                    {
                        g.DrawString(itemText, Font, textBrush, itemRect.X + 6, yCenterString);
                    }
                }
                else
                {
                    using (Brush itemBrush = new SolidBrush(BackgroundColor))
                    {
                        g.FillPath(itemBrush, path);
                    }

                    string itemText = Items[i].ToString();
                    using (Brush textBrush = new SolidBrush(ForegroundColor))
                    {
                        g.DrawString(itemText, Font, textBrush, itemRect.X + 6, yCenterString);
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int clickedIndex = IndexFromPoint(e.Location);

            if (clickedIndex >= 0 && clickedIndex < Items.Count)
            {
                SelectedIndex = clickedIndex;
                Refresh();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                OnMouseDown(e);
            }
        }


        //wndproc generated by gpt to refresh listbox on scroll
        private const int WM_VSCROLL = 0x115;
        private const int WM_MSCROLL = 0x20A;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL || m.Msg == WM_MSCROLL)
            {
                Refresh();
            }
        }

    }
}
