namespace DBPorting
{
    partial class Pagination
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pagination));
            this.patient_dgv = new System.Windows.Forms.DataGridView();
            this.PagingPanel = new System.Windows.Forms.Panel();
            this.toolStripPaging = new System.Windows.Forms.ToolStrip();
            this.btnFirst = new System.Windows.Forms.ToolStripButton();
            this.btnBackward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.btnForward = new System.Windows.Forms.ToolStripButton();
            this.btnLast = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.patient_dgv)).BeginInit();
            this.PagingPanel.SuspendLayout();
            this.toolStripPaging.SuspendLayout();
            this.SuspendLayout();
            // 
            // patient_dgv
            // 
            this.patient_dgv.AllowUserToAddRows = false;
            this.patient_dgv.AllowUserToDeleteRows = false;
            this.patient_dgv.AllowUserToResizeColumns = false;
            this.patient_dgv.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.patient_dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.patient_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.patient_dgv.BackgroundColor = System.Drawing.Color.White;
            this.patient_dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.patient_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.patient_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.patient_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patient_dgv.GridColor = System.Drawing.Color.White;
            this.patient_dgv.Location = new System.Drawing.Point(0, 0);
            this.patient_dgv.MultiSelect = false;
            this.patient_dgv.Name = "patient_dgv";
            this.patient_dgv.ReadOnly = true;
            this.patient_dgv.RowHeadersVisible = false;
            this.patient_dgv.Size = new System.Drawing.Size(703, 325);
            this.patient_dgv.TabIndex = 6;
            // 
            // PagingPanel
            // 
            this.PagingPanel.BackColor = System.Drawing.Color.White;
            this.PagingPanel.Controls.Add(this.toolStripPaging);
            this.PagingPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PagingPanel.Location = new System.Drawing.Point(0, 325);
            this.PagingPanel.Name = "PagingPanel";
            this.PagingPanel.Size = new System.Drawing.Size(703, 23);
            this.PagingPanel.TabIndex = 7;
            // 
            // toolStripPaging
            // 
            this.toolStripPaging.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.toolStripPaging.BackColor = System.Drawing.Color.White;
            this.toolStripPaging.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripPaging.CanOverflow = false;
            this.toolStripPaging.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripPaging.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripPaging.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPaging.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFirst,
            this.btnBackward,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.btnForward,
            this.btnLast});
            this.toolStripPaging.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripPaging.Location = new System.Drawing.Point(217, 0);
            this.toolStripPaging.Name = "toolStripPaging";
            this.toolStripPaging.Size = new System.Drawing.Size(249, 25);
            this.toolStripPaging.TabIndex = 0;
            this.toolStripPaging.Text = "toolStrip1";
            // 
            // btnFirst
            // 
            this.btnFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFirst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(36, 22);
            this.btnFirst.Text = "First";
            this.btnFirst.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // btnBackward
            // 
            this.btnBackward.AutoToolTip = false;
            this.btnBackward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBackward.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(30, 22);
            this.btnBackward.Text = "<<";
            this.btnBackward.Click += new System.EventHandler(this.ToolStripButtonClick);
            this.btnBackward.MouseHover += new System.EventHandler(this.btnBackward_MouseHover);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.Color.White;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "1";
            this.toolStripButton1.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "2";
            this.toolStripButton2.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "3";
            this.toolStripButton3.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "4";
            this.toolStripButton4.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "5";
            this.toolStripButton5.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // btnForward
            // 
            this.btnForward.AutoToolTip = false;
            this.btnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnForward.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(30, 22);
            this.btnForward.Text = ">>";
            this.btnForward.Click += new System.EventHandler(this.ToolStripButtonClick);
            this.btnForward.MouseHover += new System.EventHandler(this.btnForward_MouseHover);
            // 
            // btnLast
            // 
            this.btnLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLast.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(35, 22);
            this.btnLast.Tag = "";
            this.btnLast.Text = "Last";
            this.btnLast.Click += new System.EventHandler(this.ToolStripButtonClick);
            // 
            // Pagination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 348);
            this.Controls.Add(this.patient_dgv);
            this.Controls.Add(this.PagingPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Pagination";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pagination";
            this.Load += new System.EventHandler(this.PortingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.patient_dgv)).EndInit();
            this.PagingPanel.ResumeLayout(false);
            this.PagingPanel.PerformLayout();
            this.toolStripPaging.ResumeLayout(false);
            this.toolStripPaging.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView patient_dgv;
        private System.Windows.Forms.Panel PagingPanel;
        private System.Windows.Forms.ToolStrip toolStripPaging;
        private System.Windows.Forms.ToolStripButton btnFirst;
        private System.Windows.Forms.ToolStripButton btnBackward;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton btnForward;
        private System.Windows.Forms.ToolStripButton btnLast;

    }
}