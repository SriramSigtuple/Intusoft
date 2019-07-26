namespace DesignSurfaceManagerExtension
{
    partial class Font_UC
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
            this.FontFamily_cbx = new System.Windows.Forms.ComboBox();
            this.FontColor_lbl = new System.Windows.Forms.Label();
            this.FontType_lbl = new System.Windows.Forms.Label();
            this.FontFamily_lbl = new System.Windows.Forms.Label();
            this.FontSize_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.FontColorDialog_btn = new System.Windows.Forms.Button();
            this.color_tbx = new System.Windows.Forms.TextBox();
            this.FontSize_nud = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.Underline_btn = new System.Windows.Forms.Button();
            this.Italics_btn = new System.Windows.Forms.Button();
            this.Regular_btn = new System.Windows.Forms.Button();
            this.Bold_btn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontSize_nud)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.98374F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.01626F));
            this.tableLayoutPanel1.Controls.Add(this.FontFamily_cbx, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.FontColor_lbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FontType_lbl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.FontFamily_lbl, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.FontSize_lbl, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FontSize_nud, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.2551F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.72414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.75862F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(246, 145);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // FontFamily_cbx
            // 
            this.FontFamily_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontFamily_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FontFamily_cbx.FormattingEnabled = true;
            this.FontFamily_cbx.Location = new System.Drawing.Point(62, 85);
            this.FontFamily_cbx.Name = "FontFamily_cbx";
            this.FontFamily_cbx.Size = new System.Drawing.Size(181, 21);
            this.FontFamily_cbx.TabIndex = 7;
            this.FontFamily_cbx.SelectedIndexChanged += new System.EventHandler(this.FontFamily_cbx_SelectedIndexChanged);
            // 
            // FontColor_lbl
            // 
            this.FontColor_lbl.AutoSize = true;
            this.FontColor_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontColor_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontColor_lbl.Location = new System.Drawing.Point(3, 0);
            this.FontColor_lbl.Name = "FontColor_lbl";
            this.FontColor_lbl.Size = new System.Drawing.Size(53, 36);
            this.FontColor_lbl.TabIndex = 1;
            this.FontColor_lbl.Text = "Color";
            // 
            // FontType_lbl
            // 
            this.FontType_lbl.AutoSize = true;
            this.FontType_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontType_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontType_lbl.Location = new System.Drawing.Point(3, 36);
            this.FontType_lbl.Name = "FontType_lbl";
            this.FontType_lbl.Size = new System.Drawing.Size(53, 46);
            this.FontType_lbl.TabIndex = 2;
            this.FontType_lbl.Text = "Type";
            // 
            // FontFamily_lbl
            // 
            this.FontFamily_lbl.AutoSize = true;
            this.FontFamily_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontFamily_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontFamily_lbl.Location = new System.Drawing.Point(3, 82);
            this.FontFamily_lbl.Name = "FontFamily_lbl";
            this.FontFamily_lbl.Size = new System.Drawing.Size(53, 29);
            this.FontFamily_lbl.TabIndex = 3;
            this.FontFamily_lbl.Text = "Family";
            // 
            // FontSize_lbl
            // 
            this.FontSize_lbl.AutoSize = true;
            this.FontSize_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontSize_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontSize_lbl.Location = new System.Drawing.Point(3, 111);
            this.FontSize_lbl.Name = "FontSize_lbl";
            this.FontSize_lbl.Size = new System.Drawing.Size(53, 34);
            this.FontSize_lbl.TabIndex = 4;
            this.FontSize_lbl.Text = "Size";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.12904F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.87097F));
            this.tableLayoutPanel2.Controls.Add(this.FontColorDialog_btn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.color_tbx, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(62, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(181, 30);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // FontColorDialog_btn
            // 
            this.FontColorDialog_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontColorDialog_btn.Location = new System.Drawing.Point(140, 3);
            this.FontColorDialog_btn.Name = "FontColorDialog_btn";
            this.FontColorDialog_btn.Size = new System.Drawing.Size(38, 24);
            this.FontColorDialog_btn.TabIndex = 7;
            this.FontColorDialog_btn.Text = "...";
            this.FontColorDialog_btn.UseVisualStyleBackColor = true;
            this.FontColorDialog_btn.Click += new System.EventHandler(this.FontColorDialog_btn_Click);
            // 
            // color_tbx
            // 
            this.color_tbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.color_tbx.Location = new System.Drawing.Point(3, 3);
            this.color_tbx.Name = "color_tbx";
            this.color_tbx.ReadOnly = true;
            this.color_tbx.Size = new System.Drawing.Size(131, 20);
            this.color_tbx.TabIndex = 8;
            // 
            // FontSize_nud
            // 
            this.FontSize_nud.DecimalPlaces = 2;
            this.FontSize_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontSize_nud.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.FontSize_nud.Location = new System.Drawing.Point(62, 114);
            this.FontSize_nud.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.FontSize_nud.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.FontSize_nud.Name = "FontSize_nud";
            this.FontSize_nud.Size = new System.Drawing.Size(181, 20);
            this.FontSize_nud.TabIndex = 8;
            this.FontSize_nud.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.FontSize_nud.ValueChanged += new System.EventHandler(this.FontSize_nud_ValueChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.Underline_btn, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.Italics_btn, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.Regular_btn, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.Bold_btn, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(62, 39);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(181, 40);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // Underline_btn
            // 
            this.Underline_btn.BackColor = System.Drawing.Color.LightGray;
            this.Underline_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Underline_btn.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Underline_btn.Location = new System.Drawing.Point(138, 3);
            this.Underline_btn.Name = "Underline_btn";
            this.Underline_btn.Size = new System.Drawing.Size(40, 34);
            this.Underline_btn.TabIndex = 3;
            this.Underline_btn.Text = "U&";
            this.Underline_btn.UseVisualStyleBackColor = false;
            this.Underline_btn.Click += new System.EventHandler(this.UnderLine_btn_Click);
            this.Underline_btn.MouseHover += new System.EventHandler(this.UnderLine_btn_MouseHover);
            // 
            // Italics_btn
            // 
            this.Italics_btn.BackColor = System.Drawing.Color.LightGray;
            this.Italics_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Italics_btn.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Italics_btn.Location = new System.Drawing.Point(93, 3);
            this.Italics_btn.Name = "Italics_btn";
            this.Italics_btn.Size = new System.Drawing.Size(39, 34);
            this.Italics_btn.TabIndex = 2;
            this.Italics_btn.Text = "I";
            this.Italics_btn.UseVisualStyleBackColor = false;
            this.Italics_btn.Click += new System.EventHandler(this.Italics_btn_Click);
            this.Italics_btn.MouseHover += new System.EventHandler(this.Italics_btn_MouseHover);
            // 
            // Regular_btn
            // 
            this.Regular_btn.BackColor = System.Drawing.Color.LightGray;
            this.Regular_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Regular_btn.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Regular_btn.Location = new System.Drawing.Point(3, 3);
            this.Regular_btn.Name = "Regular_btn";
            this.Regular_btn.Size = new System.Drawing.Size(39, 34);
            this.Regular_btn.TabIndex = 0;
            this.Regular_btn.Text = "R";
            this.Regular_btn.UseVisualStyleBackColor = false;
            this.Regular_btn.Click += new System.EventHandler(this.Regular_btn_Click);
            this.Regular_btn.MouseHover += new System.EventHandler(this.Regular_btn_MouseHover);
            // 
            // Bold_btn
            // 
            this.Bold_btn.BackColor = System.Drawing.Color.LightGray;
            this.Bold_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bold_btn.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bold_btn.Location = new System.Drawing.Point(48, 3);
            this.Bold_btn.Name = "Bold_btn";
            this.Bold_btn.Size = new System.Drawing.Size(39, 34);
            this.Bold_btn.TabIndex = 1;
            this.Bold_btn.Text = "B";
            this.Bold_btn.UseVisualStyleBackColor = false;
            this.Bold_btn.Click += new System.EventHandler(this.Bold_btn_Click);
            this.Bold_btn.MouseCaptureChanged += new System.EventHandler(this.Bold_btn_MouseCaptureChanged);
            // 
            // Font_UC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Font_UC";
            this.Size = new System.Drawing.Size(246, 145);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontSize_nud)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label FontColor_lbl;
        private System.Windows.Forms.Label FontType_lbl;
        private System.Windows.Forms.Label FontFamily_lbl;
        private System.Windows.Forms.Label FontSize_lbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox FontFamily_cbx;
        private System.Windows.Forms.Button FontColorDialog_btn;
        private System.Windows.Forms.NumericUpDown FontSize_nud;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button Regular_btn;
        private System.Windows.Forms.Button Underline_btn;
        private System.Windows.Forms.Button Italics_btn;
        private System.Windows.Forms.Button Bold_btn;
        private System.Windows.Forms.TextBox color_tbx;
    }
}
