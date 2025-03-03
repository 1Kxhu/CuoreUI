using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CuoreUI.Controls.Forms.DatePickerPages
{
    [ToolboxItem(false)]
    [Description("Don't use, an internal part of datetime picker window.")]
    public partial class YearDatePicker : UserControl
    {
        DatePicker _datePickerForm;

        public YearDatePicker(DatePicker datePickerForm)
        {
            InitializeComponent();
            _datePickerForm = datePickerForm;
            UpdateYearButtons();

            leftMonthButton.Click += (e, s) =>
            {
                datePickerForm.SetYear(datePickerForm.Value.Year - 10, false);
                UpdateYearButtons();
            };

            rightMonthButton.Click += (e, s) =>
            {
                datePickerForm.SetYear(datePickerForm.Value.Year + 10, false);
                UpdateYearButtons();
            };
        }

        internal void UpdateYearButtons()
        {
            int i = 0;
            foreach (Control c in Controls)
            {
                if (c is cuiButtonGroup cbg)
                {
                    int thisButtonsYear = _datePickerForm.Value.Year + i;
                    cbg.Content = thisButtonsYear.ToString();

                    cbg.Checked = false;

                    cbg.Click -= YearButton_Click;
                    cbg.Click += YearButton_Click;
                    i++;
                }
            }

            cuiButtonGroup1.Checked = true; // because the current selected year always appears first
        }

        private void YearButton_Click(object sender, EventArgs e)
        {
            if (sender is cuiButtonGroup cbg && int.TryParse(cbg.Content, out int selectedYear))
            {
                _datePickerForm.SetYear(selectedYear);
                UpdateYearButtons();
            }
        }

    }
}
