namespace CuoreUI.Controls.Forms
{
    partial class ComboBoxDropDown
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cuiFormRounder1 = new CuoreUI.Components.cuiFormRounder();
            this.SuspendLayout();
            // 
            // cuiFormRounder1
            // 
            this.cuiFormRounder1.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cuiFormRounder1.Rounding = 8;
            this.cuiFormRounder1.TargetForm = this;
            // 
            // ComboBoxDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(-1000, -1000);
            this.Name = "ComboBoxDropDown";
            this.Text = "ComboBoxDropDown";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public Components.cuiFormRounder cuiFormRounder1;
    }
}