namespace DBPorting
{
    partial class TimeProfiling
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
            this.model_cbx = new System.Windows.Forms.ComboBox();
            this.model_lbl = new System.Windows.Forms.Label();
            this.method_lbl = new System.Windows.Forms.Label();
            this.method_cbx = new System.Windows.Forms.ComboBox();
            this.timechk_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // model_cbx
            // 
            this.model_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.model_cbx.FormattingEnabled = true;
            this.model_cbx.Location = new System.Drawing.Point(32, 59);
            this.model_cbx.Name = "model_cbx";
            this.model_cbx.Size = new System.Drawing.Size(121, 21);
            this.model_cbx.TabIndex = 0;
            // 
            // model_lbl
            // 
            this.model_lbl.AutoSize = true;
            this.model_lbl.Location = new System.Drawing.Point(34, 37);
            this.model_lbl.Name = "model_lbl";
            this.model_lbl.Size = new System.Drawing.Size(41, 13);
            this.model_lbl.TabIndex = 1;
            this.model_lbl.Text = "Models";
            // 
            // method_lbl
            // 
            this.method_lbl.AutoSize = true;
            this.method_lbl.Location = new System.Drawing.Point(206, 37);
            this.method_lbl.Name = "method_lbl";
            this.method_lbl.Size = new System.Drawing.Size(48, 13);
            this.method_lbl.TabIndex = 2;
            this.method_lbl.Text = "Methods";
            // 
            // method_cbx
            // 
            this.method_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.method_cbx.FormattingEnabled = true;
            this.method_cbx.Location = new System.Drawing.Point(202, 59);
            this.method_cbx.Name = "method_cbx";
            this.method_cbx.Size = new System.Drawing.Size(120, 21);
            this.method_cbx.TabIndex = 3;
            // 
            // timechk_btn
            // 
            this.timechk_btn.Location = new System.Drawing.Point(123, 97);
            this.timechk_btn.Name = "timechk_btn";
            this.timechk_btn.Size = new System.Drawing.Size(112, 23);
            this.timechk_btn.TabIndex = 4;
            this.timechk_btn.Text = "Evaluate Time";
            this.timechk_btn.UseVisualStyleBackColor = true;
            this.timechk_btn.Click += new System.EventHandler(this.timechk_btn_Click);
            // 
            // TimeProfiling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 132);
            this.Controls.Add(this.timechk_btn);
            this.Controls.Add(this.method_cbx);
            this.Controls.Add(this.method_lbl);
            this.Controls.Add(this.model_lbl);
            this.Controls.Add(this.model_cbx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeProfiling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TimeProfiling";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox model_cbx;
        private System.Windows.Forms.Label model_lbl;
        private System.Windows.Forms.Label method_lbl;
        private System.Windows.Forms.ComboBox method_cbx;
        private System.Windows.Forms.Button timechk_btn;
    }
}