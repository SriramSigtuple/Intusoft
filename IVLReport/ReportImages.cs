using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReportUtils;
using INTUSOFT.EventHandler;
using INTUSOFT.ThumbnailModule;
using INTUSOFT.Custom.Controls;
using Common;
using System.IO;
namespace IVLReport
{
    public partial class ReportImages : BaseGradientForm
    {
        public bool isOk = false;
        public static string reportImageText = string.Empty;
        public static string reportImageSizeText = string.Empty;
        public static string reportImageOkbtnText = string.Empty;
        public static string reportImageMsgText = string.Empty;
        public static string reportImageHeaderText = "Report Images";
        IVLEventHandler _eventhandler;
        private DataModel _dataModel;

        public ReportImages(Dictionary<string,object> reportData)
        {
            InitializeComponent();
            _dataModel = DataModel.GetInstance();
            //This Icon has been added by darshan on 13-08-2015 to resolve defect no 0000554: Report icon to be changed as in the Logo of Intuvision.
            string appLogoFilePath = @"ImageResources\LogoImageResources\ReportLogo.ico";
            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);
            if (!string.IsNullOrEmpty(reportImageText))
                this.Text = reportImageText;
            if (!string.IsNullOrEmpty(reportImageSizeText))
                Trackbar_lbl.Text = reportImageSizeText;
            if (!string.IsNullOrEmpty(reportImageOkbtnText))
                button1.Text = reportImageOkbtnText;
            if (reportData.ContainsKey("Color1"))
                this.Color1 = (Color)reportData["Color1"];
            if (reportData.ContainsKey("Color2"))
                this.Color2 = (Color)reportData["Color2"];
            if (reportData.ContainsKey("ForeColor"))
                this.ForeColor = (Color)reportData["ForeColor"];
            if (reportData.ContainsKey("ColorAngle"))
                this.ColorAngle = (float)reportData["ColorAngle"];
            this.button1.ForeColor = Color.Black;
            this.button1.BackColor = Color.Transparent;
            _eventhandler = IVLEventHandler.getInstance();
            _eventhandler.Register(_eventhandler.ReportImagesIsShiftControl, new NotificationHandler(reportImagesIsShiftControl));
        }

        private void LoadThumbnailImagesToReportImages()
        {
            this.isOk = false;
            this.thumbnailUI1.NoOfImagesToBeSelected = _dataModel.NoOfImagesAllowed;
            this.thumbnailUI1.NoOfImagesToBeSelectedText1 = _dataModel.NoOfImagesAllowedText1;
            this.thumbnailUI1.NoOfImagesToBeSelectedText2 = _dataModel.NoOfImagesAllowedText2;
            this.thumbnailUI1.NoOfImagesToBeSelectedHeader = _dataModel.NoOfImagesAllowedHeader;
            this.thumbnailUI1.ResetThumbnailUI();
            this.thumbnailUI1.AddThumbnails(_dataModel.VisitImageFiles.ToList(), _dataModel.VisitImageIds.ToList(), _dataModel.VisitImagesides.ToList(),_dataModel.isannotated.ToList(),_dataModel.isCDR.ToList());

            if (_dataModel.CurrentImgFiles.Length > 1)//to ensure the control key is pressed.
                this.thumbnailUI1.isControlKeyPressed = true;

            foreach (string item in _dataModel.CurrentImgFiles)
            {
                this.thumbnailUI1.ThumbnailSelected(item);
            }
            this.thumbnailUI1.isControlKeyPressed = false;
            this.trackBar1.Value = 3;
            ThumbnailChangeSize();
        }

        public void ThumbnailChangeSize()
        {
            this.thumbnailUI1.sizeChanged = this.trackBar1.Value;
            this.thumbnailUI1.SizeChanged();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_dataModel.VisitImageFiles.Count() != 0)
            {
                ThumbnailChangeSize();
            }
        }

        public Dictionary<string,List<string>> GetFileNames()
        {
            Dictionary<string,List<string>> LOC=  this.thumbnailUI1.getImageFiles();
            
            return LOC;
        }

        private void ReportImages_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:

                    {
                        this.thumbnailUI1.isShiftKeyPressed = true;
                        break;
                    }
                case Keys.ControlKey:
                    {
                        this.thumbnailUI1.isControlKeyPressed = true;
                        break;
                    }
             }
        }

        public void reportImagesIsShiftControl(string s, Args arg)
        {
            KeyEventArgs e = arg["keyArg"] as KeyEventArgs;
            if ((bool)arg["isKeyUp"])
                ReportImages_KeyUp(null, e);
            //else
            //    ReportImages_KeyDown(null, e);
        }

        private void ReportImages_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    {
                        this.thumbnailUI1.isShiftKeyPressed = false;
                        break;
                    }
                case Keys.ControlKey:
                    {
                        this.thumbnailUI1.isControlKeyPressed = false;
                        break;
                    }
             }
        }

        /// <summary>
        /// to check the number of images selected
        /// </summary>
        private void CheckNumberOfSelectedImages()
        {
            //This below code has been added by darshan to solve the ambiguty in report section on Date 13-08-2015
            Dictionary<string, List<string>> file_imagenames = GetFileNames();
            string[] files = file_imagenames["FileNames"].ToArray();
            if (files.Count() == 0)
            {
                DialogResult result = CustomMessageBox.Show(reportImageMsgText, reportImageHeaderText, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
                //if (result == DialogResult.Yes)
                //{
                //    this.isOk = true;
                //    this.Close();
                //}
                //else
                //{
                //    return;
                //}

                this.isOk = false;
            }
            else if(files.Count() > _dataModel.NoOfImagesAllowed)
            {
                DialogResult result = CustomMessageBox.Show(_dataModel.NoOfImagesAllowedText1 + " " + _dataModel.NoOfImagesAllowed.ToString() + " " + _dataModel.NoOfImagesAllowedText2, _dataModel.NoOfImagesAllowedHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
                this.isOk = false;
            }
            else
                this.isOk = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CheckNumberOfSelectedImages();
            if (this.isOk)
                this.Close();
        }

        private void Trackbar_lbl_Click(object sender, EventArgs e)
        {

        }

        private void ReportImages_Load(object sender, EventArgs e)
        {
            LoadThumbnailImagesToReportImages();//this code has been added to give image details to the thumbnail when report image window is opened
        }
    }
}
