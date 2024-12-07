﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CuoreUI.Controls.Shapes
{
    public partial class cuiHexagon : UserControl
    {
        public cuiHexagon()
        {
            InitializeComponent();
        }

        private Color privateOutlineColor = Color.Empty;
        public Color OutlineColor
        {
            get
            {
                return privateOutlineColor;
            }
            set
            {
                privateOutlineColor = value;
                Refresh();
            }
        }

        private Color privatePanelColor = Color.FromArgb(255, 106, 0);
        public Color PanelColor
        {
            get
            {
                return privatePanelColor;
            }
            set
            {
                privatePanelColor = value;
                Refresh();
            }
        }

        private int privateOutlineThickness = 1;
        public int OutlineThickness
        {
            get
            {
                return privateOutlineThickness;
            }
            set
            {
                privateOutlineThickness = value;
                Refresh();
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
                Refresh();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Width -= 1;
            modifiedCR.Height -= 1;

            modifiedCR.Inflate(-OutlineThickness, -OutlineThickness);

            GraphicsPath hexagonPath = Helper.RoundHexagon(modifiedCR, Rounding);

            e.Graphics.FillPath(new SolidBrush(PanelColor), hexagonPath);
            e.Graphics.DrawPath(new Pen(OutlineColor, OutlineThickness), hexagonPath);
        }
    }
}
