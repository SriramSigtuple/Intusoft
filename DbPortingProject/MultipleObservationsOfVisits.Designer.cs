namespace DBPorting
{
    partial class MultipleObservationsOfVisits
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
            this.report_dgv = new INTUSOFT.Custom.Controls.FormDataGridView();
            this.visitsView_dgv = new INTUSOFT.Custom.Controls.FormDataGridView();
            this.Obs_dgv = new INTUSOFT.Custom.Controls.FormDataGridView();
            this.patGridView_dgv = new INTUSOFT.Custom.Controls.FormDataGridView();
            this.annotation_dgv = new INTUSOFT.Custom.Controls.FormDataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.report_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visitsView_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Obs_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patGridView_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.annotation_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.9336F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0664F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 368F));
            this.tableLayoutPanel1.Controls.Add(this.report_dgv, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.visitsView_dgv, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Obs_dgv, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.patGridView_dgv, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotation_dgv, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.53788F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.46212F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1028, 528);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // report_dgv
            // 
            this.report_dgv.AllowUserToAddRows = false;
            this.report_dgv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.report_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.report_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.report_dgv.Location = new System.Drawing.Point(332, 254);
            this.report_dgv.Name = "report_dgv";
            this.report_dgv.ReadOnly = true;
            this.report_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.report_dgv.Size = new System.Drawing.Size(324, 271);
            this.report_dgv.TabIndex = 3;
            // 
            // visitsView_dgv
            // 
            this.visitsView_dgv.AllowUserToAddRows = false;
            this.visitsView_dgv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.visitsView_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.visitsView_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visitsView_dgv.Location = new System.Drawing.Point(3, 254);
            this.visitsView_dgv.Name = "visitsView_dgv";
            this.visitsView_dgv.ReadOnly = true;
            this.visitsView_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.visitsView_dgv.Size = new System.Drawing.Size(323, 271);
            this.visitsView_dgv.TabIndex = 2;
            // 
            // Obs_dgv
            // 
            this.Obs_dgv.AllowUserToAddRows = false;
            this.Obs_dgv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.Obs_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Obs_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Obs_dgv.Location = new System.Drawing.Point(332, 3);
            this.Obs_dgv.Name = "Obs_dgv";
            this.Obs_dgv.ReadOnly = true;
            this.Obs_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Obs_dgv.Size = new System.Drawing.Size(324, 245);
            this.Obs_dgv.TabIndex = 1;
            this.Obs_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Obs_dgv_CellClick);
            // 
            // patGridView_dgv
            // 
            this.patGridView_dgv.AllowUserToAddRows = false;
            this.patGridView_dgv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.patGridView_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.patGridView_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patGridView_dgv.Location = new System.Drawing.Point(3, 3);
            this.patGridView_dgv.Name = "patGridView_dgv";
            this.patGridView_dgv.ReadOnly = true;
            this.patGridView_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.patGridView_dgv.Size = new System.Drawing.Size(323, 245);
            this.patGridView_dgv.TabIndex = 0;
            this.patGridView_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.patGridView_dgv_CellClick);
            // 
            // annotation_dgv
            // 
            this.annotation_dgv.AllowUserToAddRows = false;
            this.annotation_dgv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.annotation_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.annotation_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotation_dgv.Location = new System.Drawing.Point(662, 3);
            this.annotation_dgv.Name = "annotation_dgv";
            this.annotation_dgv.ReadOnly = true;
            this.annotation_dgv.Size = new System.Drawing.Size(363, 245);
            this.annotation_dgv.TabIndex = 4;
            // 
            // MultipleObservationsOfVisits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 528);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MultipleObservationsOfVisits";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MultipleObservationsOfVisits";
            this.Load += new System.EventHandler(this.MultipleObservationsOfVisits_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.report_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visitsView_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Obs_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patGridView_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.annotation_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private INTUSOFT.Custom.Controls.FormDataGridView patGridView_dgv;
        private INTUSOFT.Custom.Controls.FormDataGridView report_dgv;
        private INTUSOFT.Custom.Controls.FormDataGridView visitsView_dgv;
        private INTUSOFT.Custom.Controls.FormDataGridView Obs_dgv;
        private INTUSOFT.Custom.Controls.FormDataGridView annotation_dgv;

    }
}