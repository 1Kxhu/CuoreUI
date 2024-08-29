using CuoreUI.Controls;
using CuoreUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CuoreUI.Helper.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace CuoreUI.Components.Forms
{
    public partial class ColorPickerForm : Form
    {
        public ColorPickerForm()
        {
            InitializeComponent();
            initTimer();
        }

        public ColorPickerForm(Color primaryColor, int Rounding)
        {
            InitializeComponent();
            initTimer();


        }

        private void initTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 16; // wont bother with refresh rates for this (for now)
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private bool currentlyChangingColor = false;
        private bool updateHexTextbox = true;

        private Color privateColorVal = Color.Empty;
        public Color ColorVal
        {
            get
            {
                return privateColorVal;
            }
            set
            {
                currentlyChangingColor = true;

                privateColorVal = value;
                cuiBorder1.PanelColor = value;

                cuiTextBox1.Content = value.A + "";
                cuiTextBox2.Content = value.R + "";
                cuiTextBox3.Content = value.G + "";
                cuiTextBox4.Content = value.B + "";

                if (updateHexTextbox)
                {
                    cuiTextBox5.Content = ColorToHex(value);
                }

                colorPickerWheel1.UpdatePos();

                currentlyChangingColor = false;
            }
        }

        private string ColorToHex(Color value)
        {
            return "#" + value.A.ToString("x") + value.R.ToString("x") + value.G.ToString("x") + value.B.ToString("x");
        }

        private bool isInsideColorPicker()
        {
            return colorPickerWheel1.ClientRectangle.Contains(colorPickerWheel1.PointToClient(Cursor.Position));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isInsideColorPicker() && isClickingLeftMouse())
            {
                ExtractColorFromClickPoint(sender, new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 1));
            }
        }

        static Color GetColorAtCursor()
        {
            // win32 directly instead of making bitmaps for *speed*
            IntPtr hdc = GetDC(IntPtr.Zero);

            GetCursorPos(out POINT p);

            uint pixel = GetPixel(hdc, p.X, p.Y);

            ReleaseDC(IntPtr.Zero, hdc);

            Color color = Color.FromArgb((int)(pixel & 0x000000FF),   // r
                                         (int)(pixel & 0x0000FF00) >> 8,  // g
                                         (int)(pixel & 0x00FF0000) >> 16); // b

            return color;
        }

        private void ExtractColorFromClickPoint(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetColor(GetColorAtCursor());
            }
        }

        internal void SetColor(Color target, bool arg_updatehex = true)
        {
            updateHexTextbox = arg_updatehex;
            ColorVal = target;
            updateHexTextbox = true;
        }

        private void cuiButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ContentChanged(object sender, EventArgs e)
        {
            if ((sender is cuiTextBox) == false || currentlyChangingColor)
            {
                return;
            }

            cuiTextBox textbox = sender as cuiTextBox;

            int tempValue;

            int.TryParse(textbox.Content, out tempValue);

            tempValue = Math.Min(255, tempValue);
            tempValue = Math.Max(0, tempValue);

            if (textbox == cuiTextBox1) //alpha
            {
                ColorVal = Color.FromArgb(tempValue, ColorVal.R, ColorVal.G, ColorVal.B);
            }

            if (textbox == cuiTextBox2) //red
            {
                ColorVal = Color.FromArgb(ColorVal.A, tempValue, ColorVal.G, ColorVal.B);
            }

            if (textbox == cuiTextBox3) //green
            {
                ColorVal = Color.FromArgb(ColorVal.A, ColorVal.R, tempValue, ColorVal.B);
            }

            if (textbox == cuiTextBox4) //blue
            {
                ColorVal = Color.FromArgb(ColorVal.A, ColorVal.R, ColorVal.G, tempValue);
            }
        }

        private void hexInput_ContentChanged(object sender, EventArgs e)
        {
            if ((sender is cuiTextBox) == false)
            {
                return;
            }

            try
            {
                Color gotColor = ColorTranslator.FromHtml(cuiTextBox5.Content);
                if (gotColor != Color.Empty)
                {
                    SetColor(gotColor, false);
                }
            }
            catch //not a color, ignore
            {
                
            }
        }

        private void cuiButton3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cuiButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
