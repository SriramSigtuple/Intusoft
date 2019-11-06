namespace pDesigner {
    partial class pDesigner {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
protected override void Dispose ( bool disposing ) {
    if ( disposing && ( components != null ) ) {
        components.Dispose();
    }
    base.Dispose ( disposing );
}

#region Component Designer generated code

/// <summary>
/// Required method for Designer support - do not modify
/// the contents of this method with the code editor.
/// </summary>
private void InitializeComponent() {
            this.splitterpDesigner = new System.Windows.Forms.SplitContainer();
            this.DesignSurfaceParent_pnl = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitterpDesigner)).BeginInit();
            this.splitterpDesigner.Panel1.SuspendLayout();
            this.splitterpDesigner.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitterpDesigner
            // 
            this.splitterpDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterpDesigner.Location = new System.Drawing.Point(0, 0);
            this.splitterpDesigner.Margin = new System.Windows.Forms.Padding(2);
            this.splitterpDesigner.Name = "splitterpDesigner";
            // 
            // splitterpDesigner.Panel1
            // 
            this.splitterpDesigner.Panel1.AutoScroll = true;
            this.splitterpDesigner.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitterpDesigner.Panel1.Controls.Add(this.DesignSurfaceParent_pnl);
            this.splitterpDesigner.Panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.splitterpDesigner_Panel1_Scroll);
            // 
            // splitterpDesigner.Panel2
            // 
            this.splitterpDesigner.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitterpDesigner.Size = new System.Drawing.Size(678, 317);
            this.splitterpDesigner.SplitterDistance = 560;
            this.splitterpDesigner.SplitterWidth = 3;
            this.splitterpDesigner.TabIndex = 0;
            // 
            // DesignSurfaceParent_pnl
            // 
            this.DesignSurfaceParent_pnl.AutoScroll = true;
            this.DesignSurfaceParent_pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DesignSurfaceParent_pnl.Location = new System.Drawing.Point(0, 0);
            this.DesignSurfaceParent_pnl.Name = "DesignSurfaceParent_pnl";
            this.DesignSurfaceParent_pnl.Size = new System.Drawing.Size(560, 317);
            this.DesignSurfaceParent_pnl.TabIndex = 4;
            this.DesignSurfaceParent_pnl.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DesignSurfaceParent_pnl_Scroll_1);
            this.DesignSurfaceParent_pnl.Paint += new System.Windows.Forms.PaintEventHandler(this.DesignSurfaceParent_pnl_Paint);
            // 
            // pDesigner
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitterpDesigner);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "pDesigner";
            this.Size = new System.Drawing.Size(678, 317);
            this.splitterpDesigner.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterpDesigner)).EndInit();
            this.splitterpDesigner.ResumeLayout(false);
            this.ResumeLayout(false);

}

#endregion

private System.Windows.Forms.SplitContainer splitterpDesigner;
private System.Windows.Forms.Panel DesignSurfaceParent_pnl;
    }
}
