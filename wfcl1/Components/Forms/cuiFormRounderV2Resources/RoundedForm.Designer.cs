﻿namespace CuoreUI.Components.cuiFormRounderV2Resources
{
    partial class RoundedForm
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
        public bool initialized = false;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Opacity = 0;
            Visible = false;
            Size = new System.Drawing.Size(1,1);
            Location = new System.Drawing.Point(0,0);
            this.SuspendLayout();
            // 
            // RoundedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RoundedForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RoundedForm";
            this.PaddingChanged += new System.EventHandler(this.RoundedForm_PaddingChanged);
            this.ResumeLayout(false);
        }

        #endregion
    }
}