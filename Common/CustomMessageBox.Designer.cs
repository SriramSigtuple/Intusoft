namespace Common
{
    partial class CustomMessageBox
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
            this.components = new System.ComponentModel.Container();
            this.pnlShowMessage = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttons_tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.information_tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMessageText = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlShowMessage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.information_tableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlShowMessage
            // 
            this.pnlShowMessage.Controls.Add(this.tableLayoutPanel1);
            this.pnlShowMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlShowMessage.Location = new System.Drawing.Point(0, 0);
            this.pnlShowMessage.Name = "pnlShowMessage";
            this.pnlShowMessage.Size = new System.Drawing.Size(426, 97);
            this.pnlShowMessage.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.buttons_tableLayout, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.information_tableLayout, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 97);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttons_tableLayout
            // 
            this.buttons_tableLayout.ColumnCount = 5;
            this.buttons_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.buttons_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.buttons_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.buttons_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.buttons_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.buttons_tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttons_tableLayout.Location = new System.Drawing.Point(3, 59);
            this.buttons_tableLayout.Name = "buttons_tableLayout";
            this.buttons_tableLayout.RowCount = 1;
            this.buttons_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttons_tableLayout.Size = new System.Drawing.Size(420, 35);
            this.buttons_tableLayout.TabIndex = 0;
            // 
            // information_tableLayout
            // 
            this.information_tableLayout.ColumnCount = 2;
            this.information_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.17241F));
            this.information_tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.82758F));
            this.information_tableLayout.Controls.Add(this.pictureBox1, 0, 0);
            this.information_tableLayout.Controls.Add(this.panel2, 1, 0);
            this.information_tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.information_tableLayout.Location = new System.Drawing.Point(3, 3);
            this.information_tableLayout.Name = "information_tableLayout";
            this.information_tableLayout.RowCount = 1;
            this.information_tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.information_tableLayout.Size = new System.Drawing.Size(420, 50);
            this.information_tableLayout.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblMessageText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(66, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(351, 44);
            this.panel2.TabIndex = 1;
            // 
            // lblMessageText
            // 
            this.lblMessageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessageText.Location = new System.Drawing.Point(0, 0);
            this.lblMessageText.Name = "lblMessageText";
            this.lblMessageText.Size = new System.Drawing.Size(351, 44);
            this.lblMessageText.TabIndex = 3;
            this.lblMessageText.Text = "label1";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CustomMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(426, 97);
            this.Controls.Add(this.pnlShowMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CustomMessageBox";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomMessageBox_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CustomMessageBox_KeyUp);
            this.pnlShowMessage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.information_tableLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShowMessage;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel buttons_tableLayout;
        private System.Windows.Forms.TableLayoutPanel information_tableLayout;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblMessageText;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}