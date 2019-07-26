using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace INTUSOFT.EventHandler
{
	public delegate void NotificationHandler(String n, Args args);
	public class IVLEventHandlerException : Exception {
		public String error;
        public IVLEventHandlerException(String s)
        {
			error = s;
		}
	}
	public class IVLEventHandler
	{

		#region LOGIN ---------------------------------------
        public String LOGIN_SUCCESS = "LOGIN_SUCCESS";
		#endregion

        private class Handlers : List<NotificationHandler> {};
		private class Registry : Dictionary<String, Handlers> {};
		private Registry reg = new Registry();

		#region SINGLETON -----------------------
        private static IVLEventHandler _instance = null;
        public static IVLEventHandler getInstance()
        {
			if (_instance == null)
                _instance = new IVLEventHandler();
			return _instance;
		}
		#endregion

        public string ConnectCamera = "ConnectCamera";
        public string DisconnectCamera = "DisconnectCamera";
        public string EnableStatusBar = "EnableStatusBar";
        public string UpdateStatusBar = "UpdateStatusBar";// update status bar event has been added but not used by sriram on september 9th 2015
        #region Defect number 0000581 has been fixed by calling the same function as in the case of space bar by sriram on August 18th 2015
        public string CaptureScreenUpdate = "CaptureScreenUpdate"; 
		#endregion
        public String UPDATE_CAMERA_STATUS = "UPDATE_CAMERA_STATUS";
        public String UPDATE_POWER_STATUS = "UPDATE_POWER_STATUS";
        public String EXIT_CAMERA = "EXIT_CAMERA";
        public string FrameCaptured = "FrameCaptured";
        public string FrameRateStatusUpdate = "FrameRateStatusUpdate";
        public string Save_Image = "Save_Image";
        public string Consultation2Imaging = "Consultation2Imaging";
        public string ShowThumbnails = "ShowThumbnails";
        public string ThumbnailSelected = "ThumbnailSelected";
        public string ImageUrlToDb = "ImageUrlToDb";
        public string ThumbnailAdd = "ThumbnailAdd";
        public string CameraUIShown = "CameraUIShown";
        public string SetLeftRightDetailsToDb = "SetLeftRightDetailsToDb";
        public string SetActivePatDetails = "SetActivePatDetails";
        public string Back2Search = "Back2Search";
        public string Navigate2ViewImageScreen = "Navigate2ViewImageScreen";
        public string Navigate2LiveScreen = "Navigate2LiveScreen";
        public string SetImagingScreen = "SetImagingScreen";
        public string CaptureEvent = "CaptureEvent";
        public string ExportImageFiles = "ExportImageFiles";
        public string GetImageFiles = "GetImageFiles";
        public string DisplayImage = "DisplayImage";
        public string EnableImagingBtn = "EnableImagingBtn";
        public string ImageAdded = "ImageAdded";
        public string StartResumeTimer = "StartResumeTimer";
        public string StopResumeTimer = "StopResumeTimer";
        public string ShowSplash = "ShowSplash";
        public string HideSplash = "HideSplash";
        public string triggerRecieved = "triggerRecieved";
        public string EnableZoomMagnification = "EnableZoomMagnification";
        public string ChangeThumbnailSide = "ChangeThumbnailSide";
        public string GetReportImagesList = "GetReportImagesList";
        public string ShowReportImages = "ShowReportImages";
        public string ChangedDisplayBitmap = "ChangedDisplayBitmap";
        public string GetImageFilesFromThumbnails = "GetImageFilesFromThumbnails";
        public string DisableLiveControls = "DisableLiveControls";
        public string InitCameraStatusBar = "InitCameraStatusBar";
        public string EnableCapturePowerStatusTimer = "EnableCapturePowerStatusTimer";
        public string SaveImgChanges = "SaveImgChanges";
        public string SaveAnnotationChanges = "SaveAnnotationChanges";
        public string IsShiftAndControl = "IsShiftAndControl";
        public string RGBColorchange = "RGBColorchange";
        public string TriggerRecieved = "TriggerRecieved";
        public string MotorForwardDone = "MotorForwardDone";
        public string MotorBackwardDone = "MotorBackwardDone";
        public string UpdateGlaucomaToolControls = "UpdateGlaucomaToolControls";
        public string UpdateMainWindowCursor = "UpdateMainWindowCursor";
        public string UpdateCommandLogView = "UpdateCommandLogView";
        public string PosteriorAnteriorSelection = "PosteriorAnteriorSelection";
        public string PosteriorAnteriorButtonRefresh = "PosteriorAnteriorButtonRefresh";
        public string PatientDetailsConformationPopUp = "PatientDetailsConformationPopUp";
        public string AnnotationButtonsRefresh = "AnnotationButtonsRefresh";
        public string StartLiveImaging = "StartLiveImaging";
        public string SaveFrames2Disk = "SaveFrames2Disk";
        public string FrameRateEvent = "FrameRateEvent";
        public string RotaryMovedEvent = "RotaryMovedEvent";
        public string ReportImagesIsShiftControl = "ReportImagesIsShiftControl";
        public string ChangeLeftRightPos_Live = "ChangeLeftRightPos_Live";
        public string LoadImageFromFileViewingScreen = "LoadImageFromFileViewingScreen";
        public string UpdateFFATime = "UpdateFFATime";
        public string CreateReportEvent = "CreateReportEvent";
        public string EnableMovePointInCDRTool = "EnableMovePointInCDRTool";//initialisation of string.By Ashutosh 25-07-2017.
        public string UpdateOverlay = "UpdateOverlay";
        public string EnableDisableEmrButton = "EnableDisableEmrButton";
        public string StartStopServerDatabaseTimer = "StartStopServerDatabaseTimer";
        public string DisplayCapturedImage = "DisplayCapturedImage";
        public string UpdateCaptureRLiveUI = "UpdateCaptureRLiveUI";
        public string GoToViewScreen = "GoToViewScreen";
        public string ShowSplashScreen = "ShowSplashScreen";
        // Alignment 
        public IVLEventHandler()
        {
		}
		public void Register(String n, NotificationHandler handler) {
			Handlers hs;
			if (reg.ContainsKey(n)) {
				hs = reg[n];
			} else {
				hs = new Handlers();
				reg[n] = hs;
			}
			hs.Add(handler);
		}
		public void UnRegister(String n, NotificationHandler handler) {
			Handlers hs;
			if (reg.ContainsKey(n)) {
				hs = reg[n];
				if (hs.Contains(handler))
					hs.Remove(handler);
			}
		}

        public bool isHandlerPresent(string n)
        {
            if (!reg.ContainsKey(n))
            {
                //return;
                // IVLEventHandlerException e = new IVLEventHandlerException("Can't find handlers for " + n);
                //throw e;
                return false;
            }
            return true;
        }

		public void Notify(String n) {
			Notify(n, new Args());
		}
		public void Notify(String n, Args args) {
			if ( ! reg.ContainsKey(n)) {
				//return;
                IVLEventHandlerException e = new IVLEventHandlerException("Can't find handlers for " + n);
				throw e;
			}
			Handlers handlers = reg[n];
			if (args == null) args = new Args();
			foreach (NotificationHandler h in handlers) {
				h(n, args);
			}
		}
	}

    public class Args : Dictionary<String, Object>
    {
        public Args()
        {
        }
        public Args(String key, object value)
        {
            Add(key, value);
        }
        public Args(String key1, object value1, String key2, object value2)
        {
            Add(key1, value1);
            Add(key2, value2);
        }
        public String Trim(String field)
        {
            return ToString(field).Trim();
        }
        public String ToString(String field)
        {
            if (this[field] != null)
            {
                return this[field].ToString();
            }
            else
            {
                return "";
            }
        }
        public void AddLike(String key, String value)
        {
            Add(key, "%" + value + "%");
        }
    }
}
