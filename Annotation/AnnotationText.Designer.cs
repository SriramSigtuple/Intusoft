namespace Annotation
{
    partial class AnnotationText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnnotationText));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.annotationTextCount_lbl = new System.Windows.Forms.Label();
            this.Annotation_tbx = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4);
            this.panel1.Size = new System.Drawing.Size(290, 73);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.78723F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.929078F));
            this.tableLayoutPanel1.Controls.Add(this.annotationTextCount_lbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Annotation_tbx, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(282, 65);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // annotationTextCount_lbl
            // 
            this.annotationTextCount_lbl.AutoSize = true;
            this.annotationTextCount_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationTextCount_lbl.ForeColor = System.Drawing.Color.White;
            this.annotationTextCount_lbl.Location = new System.Drawing.Point(3, 0);
            this.annotationTextCount_lbl.Name = "annotationTextCount_lbl";
            this.annotationTextCount_lbl.Size = new System.Drawing.Size(23, 65);
            this.annotationTextCount_lbl.TabIndex = 0;
            this.annotationTextCount_lbl.Text = "0";
            this.annotationTextCount_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Annotation_tbx
            // 
            this.Annotation_tbx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Annotation_tbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Annotation_tbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Annotation_tbx.Location = new System.Drawing.Point(32, 3);
            this.Annotation_tbx.MaxLength = 85;
            this.Annotation_tbx.Name = "Annotation_tbx";
            this.Annotation_tbx.Size = new System.Drawing.Size(218, 59);
            this.Annotation_tbx.TabIndex = 1;
            this.Annotation_tbx.Text = "";
            this.Annotation_tbx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Annotation_tbx_MouseClick);
            this.Annotation_tbx.TextChanged += new System.EventHandler(this.Annotation_tbx_TextChanged);
            this.Annotation_tbx.Enter += new System.EventHandler(this.Annotation_tbx_Enter);
            this.Annotation_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Annotation_tbx_KeyPress);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(253, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(29, 65);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ForeColor = System.Drawing.Color.Red;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 62);
            this.toolStripButton1.Text = "X";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // AnnotationText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.panel1);
            this.Name = "AnnotationText";
            this.Size = new System.Drawing.Size(290, 73);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Label annotationTextCount_lbl;
        public System.Windows.Forms.RichTextBox Annotation_tbx;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;

    }
}
