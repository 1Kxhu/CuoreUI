using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace CuoreUI.Controls.Forms.DatePickerPages
{
    [ToolboxItem(false)]
    [Description("Don't use, an internal part of datetime picker window.")]
    public partial class MonthDatePicker : System.Windows.Forms.UserControl
    {
        DatePicker _datePickerForm;

        public MonthDatePicker(DatePicker datePickerForm)
        {
            InitializeComponent();
            _datePickerForm = datePickerForm;

            int dayIndex = 0;
            foreach (cuiLabel dayNameLabel in panel1.Controls)
            {
                dayNameLabel.Content = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[dayIndex % 7];
                dayIndex++;
            }

            UpdateDayButtons();
        }

        internal void UpdateDayButtons()
        {
            int daysInMonth = DateTime.DaysInMonth(_datePickerForm.Value.Year, _datePickerForm.Value.Month);
            DateTime firstDay = new DateTime(_datePickerForm.Value.Year, _datePickerForm.Value.Month, 1);
            int startColumn = (int)firstDay.DayOfWeek; // Sunday = 0, Monday = 1, ..., Saturday = 6

            int[] columnPositions = { 0, 56, 112, 168, 224, 280, 336 };
            int[] rowPositions = { 3, 33, 63, 93, 123, 153, 183 };

            int currentRow = 0;
            int currentColumn = startColumn;

            foreach (cuiButtonGroup dayButton in dayPanel.Controls)
            {
                int thisButtonsDay = int.Parse(dayButton.Content);
                if (thisButtonsDay > daysInMonth)
                {
                    dayButton.Visible = false; // Hide extra buttons
                    continue;
                }

                dayButton.Checked = thisButtonsDay == _datePickerForm.Value.Day;

                dayButton.Visible = true;
                dayButton.Location = new Point(columnPositions[currentColumn], rowPositions[currentRow]);

                dayButton.Click -= DayButton_Click;
                dayButton.Click += DayButton_Click;

                currentColumn++;
                if (currentColumn > 6)
                {
                    currentColumn = 0;
                    currentRow++;
                }
            }
        }

        private void DayButton_Click(object sender, EventArgs e)
        {
            if (sender is cuiButtonGroup dayButton && int.TryParse(dayButton.Content, out int thisButtonsDay))
            {
                _datePickerForm.SetDayMonth(thisButtonsDay, _datePickerForm.Value.Month);
                UpdateDayButtons();
            }
        }

        private void leftMonthButton_Click(object sender, EventArgs e)
        {
            int wantedMonth = _datePickerForm.Value.Month - 1;
            if (wantedMonth < 1) // go back 1 year
            {
                int daysInMonth = DateTime.DaysInMonth(_datePickerForm.Value.Year - 1, 12);
                int dayToSelect = Math.Min(daysInMonth, _datePickerForm.Value.Day);
                DateTime newDatePickerValue = new DateTime(_datePickerForm.Value.Year - 1, 12, dayToSelect);
                _datePickerForm.Value = newDatePickerValue;
            }
            else
            {
                _datePickerForm.SetDayMonth(_datePickerForm.Value.Day, wantedMonth);
            }
            UpdateDayButtons();
        }

        private void rightMonthButton_Click(object sender, EventArgs e)
        {
            int wantedMonth = _datePickerForm.Value.Month + 1;
            if (wantedMonth > 12) // go back 1 year
            {
                int daysInMonth = DateTime.DaysInMonth(_datePickerForm.Value.Year + 1, 1);
                int dayToSelect = Math.Min(daysInMonth, _datePickerForm.Value.Day);
                DateTime newDatePickerValue = new DateTime(_datePickerForm.Value.Year + 1, 1, dayToSelect);
                _datePickerForm.Value = newDatePickerValue;
            }
            else
            {
                _datePickerForm.SetDayMonth(_datePickerForm.Value.Day, wantedMonth);
            }
            UpdateDayButtons();
        }
    }
}
