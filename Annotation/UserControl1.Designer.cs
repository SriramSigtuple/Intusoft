namespace Annotation
{
    partial class UserControl1
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
            this.class11 = new Annotation.Class1();
            this.SuspendLayout();
            // 
            // class11
            // 
            this.class11.AutoScroll = true;
            this.class11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.class11.Location = new System.Drawing.Point(0, 0);
            this.class11.Name = "class11";
            this.class11.Size = new System.Drawing.Size(150, 150);
            this.class11.TabIndex = 0;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.class11);
            this.Name = "UserControl1";
            this.ResumeLayout(false);

        }

        #endregion

        public Class1 class11;
    }
}
