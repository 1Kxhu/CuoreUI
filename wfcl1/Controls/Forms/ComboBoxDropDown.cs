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
        private string[] privateItems = new string[] { "" };
        public string[] Items
        {
            get
            {
                return privateItems;
            }
            set
            {
                privateItems = value;
                Invalidate();
            }
        }
        public ComboBoxDropDown()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int i = 0;
            int firstItem = 0;
            int lastItem = Items.Length - 1;
            foreach (var item in Items)
            {
                if (i == firstItem)
                {

                }
                i++;
            }
        }
    }
}
