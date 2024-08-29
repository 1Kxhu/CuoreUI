using System.Windows.Forms;

namespace CuoreUI.Components.cuiFormRounderV2Resources
{
    public partial class FakeForm : Form
    {
        public FakeForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SuspendLayout();
        }
    }
}
