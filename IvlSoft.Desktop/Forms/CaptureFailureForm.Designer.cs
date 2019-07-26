namespace INTUSOFT.Desktop.Forms
{
    partial class CaptureFailureForm
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
            this.errorMsg_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // errorMsg_lbl
            // 
            this.errorMsg_lbl.AutoSize = true;
            this.errorMsg_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorMsg_lbl.ForeColor = System.Drawing.Color.Red;
            this.errorMsg_lbl.Location = new System.Drawing.Point(60, 64);
            this.errorMsg_lbl.Name = "errorMsg_lbl";
            this.errorMsg_lbl.Size = new System.Drawing.Size(52, 17);
            this.errorMsg_lbl.TabIndex = 0;
            this.errorMsg_lbl.Text = "label1";
            // 
            // CaptureFailureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 172);
            this.ControlBox = false;
            this.Controls.Add(this.errorMsg_lbl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptureFailureForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CaptureFailureForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label errorMsg_lbl;
    }
}