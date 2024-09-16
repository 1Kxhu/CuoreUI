using CuoreUI.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    public partial class cuiColorPicker : Component
    {
        private ColorPickerForm PickerForm;

        private Color privateColor;
        public Color Color
        {
            get
            {
                return privateColor;
            }
            set
            {
                privateColor = value;
            }
        }

        public cuiColorPicker()
        {
            InitializeComponent();
        }

        public bool isShowingDialog
        {
            get
            {
                return PickerForm != null && PickerForm.Visible;
            }
        }

        public cuiColorPicker(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public async Task<DialogResult> ShowDialog()
        {
            PickerForm = new ColorPickerForm();
            PickerForm.Show();

            bool canExitLoop = false;

            PickerForm.FormClosing += (s, e) =>
            {
                Color = PickerForm.ColorVal;
                canExitLoop = true;
            };

            while (!canExitLoop) //sorry
            {
                await Task.Delay(100);
            }

            return PickerForm.DialogResult;
        }
    }
}
