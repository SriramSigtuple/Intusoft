namespace INTUSOFT.Desktop.Forms
{
    partial class Imaging_UC
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FrameRate_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ExposureStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.gainStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.totalFrameCount_Lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ImagingViewControls_p = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.maskOverlay_Pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.overlay_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.motorSensor_tbpnl = new System.Windows.Forms.TableLayoutPanel();
            this.negativeDiaptor_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.pos_pbx = new System.Windows.Forms.PictureBox();
            this.neg_pbx = new System.Windows.Forms.PictureBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.positiveDiaptor_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.negativeArrow_pbx = new System.Windows.Forms.PictureBox();
            this.positiveArrow_pbx = new System.Windows.Forms.PictureBox();
            this.resuming_lbl = new System.Windows.Forms.Label();
            this.NoImageSelected_lbl = new System.Windows.Forms.Label();
            this.display_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maskOverlay_Pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlay_pbx)).BeginInit();
            this.motorSensor_tbpnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.negativeDiaptor_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.neg_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positiveDiaptor_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.negativeArrow_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positiveArrow_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.display_pbx)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.29185F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.70815F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ImagingViewControls_p, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1117, 750);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FrameRate_lbl,
            this.ExposureStatus_lbl,
            this.gainStatus_lbl,
            this.totalFrameCount_Lbl});
            this.toolStrip1.Location = new System.Drawing.Point(248, 730);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(869, 20);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FrameRate_lbl
            // 
            this.FrameRate_lbl.ForeColor = System.Drawing.Color.White;
            this.FrameRate_lbl.Name = "FrameRate_lbl";
            this.FrameRate_lbl.Size = new System.Drawing.Size(40, 15);
            this.FrameRate_lbl.Text = "Frame";
            // 
            // ExposureStatus_lbl
            // 
            this.ExposureStatus_lbl.ForeColor = System.Drawing.Color.White;
            this.ExposureStatus_lbl.Name = "ExposureStatus_lbl";
            this.ExposureStatus_lbl.Size = new System.Drawing.Size(54, 15);
            this.ExposureStatus_lbl.Text = "Exposure";
            // 
            // gainStatus_lbl
            // 
            this.gainStatus_lbl.ForeColor = System.Drawing.Color.White;
            this.gainStatus_lbl.Name = "gainStatus_lbl";
            this.gainStatus_lbl.Size = new System.Drawing.Size(31, 15);
            this.gainStatus_lbl.Text = "Gain";
            // 
            // totalFrameCount_Lbl
            // 
            this.totalFrameCount_Lbl.ForeColor = System.Drawing.Color.White;
            this.totalFrameCount_Lbl.Name = "totalFrameCount_Lbl";
            this.totalFrameCount_Lbl.Size = new System.Drawing.Size(13, 15);
            this.totalFrameCount_Lbl.Text = "0";
            // 
            // ImagingViewControls_p
            // 
            this.ImagingViewControls_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImagingViewControls_p.Location = new System.Drawing.Point(3, 3);
            this.ImagingViewControls_p.Name = "ImagingViewControls_p";
            this.ImagingViewControls_p.Size = new System.Drawing.Size(242, 724);
            this.ImagingViewControls_p.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.maskOverlay_Pbx);
            this.panel1.Controls.Add(this.overlay_pbx);
            this.panel1.Controls.Add(this.motorSensor_tbpnl);
            this.panel1.Controls.Add(this.resuming_lbl);
            this.panel1.Controls.Add(this.NoImageSelected_lbl);
            this.panel1.Controls.Add(this.display_pbx);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(251, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(863, 724);
            this.panel1.TabIndex = 3;
            // 
            // maskOverlay_Pbx
            // 
            this.maskOverlay_Pbx.Index = 0;
            this.maskOverlay_Pbx.Location = new System.Drawing.Point(181, 75);
            this.maskOverlay_Pbx.Name = "maskOverlay_Pbx";
            this.maskOverlay_Pbx.Size = new System.Drawing.Size(100, 50);
            this.maskOverlay_Pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.maskOverlay_Pbx.TabIndex = 14;
            this.maskOverlay_Pbx.TabStop = false;
            // 
            // overlay_pbx
            // 
            this.overlay_pbx.Index = 0;
            this.overlay_pbx.Location = new System.Drawing.Point(502, 76);
            this.overlay_pbx.Name = "overlay_pbx";
            this.overlay_pbx.Size = new System.Drawing.Size(100, 50);
            this.overlay_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.overlay_pbx.TabIndex = 13;
            this.overlay_pbx.TabStop = false;
            // 
            // motorSensor_tbpnl
            // 
            this.motorSensor_tbpnl.BackColor = System.Drawing.Color.Transparent;
            this.motorSensor_tbpnl.ColumnCount = 7;
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.720003F));
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.720003F));
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.00002F));
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.119944F));
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.00002F));
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.720003F));
            this.motorSensor_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.720003F));
            this.motorSensor_tbpnl.Controls.Add(this.negativeDiaptor_pbx, 0, 0);
            this.motorSensor_tbpnl.Controls.Add(this.pos_pbx, 4, 0);
            this.motorSensor_tbpnl.Controls.Add(this.neg_pbx, 2, 0);
            this.motorSensor_tbpnl.Controls.Add(this.panel10, 3, 0);
            this.motorSensor_tbpnl.Controls.Add(this.positiveDiaptor_pbx, 6, 0);
            this.motorSensor_tbpnl.Controls.Add(this.negativeArrow_pbx, 1, 0);
            this.motorSensor_tbpnl.Controls.Add(this.positiveArrow_pbx, 5, 0);
            this.motorSensor_tbpnl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.motorSensor_tbpnl.Location = new System.Drawing.Point(0, 693);
            this.motorSensor_tbpnl.Name = "motorSensor_tbpnl";
            this.motorSensor_tbpnl.RowCount = 1;
            this.motorSensor_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.motorSensor_tbpnl.Size = new System.Drawing.Size(863, 31);
            this.motorSensor_tbpnl.TabIndex = 12;
            // 
            // negativeDiaptor_pbx
            // 
            this.negativeDiaptor_pbx.BackColor = System.Drawing.Color.Transparent;
            this.negativeDiaptor_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.negativeDiaptor_pbx.Index = 0;
            this.negativeDiaptor_pbx.Location = new System.Drawing.Point(3, 3);
            this.negativeDiaptor_pbx.Name = "negativeDiaptor_pbx";
            this.negativeDiaptor_pbx.Size = new System.Drawing.Size(34, 25);
            this.negativeDiaptor_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.negativeDiaptor_pbx.TabIndex = 3;
            this.negativeDiaptor_pbx.TabStop = false;
            // 
            // pos_pbx
            // 
            this.pos_pbx.BackColor = System.Drawing.Color.Transparent;
            this.pos_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pos_pbx.Location = new System.Drawing.Point(437, 3);
            this.pos_pbx.Name = "pos_pbx";
            this.pos_pbx.Size = new System.Drawing.Size(339, 25);
            this.pos_pbx.TabIndex = 0;
            this.pos_pbx.TabStop = false;
            // 
            // neg_pbx
            // 
            this.neg_pbx.BackColor = System.Drawing.Color.Transparent;
            this.neg_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neg_pbx.Location = new System.Drawing.Point(83, 3);
            this.neg_pbx.Name = "neg_pbx";
            this.neg_pbx.Size = new System.Drawing.Size(339, 25);
            this.neg_pbx.TabIndex = 0;
            this.neg_pbx.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Red;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(428, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(3, 25);
            this.panel10.TabIndex = 4;
            // 
            // positiveDiaptor_pbx
            // 
            this.positiveDiaptor_pbx.BackColor = System.Drawing.Color.Transparent;
            this.positiveDiaptor_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positiveDiaptor_pbx.Index = 0;
            this.positiveDiaptor_pbx.Location = new System.Drawing.Point(822, 3);
            this.positiveDiaptor_pbx.Name = "positiveDiaptor_pbx";
            this.positiveDiaptor_pbx.Size = new System.Drawing.Size(38, 25);
            this.positiveDiaptor_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.positiveDiaptor_pbx.TabIndex = 2;
            this.positiveDiaptor_pbx.TabStop = false;
            // 
            // negativeArrow_pbx
            // 
            this.negativeArrow_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.negativeArrow_pbx.Location = new System.Drawing.Point(43, 3);
            this.negativeArrow_pbx.Name = "negativeArrow_pbx";
            this.negativeArrow_pbx.Size = new System.Drawing.Size(34, 25);
            this.negativeArrow_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.negativeArrow_pbx.TabIndex = 5;
            this.negativeArrow_pbx.TabStop = false;
            // 
            // positiveArrow_pbx
            // 
            this.positiveArrow_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positiveArrow_pbx.Location = new System.Drawing.Point(782, 3);
            this.positiveArrow_pbx.Name = "positiveArrow_pbx";
            this.positiveArrow_pbx.Size = new System.Drawing.Size(34, 25);
            this.positiveArrow_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.positiveArrow_pbx.TabIndex = 6;
            this.positiveArrow_pbx.TabStop = false;
            // 
            // resuming_lbl
            // 
            this.resuming_lbl.AutoSize = true;
            this.resuming_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resuming_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resuming_lbl.Location = new System.Drawing.Point(328, 530);
            this.resuming_lbl.Name = "resuming_lbl";
            this.resuming_lbl.Size = new System.Drawing.Size(2, 26);
            this.resuming_lbl.TabIndex = 3;
            this.resuming_lbl.Visible = false;
            // 
            // NoImageSelected_lbl
            // 
            this.NoImageSelected_lbl.BackColor = System.Drawing.Color.Transparent;
            this.NoImageSelected_lbl.ForeColor = System.Drawing.Color.Red;
            this.NoImageSelected_lbl.Location = new System.Drawing.Point(234, 183);
            this.NoImageSelected_lbl.Name = "NoImageSelected_lbl";
            this.NoImageSelected_lbl.Size = new System.Drawing.Size(388, 252);
            this.NoImageSelected_lbl.TabIndex = 5;
            this.NoImageSelected_lbl.Text = "label1";
            this.NoImageSelected_lbl.Visible = false;
            // 
            // display_pbx
            // 
            this.display_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display_pbx.Index = 0;
            this.display_pbx.Location = new System.Drawing.Point(0, 0);
            this.display_pbx.Name = "display_pbx";
            this.display_pbx.Size = new System.Drawing.Size(863, 724);
            this.display_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.display_pbx.TabIndex = 2;
            this.display_pbx.TabStop = false;
            this.display_pbx.Paint += new System.Windows.Forms.PaintEventHandler(this.display_pbx_Paint);
            this.display_pbx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.display_pbx_MouseDown);
            this.display_pbx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.display_pbx_MouseMove);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(1117, 750);
            // 
            // Imaging_UC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.Khaki;
            this.Name = "Imaging_UC";
            this.Size = new System.Drawing.Size(1117, 750);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Imaging_UC_Paint);
            this.Resize += new System.EventHandler(this.Imaging_UC_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maskOverlay_Pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlay_pbx)).EndInit();
            this.motorSensor_tbpnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.negativeDiaptor_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.neg_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positiveDiaptor_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.negativeArrow_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positiveArrow_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.display_pbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label resuming_lbl;
        public INTUSOFT.Custom.Controls.PictureBoxExtended display_pbx;
        private System.Windows.Forms.Label NoImageSelected_lbl;
        //private System.Windows.Forms.PictureBox overlayRetinal_pbx;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel ImagingViewControls_p;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.TableLayoutPanel motorSensor_tbpnl;
        private Custom.Controls.PictureBoxExtended negativeDiaptor_pbx;
        private System.Windows.Forms.PictureBox pos_pbx;
        private System.Windows.Forms.PictureBox neg_pbx;
        private System.Windows.Forms.Panel panel10;
        private Custom.Controls.PictureBoxExtended positiveDiaptor_pbx;
        private System.Windows.Forms.PictureBox negativeArrow_pbx;
        private System.Windows.Forms.PictureBox positiveArrow_pbx;
        private Custom.Controls.PictureBoxExtended overlay_pbx;
        private Custom.Controls.PictureBoxExtended maskOverlay_Pbx;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripStatusLabel FrameRate_lbl;
        private System.Windows.Forms.ToolStripStatusLabel ExposureStatus_lbl;
        private System.Windows.Forms.ToolStripStatusLabel gainStatus_lbl;
        private System.Windows.Forms.ToolStripStatusLabel totalFrameCount_Lbl;

    }
}
