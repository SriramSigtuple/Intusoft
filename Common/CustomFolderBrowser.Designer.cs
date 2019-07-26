namespace Common
{
    partial class CustomFolderBrowser
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
            this.main_tbpnl = new System.Windows.Forms.TableLayoutPanel();
            this.Paths_tbpnl = new System.Windows.Forms.TableLayoutPanel();
            this.folder_lbl = new System.Windows.Forms.Label();
            this.folder_tbx = new System.Windows.Forms.TextBox();
            this.browseFolders_btn = new System.Windows.Forms.Button();
            this.compressionRatio_lbl = new System.Windows.Forms.Label();
            this.compressionRatio_cbx = new System.Windows.Forms.ComboBox();
            this.suggestion_lbl = new System.Windows.Forms.Label();
            this.imageFormat_lbl = new System.Windows.Forms.Label();
            this.imageFbormat_cbx = new System.Windows.Forms.ComboBox();
            this.fileName_lbl = new System.Windows.Forms.Label();
            this.fileName_tbx = new System.Windows.Forms.TextBox();
            this.example_lbl = new System.Windows.Forms.Label();
            this.controls_tbpnl = new System.Windows.Forms.TableLayoutPanel();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.ok_btn = new System.Windows.Forms.Button();
            this.OpenFile_cbx = new System.Windows.Forms.CheckBox();
            this.OpenFolderLocation_cbx = new System.Windows.Forms.CheckBox();
            this.main_tbpnl.SuspendLayout();
            this.Paths_tbpnl.SuspendLayout();
            this.controls_tbpnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_tbpnl
            // 
            this.main_tbpnl.ColumnCount = 1;
            this.main_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.main_tbpnl.Controls.Add(this.Paths_tbpnl, 0, 0);
            this.main_tbpnl.Controls.Add(this.controls_tbpnl, 0, 1);
            this.main_tbpnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_tbpnl.Location = new System.Drawing.Point(0, 0);
            this.main_tbpnl.Name = "main_tbpnl";
            this.main_tbpnl.RowCount = 2;
            this.main_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.59563F));
            this.main_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.40437F));
            this.main_tbpnl.Size = new System.Drawing.Size(455, 183);
            this.main_tbpnl.TabIndex = 2;
            // 
            // Paths_tbpnl
            // 
            this.Paths_tbpnl.ColumnCount = 3;
            this.Paths_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.9755F));
            this.Paths_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.74387F));
            this.Paths_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.05791F));
            this.Paths_tbpnl.Controls.Add(this.folder_lbl, 0, 0);
            this.Paths_tbpnl.Controls.Add(this.folder_tbx, 1, 0);
            this.Paths_tbpnl.Controls.Add(this.browseFolders_btn, 2, 0);
            this.Paths_tbpnl.Controls.Add(this.compressionRatio_lbl, 0, 3);
            this.Paths_tbpnl.Controls.Add(this.compressionRatio_cbx, 1, 3);
            this.Paths_tbpnl.Controls.Add(this.suggestion_lbl, 2, 3);
            this.Paths_tbpnl.Controls.Add(this.imageFormat_lbl, 0, 2);
            this.Paths_tbpnl.Controls.Add(this.imageFbormat_cbx, 1, 2);
            this.Paths_tbpnl.Controls.Add(this.fileName_lbl, 0, 1);
            this.Paths_tbpnl.Controls.Add(this.fileName_tbx, 1, 1);
            this.Paths_tbpnl.Controls.Add(this.example_lbl, 2, 1);
            this.Paths_tbpnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Paths_tbpnl.Location = new System.Drawing.Point(3, 3);
            this.Paths_tbpnl.Name = "Paths_tbpnl";
            this.Paths_tbpnl.RowCount = 4;
            this.Paths_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Paths_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Paths_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Paths_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Paths_tbpnl.Size = new System.Drawing.Size(449, 135);
            this.Paths_tbpnl.TabIndex = 0;
            // 
            // folder_lbl
            // 
            this.folder_lbl.AutoSize = true;
            this.folder_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folder_lbl.Location = new System.Drawing.Point(3, 0);
            this.folder_lbl.Name = "folder_lbl";
            this.folder_lbl.Size = new System.Drawing.Size(169, 33);
            this.folder_lbl.TabIndex = 0;
            this.folder_lbl.Text = "Label1";
            // 
            // folder_tbx
            // 
            this.folder_tbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folder_tbx.Location = new System.Drawing.Point(178, 3);
            this.folder_tbx.Name = "folder_tbx";
            this.folder_tbx.Size = new System.Drawing.Size(150, 20);
            this.folder_tbx.TabIndex = 4;
            this.folder_tbx.TextChanged += new System.EventHandler(this.folder_tbx_TextChanged);
            // 
            // browseFolders_btn
            // 
            this.browseFolders_btn.Dock = System.Windows.Forms.DockStyle.Left;
            this.browseFolders_btn.Location = new System.Drawing.Point(334, 3);
            this.browseFolders_btn.Name = "browseFolders_btn";
            this.browseFolders_btn.Size = new System.Drawing.Size(85, 27);
            this.browseFolders_btn.TabIndex = 5;
            this.browseFolders_btn.Text = "Label1";
            this.browseFolders_btn.UseVisualStyleBackColor = true;
            this.browseFolders_btn.Click += new System.EventHandler(this.BrowseFolders_btn_Click);
            // 
            // compressionRatio_lbl
            // 
            this.compressionRatio_lbl.AutoSize = true;
            this.compressionRatio_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compressionRatio_lbl.Location = new System.Drawing.Point(3, 99);
            this.compressionRatio_lbl.Name = "compressionRatio_lbl";
            this.compressionRatio_lbl.Size = new System.Drawing.Size(169, 36);
            this.compressionRatio_lbl.TabIndex = 2;
            this.compressionRatio_lbl.Text = "Label1";
            // 
            // compressionRatio_cbx
            // 
            this.compressionRatio_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compressionRatio_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.compressionRatio_cbx.FormattingEnabled = true;
            this.compressionRatio_cbx.Location = new System.Drawing.Point(178, 102);
            this.compressionRatio_cbx.Name = "compressionRatio_cbx";
            this.compressionRatio_cbx.Size = new System.Drawing.Size(150, 21);
            this.compressionRatio_cbx.TabIndex = 7;
            this.compressionRatio_cbx.SelectedIndexChanged += new System.EventHandler(this.compressionRatio_cbx_SelectedIndexChanged);
            // 
            // suggestion_lbl
            // 
            this.suggestion_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.suggestion_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suggestion_lbl.Location = new System.Drawing.Point(334, 99);
            this.suggestion_lbl.Name = "suggestion_lbl";
            this.suggestion_lbl.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.suggestion_lbl.Size = new System.Drawing.Size(112, 36);
            this.suggestion_lbl.TabIndex = 3;
            this.suggestion_lbl.Text = "Label1";
            // 
            // imageFormat_lbl
            // 
            this.imageFormat_lbl.AutoSize = true;
            this.imageFormat_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageFormat_lbl.Location = new System.Drawing.Point(3, 66);
            this.imageFormat_lbl.Name = "imageFormat_lbl";
            this.imageFormat_lbl.Size = new System.Drawing.Size(169, 33);
            this.imageFormat_lbl.TabIndex = 1;
            this.imageFormat_lbl.Text = "Label1";
            // 
            // imageFbormat_cbx
            // 
            this.imageFbormat_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageFbormat_cbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageFbormat_cbx.FormattingEnabled = true;
            this.imageFbormat_cbx.Location = new System.Drawing.Point(178, 69);
            this.imageFbormat_cbx.Name = "imageFbormat_cbx";
            this.imageFbormat_cbx.Size = new System.Drawing.Size(150, 21);
            this.imageFbormat_cbx.TabIndex = 6;
            this.imageFbormat_cbx.SelectedIndexChanged += new System.EventHandler(this.imageFbormat_cbx_SelectedIndexChanged);
            // 
            // fileName_lbl
            // 
            this.fileName_lbl.AutoSize = true;
            this.fileName_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileName_lbl.Location = new System.Drawing.Point(3, 33);
            this.fileName_lbl.Name = "fileName_lbl";
            this.fileName_lbl.Size = new System.Drawing.Size(169, 33);
            this.fileName_lbl.TabIndex = 8;
            this.fileName_lbl.Text = "label1";
            // 
            // fileName_tbx
            // 
            this.fileName_tbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileName_tbx.Location = new System.Drawing.Point(178, 36);
            this.fileName_tbx.Name = "fileName_tbx";
            this.fileName_tbx.Size = new System.Drawing.Size(150, 20);
            this.fileName_tbx.TabIndex = 9;
            this.fileName_tbx.TextChanged += new System.EventHandler(this.fileName_tbx_TextChanged);
            // 
            // example_lbl
            // 
            this.example_lbl.AutoSize = true;
            this.example_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.example_lbl.Location = new System.Drawing.Point(334, 33);
            this.example_lbl.Name = "example_lbl";
            this.example_lbl.Size = new System.Drawing.Size(112, 33);
            this.example_lbl.TabIndex = 10;
            this.example_lbl.Text = "label1";
            this.example_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // controls_tbpnl
            // 
            this.controls_tbpnl.ColumnCount = 5;
            this.controls_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.51225F));
            this.controls_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.05791F));
            this.controls_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.63344F));
            this.controls_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.807826F));
            this.controls_tbpnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.84219F));
            this.controls_tbpnl.Controls.Add(this.cancel_btn, 4, 0);
            this.controls_tbpnl.Controls.Add(this.ok_btn, 2, 0);
            this.controls_tbpnl.Controls.Add(this.OpenFile_cbx, 1, 0);
            this.controls_tbpnl.Controls.Add(this.OpenFolderLocation_cbx, 0, 0);
            this.controls_tbpnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controls_tbpnl.Location = new System.Drawing.Point(3, 144);
            this.controls_tbpnl.Name = "controls_tbpnl";
            this.controls_tbpnl.RowCount = 1;
            this.controls_tbpnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controls_tbpnl.Size = new System.Drawing.Size(449, 36);
            this.controls_tbpnl.TabIndex = 1;
            // 
            // cancel_btn
            // 
            this.cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancel_btn.Location = new System.Drawing.Point(366, 3);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(80, 30);
            this.cancel_btn.TabIndex = 7;
            this.cancel_btn.Text = "Label1";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // ok_btn
            // 
            this.ok_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ok_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ok_btn.Location = new System.Drawing.Point(257, 3);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(77, 30);
            this.ok_btn.TabIndex = 8;
            this.ok_btn.Text = "Label1";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // OpenFile_cbx
            // 
            this.OpenFile_cbx.AutoSize = true;
            this.OpenFile_cbx.Location = new System.Drawing.Point(140, 3);
            this.OpenFile_cbx.Name = "OpenFile_cbx";
            this.OpenFile_cbx.Size = new System.Drawing.Size(58, 17);
            this.OpenFile_cbx.TabIndex = 9;
            this.OpenFile_cbx.Text = "Label1";
            this.OpenFile_cbx.UseVisualStyleBackColor = true;
            // 
            // OpenFolderLocation_cbx
            // 
            this.OpenFolderLocation_cbx.AutoSize = true;
            this.OpenFolderLocation_cbx.Location = new System.Drawing.Point(3, 3);
            this.OpenFolderLocation_cbx.Name = "OpenFolderLocation_cbx";
            this.OpenFolderLocation_cbx.Size = new System.Drawing.Size(58, 17);
            this.OpenFolderLocation_cbx.TabIndex = 9;
            this.OpenFolderLocation_cbx.Text = "Label1";
            this.OpenFolderLocation_cbx.UseVisualStyleBackColor = true;
            // 
            // CustomFolderBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 183);
            this.Controls.Add(this.main_tbpnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomFolderBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomFolderBrowser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomFolderBrowser_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CustomFolderBrowser_FormClosed);
            this.Shown += new System.EventHandler(this.CustomFolderBrowser_Shown);
            this.main_tbpnl.ResumeLayout(false);
            this.Paths_tbpnl.ResumeLayout(false);
            this.Paths_tbpnl.PerformLayout();
            this.controls_tbpnl.ResumeLayout(false);
            this.controls_tbpnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel main_tbpnl;
        private System.Windows.Forms.TableLayoutPanel Paths_tbpnl;
        private System.Windows.Forms.Label folder_lbl;
        private System.Windows.Forms.Label imageFormat_lbl;
        private System.Windows.Forms.Label compressionRatio_lbl;
        private System.Windows.Forms.Label suggestion_lbl;
        private System.Windows.Forms.TextBox folder_tbx;
        private System.Windows.Forms.Button browseFolders_btn;
        private System.Windows.Forms.ComboBox imageFbormat_cbx;
        private System.Windows.Forms.ComboBox compressionRatio_cbx;
        private System.Windows.Forms.TableLayoutPanel controls_tbpnl;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Label fileName_lbl;
        private System.Windows.Forms.TextBox fileName_tbx;
        private System.Windows.Forms.Label example_lbl;
        private System.Windows.Forms.CheckBox OpenFile_cbx;
        private System.Windows.Forms.CheckBox OpenFolderLocation_cbx;
    }
}