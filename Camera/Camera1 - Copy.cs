using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
namespace CameraModule
{
    class Camera
    {
        public  PictureBox image_pbx;
        static  Camera  camera;
        ToupCam myCamera;
        Bitmap liveBm;
        Bitmap captureBm;
       public  IntPtr camHandle;
        private uint MSG_CAMEVENT = 0x8001; // WM_APP = 0x8000

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
         void WndProc(ref Message m)
        {
            if (MSG_CAMEVENT == m.Msg)
            {
                switch (m.WParam.ToInt32())
                {
                    case ToupCam.EVENT_ERROR:
                       // OnEventError();
                        break;
                    case ToupCam.EVENT_EXPOSURE:
                       // OnEventExposure();
                        break;
                    case ToupCam.EVENT_IMAGE:
                        OnEventImage();
                        break;
                    case ToupCam.EVENT_STILLIMAGE:
                        capture();
                        break;
                }
                return;
            }
        }

        public Camera()
        {
            ToupCam.Instance[] arr = ToupCam.Enum();
            if (arr.Length < 0)
                MessageBox.Show("No Device");
            else
            {
                myCamera = new ToupCam();
                image_pbx = new PictureBox();

                if (!myCamera.Open(arr[0].id))
                {
                    myCamera = null;
                }
            }
        }

        public bool stopLiveMode()
        {
          return  myCamera.Stop();
        }
        
        public static  Camera createCamera()
        {
            if (camera == null)
            {
                camera = new Camera();
            
            }
            return camera;
        }

        public bool setResolution(int mode)
        {
          bool ret =  myCamera.put_eSize((uint)mode);
          return ret;
        }

        public bool CloseCamera()
        {
            try
            {
                stopLiveMode();
                myCamera.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void OnEventImage()
        {


            BitmapData bdata = liveBm.LockBits(new Rectangle(0,0,width,height),ImageLockMode.ReadWrite,PixelFormat.Format24bppRgb);
            uint bmWidth =0;
            uint bmHeight =0;
           bool retVal= myCamera.PullImage(bdata.Scan0, 24, out bmWidth, out bmHeight);
           liveBm.UnlockBits(bdata);
           image_pbx.Image = liveBm;

        }

        public void callWndProc(ref System.Windows.Forms.Message m)
        {
            WndProc(ref m);
        }
        uint eSize = 0;
        int width = 0, height = 0;
        public bool startLiveMode()
        {
            uint resnum = myCamera.ResolutionNumber;
            if (myCamera.get_eSize(out eSize))
            {


                if (myCamera.get_Size(out width, out height))
                {
                    if (liveBm != null)
                        liveBm.Dispose();
                    liveBm = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    if (!myCamera.StartPullModeWithWndMsg(camHandle, MSG_CAMEVENT))
                    {
                        MessageBox.Show("failed to start device");
                        return false;
                    }
                    else
                        return true;
                }
                else
                   return false;
            }
            else
                return false;
        }

        public void capture()
        {
       //  stopLiveMode();
         //setResolution(1);
         captureBm = new Bitmap(2048, 1536);
         BitmapData bData =   captureBm.LockBits(new Rectangle(0,0,width,height),ImageLockMode.ReadWrite,PixelFormat.Format24bppRgb);
         uint cWidth = 0;
         uint cHeight = 0;
         myCamera.PullStillImage(bData.Scan0, 24, out  cWidth, out cHeight);
         captureBm.UnlockBits(bData);
         stopLiveMode();
         captureBm.Save(FileName);
         captureBm.Dispose();

        }
        string FileName = "";
        public void onEventStillImage(string fileName)
        {
            myCamera.Snap(0);
            FileName = fileName;
        }
    }
}
 