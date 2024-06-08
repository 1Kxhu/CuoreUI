using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI.Controls.Forms
{
    public partial class ComboBoxDropDown : Form
    {
        public cuiComboBox caller;

        private string[] privateItems = new string[0];
        public string[] Items
        {
            get
            {
                return privateItems;
            }
            set
            {
                privateItems = value;
                parseItems();
            }
        }

        public Color NoItemsForeColor
        {
            get; set;
        } = Color.Gray;

        void parseItems()
        {
            cuiButton[] options = new cuiButton[Items.Length];

            int i = 0;
            int firstItem = 0;
            int lastItem = Items.Length - 1;
            int defaultHeight = 0;
            int defaultWidth = 0;
            foreach (string item in Items)
            {
                if (item.Trim() == string.Empty)
                {
                    lastItem--;
                    continue;
                }

                cuiButton cuiButton = new cuiButton();
                cuiButton.Name = item;
                cuiButton.Width = Width;
                cuiButton.Content = item;
                cuiButton.Location = new Point(0, i * cuiButton.Height);
                defaultHeight = cuiButton.Height;
                defaultWidth = cuiButton.Width;


                if (i == firstItem)
                {
                    cuiButton.Rounding = new Padding(Rounding.All, Rounding.All, 0, 0);
                }
                else if (i == lastItem)
                {
                    cuiButton.Rounding = new Padding(0, 0, Rounding.All, Rounding.All);
                }
                else
                {
                    cuiButton.Rounding = new Padding(0);
                }

                if (i == firstItem && i == lastItem)
                {
                    cuiButton.Rounding = Rounding;
                }

                cuiButton.Click += (e, s) =>
                {
                    // Ensure the caller's SelectedItem is updated correctly
                    caller.SelectedItem = item;

                    // Ensure the dropdown's SelectedItem is updated correctly
                    SelectedItem = item;

                    // Hide the dropdown
                    Visible = false;

                    // Invoke the SelectedIndexChanged event
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                };

                options[i] = cuiButton;
                i++;
            }

            SuspendLayout();
            Opacity = 0;
            Size = new Size(Width, i * defaultHeight);
            Controls.Clear();
            cuiLabel label = new cuiLabel();

            if (i < 1)
            {
                label.Content = "*Nothing Here*";
                label.ForeColor = NoItemsForeColor;
                label.Font = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point);
                Controls.Add(label);
            }
            else
            {
                Controls.AddRange(options);
            }

            Visible = true;
            Opacity = 1;
            ResumeLayout();

            if (i < 1)
            {
                label.Width = Width;
                label.Location = new Point(label.Location.X, label.Location.Y + (Height / 2) - (Font.Height / 2));
            }

            Invalidate();
        }

        public void updateButtons()
        {
            foreach (Control ctrl in Controls)
            {
                if (ctrl is cuiButton cb)
                {
                    cb.NormalBackground = NormalBackground;
                    cb.HoverBackground = HoverBackground;
                    cb.PressedBackground = PressedBackground;

                    cb.NormalOutline = NormalOutline;
                    cb.HoverOutline = HoverOutline;
                    cb.PressedOutline = PressedOutline;
                }
            }
        }

        public EventHandler SelectedIndexChanged;

        internal void GoTo(Point position)
        {
            Location = position;
        }

        public void SetWidth(int userWidth)
        {
            Width = userWidth - (comboBoxRounder1.Rounding * 2);
        }

        public ComboBoxDropDown(int x, int y)
        {
            InitializeComponent();
            Location = new Point(x, y);
        }

        public ComboBoxDropDown(string[] userItems, int userWidth, Color bg, Color outline, cuiComboBox userCaller)
        {
            InitializeComponent();
            Width = userWidth - (comboBoxRounder1.Rounding*2);
            comboBoxRounder1.BackColor = bg;
            comboBoxRounder1.OutlineColor = outline;
            BackColor = bg;
            caller = userCaller;
            Items = userItems;
        }

        public ComboBoxDropDown()
        {
            InitializeComponent();
        }

        public int SelectedIndex = 0;
        public string SelectedItem = string.Empty;

        private void ComboBoxDropDown_MouseLeave(object sender, EventArgs e)
        {
        
        }

        private void ComboBoxDropDown_Click(object sender, EventArgs e)
        {
        
        }

        // button properties

        private Padding privateRounding = new Padding(8);
        public Padding Rounding
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

        private Color privateNormalBackground = Color.MediumSlateBlue;
        public Color NormalBackground
        {
            get
            {
                return privateNormalBackground;
            }
            set
            {
                privateNormalBackground = value;
                Invalidate();
            }
        }

        private Color privateHoverBackground = Color.FromArgb(200, 123, 104, 238);
        public Color HoverBackground
        {
            get
            {
                return privateHoverBackground;
            }
            set
            {
                privateHoverBackground = value;
                Invalidate();
            }
        }

        private Color privatePressedBackground = Color.MediumSlateBlue;
        public Color PressedBackground
        {
            get
            {
                return privatePressedBackground;
            }
            set
            {
                privatePressedBackground = value;
                Invalidate();
            }
        }

        private Color privateNormalOutline = Color.MediumSlateBlue;
        public Color NormalOutline
        {
            get
            {
                return privateNormalOutline;
            }
            set
            {
                privateNormalOutline = value;
                Invalidate();
            }
        }

        private Color privateHoverOutline = Color.MediumSlateBlue;
        public Color HoverOutline
        {
            get
            {
                return privateHoverOutline;
            }
            set
            {
                privateHoverOutline = value;
                Invalidate();
            }
        }

        private Color privatePressedOutline = Color.MediumSlateBlue;
        public Color PressedOutline
        {
            get
            {
                return privatePressedOutline;
            }
            set
            {
                privatePressedOutline = value;
                Invalidate();
            }
        }
    }
}
