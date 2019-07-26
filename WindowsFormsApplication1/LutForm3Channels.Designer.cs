namespace AssemblySoftware
{
    partial class LutForm3Channels
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
            this.redChannelLbl = new System.Windows.Forms.Label();
            this.bkueChannelLbl = new System.Windows.Forms.Label();
            this.blueChannelLbl = new System.Windows.Forms.Label();
            this.redChannelPbx = new System.Windows.Forms.PictureBox();
            this.greenChannelPbx = new System.Windows.Forms.PictureBox();
            this.blueChannelPbx = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelPbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelPbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelPbx)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.redChannelLbl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bkueChannelLbl, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.blueChannelLbl, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.redChannelPbx, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.greenChannelPbx, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.blueChannelPbx, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(723, 309);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // redChannelLbl
            // 
            this.redChannelLbl.AutoSize = true;
            this.redChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redChannelLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redChannelLbl.Location = new System.Drawing.Point(3, 281);
            this.redChannelLbl.Name = "redChannelLbl";
            this.redChannelLbl.Size = new System.Drawing.Size(234, 28);
            this.redChannelLbl.TabIndex = 0;
            this.redChannelLbl.Text = "Red Channel";
            this.redChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bkueChannelLbl
            // 
            this.bkueChannelLbl.AutoSize = true;
            this.bkueChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bkueChannelLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bkueChannelLbl.Location = new System.Drawing.Point(243, 281);
            this.bkueChannelLbl.Name = "bkueChannelLbl";
            this.bkueChannelLbl.Size = new System.Drawing.Size(235, 28);
            this.bkueChannelLbl.TabIndex = 0;
            this.bkueChannelLbl.Text = "Green Channel";
            this.bkueChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blueChannelLbl
            // 
            this.blueChannelLbl.AutoSize = true;
            this.blueChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChannelLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blueChannelLbl.Location = new System.Drawing.Point(484, 281);
            this.blueChannelLbl.Name = "blueChannelLbl";
            this.blueChannelLbl.Size = new System.Drawing.Size(236, 28);
            this.blueChannelLbl.TabIndex = 0;
            this.blueChannelLbl.Text = "Blue Channel";
            this.blueChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // redChannelPbx
            // 
            this.redChannelPbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redChannelPbx.Location = new System.Drawing.Point(3, 3);
            this.redChannelPbx.Name = "redChannelPbx";
            this.redChannelPbx.Size = new System.Drawing.Size(234, 275);
            this.redChannelPbx.TabIndex = 1;
            this.redChannelPbx.TabStop = false;
            // 
            // greenChannelPbx
            // 
            this.greenChannelPbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenChannelPbx.Location = new System.Drawing.Point(243, 3);
            this.greenChannelPbx.Name = "greenChannelPbx";
            this.greenChannelPbx.Size = new System.Drawing.Size(235, 275);
            this.greenChannelPbx.TabIndex = 1;
            this.greenChannelPbx.TabStop = false;
            // 
            // blueChannelPbx
            // 
            this.blueChannelPbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChannelPbx.Location = new System.Drawing.Point(484, 3);
            this.blueChannelPbx.Name = "blueChannelPbx";
            this.blueChannelPbx.Size = new System.Drawing.Size(236, 275);
            this.blueChannelPbx.TabIndex = 1;
            this.blueChannelPbx.TabStop = false;
            // 
            // LutForm3Channels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(723, 309);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LutForm3Channels";
            this.Text = "LutForm3Channels";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LutForm3Channels_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelPbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelPbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelPbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label redChannelLbl;
        private System.Windows.Forms.Label bkueChannelLbl;
        private System.Windows.Forms.Label blueChannelLbl;
        private System.Windows.Forms.PictureBox redChannelPbx;
        private System.Windows.Forms.PictureBox greenChannelPbx;
        private System.Windows.Forms.PictureBox blueChannelPbx;
    }
}