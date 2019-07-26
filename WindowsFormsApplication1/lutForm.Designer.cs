namespace AssemblySoftware
{
    partial class lutForm
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
            this.pictureBoxExtended1 = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExtended1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxExtended1
            // 
            this.pictureBoxExtended1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBoxExtended1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxExtended1.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxExtended1.Name = "pictureBoxExtended1";
            this.pictureBoxExtended1.Size = new System.Drawing.Size(284, 262);
            this.pictureBoxExtended1.TabIndex = 0;
            this.pictureBoxExtended1.TabStop = false;
            // 
            // lutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.pictureBoxExtended1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "lutForm";
            this.Text = "lutForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExtended1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private INTUSOFT.Custom.Controls.PictureBoxExtended pictureBoxExtended1;
    }
}