namespace INTUSOFT.Desktop
{
    partial class ImageCaptureSplashScreen
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
            this.ImageCapture_pb = new System.Windows.Forms.ProgressBar();
            this.progress_lbl = new System.Windows.Forms.Label();
            this.ImageCapture_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ImageCapture_pb
            // 
            this.ImageCapture_pb.Location = new System.Drawing.Point(12, 111);
            this.ImageCapture_pb.Name = "ImageCapture_pb";
            this.ImageCapture_pb.Size = new System.Drawing.Size(372, 23);
            this.ImageCapture_pb.TabIndex = 0;
            // 
            // progress_lbl
            // 
            this.progress_lbl.AutoSize = true;
            this.progress_lbl.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.progress_lbl.Location = new System.Drawing.Point(390, 115);
            this.progress_lbl.Name = "progress_lbl";
            this.progress_lbl.Size = new System.Drawing.Size(13, 13);
            this.progress_lbl.TabIndex = 1;
            this.progress_lbl.Text = "0";
            // 
            // ImageCapture_lbl
            // 
            this.ImageCapture_lbl.AutoSize = true;
            this.ImageCapture_lbl.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ImageCapture_lbl.Location = new System.Drawing.Point(12, 86);
            this.ImageCapture_lbl.Name = "ImageCapture_lbl";
            this.ImageCapture_lbl.Size = new System.Drawing.Size(90, 13);
            this.ImageCapture_lbl.TabIndex = 1;
            this.ImageCapture_lbl.Text = "Image Capturing :";
            // 
            // ImageCaptureSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(431, 262);
            this.Controls.Add(this.ImageCapture_lbl);
            this.Controls.Add(this.progress_lbl);
            this.Controls.Add(this.ImageCapture_pb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageCaptureSplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ImageCaptureSplashScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar ImageCapture_pb;
        private System.Windows.Forms.Label progress_lbl;
        private System.Windows.Forms.Label ImageCapture_lbl;
    }
}