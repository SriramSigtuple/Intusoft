using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Imaging
{
    #region //////////////////// Enums used in the camera module//////////////////////////////////
    public enum CapturedImageType { GbGr, Flash, NoImage };
    public enum ImageSaveFormat { jpg, png, bmp, tiff };
    public enum CameraModel { A, B, C, D };
    public enum ImagingMode { Posterior_45, FFA, Anterior_Prime, Posterior_Prime, FFAColor, FFA_Plus, Anterior_FFA, Anterior_45 };
    public enum ImageCategory { IR, Dark, GB, GR, Flash };
    public enum CameraVendors { Vendor1, Vendor2 };
    public enum BitDepth { Depth_8, Depth_10, Depth_12, Depth_14, Depth_16 };
    public enum LeftRightPosition { Left, Right };
    public enum TriggerStatus { Off, On };
    public enum Led { IR = 4, Flash = 2, Blue = 1, Cornea_Flash = 3, Cornea_IR = 5, Cornea_Blue = 6 };
    public enum LedCode {IR = 0, Flash = 1, Blue = 2 };
    public enum GainLevels { Low, Medium, High };
    public enum InterruptCode {TriggerPressed = 1, MotorFastForward, MotorFastBackward, FlashOnDone, FlashOffDone, RotaryDone, MotorResetDone, PotIntensityChanged, LR_Event, Camera_Arrived, Camera_Removed, OtherCommands, Timeout, Halted, DeviceRemoved, Overflow };
    public enum CaptureFailureCode {PowerDisconnected, CameraDisconnected, FlashOnNotReceived, FlashOffNotReceived, FrameNotReceived, MotorTimeout, CaptureCommandNotReceived, CaptureTimeout, None};
    public enum Devices {PowerConnected, PowerDisconnected, CameraConnected, CameraDisconnected, OtherDevices };
    public enum PostProcessingStep { ShiftImage = 1,HSVBoost = 2, LUT = 3, Clahe = 4, HotSpotCorrection = 5, UnsharpMask = 6, ColorCorrection = 7,  BrightnessContrast = 8,  Mask = 9, Gamma = 10 };
    //    #define LED_TYPE_WHITE_FLASH		1
    //#define LED_TYPE_BLUE_FLASH		2
    //#define LED_TYPE_CORNEA_FLASH		3
    //#define LED_TYPE_INTERNAL_IR		4
    //#define LED_TYPE_CORNEA_IR		5
    //#define LED_TYPE_CORNEA_BLUE_FLASH	6
    #endregion
}
