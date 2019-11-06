namespace DesignSurfaceManagerExtension
{
    partial class ImageNameSettingsWindow
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.regularName_rb = new System.Windows.Forms.RadioButton();
            this.medicalName_rb = new System.Windows.Forms.RadioButton();
            this.eyeSideName_lbl = new System.Windows.Forms.Label();
            this.textAlign_lbl = new System.Windows.Forms.Label();
            this.alignment_tlp = new System.Windows.Forms.TableLayoutPanel();
            this.horizontalTextAlign_cmbx = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.apply_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.alignment_tlp.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.54054F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.45946F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 145);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.4964F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.50359F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.eyeSideName_lbl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textAlign_lbl, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.alignment_tlp, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(278, 81);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.regularName_rb);
            this.panel1.Controls.Add(this.medicalName_rb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(85, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 42);
            this.panel1.TabIndex = 0;
            // 
            // regularName_rb
            // 
            this.regularName_rb.AutoSize = true;
            this.regularName_rb.Location = new System.Drawing.Point(97, 11);
            this.regularName_rb.Name = "regularName_rb";
            this.regularName_rb.Size = new System.Drawing.Size(93, 17);
            this.regularName_rb.TabIndex = 0;
            this.regularName_rb.TabStop = true;
            this.regularName_rb.Text = "Regular Name";
            this.regularName_rb.UseVisualStyleBackColor = true;
            // 
            // medicalName_rb
            // 
            this.medicalName_rb.AutoSize = true;
            this.medicalName_rb.Location = new System.Drawing.Point(7, 10);
            this.medicalName_rb.Name = "medicalName_rb";
            this.medicalName_rb.Size = new System.Drawing.Size(93, 17);
            this.medicalName_rb.TabIndex = 0;
            this.medicalName_rb.TabStop = true;
            this.medicalName_rb.Text = "Medical Name";
            this.medicalName_rb.UseVisualStyleBackColor = true;
            this.medicalName_rb.CheckedChanged += new System.EventHandler(this.medicalName_rb_CheckedChanged);
            // 
            // eyeSideName_lbl
            // 
            this.eyeSideName_lbl.AutoSize = true;
            this.eyeSideName_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eyeSideName_lbl.Location = new System.Drawing.Point(3, 0);
            this.eyeSideName_lbl.Name = "eyeSideName_lbl";
            this.eyeSideName_lbl.Size = new System.Drawing.Size(76, 48);
            this.eyeSideName_lbl.TabIndex = 1;
            this.eyeSideName_lbl.Text = "Eye Side Name";
            this.eyeSideName_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textAlign_lbl
            // 
            this.textAlign_lbl.AutoSize = true;
            this.textAlign_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textAlign_lbl.Location = new System.Drawing.Point(3, 48);
            this.textAlign_lbl.Name = "textAlign_lbl";
            this.textAlign_lbl.Size = new System.Drawing.Size(76, 33);
            this.textAlign_lbl.TabIndex = 2;
            this.textAlign_lbl.Text = "Text Alignment:";
            this.textAlign_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alignment_tlp
            // 
            this.alignment_tlp.ColumnCount = 1;
            this.alignment_tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.alignment_tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.alignment_tlp.Controls.Add(this.horizontalTextAlign_cmbx, 0, 0);
            this.alignment_tlp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alignment_tlp.Location = new System.Drawing.Point(85, 51);
            this.alignment_tlp.Name = "alignment_tlp";
            this.alignment_tlp.RowCount = 1;
            this.alignment_tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.alignment_tlp.Size = new System.Drawing.Size(190, 27);
            this.alignment_tlp.TabIndex = 3;
            // 
            // horizontalTextAlign_cmbx
            // 
            this.horizontalTextAlign_cmbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalTextAlign_cmbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.horizontalTextAlign_cmbx.FormattingEnabled = true;
            this.horizontalTextAlign_cmbx.Items.AddRange(new object[] {
            "MiddleLeft",
            "MiddleCenter",
            "MiddleRight"});
            this.horizontalTextAlign_cmbx.Location = new System.Drawing.Point(3, 3);
            this.horizontalTextAlign_cmbx.Name = "horizontalTextAlign_cmbx";
            this.horizontalTextAlign_cmbx.Size = new System.Drawing.Size(184, 21);
            this.horizontalTextAlign_cmbx.TabIndex = 5;
            this.horizontalTextAlign_cmbx.SelectedIndexChanged += new System.EventHandler(this.horizontalTextAlign_cmbx_SelectedIndexChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.799F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.201F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel3.Controls.Add(this.apply_btn, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cancel_btn, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 90);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(278, 52);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // apply_btn
            // 
            this.apply_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.apply_btn.Location = new System.Drawing.Point(122, 3);
            this.apply_btn.Name = "apply_btn";
            this.apply_btn.Size = new System.Drawing.Size(74, 46);
            this.apply_btn.TabIndex = 0;
            this.apply_btn.Text = "Apply";
            this.apply_btn.UseVisualStyleBackColor = true;
            this.apply_btn.Click += new System.EventHandler(this.apply_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancel_btn.Location = new System.Drawing.Point(202, 3);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(73, 46);
            this.cancel_btn.TabIndex = 0;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // ImageNameSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_btn;
            this.ClientSize = new System.Drawing.Size(284, 145);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageNameSettingsWindow";
            this.Text = "ImageNameSettingsWindow";
            this.Load += new System.EventHandler(this.ImageNameSettingsWindow_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.alignment_tlp.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton regularName_rb;
        private System.Windows.Forms.RadioButton medicalName_rb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button apply_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.Label eyeSideName_lbl;
        private System.Windows.Forms.Label textAlign_lbl;
        private System.Windows.Forms.TableLayoutPanel alignment_tlp;
        private System.Windows.Forms.ComboBox horizontalTextAlign_cmbx;
    }
}