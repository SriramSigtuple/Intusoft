namespace INTUSOFT.Desktop.Forms
{
    partial class SplitScreen
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.image1_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.image2_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.thumbnailUI1 = new INTUSOFT.ThumbnailModule.ThumbnailUI();
            this.thumbnailUI2 = new INTUSOFT.ThumbnailModule.ThumbnailUI();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image1_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.image2_pbx)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.687615F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.31239F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(939, 541);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.image2_pbx, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.thumbnailUI1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.image1_pbx, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.thumbnailUI2, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 49);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.59714F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.40286F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(933, 489);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(939, 46);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // image1_pbx
            // 
            this.image1_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.image1_pbx.Index = 0;
            this.image1_pbx.Location = new System.Drawing.Point(3, 3);
            this.image1_pbx.Name = "image1_pbx";
            this.image1_pbx.Size = new System.Drawing.Size(460, 349);
            this.image1_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.image1_pbx.TabIndex = 0;
            this.image1_pbx.TabStop = false;
            // 
            // image2_pbx
            // 
            this.image2_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.image2_pbx.Index = 0;
            this.image2_pbx.Location = new System.Drawing.Point(469, 3);
            this.image2_pbx.Name = "image2_pbx";
            this.image2_pbx.Size = new System.Drawing.Size(461, 349);
            this.image2_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.image2_pbx.TabIndex = 0;
            this.image2_pbx.TabStop = false;
            // 
            // thumbnailUI1
            // 
            this.thumbnailUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailUI1.Location = new System.Drawing.Point(3, 358);
            this.thumbnailUI1.Name = "thumbnailUI1";
            this.thumbnailUI1.Size = new System.Drawing.Size(460, 128);
            this.thumbnailUI1.TabIndex = 1;
            // 
            // thumbnailUI2
            // 
            this.thumbnailUI2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailUI2.Location = new System.Drawing.Point(469, 358);
            this.thumbnailUI2.Name = "thumbnailUI2";
            this.thumbnailUI2.Size = new System.Drawing.Size(461, 128);
            this.thumbnailUI2.TabIndex = 1;
            // 
            // SplitScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 541);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplitScreen";
            this.Text = "SplitScreen";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.image1_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.image2_pbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Custom.Controls.PictureBoxExtended image1_pbx;
        private Custom.Controls.PictureBoxExtended image2_pbx;
        private ThumbnailModule.ThumbnailUI thumbnailUI1;
        private ThumbnailModule.ThumbnailUI thumbnailUI2;
    }
}