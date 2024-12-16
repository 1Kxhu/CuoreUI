using CuoreUI.Components.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuoreUI.Components
{
    public partial class cuiTooltipHover : Component
    {
        private TooltipForm tooltipForm => TooltipController.tooltipForm;
        public cuiTooltipHover()
        {
            InitializeComponent();
        }

        public cuiTooltipHover(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private Control privateTargetControl;
        public Control TargetControl
        {
            get => privateTargetControl;
            set
            {
                privateTargetControl = value;
                if (privateTargetControl != null)
                {
                    value.MouseHover += MouseHover;
                }
            }
        }

        private string privateContent = "Tooltip Text";
        public string Content
        {
            get => privateContent;
            set
            {
                privateContent = value;
            }
        }

        public Color ForeColor
        {
            get
            {
                return tooltipForm.ForeColor;
            }
            set
            {
                tooltipForm.ForeColor = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return tooltipForm.BackColor;
            }
            set
            {
                tooltipForm.BackColor = value;
                tooltipForm.cuiFormRounder.OutlineColor = value;
            }
        }

        private async void MouseHover(object sender, System.EventArgs e)
        {
            tooltipForm.Show();
            tooltipForm.Hide();

            tooltipForm.Text = privateContent;

            tooltipForm.Location = Cursor.Position - new Size((tooltipForm.Width / 2), -1);

            tooltipForm.Show();
            TargetControl.Focus();

            tooltipForm.BringToFront();

            tooltipForm.ToggleRoundedObj(true);

            while (true)
            {
                await Task.Delay(Drawing.LazyInt32TimeDelta);
                if (TargetControl.ClientRectangle.Contains(TargetControl.PointToClient(Cursor.Position)) == false)
                {
                    break;
                }

                tooltipForm.Location = Cursor.Position - new Size((tooltipForm.Width / 2), -1);
            }

            tooltipForm.ToggleRoundedObj(false);
            tooltipForm.Hide();
        }
    }
}
