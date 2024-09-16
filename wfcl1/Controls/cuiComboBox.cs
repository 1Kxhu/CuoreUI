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
                int dropdownTop = LocationScreen.Y + Height + tempdropdown.cuiFormRounder1.Rounding;
                int dropdownLeft = LocationScreen.X + 3;
                tempdropdown.Location = new Point(dropdownLeft, dropdownTop);
            }
        }

        int timercountdown = 0;
        int maxcountdown = 15;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DesignMode || tempdropdown == null)
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
                CloseDropDown(tempdropdown, EventArgs.Empty);
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

            GraphicsPath roundBackground = Helper.RoundRect(cr, Rounding);

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

            if (tempdropdown != null)
            {
                tempdropdown.Close();
            }

            ComboBoxDropDown DropDown = new ComboBoxDropDown(Items, Width, DropDownBackgroundColor, DropDownOutlineColor, this, Rounding);
            DropDown.NormalBackground = ButtonNormalBackground;
            DropDown.HoverBackground = ButtonHoverBackground;
            DropDown.PressedBackground = ButtonPressedBackground;

            DropDown.NormalOutline = ButtonNormalOutline;
            DropDown.HoverOutline = ButtonHoverOutline;
            DropDown.PressedOutline = ButtonPressedOutline;

            DropDown.Rounding = new Padding(Rounding, Rounding, Rounding, Rounding);
            DropDown.updateButtons();

            DropDown.Invalidate();

            isBrowsingOptions = true;
            Refresh();

            int a = Items.Length * 45;
            Rectangle clientrect = ClientRectangle;
            clientrect.Offset(0, clientrect.Height);
            clientrect.Height = a;

            // Convert client rectangle to screen coordinates
            clientrect = RectangleToScreen(clientrect);


            DropDown.cuiFormRounder1.roundedFormObj.Invalidate();

            Point LocationScreen = PointToScreen(Point.Empty);
            int dropdownTop = LocationScreen.Y + Height + 2;
            int dropdownLeft = LocationScreen.X + DropDown.cuiFormRounder1.Rounding;
            DropDown.Location = new Point(dropdownLeft - 1, dropdownTop - 1);
            DropDown.Size = DropDown.Size + new Size(2, 2);

            tempdropdown = DropDown;
            tempdropdown.Rounding = new Padding(Rounding, Rounding, Rounding, Rounding);

            DropDown.cuiFormRounder1.roundedFormObj.Location = DropDown.Location;
            DropDown.Width = Width - 4;
            DropDown.cuiFormRounder1.roundedFormObj.Invalidate();
            DropDown.SelectedIndexChanged += IndexChanged;

            DropDown.cuiFormRounder1.roundedFormObj.Show();
            DropDown.Show();

            DropDown.cuiFormRounder1.TargetForm.Invalidate();
        }
        private void CloseDropDown(object sender, EventArgs e)
        {
            if (tempdropdown != null)
            {
                tempdropdown.Close();
            }
            if (sender is ComboBoxDropDown dropdown)
            {
                dropdown.Close();

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
            if (tempdropdown != null)
            {
                tempdropdown.Close();
            }
            if (sender is ComboBoxDropDown dropdown)
            {
                SelectedItem = dropdown.SelectedItem;

                dropdown.Close();
                tempdropdown.Close();

                isBrowsingOptions = false;
                Refresh();

                dropdown = null;
            }
            else if (tempdropdown != null)
            {
                tempdropdown.cuiFormRounder1.roundedFormObj.Close();
                tempdropdown.cuiFormRounder1.TargetForm = null;
                tempdropdown.cuiFormRounder1.Dispose();
                tempdropdown.Dispose();

                tempdropdown.Close();

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
        } = CuoreUI.Drawing.PrimaryColor;

        public Color ButtonHoverBackground
        {
            get; set;
        } = Color.FromArgb(200, 255, 127, 80);

        public Color ButtonPressedBackground
        {
            get; set;
        } = CuoreUI.Drawing.PrimaryColor;

        public Color ButtonNormalOutline
        {
            get; set;
        } = CuoreUI.Drawing.PrimaryColor;

        public Color ButtonHoverOutline
        {
            get; set;
        } = CuoreUI.Drawing.PrimaryColor;

        public Color ButtonPressedOutline
        {
            get; set;
        } = CuoreUI.Drawing.PrimaryColor;
    }
}
