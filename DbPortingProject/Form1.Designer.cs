namespace DBPorting
{
    partial class DbTransferForm
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
            this.insertIntoNewDb_btn = new System.Windows.Forms.Button();
            this.PatientNo_lbl = new System.Windows.Forms.Label();
            this.Patient_pgbar = new System.Windows.Forms.ProgressBar();
            this.InsertPatient_lbl = new System.Windows.Forms.Label();
            this.Visit_pgbar = new System.Windows.Forms.ProgressBar();
            this.NoOfVisit_lbl = new System.Windows.Forms.Label();
            this.InsertVisit_lbl = new System.Windows.Forms.Label();
            this.Observation_pgbar = new System.Windows.Forms.ProgressBar();
            this.NoOfObs_lbl = new System.Windows.Forms.Label();
            this.InsertObs_lbl = new System.Windows.Forms.Label();
            this.Report_pgbar = new System.Windows.Forms.ProgressBar();
            this.NoOfReports_lbl = new System.Windows.Forms.Label();
            this.InsertReports_lbl = new System.Windows.Forms.Label();
            this.Annotation_pgbar = new System.Windows.Forms.ProgressBar();
            this.NoOfAnnotation_lbl = new System.Windows.Forms.Label();
            this.InsertAnnotation_lbl = new System.Windows.Forms.Label();
            this.Patient_grpbx = new System.Windows.Forms.GroupBox();
            this.Visits_gpbx = new System.Windows.Forms.GroupBox();
            this.Images_gpbx = new System.Windows.Forms.GroupBox();
            this.Report_gpbx = new System.Windows.Forms.GroupBox();
            this.Annotations_gpbx = new System.Windows.Forms.GroupBox();
            this.SelectConfig_btn = new System.Windows.Forms.Button();
            this.Patient_grpbx.SuspendLayout();
            this.Visits_gpbx.SuspendLayout();
            this.Images_gpbx.SuspendLayout();
            this.Report_gpbx.SuspendLayout();
            this.Annotations_gpbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // insertIntoNewDb_btn
            // 
            this.insertIntoNewDb_btn.Location = new System.Drawing.Point(206, 15);
            this.insertIntoNewDb_btn.Name = "insertIntoNewDb_btn";
            this.insertIntoNewDb_btn.Size = new System.Drawing.Size(185, 25);
            this.insertIntoNewDb_btn.TabIndex = 9;
            this.insertIntoNewDb_btn.Text = "Insert Old Data Into New DB";
            this.insertIntoNewDb_btn.UseVisualStyleBackColor = true;
            this.insertIntoNewDb_btn.Click += new System.EventHandler(this.insertIntoNewDb_btn_Click);
            // 
            // PatientNo_lbl
            // 
            this.PatientNo_lbl.AutoSize = true;
            this.PatientNo_lbl.Location = new System.Drawing.Point(344, 24);
            this.PatientNo_lbl.Name = "PatientNo_lbl";
            this.PatientNo_lbl.Size = new System.Drawing.Size(13, 13);
            this.PatientNo_lbl.TabIndex = 10;
            this.PatientNo_lbl.Text = "0";
            // 
            // Patient_pgbar
            // 
            this.Patient_pgbar.Location = new System.Drawing.Point(10, 20);
            this.Patient_pgbar.Name = "Patient_pgbar";
            this.Patient_pgbar.Size = new System.Drawing.Size(326, 20);
            this.Patient_pgbar.TabIndex = 11;
            this.Patient_pgbar.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // InsertPatient_lbl
            // 
            this.InsertPatient_lbl.AutoSize = true;
            this.InsertPatient_lbl.Location = new System.Drawing.Point(13, 43);
            this.InsertPatient_lbl.Name = "InsertPatient_lbl";
            this.InsertPatient_lbl.Size = new System.Drawing.Size(40, 13);
            this.InsertPatient_lbl.TabIndex = 10;
            this.InsertPatient_lbl.Text = "Patient";
            // 
            // Visit_pgbar
            // 
            this.Visit_pgbar.Location = new System.Drawing.Point(10, 19);
            this.Visit_pgbar.Name = "Visit_pgbar";
            this.Visit_pgbar.Size = new System.Drawing.Size(323, 21);
            this.Visit_pgbar.TabIndex = 12;
            // 
            // NoOfVisit_lbl
            // 
            this.NoOfVisit_lbl.Location = new System.Drawing.Point(346, 23);
            this.NoOfVisit_lbl.Name = "NoOfVisit_lbl";
            this.NoOfVisit_lbl.Size = new System.Drawing.Size(64, 13);
            this.NoOfVisit_lbl.TabIndex = 13;
            this.NoOfVisit_lbl.Text = "0";
            // 
            // InsertVisit_lbl
            // 
            this.InsertVisit_lbl.Location = new System.Drawing.Point(11, 42);
            this.InsertVisit_lbl.Name = "InsertVisit_lbl";
            this.InsertVisit_lbl.Size = new System.Drawing.Size(145, 13);
            this.InsertVisit_lbl.TabIndex = 14;
            this.InsertVisit_lbl.Text = "Visit";
            // 
            // Observation_pgbar
            // 
            this.Observation_pgbar.Location = new System.Drawing.Point(7, 19);
            this.Observation_pgbar.Name = "Observation_pgbar";
            this.Observation_pgbar.Size = new System.Drawing.Size(326, 22);
            this.Observation_pgbar.TabIndex = 15;
            // 
            // NoOfObs_lbl
            // 
            this.NoOfObs_lbl.Location = new System.Drawing.Point(345, 24);
            this.NoOfObs_lbl.Name = "NoOfObs_lbl";
            this.NoOfObs_lbl.Size = new System.Drawing.Size(64, 13);
            this.NoOfObs_lbl.TabIndex = 16;
            this.NoOfObs_lbl.Text = "0";
            // 
            // InsertObs_lbl
            // 
            this.InsertObs_lbl.Location = new System.Drawing.Point(6, 44);
            this.InsertObs_lbl.Name = "InsertObs_lbl";
            this.InsertObs_lbl.Size = new System.Drawing.Size(239, 13);
            this.InsertObs_lbl.TabIndex = 17;
            this.InsertObs_lbl.Text = "Images";
            // 
            // Report_pgbar
            // 
            this.Report_pgbar.Location = new System.Drawing.Point(6, 19);
            this.Report_pgbar.Name = "Report_pgbar";
            this.Report_pgbar.Size = new System.Drawing.Size(326, 18);
            this.Report_pgbar.TabIndex = 18;
            // 
            // NoOfReports_lbl
            // 
            this.NoOfReports_lbl.Location = new System.Drawing.Point(345, 22);
            this.NoOfReports_lbl.Name = "NoOfReports_lbl";
            this.NoOfReports_lbl.Size = new System.Drawing.Size(64, 13);
            this.NoOfReports_lbl.TabIndex = 19;
            this.NoOfReports_lbl.Text = "0";
            // 
            // InsertReports_lbl
            // 
            this.InsertReports_lbl.Location = new System.Drawing.Point(6, 40);
            this.InsertReports_lbl.Name = "InsertReports_lbl";
            this.InsertReports_lbl.Size = new System.Drawing.Size(205, 13);
            this.InsertReports_lbl.TabIndex = 20;
            this.InsertReports_lbl.Text = "Report";
            // 
            // Annotation_pgbar
            // 
            this.Annotation_pgbar.Location = new System.Drawing.Point(6, 20);
            this.Annotation_pgbar.Name = "Annotation_pgbar";
            this.Annotation_pgbar.Size = new System.Drawing.Size(326, 20);
            this.Annotation_pgbar.TabIndex = 21;
            // 
            // NoOfAnnotation_lbl
            // 
            this.NoOfAnnotation_lbl.Location = new System.Drawing.Point(345, 21);
            this.NoOfAnnotation_lbl.Name = "NoOfAnnotation_lbl";
            this.NoOfAnnotation_lbl.Size = new System.Drawing.Size(64, 13);
            this.NoOfAnnotation_lbl.TabIndex = 22;
            this.NoOfAnnotation_lbl.Text = "0";
            // 
            // InsertAnnotation_lbl
            // 
            this.InsertAnnotation_lbl.Location = new System.Drawing.Point(6, 45);
            this.InsertAnnotation_lbl.Name = "InsertAnnotation_lbl";
            this.InsertAnnotation_lbl.Size = new System.Drawing.Size(297, 13);
            this.InsertAnnotation_lbl.TabIndex = 23;
            this.InsertAnnotation_lbl.Text = "Annotations";
            // 
            // Patient_grpbx
            // 
            this.Patient_grpbx.Controls.Add(this.Patient_pgbar);
            this.Patient_grpbx.Controls.Add(this.PatientNo_lbl);
            this.Patient_grpbx.Controls.Add(this.InsertPatient_lbl);
            this.Patient_grpbx.Location = new System.Drawing.Point(15, 66);
            this.Patient_grpbx.Name = "Patient_grpbx";
            this.Patient_grpbx.Size = new System.Drawing.Size(416, 64);
            this.Patient_grpbx.TabIndex = 24;
            this.Patient_grpbx.TabStop = false;
            this.Patient_grpbx.Text = "Patients Addition";
            // 
            // Visits_gpbx
            // 
            this.Visits_gpbx.Controls.Add(this.Visit_pgbar);
            this.Visits_gpbx.Controls.Add(this.NoOfVisit_lbl);
            this.Visits_gpbx.Controls.Add(this.InsertVisit_lbl);
            this.Visits_gpbx.Location = new System.Drawing.Point(15, 150);
            this.Visits_gpbx.Name = "Visits_gpbx";
            this.Visits_gpbx.Size = new System.Drawing.Size(416, 60);
            this.Visits_gpbx.TabIndex = 25;
            this.Visits_gpbx.TabStop = false;
            this.Visits_gpbx.Text = "Visits Addition";
            // 
            // Images_gpbx
            // 
            this.Images_gpbx.Controls.Add(this.Observation_pgbar);
            this.Images_gpbx.Controls.Add(this.NoOfObs_lbl);
            this.Images_gpbx.Controls.Add(this.InsertObs_lbl);
            this.Images_gpbx.Location = new System.Drawing.Point(16, 228);
            this.Images_gpbx.Name = "Images_gpbx";
            this.Images_gpbx.Size = new System.Drawing.Size(415, 68);
            this.Images_gpbx.TabIndex = 26;
            this.Images_gpbx.TabStop = false;
            this.Images_gpbx.Text = "Images Addition";
            // 
            // Report_gpbx
            // 
            this.Report_gpbx.Controls.Add(this.Report_pgbar);
            this.Report_gpbx.Controls.Add(this.NoOfReports_lbl);
            this.Report_gpbx.Controls.Add(this.InsertReports_lbl);
            this.Report_gpbx.Location = new System.Drawing.Point(16, 315);
            this.Report_gpbx.Name = "Report_gpbx";
            this.Report_gpbx.Size = new System.Drawing.Size(415, 63);
            this.Report_gpbx.TabIndex = 27;
            this.Report_gpbx.TabStop = false;
            this.Report_gpbx.Text = "Report Addition";
            // 
            // Annotations_gpbx
            // 
            this.Annotations_gpbx.Controls.Add(this.Annotation_pgbar);
            this.Annotations_gpbx.Controls.Add(this.NoOfAnnotation_lbl);
            this.Annotations_gpbx.Controls.Add(this.InsertAnnotation_lbl);
            this.Annotations_gpbx.Location = new System.Drawing.Point(16, 402);
            this.Annotations_gpbx.Name = "Annotations_gpbx";
            this.Annotations_gpbx.Size = new System.Drawing.Size(415, 64);
            this.Annotations_gpbx.TabIndex = 28;
            this.Annotations_gpbx.TabStop = false;
            this.Annotations_gpbx.Text = "Annotation Addition";
            // 
            // SelectConfig_btn
            // 
            this.SelectConfig_btn.Location = new System.Drawing.Point(45, 14);
            this.SelectConfig_btn.Name = "SelectConfig_btn";
            this.SelectConfig_btn.Size = new System.Drawing.Size(126, 27);
            this.SelectConfig_btn.TabIndex = 29;
            this.SelectConfig_btn.Text = "Open Config File";
            this.SelectConfig_btn.UseVisualStyleBackColor = true;
            this.SelectConfig_btn.Click += new System.EventHandler(this.SelectConfig_btn_Click);
            // 
            // DbTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 490);
            this.Controls.Add(this.SelectConfig_btn);
            this.Controls.Add(this.Annotations_gpbx);
            this.Controls.Add(this.Report_gpbx);
            this.Controls.Add(this.Images_gpbx);
            this.Controls.Add(this.Visits_gpbx);
            this.Controls.Add(this.Patient_grpbx);
            this.Controls.Add(this.insertIntoNewDb_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DbTransferForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DB Porting Tool";
            this.Load += new System.EventHandler(this.DbTransferForm_Load);
            this.Patient_grpbx.ResumeLayout(false);
            this.Patient_grpbx.PerformLayout();
            this.Visits_gpbx.ResumeLayout(false);
            this.Images_gpbx.ResumeLayout(false);
            this.Report_gpbx.ResumeLayout(false);
            this.Annotations_gpbx.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button insertIntoNewDb_btn;
        private System.Windows.Forms.Label PatientNo_lbl;
        private System.Windows.Forms.ProgressBar Patient_pgbar;
        private System.Windows.Forms.Label InsertPatient_lbl;
        private System.Windows.Forms.ProgressBar Visit_pgbar;
        private System.Windows.Forms.Label NoOfVisit_lbl;
        private System.Windows.Forms.Label InsertVisit_lbl;
        private System.Windows.Forms.ProgressBar Observation_pgbar;
        private System.Windows.Forms.Label NoOfObs_lbl;
        private System.Windows.Forms.Label InsertObs_lbl;
        private System.Windows.Forms.ProgressBar Report_pgbar;
        private System.Windows.Forms.Label NoOfReports_lbl;
        private System.Windows.Forms.Label InsertReports_lbl;
        private System.Windows.Forms.ProgressBar Annotation_pgbar;
        private System.Windows.Forms.Label NoOfAnnotation_lbl;
        private System.Windows.Forms.Label InsertAnnotation_lbl;
        private System.Windows.Forms.GroupBox Patient_grpbx;
        private System.Windows.Forms.GroupBox Visits_gpbx;
        private System.Windows.Forms.GroupBox Images_gpbx;
        private System.Windows.Forms.GroupBox Report_gpbx;
        private System.Windows.Forms.GroupBox Annotations_gpbx;
        private System.Windows.Forms.Button SelectConfig_btn;
    }
}

