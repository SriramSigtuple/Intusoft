namespace INTUSOFT.Desktop.Forms
{
    partial class DiagnosisUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnosisUC));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.removeDiagnosis_btn = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.diagnosis_lbl = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeDiagnosis_btn});
            this.toolStrip1.Location = new System.Drawing.Point(165, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(43, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // removeDiagnosis_btn
            // 
            this.removeDiagnosis_btn.AutoSize = false;
            this.removeDiagnosis_btn.BackColor = System.Drawing.Color.Transparent;
            this.removeDiagnosis_btn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.removeDiagnosis_btn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.removeDiagnosis_btn.ForeColor = System.Drawing.Color.Red;
            this.removeDiagnosis_btn.Image = ((System.Drawing.Image)(resources.GetObject("removeDiagnosis_btn.Image")));
            this.removeDiagnosis_btn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeDiagnosis_btn.Name = "removeDiagnosis_btn";
            this.removeDiagnosis_btn.Size = new System.Drawing.Size(23, 60);
            this.removeDiagnosis_btn.Text = "x";
            this.removeDiagnosis_btn.Click += new System.EventHandler(this.removeDiagnosis_btn_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.66102F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.33898F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.diagnosis_lbl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(208, 27);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // diagnosis_lbl
            // 
            this.diagnosis_lbl.AutoEllipsis = true;
            this.diagnosis_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosis_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diagnosis_lbl.Location = new System.Drawing.Point(3, 0);
            this.diagnosis_lbl.Name = "diagnosis_lbl";
            this.diagnosis_lbl.Size = new System.Drawing.Size(159, 27);
            this.diagnosis_lbl.TabIndex = 1;
            this.diagnosis_lbl.Text = "label1";
            this.diagnosis_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiagnosisUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DiagnosisUC";
            this.Size = new System.Drawing.Size(208, 27);
            this.Load += new System.EventHandler(this.DiagnosisUC_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton removeDiagnosis_btn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label diagnosis_lbl;
    }
}
