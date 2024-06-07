﻿using CuoreUI.Controls.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiComboBox : UserControl
    {
        private string privateSelectedItem = string.Empty;
        private string[] privateItems = new string[0];

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string[] Items
        {
            get => privateItems;
            set
            {
                privateItems = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedItem
        {
            get => privateSelectedItem;
            set
            {
                value = value.Trim();
                if (Items.Contains(value))
                {
                    privateSelectedItem = value;
                    Invalidate();
                }
                else
                {
                    SelectedItem = string.Empty;
                }
            }
        }

        public cuiComboBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);

            ForeColor = SystemColors.Control;

            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();

            Timer dropdownmovetimer = new Timer();
            dropdownmovetimer.Interval = 10;
            dropdownmovetimer.Tick += dropdownmove;
            dropdownmovetimer.Start();
        }

        private void dropdownmove(object sender, EventArgs e)
        {
            if (tempdropdown != null)
            {
                Point LocationScreen = PointToScreen(Point.Empty);
                int dropdownTop = LocationScreen.Y + Height + tempdropdown.comboBoxRounder1.Rounding;
                int dropdownLeft = LocationScreen.X + tempdropdown.comboBoxRounder1.Rounding;
                tempdropdown.Location = new Point(dropdownLeft, dropdownTop);
            }
        }

        int timercountdown = 0;
        int maxcountdown = 15;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DesignMode || tempdropdown is null)
            {
                return;
            }

            Point pos = Cursor.Position;
            Rectangle dropdownRect = RectangleToScreen(ClientRectangle);
            dropdownRect.Height += Items.Length * 45;

            // Check if the cursor is within the dropdown rectangle
            bool cursorInRectangle = dropdownRect.Contains(pos);

            if (cursorInRectangle)
            {
                // Reset the countdown if the cursor is inside the rectangle
                timercountdown = 0;
            }
            else
            {
                // Increment the countdown if the cursor is outside the rectangle
                timercountdown++;
            }

            if (timercountdown >= maxcountdown)
            {
                // Close if the countdown reaches the maximum value
                timercountdown = 0;
                IndexChanged(null, EventArgs.Empty);
            }
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

        private Color privateOutlineColor = Color.FromArgb(35, 255, 255, 255);
        public Color OutlineColor
        {
            get
            {
                return privateOutlineColor;
            }
            set
            {
                privateOutlineColor = value;
                Invalidate();
            }
        }

        private Color privateDropDownBackgroundColor = Color.FromArgb(14, 14, 14);
        public Color DropDownBackgroundColor
        {
            get
            {
                return privateDropDownBackgroundColor;
            }
            set
            {
                privateDropDownBackgroundColor = value;
                Invalidate();
            }
        }


        private Color privateDropDownOutlineColor = Color.FromArgb(30, 255, 255, 255);
        public Color DropDownOutlineColor
        {
            get
            {
                return privateDropDownOutlineColor;
            }
            set
            {
                privateDropDownOutlineColor = value;
                Invalidate();
            }
        }

        private float privateOutlineThickness = 1;
        public float OutlineThickness
        {
            get
            {
                return privateOutlineThickness;
            }
            set
            {
                privateOutlineThickness = value;
                Invalidate();
            }
        }

        public bool isBrowsingOptions = false;

        private void cuiComboBox_Paint(object sender, PaintEventArgs e)
        {
            if (SelectedIndex == -1 && privateSelectedItem != string.Empty)
            {
                privateSelectedItem = string.Empty;
                Refresh();
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle cr = ClientRectangle;
            cr.Inflate(-1, -1);

            GraphicsPath roundBackground = Helper.RoundRect(cr, 8);

            using (SolidBrush brush = new SolidBrush(BackgroundColor))
            using (Pen pen = new Pen(OutlineColor, OutlineThickness))
            {
                e.Graphics.FillPath(brush, roundBackground);
                e.Graphics.DrawPath(pen, roundBackground);
            }

            StringFormat centerText = new StringFormat();
            centerText.Alignment = StringAlignment.Center;

            string tempItemString = privateSelectedItem;

            if (privateSelectedItem.Trim() == string.Empty)
            {
                tempItemString = "None";
            }

            e.Graphics.DrawString(tempItemString, Font, new SolidBrush(ForeColor), new Point(Width / 2, (Height / 2) - (Font.Height / 2)), centerText);

            //e.Graphics.DrawString(isBrowsingOptions.ToString(), Font, new SolidBrush(ForeColor), new Point(Width / 2, 0), centerText);

            Rectangle expandRect = ClientRectangle;
            expandRect.Width = Height / 2;
            expandRect.X = ClientRectangle.Right - Height / 2;
            expandRect.Height = expandRect.Width;
            expandRect.Offset(-expandRect.Width / 2, expandRect.Height / 2);

            expandRect.Width = expandRect.Width / 2;
            expandRect.X = ClientRectangle.Right - Height / 2;
            expandRect.Height = expandRect.Width;
            expandRect.Offset(-expandRect.Width / 2, (expandRect.Height / 2));

            GraphicsPath expandAvailable;
            if (isBrowsingOptions)
            {
                expandRect.Height -= 2;
                expandRect.Y += 1;
                expandAvailable = Helper.DownArrow(expandRect);
            }
            else
            {
                expandRect.Width -= 2;
                expandAvailable = Helper.LeftArrowtest(expandRect);
            }
            e.Graphics.FillPath(new SolidBrush(ExpandColor), expandAvailable);
            //e.Graphics.DrawRectangle(new Pen(Color.Red), expandRect);
            //e.Graphics.DrawRectangle(new Pen(Color.Green), ClientRectangle);

        }

        private Color privateExpandColor = Color.White;
        public Color ExpandColor
        {
            get
            {
                return privateExpandColor;
            }
            set
            {
                privateExpandColor = value;
                Invalidate();
            }
        }


        public void AddItem(string itemToAdd)
        {
            int newLength = privateItems.Length + 1;
            string[] newItemsArray = new string[newLength];

            Array.Copy(privateItems, newItemsArray, privateItems.Length);
            newItemsArray[newLength - 1] = itemToAdd;

            Items = newItemsArray;
        }

        public void RemoveItem(string itemToRemove)
        {
            int indexToRemove = Array.IndexOf(privateItems, itemToRemove);

            if (indexToRemove != -1)
            {
                string[] newItemsArray = new string[privateItems.Length - 1];

                Array.Copy(privateItems, 0, newItemsArray, 0, indexToRemove);
                Array.Copy(privateItems, indexToRemove + 1, newItemsArray, indexToRemove, privateItems.Length - indexToRemove - 1);

                Items = newItemsArray;
            }
        }

        public int SelectedIndex => Array.IndexOf(privateItems, SelectedItem);

        private void cuiComboBox_Click(object sender, EventArgs e)
        {
            if (isBrowsingOptions)
            {
                IndexChanged(null, EventArgs.Empty);
                return;
            }

            ComboBoxDropDown DropDown = new ComboBoxDropDown(Items, Width, DropDownBackgroundColor, DropDownOutlineColor, this);
            DropDown.NormalBackground = ButtonNormalBackground;
            DropDown.HoverBackground = ButtonHoverBackground;
            DropDown.PressedBackground = ButtonPressedBackground;

            DropDown.NormalBackground = ButtonNormalOutline;
            DropDown.HoverBackground = ButtonHoverOutline;
            DropDown.PressedBackground = ButtonPressedOutline;

            DropDown.Rounding = new Padding(Rounding);
            DropDown.updateButtons();

            isBrowsingOptions = true;
            Refresh();

            int a = Items.Length * 45;
            Rectangle clientrect = ClientRectangle;
            clientrect.Offset(0, clientrect.Height);
            clientrect.Height = a;

            // Convert client rectangle to screen coordinates
            clientrect = RectangleToScreen(clientrect);


            DropDown.comboBoxRounder1.RoundedForm.Invalidate();

            Point LocationScreen = PointToScreen(Point.Empty);
            int dropdownTop = LocationScreen.Y + Height + DropDown.comboBoxRounder1.Rounding;
            int dropdownLeft = LocationScreen.X + DropDown.comboBoxRounder1.Rounding;
            DropDown.Location = new Point(dropdownLeft, dropdownTop);

            tempdropdown = DropDown;

            DropDown.Show();
            DropDown.comboBoxRounder1.RoundedForm.Location = DropDown.Location;
            DropDown.comboBoxRounder1.RoundedForm.Invalidate();
            DropDown.SelectedIndexChanged += IndexChanged;

            DropDown.comboBoxRounder1.RoundedForm.Show();
            DropDown.Show();
        }
        private void CloseDropDown(object sender, EventArgs e)
        {
            if (sender is ComboBoxDropDown dropdown)
            {
                dropdown.comboBoxRounder1.GetRoundedForm().Close();
                dropdown.comboBoxRounder1.TargetForm = null;
                dropdown.comboBoxRounder1.Dispose();
                dropdown.Dispose();

                isBrowsingOptions = false;
                Refresh();
            }
            else if (sender is null)
            {
                isBrowsingOptions = false;
                Refresh();
            }
            else
            {
                throw new Exception($"Invalid sender\n{sender}");
            }
        }

        ComboBoxDropDown tempdropdown;

        private void IndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBoxDropDown dropdown)
            {
                SelectedItem = dropdown.SelectedItem;

                dropdown.comboBoxRounder1.GetRoundedForm().Close();
                dropdown.comboBoxRounder1.TargetForm = null;
                dropdown.comboBoxRounder1.Dispose();
                dropdown.Dispose();

                isBrowsingOptions = false;
                Refresh();
            }
            else if (tempdropdown != null)
            {
                tempdropdown.comboBoxRounder1.GetRoundedForm().Close();
                tempdropdown.comboBoxRounder1.TargetForm = null;
                tempdropdown.comboBoxRounder1.Dispose();
                tempdropdown.Dispose();

                isBrowsingOptions = false;
                Refresh();

                tempdropdown = null;
            }
            else
            {
                throw new Exception($"Invalid sender\n{sender}");
            }
        }

        // dropdown buttons

        public int Rounding
        {
            get; set;
        }

        public Color ButtonNormalBackground
        {
            get; set;
        }

        public Color ButtonHoverBackground
        {
            get; set;
        }

        public Color ButtonPressedBackground
        {
            get; set;
        }

        public Color ButtonNormalOutline
        {
            get; set;
        }

        public Color ButtonHoverOutline
        {
            get; set;
        }

        public Color ButtonPressedOutline
        {
            get; set;
        }
    }
}
