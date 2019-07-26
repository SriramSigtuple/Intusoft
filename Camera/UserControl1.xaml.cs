using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace CameraModule
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
    //    string fileName = "";
    //    DirectoryInfo saveFrameDinf;
    //    FileInfo[] saveFrameFinf;
    //    Camera ivl_camera;
    //    DirectoryInfo dInf, saveDinf;
    //    string SaveDirectoryPath = @"C:\ToupCam\Images";
    //    static BitmapImage srcImg, cameraConnectImg, cameraDisconnectImg;
    //    bool isCameraOpen = false;
    //    int FrameRate = 0;
    //    int tempVal = 6500;
    //    int tintVal = 1000;
    //    ushort minGVal = 0, maxGVal = 0, defGVal = 0;
    //    uint minEVal = 0, maxEVal = 0, defEVal = 0;
    //    string titleText = "";
        public UserControl1()
        {
            InitializeComponent();
        }

    //    private void connectCamera()
    //    {
    //        if (!isCameraOpen)
    //        {
    //            #region toupCam
    //            ivl_camera = Camera.createCamera();
    //            if (ivl_camera != null)
    //            {
    //                //ivl_camera.statusBarUpdate;
    //                //ivl_camera.FrameTransferStatus += ivl_camera_FrameTransferStatus;
    //                //ivl_camera.FrameCaptured += ivl_camera_FrameCaptured;
    //               // ivl_camera.image_pbx = display_pbx;
    //                //ivl_camera.camHandle = this.Handle;
    //                //ivl_camera.FrameRate_lbl = this.FrameRate_lbl;
    //                //ivl_camera.FrameTransfer_lbl = this.FrameStatus_lbl;


    //            #endregion
    //                isCameraOpen = true;

    //                if (!Directory.Exists(dInf.FullName + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy")))
    //                    saveDinf = Directory.CreateDirectory(dInf.FullName + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy"));
    //                else
    //                    saveDinf = new DirectoryInfo(dInf.FullName + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy"));

    //                //EnableControls(true);
    //                //cameraStatus_lbl.Image = cameraConnectImg;
    //                Dictionary<int, int> widthHeight = new Dictionary<int, int>();
    //                IntucamBoardCommHelper.Open();
    //                ivl_camera.GetResolution(ref widthHeight);
    //                //foreach (var item in widthHeight)
    //                //    resolution_combx.Items.Add(item.Key.ToString() + "X" + item.Value.ToString());
    //                ivl_camera.SetFrameRateLevel(0);

    //               // resolution_combx.SelectedIndex = 0;
    //                //t.Enabled = true;
    //                //t.Start();
    //            }

    //        }
    //        else
    //        {
    //            isCameraOpen = false;

    //           // EnableControls(false);
    //           // cameraStatus_lbl.Image = cameraDisconnectImg;
    //            //resolution_combx.Items.Clear();
    //            //t.Enabled = false;
    //            //t.Stop();
    //            ivl_camera.StopLiveMode();
    //            IntucamBoardCommHelper.Close();
    //        }
          
    //    }
    //    private void SetCameraSettings()
    //    {
    //        if (ivl_camera != null)
    //        {
    //            ivl_camera.StopLiveMode();
    //            ivl_camera.setResolution(0);
    //            ivl_camera.GetExposureRange(ref minEVal, ref maxEVal, ref defEVal);
    //            ivl_camera.SetExposure((uint)77000);
    //            ivl_camera.SetGain(200);
    //            ivl_camera.StartLiveMode();
    //            IntucamBoardCommHelper.IRLightOn();

    //        }
    //    }
    //    private void SaveSettingsLog()
    //    {
    //        string[] logFileNameArr = fileName.Split('.');
    //        StreamWriter st = new StreamWriter(logFileNameArr[0] + "_Settings.log");
    //        //st.WriteLine("Exposure =" + ivl_camera);
    //        //st.WriteLine("Gain =" + ivl_camera.ge.ToString());
    //        //st.WriteLine("Brightness =" + brightnessVal_tb.Value.ToString());
    //        //st.WriteLine("Contrast =" + contrastVal_tb.Value.ToString());
    //        //st.WriteLine("Hue =" + hueVal_tb.Value.ToString());
    //        //st.WriteLine("Saturation =" + saturationVal_tb.Value.ToString());
    //        //st.WriteLine("Gamma =" + gammaVal_tb.Value.ToString());
    //        //st.WriteLine("Temperature =" + temperature_tb.Value.ToString());
    //        //st.WriteLine("Tint =" + tint_tb.Value.ToString());
    //        st.Close();
    //        st.Dispose();

    //    }

        private void capture_btn_Click(object sender, RoutedEventArgs e)
        {
    //        if (capture_btn.Content == "Capture")
    //        {

    //            fileName = saveDinf.FullName + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("HHmmss") + ".png";

    //            ivl_camera.onEventStillImage(fileName);
    //            SaveSettingsLog();

    //            capture_btn.Content = "Resume";
    //        }
    //        else
    //        {
    //            capture_btn.Content = "Capture";

    //            ivl_camera.setResolution(0);
    //            ivl_camera.StartLiveMode();

    //        }

        }

        private void UserControl_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                // Alt+F pressed

                //if (isCameraOpen)
                //{
                //    this.Cursor = Cursors.Wait;
                //    saveFrames();
                //}
            }
            else if (e.Key == Key.Right)
                next_btn_Click_1(null, null);
            else if (e.Key == Key.Left)
                previous_btn_Click_1(null, null);
        }
    //    private void saveFrames()
    //    {
    //        if (capture_btn.Content == "Capture")
    //        {
    //            string fileDir = saveDinf.FullName + System.IO.Path.DirectorySeparatorChar + "save_" + DateTime.Now.ToString("HHmmss");
    //            if (!Directory.Exists(fileDir))
    //                saveFrameDinf = Directory.CreateDirectory(fileDir);
    //            Camera.FileName = fileDir;
    //            // ivl_camera.SetExposure(47000);
    //            //  ivl_camera.StopLiveMode();
    //            System.Threading.Thread.Sleep(30);
    //            // ivl_camera.StartLiveMode();
    //            IntucamBoardCommHelper.VSync();
    //            Camera.saveMultipleFrames = true;
    //            // System.Threading.Thread.Sleep(200);

    //            //  ivl_camera.Capture();
    //            //  ivl_camera.Capture();
    //            //  ivl_camera.Capture();
    //            //ivl_camera.saveImageFramesList();
    //        }
    //        else
    //        {

    //            IntucamBoardCommHelper.ResetVSync();
    //            ivl_camera.StartLiveMode();
    //            ivl_camera.SetExposure((uint)77350);

    //            IntucamBoardCommHelper.IRLightOn();


    //            next_btn.IsTabStop = false;
    //            previous_btn.IsTabStop = false;
    //            next_btn.IsEnabled = false;
    //            previous_btn.IsEnabled = false;

    //            this.Cursor = Cursors.Arrow;
    //            return;
    //        }


    //    }

        private void browse_btn_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void previous_btn_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void next_btn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
