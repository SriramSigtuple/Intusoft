namespace INTUSOFT.Desktop
{
    partial class ThemesForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ok_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.newTheme_btn = new System.Windows.Forms.Button();
            this.themeName_lbl = new System.Windows.Forms.Label();
            this.color1_lbl = new System.Windows.Forms.Label();
            this.fontColor_lbl = new System.Windows.Forms.Label();
            this.angle_lbl = new System.Windows.Forms.Label();
            this.color2_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(41, 47);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ok_btn
            // 
            this.ok_btn.Location = new System.Drawing.Point(282, 253);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(75, 23);
            this.ok_btn.TabIndex = 3;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.Location = new System.Drawing.Point(363, 253);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.cancel_btn.TabIndex = 3;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Theme Name :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Color 1 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Font Color :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Color 2 :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Gradient Angle :";
            // 
            // newTheme_btn
            // 
            this.newTheme_btn.Location = new System.Drawing.Point(308, 45);
            this.newTheme_btn.Name = "newTheme_btn";
            this.newTheme_btn.Size = new System.Drawing.Size(130, 23);
            this.newTheme_btn.TabIndex = 12;
            this.newTheme_btn.Text = "Add Customize Theme";
            this.newTheme_btn.UseVisualStyleBackColor = true;
            this.newTheme_btn.Click += new System.EventHandler(this.newTheme_btn_Click);
            // 
            // themeName_lbl
            // 
            this.themeName_lbl.AutoSize = true;
            this.themeName_lbl.Location = new System.Drawing.Point(93, 136);
            this.themeName_lbl.Name = "themeName_lbl";
            this.themeName_lbl.Size = new System.Drawing.Size(35, 13);
            this.themeName_lbl.TabIndex = 13;
            this.themeName_lbl.Text = "label6";
            // 
            // color1_lbl
            // 
            this.color1_lbl.AutoSize = true;
            this.color1_lbl.Location = new System.Drawing.Point(93, 175);
            this.color1_lbl.Name = "color1_lbl";
            this.color1_lbl.Size = new System.Drawing.Size(35, 13);
            this.color1_lbl.TabIndex = 14;
            this.color1_lbl.Text = "label7";
            // 
            // fontColor_lbl
            // 
            this.fontColor_lbl.AutoSize = true;
            this.fontColor_lbl.Location = new System.Drawing.Point(93, 216);
            this.fontColor_lbl.Name = "fontColor_lbl";
            this.fontColor_lbl.Size = new System.Drawing.Size(35, 13);
            this.fontColor_lbl.TabIndex = 15;
            this.fontColor_lbl.Text = "label8";
            // 
            // angle_lbl
            // 
            this.angle_lbl.AutoSize = true;
            this.angle_lbl.Location = new System.Drawing.Point(313, 136);
            this.angle_lbl.Name = "angle_lbl";
            this.angle_lbl.Size = new System.Drawing.Size(35, 13);
            this.angle_lbl.TabIndex = 16;
            this.angle_lbl.Text = "label9";
            // 
            // color2_lbl
            // 
            this.color2_lbl.AutoSize = true;
            this.color2_lbl.Location = new System.Drawing.Point(313, 175);
            this.color2_lbl.Name = "color2_lbl";
            this.color2_lbl.Size = new System.Drawing.Size(41, 13);
            this.color2_lbl.TabIndex = 17;
            this.color2_lbl.Text = "label10";
            // 
            // ThemesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(552, 299);
            this.Controls.Add(this.color2_lbl);
            this.Controls.Add(this.angle_lbl);
            this.Controls.Add(this.fontColor_lbl);
            this.Controls.Add(this.color1_lbl);
            this.Controls.Add(this.themeName_lbl);
            this.Controls.Add(this.newTheme_btn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.ok_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThemesForm";
            this.Text = "Themes";
            this.Load += new System.EventHandler(this.ThemesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button newTheme_btn;
        private System.Windows.Forms.Label themeName_lbl;
        private System.Windows.Forms.Label color1_lbl;
        private System.Windows.Forms.Label fontColor_lbl;
        private System.Windows.Forms.Label angle_lbl;
        private System.Windows.Forms.Label color2_lbl;
    }
}