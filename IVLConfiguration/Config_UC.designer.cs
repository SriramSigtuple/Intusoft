namespace INTUSOFT.Configuration
{
    partial class Config_UC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.save_btn = new System.Windows.Forms.Button();
            this.password_pnl = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.passwd_tbx = new System.Windows.Forms.TextBox();
            this.password_pnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 341);
            this.panel1.TabIndex = 0;
            this.panel1.Visible = false;
            // 
            // save_btn
            // 
            this.save_btn.Location = new System.Drawing.Point(309, 343);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(75, 23);
            this.save_btn.TabIndex = 1;
            this.save_btn.Text = "Save";
            this.save_btn.UseVisualStyleBackColor = true;
            this.save_btn.Visible = false;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // password_pnl
            // 
            this.password_pnl.Controls.Add(this.label2);
            this.password_pnl.Controls.Add(this.passwd_tbx);
            this.password_pnl.Location = new System.Drawing.Point(33, 124);
            this.password_pnl.Name = "password_pnl";
            this.password_pnl.Size = new System.Drawing.Size(309, 100);
            this.password_pnl.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password :";
            // 
            // passwd_tbx
            // 
            this.passwd_tbx.Location = new System.Drawing.Point(93, 40);
            this.passwd_tbx.Name = "passwd_tbx";
            this.passwd_tbx.PasswordChar = '*';
            this.passwd_tbx.Size = new System.Drawing.Size(179, 20);
            this.passwd_tbx.TabIndex = 2;
            this.passwd_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwd_tbx_KeyPress);
            // 
            // Config_UC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.save_btn);
            this.Controls.Add(this.password_pnl);
            this.Controls.Add(this.panel1);
            this.Name = "Config_UC";
            this.Size = new System.Drawing.Size(387, 371);
            this.password_pnl.ResumeLayout(false);
            this.password_pnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.Panel password_pnl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passwd_tbx;
    }
}
