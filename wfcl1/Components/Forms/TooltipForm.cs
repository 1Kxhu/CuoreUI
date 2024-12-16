using System;
using System.Drawing;
using System.Windows.Forms;

namespace CuoreUI.Components.Forms
{
    public partial class TooltipForm : Form
    {
        public TooltipForm()
        {
            InitializeComponent();
            TextChanged += TooltipForm_TextChanged;
        }

        public cuiFormRounder cuiFormRounder => cuiFormRounder1;

        private void TooltipForm_TextChanged(object sender, EventArgs e)
        {
            cuiLabel1.Content = Text;
            Size textSize = CreateGraphics().MeasureString(Text, cuiLabel1.Font).ToSize();
            Size = new Size(textSize.Width + 2 + cuiFormRounder1.Rounding, textSize.Height * 2);
        }

        public void ToggleRoundedObj(bool value)
        {
            if (value)
            {
                cuiFormRounder1.UpdateRoundedFormRegion();
            }
            cuiFormRounder1.roundedFormObj.Visible = value;
            if (!value)
            {
                Region a = cuiFormRounder1.roundedFormObj.Region.Clone();
                a.Exclude(cuiFormRounder1.roundedFormObj.Region);
                cuiFormRounder1.roundedFormObj.Region = a;
            }
        }

        private void TooltipForm_Resize(object sender, EventArgs e)
        {
            cuiLabel1.Location = new Point(0, cuiLabel1.Font.Height / 2);
            cuiLabel1.Width = Width;
            cuiLabel1.Height = Height;
        }

        private void TooltipForm_ForeColorChanged(object sender, EventArgs e)
        {
            cuiLabel1.ForeColor = ForeColor;
        }
    }
}
