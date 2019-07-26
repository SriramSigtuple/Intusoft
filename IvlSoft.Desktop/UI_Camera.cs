using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
namespace Ivlsoft.Desktop.Forms.CameraUI
{
    public partial class UI_Camera : UserControl
    {
    //    Camera ivl_camera;
    //    Timer t = new Timer();
    //    DirectoryInfo dInf,saveDinf;
    //    string SaveDirectoryPath = @"C:\ToupCam\Images";
    //    static Bitmap srcImg,cameraConnectImg,cameraDisconnectImg;
    //    bool isCameraOpen = false;
    //    int FrameRate = 0;
    //    int tempVal = 6500;
    //    int tintVal = 1000;
    //    ushort minGVal = 0, maxGVal = 0, defGVal = 0;
    //    uint minEVal = 0, maxEVal = 0, defEVal = 0;
    //    string titleText = "";
    //    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    //    {
    //        if (keyData == Keys.Space)
    //        {
    //            // Alt+F pressed

    //            if (isCameraOpen)
    //            {
    //                this.Cursor = Cursors.WaitCursor;
    //                saveFrames();
    //            }
    //        }
    //        else if (keyData == Keys.Right)
    //            next_btn_Click(null, null);
    //        else if (keyData == Keys.Left)
    //            previous_btn_Click(null, null);

    //        return base.ProcessCmdKey(ref msg, keyData);
    //    public UI_Camera()
    //    {
    //        InitializeComponent();
    //    }
    //}
}


    public class FormButtons : Button
    {
        public FormButtons()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
    public class FormCheckBox : CheckBox
    {
        public FormCheckBox()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

}
