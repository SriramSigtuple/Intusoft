namespace IVLTemplateCreator
{
    partial class TemplateForm
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
            this.centre_pnl = new System.Windows.Forms.Panel();
            this.reportCanvas_pnl = new System.Windows.Forms.Panel();
            this.toolBox_pnl = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.colStylePercent_nud = new System.Windows.Forms.NumericUpDown();
            this.rowStylepercent_nud = new System.Windows.Forms.NumericUpDown();
            this.height_nud = new System.Windows.Forms.NumericUpDown();
            this.yLocation_nud = new System.Windows.Forms.NumericUpDown();
            this.width_nud = new System.Windows.Forms.NumericUpDown();
            this.xlocation_nud = new System.Windows.Forms.NumericUpDown();
            this.Fontstyle_cbx_2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Fontstyle_cbx = new System.Windows.Forms.ComboBox();
            this.Fonttype_lbl = new System.Windows.Forms.Label();
            this.binding_cbx = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.colorPicker_btn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.fontName_cbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fontSize_cbx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.controlText_tbx = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.save_btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_btn = new System.Windows.Forms.Button();
            this.bitmap_btn = new System.Windows.Forms.Button();
            this.textbox_btn = new System.Windows.Forms.Button();
            this.lbl_btn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.language_cbx = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.preview_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.formButtons1 = new INTUSOFT.Custom.Controls.FormButtons();
            this.label2 = new System.Windows.Forms.Label();
            this.templates_cbx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.noOfImg_cbx = new System.Windows.Forms.ComboBox();
            this.landscape_rb = new System.Windows.Forms.RadioButton();
            this.portrait_rb = new System.Windows.Forms.RadioButton();
            this.centre_pnl.SuspendLayout();
            this.toolBox_pnl.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colStylePercent_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowStylepercent_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.height_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yLocation_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.width_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xlocation_nud)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // centre_pnl
            // 
            this.centre_pnl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.centre_pnl.Controls.Add(this.reportCanvas_pnl);
            this.centre_pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centre_pnl.Location = new System.Drawing.Point(0, 0);
            this.centre_pnl.Name = "centre_pnl";
            this.centre_pnl.Size = new System.Drawing.Size(1144, 730);
            this.centre_pnl.TabIndex = 0;
            // 
            // reportCanvas_pnl
            // 
            this.reportCanvas_pnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportCanvas_pnl.BackColor = System.Drawing.Color.White;
            this.reportCanvas_pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportCanvas_pnl.Location = new System.Drawing.Point(12, 7);
            this.reportCanvas_pnl.Name = "reportCanvas_pnl";
            this.reportCanvas_pnl.Size = new System.Drawing.Size(544, 730);
            this.reportCanvas_pnl.TabIndex = 2;
            this.reportCanvas_pnl.Click += new System.EventHandler(this.reportCanvas_pnl_Click);
            // 
            // toolBox_pnl
            // 
            this.toolBox_pnl.BackColor = System.Drawing.Color.Black;
            this.toolBox_pnl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolBox_pnl.Controls.Add(this.propertyGrid1);
            this.toolBox_pnl.Controls.Add(this.panel3);
            this.toolBox_pnl.Controls.Add(this.save_btn);
            this.toolBox_pnl.Controls.Add(this.panel1);
            this.toolBox_pnl.Controls.Add(this.panel2);
            this.toolBox_pnl.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolBox_pnl.Location = new System.Drawing.Point(903, 0);
            this.toolBox_pnl.Name = "toolBox_pnl";
            this.toolBox_pnl.Size = new System.Drawing.Size(241, 730);
            this.toolBox_pnl.TabIndex = 3;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 565);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(239, 126);
            this.propertyGrid1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.colStylePercent_nud);
            this.panel3.Controls.Add(this.rowStylepercent_nud);
            this.panel3.Controls.Add(this.height_nud);
            this.panel3.Controls.Add(this.yLocation_nud);
            this.panel3.Controls.Add(this.width_nud);
            this.panel3.Controls.Add(this.xlocation_nud);
            this.panel3.Controls.Add(this.Fontstyle_cbx_2);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.Fontstyle_cbx);
            this.panel3.Controls.Add(this.Fonttype_lbl);
            this.panel3.Controls.Add(this.binding_cbx);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.colorPicker_btn);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.fontName_cbx);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.fontSize_cbx);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.controlText_tbx);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 237);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 328);
            this.panel3.TabIndex = 3;
            // 
            // colStylePercent_nud
            // 
            this.colStylePercent_nud.Location = new System.Drawing.Point(162, 207);
            this.colStylePercent_nud.Name = "colStylePercent_nud";
            this.colStylePercent_nud.Size = new System.Drawing.Size(59, 20);
            this.colStylePercent_nud.TabIndex = 17;
            this.colStylePercent_nud.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // rowStylepercent_nud
            // 
            this.rowStylepercent_nud.Location = new System.Drawing.Point(10, 207);
            this.rowStylepercent_nud.Name = "rowStylepercent_nud";
            this.rowStylepercent_nud.Size = new System.Drawing.Size(63, 20);
            this.rowStylepercent_nud.TabIndex = 16;
            this.rowStylepercent_nud.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // height_nud
            // 
            this.height_nud.Location = new System.Drawing.Point(179, 292);
            this.height_nud.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.height_nud.Name = "height_nud";
            this.height_nud.Size = new System.Drawing.Size(42, 20);
            this.height_nud.TabIndex = 15;
            this.height_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.height_nud.ValueChanged += new System.EventHandler(this.height_nud_ValueChanged);
            // 
            // yLocation_nud
            // 
            this.yLocation_nud.Location = new System.Drawing.Point(110, 232);
            this.yLocation_nud.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.yLocation_nud.Name = "yLocation_nud";
            this.yLocation_nud.Size = new System.Drawing.Size(42, 20);
            this.yLocation_nud.TabIndex = 15;
            this.yLocation_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.yLocation_nud.ValueChanged += new System.EventHandler(this.yLocation_nud_ValueChanged);
            // 
            // width_nud
            // 
            this.width_nud.Location = new System.Drawing.Point(62, 290);
            this.width_nud.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.width_nud.Name = "width_nud";
            this.width_nud.Size = new System.Drawing.Size(42, 20);
            this.width_nud.TabIndex = 15;
            this.width_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.width_nud.ValueChanged += new System.EventHandler(this.width_nud_ValueChanged);
            // 
            // xlocation_nud
            // 
            this.xlocation_nud.Location = new System.Drawing.Point(110, 205);
            this.xlocation_nud.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.xlocation_nud.Name = "xlocation_nud";
            this.xlocation_nud.Size = new System.Drawing.Size(42, 20);
            this.xlocation_nud.TabIndex = 15;
            this.xlocation_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.xlocation_nud.ValueChanged += new System.EventHandler(this.xlocation_nud_ValueChanged);
            // 
            // Fontstyle_cbx_2
            // 
            this.Fontstyle_cbx_2.FormattingEnabled = true;
            this.Fontstyle_cbx_2.Items.AddRange(new object[] {
            "Bold",
            "Regular",
            "Italic",
            "Strikeout",
            "Underline"});
            this.Fontstyle_cbx_2.Location = new System.Drawing.Point(145, 129);
            this.Fontstyle_cbx_2.Name = "Fontstyle_cbx_2";
            this.Fontstyle_cbx_2.Size = new System.Drawing.Size(83, 21);
            this.Fontstyle_cbx_2.TabIndex = 14;
            this.Fontstyle_cbx_2.TextChanged += new System.EventHandler(this.Fontstyle_cbx_2_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(107, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "And";
            // 
            // Fontstyle_cbx
            // 
            this.Fontstyle_cbx.FormattingEnabled = true;
            this.Fontstyle_cbx.Items.AddRange(new object[] {
            "Bold",
            "Regular",
            "Italic",
            "Strikeout",
            "Underline"});
            this.Fontstyle_cbx.Location = new System.Drawing.Point(6, 130);
            this.Fontstyle_cbx.Name = "Fontstyle_cbx";
            this.Fontstyle_cbx.Size = new System.Drawing.Size(83, 21);
            this.Fontstyle_cbx.TabIndex = 12;
            this.Fontstyle_cbx.TextChanged += new System.EventHandler(this.Fontstyle_cbx_TextChanged);
            // 
            // Fonttype_lbl
            // 
            this.Fonttype_lbl.AutoSize = true;
            this.Fonttype_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fonttype_lbl.Location = new System.Drawing.Point(7, 114);
            this.Fonttype_lbl.Name = "Fonttype_lbl";
            this.Fonttype_lbl.Size = new System.Drawing.Size(68, 13);
            this.Fonttype_lbl.TabIndex = 3;
            this.Fonttype_lbl.Text = "Font Style:";
            // 
            // binding_cbx
            // 
            this.binding_cbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.binding_cbx.FormattingEnabled = true;
            this.binding_cbx.Items.AddRange(new object[] {
            "$name",
            "$mrn",
            "$age",
            "$gender",
            "$date",
            "$hospital",
            "$comment",
            "$disclaimer",
            "$operator"});
            this.binding_cbx.Location = new System.Drawing.Point(69, 156);
            this.binding_cbx.Name = "binding_cbx";
            this.binding_cbx.Size = new System.Drawing.Size(125, 21);
            this.binding_cbx.TabIndex = 11;
            this.binding_cbx.SelectedIndexChanged += new System.EventHandler(this.binding_cbx_SelectedIndexChanged);
            this.binding_cbx.TextChanged += new System.EventHandler(this.binding_cbx_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(125, 294);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 13);
            this.label16.TabIndex = 10;
            this.label16.Text = "Height:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(85, 232);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(19, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Y:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(13, 292);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Width:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(87, 207);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "X:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(16, 266);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(39, 13);
            this.label14.TabIndex = 10;
            this.label14.Text = "Size :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(95, 189);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Location :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(159, 189);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Columns :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Rows  :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Bind to  :";
            // 
            // colorPicker_btn
            // 
            this.colorPicker_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorPicker_btn.Location = new System.Drawing.Point(162, 84);
            this.colorPicker_btn.Name = "colorPicker_btn";
            this.colorPicker_btn.Size = new System.Drawing.Size(32, 23);
            this.colorPicker_btn.TabIndex = 9;
            this.colorPicker_btn.UseVisualStyleBackColor = true;
            this.colorPicker_btn.Click += new System.EventHandler(this.colorPicker_btn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(112, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Color :";
            // 
            // fontName_cbx
            // 
            this.fontName_cbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontName_cbx.FormattingEnabled = true;
            this.fontName_cbx.Items.AddRange(new object[] {
            "Arial",
            "Calibri",
            "Microsoft Sans Serif",
            "MS Outlook",
            "Rockwell",
            "Symbol",
            "Tahoma",
            "Times New Roman",
            "Verdana"});
            this.fontName_cbx.Location = new System.Drawing.Point(53, 49);
            this.fontName_cbx.Name = "fontName_cbx";
            this.fontName_cbx.Size = new System.Drawing.Size(126, 21);
            this.fontName_cbx.TabIndex = 7;
            this.fontName_cbx.SelectedIndexChanged += new System.EventHandler(this.fontName_cbx_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Font :";
            // 
            // fontSize_cbx
            // 
            this.fontSize_cbx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontSize_cbx.FormattingEnabled = true;
            this.fontSize_cbx.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "16",
            "20"});
            this.fontSize_cbx.Location = new System.Drawing.Point(53, 81);
            this.fontSize_cbx.Name = "fontSize_cbx";
            this.fontSize_cbx.Size = new System.Drawing.Size(53, 21);
            this.fontSize_cbx.TabIndex = 5;
            this.fontSize_cbx.SelectedIndexChanged += new System.EventHandler(this.fontSize_cbx_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Size :";
            // 
            // controlText_tbx
            // 
            this.controlText_tbx.Location = new System.Drawing.Point(53, 17);
            this.controlText_tbx.Name = "controlText_tbx";
            this.controlText_tbx.Size = new System.Drawing.Size(126, 20);
            this.controlText_tbx.TabIndex = 1;
            this.controlText_tbx.TextChanged += new System.EventHandler(this.controlText_tbx_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Text :";
            // 
            // save_btn
            // 
            this.save_btn.Location = new System.Drawing.Point(-1, 697);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(241, 32);
            this.save_btn.TabIndex = 2;
            this.save_btn.Text = "Save";
            this.save_btn.UseVisualStyleBackColor = true;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.tableLayoutPanel_btn);
            this.panel1.Controls.Add(this.bitmap_btn);
            this.panel1.Controls.Add(this.textbox_btn);
            this.panel1.Controls.Add(this.lbl_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 145);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 92);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel_btn
            // 
            this.tableLayoutPanel_btn.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel_btn.Location = new System.Drawing.Point(0, 74);
            this.tableLayoutPanel_btn.Name = "tableLayoutPanel_btn";
            this.tableLayoutPanel_btn.Size = new System.Drawing.Size(239, 21);
            this.tableLayoutPanel_btn.TabIndex = 3;
            this.tableLayoutPanel_btn.Text = "Tabel Layout";
            this.tableLayoutPanel_btn.UseVisualStyleBackColor = true;
            this.tableLayoutPanel_btn.Click += new System.EventHandler(this.tableLayoutPanel_btn_Click);
            // 
            // bitmap_btn
            // 
            this.bitmap_btn.Dock = System.Windows.Forms.DockStyle.Top;
            this.bitmap_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bitmap_btn.Location = new System.Drawing.Point(0, 52);
            this.bitmap_btn.Name = "bitmap_btn";
            this.bitmap_btn.Size = new System.Drawing.Size(239, 22);
            this.bitmap_btn.TabIndex = 2;
            this.bitmap_btn.Text = "Logo";
            this.bitmap_btn.UseVisualStyleBackColor = true;
            this.bitmap_btn.Click += new System.EventHandler(this.bitmap_btn_Click);
            // 
            // textbox_btn
            // 
            this.textbox_btn.Dock = System.Windows.Forms.DockStyle.Top;
            this.textbox_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textbox_btn.Location = new System.Drawing.Point(0, 29);
            this.textbox_btn.Name = "textbox_btn";
            this.textbox_btn.Size = new System.Drawing.Size(239, 23);
            this.textbox_btn.TabIndex = 1;
            this.textbox_btn.Text = "TextBox";
            this.textbox_btn.UseVisualStyleBackColor = true;
            this.textbox_btn.Click += new System.EventHandler(this.textbox_btn_Click);
            // 
            // lbl_btn
            // 
            this.lbl_btn.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_btn.Location = new System.Drawing.Point(0, 0);
            this.lbl_btn.Name = "lbl_btn";
            this.lbl_btn.Size = new System.Drawing.Size(239, 29);
            this.lbl_btn.TabIndex = 0;
            this.lbl_btn.Text = "Label";
            this.lbl_btn.UseVisualStyleBackColor = true;
            this.lbl_btn.Click += new System.EventHandler(this.lbl_btn_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.language_cbx);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.preview_btn);
            this.panel2.Controls.Add(this.formButtons1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.templates_cbx);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.noOfImg_cbx);
            this.panel2.Controls.Add(this.landscape_rb);
            this.panel2.Controls.Add(this.portrait_rb);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 145);
            this.panel2.TabIndex = 0;
            // 
            // language_cbx
            // 
            this.language_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language_cbx.FormattingEnabled = true;
            this.language_cbx.Items.AddRange(new object[] {
            "en",
            "es"});
            this.language_cbx.Location = new System.Drawing.Point(116, 29);
            this.language_cbx.Name = "language_cbx";
            this.language_cbx.Size = new System.Drawing.Size(121, 21);
            this.language_cbx.TabIndex = 8;
            this.language_cbx.SelectedIndexChanged += new System.EventHandler(this.language_cbx_SelectedIndexChanged);
            this.language_cbx.SelectionChangeCommitted += new System.EventHandler(this.language_cbx_SelectionChangeCommitted);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(146, 8);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(63, 13);
            this.label17.TabIndex = 7;
            this.label17.Text = "Language";
            // 
            // preview_btn
            // 
            this.preview_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preview_btn.Location = new System.Drawing.Point(98, 116);
            this.preview_btn.Name = "preview_btn";
            this.preview_btn.Size = new System.Drawing.Size(110, 23);
            this.preview_btn.TabIndex = 6;
            this.preview_btn.Text = "Report Preview";
            this.preview_btn.UseVisualStyleBackColor = true;
            this.preview_btn.Click += new System.EventHandler(this.preview_btn_Click);
            // 
            // formButtons1
            // 
            this.formButtons1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formButtons1.Location = new System.Drawing.Point(6, 116);
            this.formButtons1.Name = "formButtons1";
            this.formButtons1.Size = new System.Drawing.Size(86, 23);
            this.formButtons1.TabIndex = 6;
            this.formButtons1.Text = "Templates";
            this.formButtons1.UseVisualStyleBackColor = true;
            this.formButtons1.Click += new System.EventHandler(this.formButtons1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Templates";
            // 
            // templates_cbx
            // 
            this.templates_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templates_cbx.FormattingEnabled = true;
            this.templates_cbx.Location = new System.Drawing.Point(75, 60);
            this.templates_cbx.Name = "templates_cbx";
            this.templates_cbx.Size = new System.Drawing.Size(120, 21);
            this.templates_cbx.TabIndex = 4;
            this.templates_cbx.SelectedIndexChanged += new System.EventHandler(this.templates_cbx_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "No of Images";
            // 
            // noOfImg_cbx
            // 
            this.noOfImg_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.noOfImg_cbx.FormattingEnabled = true;
            this.noOfImg_cbx.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "6"});
            this.noOfImg_cbx.Location = new System.Drawing.Point(91, 87);
            this.noOfImg_cbx.Name = "noOfImg_cbx";
            this.noOfImg_cbx.Size = new System.Drawing.Size(45, 21);
            this.noOfImg_cbx.TabIndex = 2;
            this.noOfImg_cbx.SelectedIndexChanged += new System.EventHandler(this.noOfImg_cbx_SelectedIndexChanged);
            // 
            // landscape_rb
            // 
            this.landscape_rb.AutoSize = true;
            this.landscape_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.landscape_rb.Location = new System.Drawing.Point(19, 29);
            this.landscape_rb.Name = "landscape_rb";
            this.landscape_rb.Size = new System.Drawing.Size(87, 17);
            this.landscape_rb.TabIndex = 1;
            this.landscape_rb.TabStop = true;
            this.landscape_rb.Text = "Landscape";
            this.landscape_rb.UseVisualStyleBackColor = true;
            // 
            // portrait_rb
            // 
            this.portrait_rb.AutoSize = true;
            this.portrait_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portrait_rb.Location = new System.Drawing.Point(19, 6);
            this.portrait_rb.Name = "portrait_rb";
            this.portrait_rb.Size = new System.Drawing.Size(66, 17);
            this.portrait_rb.TabIndex = 0;
            this.portrait_rb.TabStop = true;
            this.portrait_rb.Text = "Portrait";
            this.portrait_rb.UseVisualStyleBackColor = true;
            this.portrait_rb.CheckedChanged += new System.EventHandler(this.portrait_rb_CheckedChanged);
            // 
            // TemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 730);
            this.Controls.Add(this.toolBox_pnl);
            this.Controls.Add(this.centre_pnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.centre_pnl.ResumeLayout(false);
            this.toolBox_pnl.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colStylePercent_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowStylepercent_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.height_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yLocation_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.width_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xlocation_nud)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel centre_pnl;
        private System.Windows.Forms.Panel toolBox_pnl;
        private System.Windows.Forms.Panel reportCanvas_pnl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton landscape_rb;
        private System.Windows.Forms.RadioButton portrait_rb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button textbox_btn;
        private System.Windows.Forms.Button lbl_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox noOfImg_cbx;
        private System.Windows.Forms.Button bitmap_btn;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox templates_cbx;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox fontSize_cbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox controlText_tbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox fontName_cbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button colorPicker_btn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox binding_cbx;
        private System.Windows.Forms.Label label7;
        private INTUSOFT.Custom.Controls.FormButtons formButtons1;
        private System.Windows.Forms.Button tableLayoutPanel_btn;
        private System.Windows.Forms.Label Fonttype_lbl;
        private System.Windows.Forms.ComboBox Fontstyle_cbx;
        private System.Windows.Forms.ComboBox Fontstyle_cbx_2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.NumericUpDown colStylePercent_nud;
        private System.Windows.Forms.NumericUpDown rowStylepercent_nud;
        private INTUSOFT.Custom.Controls.FormButtons preview_btn;
        private System.Windows.Forms.NumericUpDown yLocation_nud;
        private System.Windows.Forms.NumericUpDown xlocation_nud;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown height_nud;
        private System.Windows.Forms.NumericUpDown width_nud;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox language_cbx;

    }
}

