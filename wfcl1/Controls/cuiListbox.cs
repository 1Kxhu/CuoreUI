using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiListbox : ListBox
    {
        public cuiListbox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            DrawMode = DrawMode.OwnerDrawFixed;
            BorderStyle = BorderStyle.None;
            ItemHeight = 30;
            ForeColor = Color.FromArgb(84, 84, 84);
            SelectionMode = SelectionMode.One;
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

            GraphicsPath path2 = Helper.RoundRect(cr, Rounding * 2);

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

                GraphicsPath path = Helper.RoundRect(itemRect, Rounding*2);

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
                Invalidate();
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
    }
}
