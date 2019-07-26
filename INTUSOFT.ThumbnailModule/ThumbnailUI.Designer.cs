namespace INTUSOFT.ThumbnailModule
{
    partial class ThumbnailUI
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
            this.thumbnail_FLP = new INTUSOFT.ThumbnailModule.ThumbnailFlowLayoutPanel();
            this.SuspendLayout();
            // 
            // thumbnail_FLP
            // 
            this.thumbnail_FLP.AutoScroll = true;
            this.thumbnail_FLP.AutoSize = true;
            this.thumbnail_FLP.Dock = System.Windows.Forms.DockStyle.Top;
            this.thumbnail_FLP.Location = new System.Drawing.Point(0, 0);
            this.thumbnail_FLP.Name = "thumbnail_FLP";
            this.thumbnail_FLP.Size = new System.Drawing.Size(150, 0);
            this.thumbnail_FLP.TabIndex = 0;
            // 
            // ThumbnailUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.thumbnail_FLP);
            this.Name = "ThumbnailUI";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ThumbnailUI_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ThumbnailUI_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ThumbnailUI_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ThumbnailFlowLayoutPanel thumbnail_FLP;
    }
}
