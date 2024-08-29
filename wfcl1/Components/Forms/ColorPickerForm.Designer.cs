namespace CuoreUI.Components.Forms
{
    partial class ColorPickerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPickerForm));
            this.colorPickerWheel1 = new CuoreUI.Controls.ColorPickerWheel();
            this.cuiLabel3 = new CuoreUI.Controls.cuiLabel();
            this.cuiButton3 = new CuoreUI.Controls.cuiButton();
            this.cuiButton2 = new CuoreUI.Controls.cuiButton();
            this.cuiButton1 = new CuoreUI.Controls.cuiButton();
            this.cuiLabel1 = new CuoreUI.Controls.cuiLabel();
            this.cuiLabel2 = new CuoreUI.Controls.cuiLabel();
            this.cuiBorder1 = new CuoreUI.Controls.cuiBorder();
            this.cuiTextBox5 = new cuiTextBox();
            this.cuiTextBox4 = new cuiTextBox();
            this.cuiTextBox3 = new cuiTextBox();
            this.cuiTextBox2 = new cuiTextBox();
            this.cuiTextBox1 = new cuiTextBox();
            this.cuiFormRounder1 = new CuoreUI.Components.cuiFormRounder();
            this.cuiFormDrag1 = new CuoreUI.cuiFormDrag(this.components);
            this.SuspendLayout();
            // 
            // colorPickerWheel1
            // 
            this.colorPickerWheel1.Content = ((System.Drawing.Image)(resources.GetObject("colorPickerWheel1.Content")));
            this.colorPickerWheel1.CornerRadius = 8;
            this.colorPickerWheel1.ImageTint = System.Drawing.Color.White;
            this.colorPickerWheel1.Location = new System.Drawing.Point(10, 86);
            this.colorPickerWheel1.Name = "colorPickerWheel1";
            this.colorPickerWheel1.Size = new System.Drawing.Size(186, 186);
            this.colorPickerWheel1.TabIndex = 15;
            // 
            // cuiLabel3
            // 
            this.cuiLabel3.Content = "Hex:\\ ";
            this.cuiLabel3.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cuiLabel3.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.cuiLabel3.HorizontalAlignment = CuoreUI.Controls.cuiLabel.HorizontalAlignments.Right;
            this.cuiLabel3.Location = new System.Drawing.Point(12, 353);
            this.cuiLabel3.Name = "cuiLabel3";
            this.cuiLabel3.Size = new System.Drawing.Size(88, 24);
            this.cuiLabel3.TabIndex = 14;
            // 
            // cuiButton3
            // 
            this.cuiButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiButton3.CheckButton = false;
            this.cuiButton3.Checked = false;
            this.cuiButton3.CheckedBackground = System.Drawing.Color.Coral;
            this.cuiButton3.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton3.CheckedOutline = System.Drawing.Color.Coral;
            this.cuiButton3.Content = "Cancel";
            this.cuiButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cuiButton3.HoverBackground = System.Drawing.Color.Empty;
            this.cuiButton3.HoveredImageTint = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cuiButton3.HoverOutline = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(80)))));
            this.cuiButton3.Image = global::CuoreUI.Properties.Resources.crossmark;
            this.cuiButton3.ImageAutoCenter = true;
            this.cuiButton3.ImageExpand = new System.Drawing.Point(2, 2);
            this.cuiButton3.ImageOffset = new System.Drawing.Point(-2, 0);
            this.cuiButton3.ImageTint = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cuiButton3.Location = new System.Drawing.Point(202, 408);
            this.cuiButton3.Name = "cuiButton3";
            this.cuiButton3.NormalBackground = System.Drawing.Color.Empty;
            this.cuiButton3.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cuiButton3.OutlineThickness = 1.6F;
            this.cuiButton3.PressedBackground = System.Drawing.Color.Coral;
            this.cuiButton3.PressedImageTint = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cuiButton3.PressedOutline = System.Drawing.Color.Empty;
            this.cuiButton3.Rounding = new System.Windows.Forms.Padding(8);
            this.cuiButton3.Size = new System.Drawing.Size(186, 43);
            this.cuiButton3.TabIndex = 13;
            this.cuiButton3.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton3.Click += new System.EventHandler(this.cuiButton3_Click);
            // 
            // cuiButton2
            // 
            this.cuiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiButton2.CheckButton = false;
            this.cuiButton2.Checked = false;
            this.cuiButton2.CheckedBackground = System.Drawing.Color.Coral;
            this.cuiButton2.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton2.CheckedOutline = System.Drawing.Color.Coral;
            this.cuiButton2.Content = "Done";
            this.cuiButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton2.ForeColor = System.Drawing.Color.White;
            this.cuiButton2.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(80)))));
            this.cuiButton2.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton2.HoverOutline = System.Drawing.Color.Empty;
            this.cuiButton2.Image = global::CuoreUI.Properties.Resources.yes;
            this.cuiButton2.ImageAutoCenter = true;
            this.cuiButton2.ImageExpand = new System.Drawing.Point(2, 2);
            this.cuiButton2.ImageOffset = new System.Drawing.Point(-3, 0);
            this.cuiButton2.ImageTint = System.Drawing.Color.White;
            this.cuiButton2.Location = new System.Drawing.Point(10, 408);
            this.cuiButton2.Name = "cuiButton2";
            this.cuiButton2.NormalBackground = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cuiButton2.NormalOutline = System.Drawing.Color.Empty;
            this.cuiButton2.OutlineThickness = 1.6F;
            this.cuiButton2.PressedBackground = System.Drawing.Color.Coral;
            this.cuiButton2.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton2.PressedOutline = System.Drawing.Color.Empty;
            this.cuiButton2.Rounding = new System.Windows.Forms.Padding(8);
            this.cuiButton2.Size = new System.Drawing.Size(186, 43);
            this.cuiButton2.TabIndex = 12;
            this.cuiButton2.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton2.Click += new System.EventHandler(this.cuiButton2_Click);
            // 
            // cuiButton1
            // 
            this.cuiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiButton1.CheckButton = false;
            this.cuiButton1.Checked = false;
            this.cuiButton1.CheckedBackground = System.Drawing.Color.Coral;
            this.cuiButton1.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton1.CheckedOutline = System.Drawing.Color.Coral;
            this.cuiButton1.Content = "";
            this.cuiButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton1.ForeColor = System.Drawing.Color.White;
            this.cuiButton1.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.cuiButton1.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton1.HoverOutline = System.Drawing.Color.Empty;
            this.cuiButton1.Image = global::CuoreUI.Properties.Resources.crossmark;
            this.cuiButton1.ImageAutoCenter = true;
            this.cuiButton1.ImageExpand = new System.Drawing.Point(2, 2);
            this.cuiButton1.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton1.ImageTint = System.Drawing.Color.White;
            this.cuiButton1.Location = new System.Drawing.Point(354, 1);
            this.cuiButton1.Name = "cuiButton1";
            this.cuiButton1.NormalBackground = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cuiButton1.NormalOutline = System.Drawing.Color.Empty;
            this.cuiButton1.OutlineThickness = 1.6F;
            this.cuiButton1.PressedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.cuiButton1.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton1.PressedOutline = System.Drawing.Color.Empty;
            this.cuiButton1.Rounding = new System.Windows.Forms.Padding(8);
            this.cuiButton1.Size = new System.Drawing.Size(43, 43);
            this.cuiButton1.TabIndex = 10;
            this.cuiButton1.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton1.Click += new System.EventHandler(this.cuiButton1_Click);
            // 
            // cuiLabel1
            // 
            this.cuiLabel1.Content = "Preview";
            this.cuiLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cuiLabel1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiLabel1.HorizontalAlignment = CuoreUI.Controls.cuiLabel.HorizontalAlignments.Center;
            this.cuiLabel1.Location = new System.Drawing.Point(202, 61);
            this.cuiLabel1.Name = "cuiLabel1";
            this.cuiLabel1.Size = new System.Drawing.Size(186, 19);
            this.cuiLabel1.TabIndex = 8;
            // 
            // cuiLabel2
            // 
            this.cuiLabel2.Content = "Color\\ Picker";
            this.cuiLabel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cuiLabel2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiLabel2.HorizontalAlignment = CuoreUI.Controls.cuiLabel.HorizontalAlignments.Center;
            this.cuiLabel2.Location = new System.Drawing.Point(10, 61);
            this.cuiLabel2.Name = "cuiLabel2";
            this.cuiLabel2.Size = new System.Drawing.Size(186, 19);
            this.cuiLabel2.TabIndex = 9;
            // 
            // cuiBorder1
            // 
            this.cuiBorder1.Location = new System.Drawing.Point(202, 86);
            this.cuiBorder1.Name = "cuiBorder1";
            this.cuiBorder1.OutlineThickness = 1F;
            this.cuiBorder1.PanelColor = System.Drawing.Color.Empty;
            this.cuiBorder1.PanelOutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cuiBorder1.Rounding = new System.Windows.Forms.Padding(8);
            this.cuiBorder1.Size = new System.Drawing.Size(186, 186);
            this.cuiBorder1.TabIndex = 7;
            // 
            // cuiTextBox5
            // 
            this.cuiTextBox5.Background = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox5.Border = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox5.BorderRadius = new System.Windows.Forms.Padding(6);
            this.cuiTextBox5.BorderSize = 1.6F;
            this.cuiTextBox5.Content = "";
            this.cuiTextBox5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cuiTextBox5.DesignStyle = cuiTextBox.Styles.Partial;
            this.cuiTextBox5.FocusedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox5.FocusedBorder = System.Drawing.Color.Coral;
            this.cuiTextBox5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiTextBox5.Location = new System.Drawing.Point(106, 343);
            this.cuiTextBox5.Name = "cuiTextBox5";
            this.cuiTextBox5.PartialThickness = 2F;
            this.cuiTextBox5.Placeholder = "Hex ARGB (f.e. \'#f0f0f0\' or \'red\')";
            this.cuiTextBox5.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cuiTextBox5.Size = new System.Drawing.Size(282, 34);
            this.cuiTextBox5.TabIndex = 5;
            this.cuiTextBox5.ContentChanged += new System.EventHandler(this.hexInput_ContentChanged);
            // 
            // cuiTextBox4
            // 
            this.cuiTextBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiTextBox4.Background = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox4.Border = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox4.BorderRadius = new System.Windows.Forms.Padding(6);
            this.cuiTextBox4.BorderSize = 1.6F;
            this.cuiTextBox4.Content = "";
            this.cuiTextBox4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cuiTextBox4.DesignStyle = cuiTextBox.Styles.Partial;
            this.cuiTextBox4.FocusedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox4.FocusedBorder = System.Drawing.Color.Coral;
            this.cuiTextBox4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiTextBox4.Location = new System.Drawing.Point(298, 303);
            this.cuiTextBox4.Name = "cuiTextBox4";
            this.cuiTextBox4.PartialThickness = 2F;
            this.cuiTextBox4.Placeholder = "Blue";
            this.cuiTextBox4.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cuiTextBox4.Size = new System.Drawing.Size(90, 34);
            this.cuiTextBox4.TabIndex = 4;
            this.cuiTextBox4.ContentChanged += new System.EventHandler(this.ContentChanged);
            // 
            // cuiTextBox3
            // 
            this.cuiTextBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiTextBox3.Background = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox3.Border = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox3.BorderRadius = new System.Windows.Forms.Padding(6);
            this.cuiTextBox3.BorderSize = 1.6F;
            this.cuiTextBox3.Content = "";
            this.cuiTextBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cuiTextBox3.DesignStyle = cuiTextBox.Styles.Partial;
            this.cuiTextBox3.FocusedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox3.FocusedBorder = System.Drawing.Color.Coral;
            this.cuiTextBox3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiTextBox3.Location = new System.Drawing.Point(202, 303);
            this.cuiTextBox3.Name = "cuiTextBox3";
            this.cuiTextBox3.PartialThickness = 2F;
            this.cuiTextBox3.Placeholder = "Green";
            this.cuiTextBox3.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cuiTextBox3.Size = new System.Drawing.Size(90, 34);
            this.cuiTextBox3.TabIndex = 3;
            this.cuiTextBox3.ContentChanged += new System.EventHandler(this.ContentChanged);
            // 
            // cuiTextBox2
            // 
            this.cuiTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiTextBox2.Background = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox2.Border = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox2.BorderRadius = new System.Windows.Forms.Padding(6);
            this.cuiTextBox2.BorderSize = 1.6F;
            this.cuiTextBox2.Content = "";
            this.cuiTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cuiTextBox2.DesignStyle = cuiTextBox.Styles.Partial;
            this.cuiTextBox2.FocusedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox2.FocusedBorder = System.Drawing.Color.Coral;
            this.cuiTextBox2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiTextBox2.Location = new System.Drawing.Point(106, 303);
            this.cuiTextBox2.Name = "cuiTextBox2";
            this.cuiTextBox2.PartialThickness = 2F;
            this.cuiTextBox2.Placeholder = "Red";
            this.cuiTextBox2.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cuiTextBox2.Size = new System.Drawing.Size(90, 34);
            this.cuiTextBox2.TabIndex = 2;
            this.cuiTextBox2.ContentChanged += new System.EventHandler(this.ContentChanged);
            // 
            // cuiTextBox1
            // 
            this.cuiTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cuiTextBox1.Background = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox1.Border = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox1.BorderRadius = new System.Windows.Forms.Padding(6);
            this.cuiTextBox1.BorderSize = 1.6F;
            this.cuiTextBox1.Content = "";
            this.cuiTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cuiTextBox1.DesignStyle = cuiTextBox.Styles.Partial;
            this.cuiTextBox1.FocusedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.cuiTextBox1.FocusedBorder = System.Drawing.Color.Coral;
            this.cuiTextBox1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cuiTextBox1.Location = new System.Drawing.Point(10, 303);
            this.cuiTextBox1.Name = "cuiTextBox1";
            this.cuiTextBox1.PartialThickness = 2F;
            this.cuiTextBox1.Placeholder = "Alpha";
            this.cuiTextBox1.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.cuiTextBox1.Size = new System.Drawing.Size(90, 34);
            this.cuiTextBox1.TabIndex = 1;
            this.cuiTextBox1.ContentChanged += new System.EventHandler(this.ContentChanged);
            // 
            // cuiFormRounder1
            // 
            this.cuiFormRounder1.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cuiFormRounder1.Rounding = 8;
            this.cuiFormRounder1.TargetForm = this;
            // 
            // cuiFormDrag1
            // 
            this.cuiFormDrag1.TargetForm = this;
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.ClientSize = new System.Drawing.Size(400, 463);
            this.Controls.Add(this.colorPickerWheel1);
            this.Controls.Add(this.cuiLabel3);
            this.Controls.Add(this.cuiButton3);
            this.Controls.Add(this.cuiButton2);
            this.Controls.Add(this.cuiButton1);
            this.Controls.Add(this.cuiLabel1);
            this.Controls.Add(this.cuiLabel2);
            this.Controls.Add(this.cuiBorder1);
            this.Controls.Add(this.cuiTextBox5);
            this.Controls.Add(this.cuiTextBox4);
            this.Controls.Add(this.cuiTextBox3);
            this.Controls.Add(this.cuiTextBox2);
            this.Controls.Add(this.cuiTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ColorPickerForm";
            this.Text = "Color Picker";
            this.ResumeLayout(false);

        }

        #endregion
        private cuiTextBox cuiTextBox5;
        private cuiTextBox cuiTextBox4;
        private cuiTextBox cuiTextBox3;
        private cuiTextBox cuiTextBox2;
        private cuiTextBox cuiTextBox1;
        private cuiFormDrag cuiFormDrag1;
        private cuiFormRounder cuiFormRounder1;
        private Controls.cuiLabel cuiLabel1;
        private Controls.cuiLabel cuiLabel2;
        private Controls.cuiButton cuiButton1;
        private Controls.cuiButton cuiButton3;
        private Controls.cuiButton cuiButton2;
        private Controls.cuiLabel cuiLabel3;
        private Controls.cuiBorder cuiBorder1;
        private Controls.ColorPickerWheel colorPickerWheel1;
    }
}