namespace AssemblySoftware
{
    partial class EEPROM_Window
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
            this.resetEEPROM_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.SaveEEPROM_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.readEEPROM_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.readSingleEEPROM_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.writeSingleEEPROM_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.eepromPageNumber_nud = new System.Windows.Forms.NumericUpDown();
            this.testString_tbx = new INTUSOFT.Custom.Controls.FormTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eepromPageNumber_nud)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 556);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tableLayoutPanel2.Controls.Add(this.resetEEPROM_btn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.SaveEEPROM_btn, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.readEEPROM_btn, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.testString_tbx, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 503);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(894, 50);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // resetEEPROM_btn
            // 
            this.resetEEPROM_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resetEEPROM_btn.Location = new System.Drawing.Point(579, 3);
            this.resetEEPROM_btn.Name = "resetEEPROM_btn";
            this.resetEEPROM_btn.Size = new System.Drawing.Size(109, 44);
            this.resetEEPROM_btn.TabIndex = 0;
            this.resetEEPROM_btn.Text = "Reset";
            this.resetEEPROM_btn.UseVisualStyleBackColor = true;
            this.resetEEPROM_btn.Click += new System.EventHandler(this.resetEEPROM_btn_Click);
            // 
            // SaveEEPROM_btn
            // 
            this.SaveEEPROM_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveEEPROM_btn.Location = new System.Drawing.Point(694, 3);
            this.SaveEEPROM_btn.Name = "SaveEEPROM_btn";
            this.SaveEEPROM_btn.Size = new System.Drawing.Size(96, 44);
            this.SaveEEPROM_btn.TabIndex = 0;
            this.SaveEEPROM_btn.Text = "Save";
            this.SaveEEPROM_btn.UseVisualStyleBackColor = true;
            this.SaveEEPROM_btn.Click += new System.EventHandler(this.SaveEEPROM_btn_Click);
            // 
            // readEEPROM_btn
            // 
            this.readEEPROM_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readEEPROM_btn.Location = new System.Drawing.Point(796, 3);
            this.readEEPROM_btn.Name = "readEEPROM_btn";
            this.readEEPROM_btn.Size = new System.Drawing.Size(95, 44);
            this.readEEPROM_btn.TabIndex = 1;
            this.readEEPROM_btn.Text = "Read";
            this.readEEPROM_btn.UseVisualStyleBackColor = true;
            this.readEEPROM_btn.Click += new System.EventHandler(this.readEEPROM_btn_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.readSingleEEPROM_btn, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.writeSingleEEPROM_btn, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.eepromPageNumber_nud, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(291, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(282, 44);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // readSingleEEPROM_btn
            // 
            this.readSingleEEPROM_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readSingleEEPROM_btn.Location = new System.Drawing.Point(3, 3);
            this.readSingleEEPROM_btn.Name = "readSingleEEPROM_btn";
            this.readSingleEEPROM_btn.Size = new System.Drawing.Size(88, 38);
            this.readSingleEEPROM_btn.TabIndex = 0;
            this.readSingleEEPROM_btn.Text = "Read Single Page";
            this.readSingleEEPROM_btn.UseVisualStyleBackColor = true;
            this.readSingleEEPROM_btn.Click += new System.EventHandler(this.readSingleEEPROM_btn_Click);
            // 
            // writeSingleEEPROM_btn
            // 
            this.writeSingleEEPROM_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.writeSingleEEPROM_btn.Location = new System.Drawing.Point(191, 3);
            this.writeSingleEEPROM_btn.Name = "writeSingleEEPROM_btn";
            this.writeSingleEEPROM_btn.Size = new System.Drawing.Size(88, 38);
            this.writeSingleEEPROM_btn.TabIndex = 0;
            this.writeSingleEEPROM_btn.Text = "Write Single Page";
            this.writeSingleEEPROM_btn.UseVisualStyleBackColor = true;
            this.writeSingleEEPROM_btn.Click += new System.EventHandler(this.writeSingleEEPROM_btn_Click);
            // 
            // eepromPageNumber_nud
            // 
            this.eepromPageNumber_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eepromPageNumber_nud.Location = new System.Drawing.Point(97, 3);
            this.eepromPageNumber_nud.Maximum = new decimal(new int[] {
            252,
            0,
            0,
            0});
            this.eepromPageNumber_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.eepromPageNumber_nud.Name = "eepromPageNumber_nud";
            this.eepromPageNumber_nud.Size = new System.Drawing.Size(88, 20);
            this.eepromPageNumber_nud.TabIndex = 1;
            this.eepromPageNumber_nud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // testString_tbx
            // 
            this.testString_tbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testString_tbx.Location = new System.Drawing.Point(3, 3);
            this.testString_tbx.Multiline = true;
            this.testString_tbx.Name = "testString_tbx";
            this.testString_tbx.Size = new System.Drawing.Size(282, 44);
            this.testString_tbx.TabIndex = 3;
            this.testString_tbx.Text = "This is page one data...Describing all the contents of page one.";
            this.testString_tbx.TextChanged += new System.EventHandler(this.formTextBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(894, 494);
            this.panel1.TabIndex = 1;
            // 
            // EEPROM_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 556);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EEPROM_Window";
            this.Text = "EEPROM_Window";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eepromPageNumber_nud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private INTUSOFT.Custom.Controls.FormButtons resetEEPROM_btn;
        private INTUSOFT.Custom.Controls.FormButtons SaveEEPROM_btn;
        private INTUSOFT.Custom.Controls.FormButtons readEEPROM_btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private INTUSOFT.Custom.Controls.FormButtons readSingleEEPROM_btn;
        private INTUSOFT.Custom.Controls.FormButtons writeSingleEEPROM_btn;
        private System.Windows.Forms.NumericUpDown eepromPageNumber_nud;
        private INTUSOFT.Custom.Controls.FormTextBox testString_tbx;
    }
}