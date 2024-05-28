﻿using CuoreUI.TabControlStuff;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiTabControl : TabControl
    {
        public cuiTabControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Appearance = TabAppearance.Buttons;
            DrawMode = TabDrawMode.OwnerDrawFixed;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(126, 42);

            ControlAdded += TabsChanged;
            ControlRemoved += TabsChanged;
        }

        private void TabsChanged(object sender, ControlEventArgs e)
        {
            Invalidate();
        }

        private Color privateBackgroundColor = Color.FromArgb(10, 10, 10);
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

        private int privateRounding = 8;
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

        private Color privateUnselectedTabBackColor = Color.FromArgb(24, 24, 24);
        public Color UnselectedTabBackColor
        {
            get
            {
                return privateUnselectedTabBackColor;
            }
            set
            {
                privateUnselectedTabBackColor = value;
                Invalidate();
            }
        }

        private Color privateSelectedTabBackColor = Color.MediumSlateBlue;
        public Color SelectedTabBackColor
        {
            get
            {
                return privateSelectedTabBackColor;
            }
            set
            {
                privateSelectedTabBackColor = value;
                Invalidate();
            }
        }

        private Color privateHoveredTabBackColor = Color.FromArgb(38, 38, 38);
        public Color HoveredTabBackColor
        {
            get
            {
                return privateHoveredTabBackColor;
            }
            set
            {
                privateHoveredTabBackColor = value;
                Invalidate();
            }
        }

        //

        private Color privateUnselectedTabTextBackColor = Color.FromArgb(64, 64, 64);
        public Color UnselectedTabTextBackColor
        {
            get
            {
                return privateUnselectedTabTextBackColor;
            }
            set
            {
                privateUnselectedTabTextBackColor = value;
                Invalidate();
            }
        }

        private Color privateSelectedTabTextBackColor = Color.White;
        public Color SelectedTabTextBackColor
        {
            get
            {
                return privateSelectedTabTextBackColor;
            }
            set
            {
                privateSelectedTabTextBackColor = value;
                Invalidate();
            }
        }

        private Color privateHoveredTabTextBackColor = Color.FromArgb(92, 92, 92);
        public Color HoveredTabTextBackColor
        {
            get
            {
                return privateHoveredTabTextBackColor;
            }
            set
            {
                privateHoveredTabTextBackColor = value;
                Invalidate();
            }
        }

        private Color privateDeletionTabBackgroundColor = Color.Crimson;
        public Color DeletionTabBackgroundColor
        {
            get
            {
                return privateDeletionTabBackgroundColor;
            }
            set
            {
                privateDeletionTabBackgroundColor = value;
                Invalidate();
            }
        }

        private Color privateDeletionColor = Color.White;
        public Color DeletionColor
        {
            get
            {
                return privateDeletionColor;
            }
            set
            {
                privateDeletionColor = value;
                Invalidate();
            }
        }

        private Color privateAddButtonBackgroundColor = Color.White;
        public Color AddButtonBackgroundColor
        {
            get
            {
                return privateAddButtonBackgroundColor;
            }
            set
            {
                privateAddButtonBackgroundColor = value;
            }
        }

        #region VisualProperties
        public object HoveredTab_ => null;

        public object SelectedTab_ => null;

        public object UnselectedTab_ => null;
        #endregion

        private int HoveredTabIndex = -1;

        protected override void OnPaint(PaintEventArgs e)
        {
            OnMouseMove(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0));

            e.Graphics.FillRectangle(new SolidBrush(BackgroundColor), ClientRectangle);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            for (int i = 0; i < TabPages.Count; i++)
            {
                bool isSelectedTab = i == SelectedIndex;
                bool isHoveredTab = i == HoveredTabIndex;
                Rectangle tabRectangle = GetTabRect(i);
                tabRectangle.Offset(0, 2);
                Color tempTabColor;
                Color tempTextColor;
                Color tempAddButtonColor;

                if (isSelectedTab)
                {
                    //selected
                    tempTabColor = SelectedTabBackColor;
                    tempTextColor = SelectedTabTextBackColor;
                    tempAddButtonColor = SelectedTabTextBackColor;
                }
                else if (isHoveredTab)
                {
                    //hovered
                    tempTabColor = HoveredTabBackColor;
                    tempTextColor = HoveredTabTextBackColor;
                    tempAddButtonColor = HoveredTabTextBackColor;
                }
                else
                {
                    //normal
                    tempTabColor = UnselectedTabBackColor;
                    tempTextColor = UnselectedTabTextBackColor;
                    tempAddButtonColor = UnselectedTabTextBackColor;
                }

                GraphicsPath roundedRectanglePath = Helper.RoundRect(tabRectangle, Rounding);

                if (TabSelectedToDeletion == i)
                {
                    //deletion
                    tempTabColor = DeletionTabBackgroundColor;

                    Rectangle crossmarkRectangle = tabRectangle;
                    crossmarkRectangle.Width = Font.Height;
                    crossmarkRectangle.Height = crossmarkRectangle.Width;
                    crossmarkRectangle.X = tabRectangle.X + ((tabRectangle.Width / 2) - (crossmarkRectangle.Width / 2));
                    crossmarkRectangle.Y = (tabRectangle.Height / 2) - (crossmarkRectangle.Height / 2);
                    GraphicsPath crossmark = Helper.Crossmark(crossmarkRectangle);

                    e.Graphics.FillPath(new SolidBrush(DeletionTabBackgroundColor), roundedRectanglePath);
                    Pen DeletionCrossmarkPen = new Pen(DeletionColor, 2);
                    DeletionCrossmarkPen.EndCap = LineCap.Round;
                    DeletionCrossmarkPen.StartCap = LineCap.Round;
                    e.Graphics.DrawPath(DeletionCrossmarkPen, crossmark);
                }
                else
                {
                    e.Graphics.FillPath(new SolidBrush(tempTabColor), roundedRectanglePath);

                    tabRectangle.Offset(0, tabRectangle.Height / 2);
                    tabRectangle.Offset(0, -1 + -Font.Height / 2);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    e.Graphics.DrawString(TabPages[i].Text, Font, new SolidBrush(tempTextColor), tabRectangle, stringFormat);
                }

                if (i == (TabPages.Count - 1))
                {
                    //last tab, draw plus button
                    Rectangle rect = GetTabRect(i);
                    int height = rect.Height / 2;
                    int scaleDown = height / 12;
                    int scaleDownHalf = scaleDown / 2;

                    addTabRectangle = new Rectangle(rect.Right - height - height / 2, 2 + (height / 2), height, height);
                    addTabRectangle.Offset(scaleDownHalf, scaleDownHalf);
                    addTabRectangle.Inflate(-scaleDown, -scaleDown);

                    GraphicsPath plusBackground = Helper.RoundRect(addTabRectangle, Rounding / 2);
                    e.Graphics.FillPath(new SolidBrush(tempAddButtonColor), plusBackground);

                    addTabRectangle.Offset(0, 0);
                    addTabRectangle.Inflate(-4, -3);

                    GraphicsPath plusSymbol = Helper.Plus(addTabRectangle);
                    Pen AddSymbolPen = new Pen(tempTabColor, 2);
                    AddSymbolPen.EndCap = LineCap.Round;
                    AddSymbolPen.StartCap = LineCap.Round;
                    AddSymbolPen.LineJoin = LineJoin.Round;
                    e.Graphics.DrawPath(AddSymbolPen, plusSymbol);
                }
            }
        }

        Rectangle addTabRectangle = new Rectangle(0, 0, 0, 0);

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(e.Location))
                {
                    if (HoveredTabIndex != i)
                    {
                        HoveredTabIndex = i;
                        Invalidate();
                    }
                    break;
                }
            }
        }

        public string tabNamingConvention
        {
            get;
            set;
        } = "tabPage";

        public string GetUniqueTabName()
        {
            int i = 1;
            string finalUniqueName = string.Empty;
            bool foundUniqueName = false;

            while (!foundUniqueName)
            {
                string generatedTabName = tabNamingConvention + i;
                bool nameExists = false;

                foreach (TabPage tb in TabPages)
                {
                    if (generatedTabName == tb.Name || generatedTabName == tb.Text)
                    {
                        nameExists = true;
                        break;
                    }
                }

                if (!nameExists)
                {
                    finalUniqueName = generatedTabName;
                    foundUniqueName = true;
                }

                i++;
            }

            return finalUniqueName;
        }


        public int TabSelectedToDeletion = -1;

        public void AddTab()
        {
            cuiTabPage tabPage = new cuiTabPage();
            tabPage.Name = GetUniqueTabName();
            tabPage.Text = tabPage.Name;
            tabPage.BackColor = BackgroundColor;
            tabPage.ForeColor = BackgroundColor;

            AddTab(tabPage);
        }

        public void AddTab(string tabName)
        {
            cuiTabPage tabPage = new cuiTabPage();
            tabPage.Name = tabName;
            tabPage.Text = tabName;
            tabPage.BackColor = BackgroundColor;

            AddTab(tabPage);
        }

        public void AddTab(TabPage tabPage)
        {
            CallTabAddedEvent(tabPage);
            TabPages.Add(tabPage);
            SelectedTab = tabPage;
        }

        public void AddTab(cuiTabPage tabPage)
        {
            CallTabAddedEvent(tabPage);
            TabPages.Add(tabPage);
            SelectedTab = tabPage;
        }

        [Description("sender is the added tab!")]
        [Browsable(true)]
        public event EventHandler TabAdded;

        public void CallTabAddedEvent(cuiTabPage tabPage)
        {
            TabAdded.Invoke(tabPage, new EventArgs());
        }

        public void CallTabAddedEvent(TabPage tabPage)
        {
            TabAdded.Invoke(tabPage, new EventArgs());
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button != MouseButtons.Right && e.Button != MouseButtons.Left)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < TabCount; i++)
                {
                    if (addTabRectangle.Contains(e.Location))
                    {
                        AddTab();
                        return;
                    }

                    if (GetTabRect(i).Contains(e.Location))
                    {
                        if (i == TabSelectedToDeletion)
                        {
                            TabPages.RemoveAt(i);
                            break;
                        }
                        Invalidate();
                    }
                }
                TabSelectedToDeletion = -1;
                return;
            }

            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(e.Location))
                {
                    TabSelectedToDeletion = i;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            TabSelectedToDeletion = -1;
            HoveredTabIndex = -1;
            Invalidate();
        }
    }
}
