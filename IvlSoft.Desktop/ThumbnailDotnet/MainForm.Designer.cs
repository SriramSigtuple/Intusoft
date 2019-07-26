namespace marlie.TumbnailDotnet
{
    partial class MainForm
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
            this.flowLayoutPanelMain = new marlie.TumbnailDotnet.ThumbnailFlowLayoutPanel();
            this.buttonBrowseFolder = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.trackBarSize = new System.Windows.Forms.TrackBar();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanelMain
            // 
            this.flowLayoutPanelMain.AutoScroll = true;
            this.flowLayoutPanelMain.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.flowLayoutPanelMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelMain.CausesValidation = false;
            this.flowLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelMain.Name = "flowLayoutPanelMain";
            this.flowLayoutPanelMain.Size = new System.Drawing.Size(611, 451);
            this.flowLayoutPanelMain.TabIndex = 0;
            // 
            // buttonBrowseFolder
            // 
            this.buttonBrowseFolder.Location = new System.Drawing.Point(12, 12);
            this.buttonBrowseFolder.Name = "buttonBrowseFolder";
            this.buttonBrowseFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseFolder.TabIndex = 1;
            this.buttonBrowseFolder.Text = "Browse...";
            this.buttonBrowseFolder.UseVisualStyleBackColor = true;
            this.buttonBrowseFolder.Click += new System.EventHandler(this.buttonBrowseFolder_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(93, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.flowLayoutPanelMain);
            this.panelMain.Location = new System.Drawing.Point(12, 41);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(611, 451);
            this.panelMain.TabIndex = 4;
            // 
            // trackBarSize
            // 
            this.trackBarSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarSize.AutoSize = false;
            this.trackBarSize.LargeChange = 1;
            this.trackBarSize.Location = new System.Drawing.Point(499, 12);
            this.trackBarSize.Maximum = 2;
            this.trackBarSize.Name = "trackBarSize";
            this.trackBarSize.Size = new System.Drawing.Size(124, 23);
            this.trackBarSize.TabIndex = 5;
            this.trackBarSize.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSize.Value = 1;
            this.trackBarSize.ValueChanged += new System.EventHandler(this.trackBarSize_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 504);
            this.Controls.Add(this.trackBarSize);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBrowseFolder);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Thumbnail Viewer";
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ThumbnailFlowLayoutPanel flowLayoutPanelMain;
        private System.Windows.Forms.Button buttonBrowseFolder;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TrackBar trackBarSize;
    }
}

