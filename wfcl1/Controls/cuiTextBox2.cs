using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// THIS FILE CONTAINS A MODIFIED VERSION OF A TEXTBOX TAKEN FROM:
// https://github.com/RJCodeAdvance/Custom-TextBox-2--Rounded-Placeholder

// ORIGINAL AUTHOR: RJCodeAdvance
// LICENSE: Unlicense (https://unlicense.org/)

// MODIFICATIONS HAVE BEEN MADE TO THE ORIGINAL CODE TO SUIT CuoreUI
// LIKE ADDING CERTAIN PROPERTIES, OR SLIGHTLY MODIFYING HOW THE CONTROL IS DRAWN

namespace CuoreUI.Controls
{
    [ToolboxBitmap(typeof(TextBox))]
    [DefaultEvent("ContentChanged")]
    public partial class cuiTextBox2 : UserControl
    {
        private Color privateBorderColor = Color.FromArgb(64, 64, 64);
        private Color privateBorderFocusColor = Color.FromArgb(255, 106, 0);
        private int privateBorderSize = 1;
        private bool privateUnderlinedStyle = false;
        private bool privateIsFocused = false;

        private int privateBorderRadius = 8;
        private Color privatePlaceholderColor = Color.DimGray;
        private string privatePlaceholderText = "";
        private bool privateIsPlaceholder = false;
        private bool privateIsPasswordChar = false;

        public event EventHandler ContentChanged;

        public cuiTextBox2()
        {
            InitializeComponent();
            BackColor = Color.FromArgb(10, 10, 10);
            ForeColor = Color.Gray;
            Multiline = false;
            Load += OnLoad;
            GotFocus += OnLoad;
            GlobalMouseHook.OnGlobalMouseClick += HandleGlobalMouseClick; // Subscribe to global mouse clicks
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.privateIsFocused = false;
        }

        private void HandleGlobalMouseClick()
        {
            if (DesignMode)
            {
                return;
            }

            // Check if the mouse click is outside this control
            Point mousePosition = Control.MousePosition;
            if (!this.Bounds.Contains(mousePosition) && privateIsFocused == true)
            {
                ActiveControl = panel1;
                privateIsFocused = false; // Set to false if the click is outside
                Refresh(); // Refresh to update the visual state
                SetPlaceholder(); // Optionally reset placeholder
            }
            else if (this.Bounds.Contains(mousePosition))
            {
                privateIsFocused = true;
                Refresh(); // Refresh to update the visual state
            }
            if (GlobalMouseHook.isHooked)
            {
                GlobalMouseHook.Stop();
            }
            Refresh();
            if (GlobalMouseHook.isHooked)
            {
                GlobalMouseHook.Stop();
            }
        }

        [Category("CuoreUI")]
        public Color BorderColor
        {
            get
            {
                return privateBorderColor;
            }
            set
            {
                privateBorderColor = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public Color BorderFocusColor
        {
            get
            {
                return privateBorderFocusColor;
            }
            set
            {
                privateBorderFocusColor = value;
            }
        }

        [Category("CuoreUI")]
        public int BorderSize
        {
            get
            {
                return privateBorderSize;
            }
            set
            {
                if (value >= 1)
                {
                    privateBorderSize = value;
                    Invalidate();
                }
            }
        }

        [Category("CuoreUI")]
        public bool UnderlinedStyle
        {
            get
            {
                return privateUnderlinedStyle;
            }
            set
            {
                privateUnderlinedStyle = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public bool PasswordChar
        {
            get
            {
                return privateIsPasswordChar;
            }
            set
            {
                privateIsPasswordChar = value;
                if (!privateIsPlaceholder)
                    textBox1.UseSystemPasswordChar = value;
            }
        }

        [Category("CuoreUI")]
        public bool Multiline
        {
            get
            {
                return textBox1.Multiline;
            }
            set
            {
                textBox1.Multiline = value;
                textBox2.Multiline = value;
            }
        }

        [Category("CuoreUI")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                textBox1.BackColor = value;
                textBox2.BackColor = value;
            }
        }

        [Category("CuoreUI")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }

        [Category("CuoreUI")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                textBox1.Font = value;
                textBox2.Font = value;
            }
        }

        private string actualText = "";

        [Category("CuoreUI")]
        public string Content
        {
            get
            {
                return actualText;
            }
            set
            {
                actualText = value;
                textBox1.Text = value;

                SetPlaceholder();
            }
        }

        [Category("CuoreUI")]
        public int Rounding
        {
            get
            {
                return privateBorderRadius;
            }
            set
            {
                if (value >= 0)
                {
                    privateBorderRadius = value;
                    Invalidate();//Redraw control
                }
            }
        }

        [Category("CuoreUI")]
        public Color PlaceholderColor
        {
            get
            {
                return privatePlaceholderColor;
            }
            set
            {
                privatePlaceholderColor = value;
                textBox2.ForeColor = value;
            }
        }

        [Category("CuoreUI")]
        public string PlaceholderText
        {
            get
            {
                return privatePlaceholderText;
            }
            set
            {
                privatePlaceholderText = value;
                SetPlaceholder();
            }
        }

        private Size privateTextOffset = new Size(0, 0);
        [Category("CuoreUI")]
        public Size TextOffset
        {
            get
            {
                return privateTextOffset;
            }
            set
            {
                privateTextOffset = value;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (privateIsPlaceholder)
            {
                textBox2.Visible = true;
            }
            else
            {
                textBox2.Visible = false;
            }

            base.OnPaint(e);
            Graphics graph = e.Graphics;

            Padding newPadding;

            if (Multiline)
            {
                int b = (Rounding / 2) + (Font.Height / 8);
                newPadding = new Padding(Font.Height, b, Font.Height, b);
            }
            else
            {
                int newTextboxY = (Height / 2) - (Font.Height / 2);
                if (newTextboxY < 0)
                {
                    newTextboxY = -newTextboxY;
                }
                newPadding = new Padding(Font.Height, newTextboxY, Font.Height, 0);
            }

            newPadding.Left += TextOffset.Width;
            newPadding.Right += TextOffset.Width;
            newPadding.Top += TextOffset.Height;
            newPadding.Bottom += TextOffset.Height;

            Padding = newPadding;

            if (privateBorderRadius > 1)//Rounded TextBox
            {
                //-Fields
                var rectBorderSmooth = ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -BorderSize, -BorderSize);
                int smoothSize = privateBorderSize > 0 ? privateBorderSize : 1;

                using (GraphicsPath pathBorderSmooth = Helper.RoundRect(rectBorderSmooth, Rounding))
                using (GraphicsPath pathBorder = Helper.RoundRect(rectBorder, Rounding - BorderSize))
                using (Pen penBorderSmooth = new Pen(Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(BorderColor, BorderSize))
                {
                    //-Drawing
                    Region = new Region(pathBorderSmooth);

                    /* { //Old way
                        //Set the rounded region of UserControl
                        if (BorderRadius > 15)
                        SetTextBoxRoundedRegion();//Set the rounded region of TextBox component
                    } */

                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                    if (privateIsFocused)
                        penBorder.Color = BorderFocusColor;

                    if (UnderlinedStyle) //Line Style
                    {
                        //Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                    }
                    else //Normal Style
                    {
                        //Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else //Square/Normal TextBox
            {
                //Draw border
                using (Pen penBorder = new Pen(BorderColor, BorderSize))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    if (privateIsFocused)
                        penBorder.Color = BorderFocusColor;

                    if (UnderlinedStyle) //Line Style
                        graph.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                    else //Normal Style
                        graph.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
                }
            }

            base.OnPaint(e);
        }

        private void SetPlaceholder()
        {
            textBox2.Text = PlaceholderText;
            if (privateIsPasswordChar)
            {
                textBox2.UseSystemPasswordChar = false;
            }

            if (actualText == "")
            {
                textBox2.Visible = true;
                privateIsPlaceholder = true;
            }
            else
            {
                privateIsPlaceholder = false;
                Refresh();
            }
        }
        private void RemovePlaceholder()
        {
            if (actualText == "")
            {
                textBox2.Visible = false;
                privateIsPlaceholder = false;
                textBox2.Text = actualText;
                textBox2.ForeColor = ForeColor;
                if (privateIsPasswordChar)
                    textBox2.UseSystemPasswordChar = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            actualText = textBox1.Text;
            ContentChanged?.Invoke(this, e);
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            OnClick(e);
            GlobalMouseHook.Start(); // Start the global mouse hook
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }
        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            privateIsFocused = true;
            Refresh();
            RemovePlaceholder();
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            privateIsFocused = false;
            Refresh();
            SetPlaceholder();
        }

        private void cuiTextBox2_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            GlobalMouseHook.Start(); // Start the global mouse hook
            Refresh();
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            cuiTextBox2_Click(sender, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != PlaceholderText)
            {
                textBox2.Text = PlaceholderText;
            }
        }
    }
}
