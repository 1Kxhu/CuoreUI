using System.Drawing;
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

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            if (SharedVariables.FakeBitmap != null)
            {
                e.Graphics.DrawImage(SharedVariables.FakeBitmap, Point.Empty);
                SharedVariables.FakeBitmap.Dispose();
            }
        }
    }
}
