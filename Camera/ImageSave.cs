using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using INTUSOFT.EventHandler;
using System.Drawing;
using System.IO;
using Common;
using NLog;
using NLog.Config;
using NLog.Targets;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using System.Xml.Serialization;
using System.Xml;
using Emgu.CV;
using NLog;
using NLog.Config;
using NLog.Targets;
using Newtonsoft.Json;

namespace INTUSOFT.Imaging
{
    public class ImageSaveHelper
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(Camera));
        //private static readonly Logger log = LogManager.GetLogger("INTUSOFT.Imaging");
        //private static readonly Logger captureLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureLog");
        private static readonly Logger CaptureSettingsLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureSettingsLog");
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");

        string imagePath = string.Empty;
        public static List<Args> capture_log;// 
        
        // private static ILog captureLog = LogManager.GetLogger("CaptureFile");

        System.ComponentModel.BackgroundWorker bg;// Background worker to save the images and log to the hard drive
        XmlSerializer maskSettingsSerializer;
        XmlSerializer captureSettingsSerializer;
        private static ImageSaveHelper _imageSaveHelper;// static instance of the class to handle singleton

        /// <summary>
        /// Constructor to create an instance of the image save helper object
        /// </summary>
        private ImageSaveHelper()
        {
            capture_log = new List<Args>();
            if(IVLCamVariables._Settings == null)
            IVLCamVariables._Settings = CameraModuleSettings.GetInstance();
            bg = new System.ComponentModel.BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            IVLCamVariables._eventHandler.Register(IVLCamVariables._eventHandler.SaveFrames2Disk, new EventHandler.NotificationHandler(SaveFrames));
            maskSettingsSerializer = new XmlSerializer(typeof(INTUSOFT.Imaging.MaskSettings));
            captureSettingsSerializer = new XmlSerializer(typeof(CaptureLog));
        }
        /// <summary>
        /// Get instance creates an object of image save helper class if the local static object is null
        /// </summary>
        /// <returns>returns the instance of the class </returns>
        public static ImageSaveHelper GetInstance()
        {
            try
            {
                if (_imageSaveHelper == null)
                    _imageSaveHelper = new ImageSaveHelper();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log); 
            }
            return _imageSaveHelper;
        }
        public static void Setup(string path)
        {
            //Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            //PatternLayout patternLayout = new PatternLayout();
            //patternLayout.ConversionPattern = "%d, %method, %c{1},  %m%n";
            //patternLayout.ActivateOptions();

            //RollingFileAppender roller = new RollingFileAppender();
            //roller.AppendToFile = false;
            //roller.Name = "CaptureFile";
            //roller.File = path;
            //roller.Layout = patternLayout;
            //roller.MaxSizeRollBackups = 5;
            //roller.MaximumFileSize = "1MB";
            //roller.RollingStyle = RollingFileAppender.RollingMode.Size;

            //roller.ActivateOptions();
            //hierarchy.Root.AddAppender(roller);


            //hierarchy.Configured = true;
        }
        /// <summary>
        /// Creates the raw image directory and processed image directory if not present
        /// </summary>
        public void CreateImageCaptureDirectory()
        {
            try
            {
                // Create current date's directory in the SavePath
                LogManager.Configuration.Variables["dir4"] = IVLCamVariables._Settings.ImageSaveSettings.RawImageDirPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar + "save_" + DateTime.Now.ToString("HHmmss");
                IVLCamVariables.RawImageSaveDirPath = IVLCamVariables._Settings.ImageSaveSettings.RawImageDirPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar + "save_" + DateTime.Now.ToString("HHmmss");
                if (!Directory.Exists(IVLCamVariables.RawImageSaveDirPath))
                    Directory.CreateDirectory(IVLCamVariables.RawImageSaveDirPath);
                //captureLog.Debug("Debug image directory created");
                Args logArg = new Args();
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Debug image directory created  ");

                //logArg["callstack"] = Environment.StackTrace;
                //IVLCamVariables.logClass.CaptureLogList.Add();
                capture_log.Add(logArg);
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Debug image directory created  ", DateTime.Now.ToString("HH-mm-ss-fff")));

                //If the process directory path is not empty or null
                if (!string.IsNullOrEmpty(IVLCamVariables._Settings.ImageSaveSettings.ProcessedImageDirPath))
                {
                    if (!Directory.Exists(IVLCamVariables._Settings.ImageSaveSettings.ProcessedImageDirPath))
                        IVLCamVariables.ProcessedImageSaveDirPath = Directory.CreateDirectory(IVLCamVariables._Settings.ImageSaveSettings.ProcessedImageDirPath).FullName;
                    else
                        IVLCamVariables.ProcessedImageSaveDirPath = Directory.CreateDirectory(IVLCamVariables._Settings.ImageSaveSettings.ProcessedImageDirPath).FullName;
                }
                else
                    IVLCamVariables.ProcessedImageSaveDirPath = IVLCamVariables.RawImageSaveDirPath;// +Path.DirectorySeparatorChar + "save_" + DateTime.Now.ToString("HHmmss");
            }
            catch (Exception ex )
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
            

        }
        private void SaveFrames(string s, Args arg)
        {
            try
            {
                if(!IVLCamVariables.ImageSavingInProgress)
                    bg.RunWorkerAsync();
            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);

                //CustomMessageBox.Show(stringBuilder.ToString());
                //CustomMessageBox.Show(ex.StackTrace);
                //captureLog.Info(ex.StackTrace);
                //log.Info(ex.StackTrace);
                //Exception_Log.Info(ex.StackTrace);

            }

        }
        /// <summary>
        /// Save the images to the hard disc from the RAM memory
        /// </summary>
        private void SaveImagesToDisk()
        {
            try
            {
                #region Save IR
                if (IVLCamVariables._Settings.ImageSaveSettings.isIR_ImageSave)
                {
                    //captureLog.Debug("IR Save Started");
                    Args logArg = new Args();
                    
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "IR Save Started  ";
                     //IVLCamVariables.logClass.CaptureLogList.Add(str);
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                     //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                    if (IVLCamVariables.IRImage != null)
                        IVLCamVariables.IRImage.Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + "IR.png");

                    Args logArg1 = new Args();
                    
                    logArg1["TimeStamp"] = DateTime.Now;
                    logArg1["Msg"] = "IR Save completed  ";
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg1);
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                }
                #endregion

                #region Save Raw Bytes
                if (IVLCamVariables._Settings.CameraSettings.isRawMode)
                {
                    Args logArg = new Args();
                    List<byte[]> RawByteList = new List<byte[]>();
                    RawByteList.AddRange(IVLCamVariables.ivl_Camera.RawImageList);
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "Raw Save Started  ";
                    string str = string.Format("Time = {0},Raw Save Started  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                    //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                    if (IVLCamVariables.ivl_Camera.RawImageList.Count != 0)
                    {
                        switch (IVLCamVariables.captureImageType)
                        {

                            case CapturedImageType.Flash:
                                {
                                    if (IVLCamVariables._Settings.ImageSaveSettings.isRawSave)// Added this to save files if the isRawSave is enabled in the config
                                    {
                                        for (int i = 0; i < RawByteList.Count; i++)
                                            File.WriteAllBytes(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + (i).ToString() + ".raw", RawByteList[i]);
                                    }
                                    if (IVLCamVariables._Settings.ImageSaveSettings.isSaveRawImage)
                                    {
                                        Args Arg1 = new Args();

                                        Arg1["TimeStamp"] = DateTime.Now;
                                        Arg1["Msg"] = "Raw Image Flash Frame Started  ";
                                        str = string.Format("Time = {0},Raw Image Flash Frame Started  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                        //Arg1["callstack"] = Environment.StackTrace;
                                        capture_log.Add(Arg1);
                                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                        //IVLCamVariables.CameraLogList.Add(logArg);
                                        //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                        Args logArg1 = new Args();

                                        logArg1["TimeStamp"] = DateTime.Now;
                                        logArg1["Msg"] = "Raw Image Flash Frame Completed  ";
                                        //logArg1["callstack"] = Environment.StackTrace;
                                        str = string.Format("Time = {0},Raw Image Flash Frame Completed  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                        IVLCamVariables.ivl_Camera.RawImage.Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + "resultImage_" + IVLCamVariables.CaptureImageIndx.ToString() + "_" + IVLCamVariables.captureImageType.ToString() + ".png");
                                        //IVLCamVariables.RawImage.Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + "resultImage_" + IVLCamVariables.FlashIndx.ToString() + "_" + IVLCamVariables.captureImageType.ToString() + ".png");

                                        //IVLCamVariables.ProcessedImage.Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + "resultImage_" + IVLCamVariables.captureImageType.ToString() + ".png");
                                        //captureLog.Debug("Raw Image Flash Frame Completed");
                                        capture_log.Add(logArg1);
                                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                        //IVLCamVariables.CameraLogList.Add(logArg);
                                        //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                    }
                                    break;
                                }
                            case CapturedImageType.GbGr:
                                {

                                    if (IVLCamVariables._Settings.ImageSaveSettings.isRawSave)// Added this to save files if the isRawSave is enabled in the config
                                    {
                                        for (int i = 0; i < RawByteList.Count; i++)
                                            File.WriteAllBytes(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + (i).ToString() + ".raw", RawByteList[i]);
                                    }
                                    if (IVLCamVariables._Settings.ImageSaveSettings.isSaveRawImage)
                                    {
                                        //captureLog.Debug("Raw Image GbGr Started");

                                        Args logArg1 = new Args();

                                        logArg1["TimeStamp"] = DateTime.Now;
                                        logArg1["Msg"] = "Raw Image GbGr Started  ";
                                        str = string.Format("Time = {0},Raw Image GbGr Started  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                        ///logArg1["callstack"] = Environment.StackTrace;
                                        capture_log.Add(logArg1);
                                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                        //IVLCamVariables.CameraLogList.Add(logArg);
                                        //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                                        IVLCamVariables.ivl_Camera.RawImage.Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + "resultImage_" + IVLCamVariables.GRIndx.ToString() + "_" + IVLCamVariables.GBIndx.ToString() + IVLCamVariables.captureImageType.ToString() + ".png");
                                        //IVLCamVariables.ProcessedImage.Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + "resultImage_" + IVLCamVariables.GBIndx.ToString() + "_" + IVLCamVariables.captureImageType.ToString() + ".png");
                                        //captureLog.Debug("Raw Image GbGr Completed");
                                        Args logArg2 = new Args();

                                        logArg2["TimeStamp"] = DateTime.Now;
                                        logArg2["Msg"] = "Raw Image GbGr Completed ";
                                        str = string.Format("Time = {0},Raw Image GbGr Completed  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                        //logArg2["callstack"] = Environment.StackTrace;
                                        capture_log.Add(logArg2);
                                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                        //IVLCamVariables.CameraLogList.Add(logArg);
                                        //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                                    } 
                                    break;
                                }
                            default:
                                if (IVLCamVariables._Settings.ImageSaveSettings.isRawSave)// Added this to save files if the isRawSave is enabled in the config
                                {
                                    int i = 0;
                                    //captureLog.Debug("Raw Image  Started");

                                    Args logArg1 = new Args();

                                    logArg1["TimeStamp"] = DateTime.Now;
                                    logArg1["Msg"] = "Raw Image  Started ";
                                    str = string.Format("Time = {0},Raw Image  Started  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                    //logArg1["callstack"] = Environment.StackTrace;
                                    capture_log.Add(logArg1);
                                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                    //IVLCamVariables.CameraLogList.Add(logArg);
                                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                                    if (IVLCamVariables.CaptureImageIndx != 0)
                                    {
                                        for (i = IVLCamVariables.CaptureImageIndx - 1; i < IVLCamVariables.CaptureImageIndx + 2; i++)
                                            File.WriteAllBytes(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + (i).ToString() + ".raw", RawByteList[i]);
                                    }
                                    else
                                    {
                                        for (i = 0; i < IVLCamVariables.CaptureImageIndx + 2; i++)
                                            File.WriteAllBytes(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + (i).ToString() + ".raw", RawByteList[i]);
                                    }
                                    //captureLog.Debug("Raw Image Completed");
                                    Args logArg3 = new Args();

                                    logArg3["TimeStamp"] = DateTime.Now;
                                    logArg3["Msg"] = " Raw Image Completed ";
                                    str = string.Format("Time = {0},Raw Image Completed  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                    //logArg3["callstack"] = Environment.StackTrace;
                                    capture_log.Add(logArg3);
                                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                    //IVLCamVariables.CameraLogList.Add(logArg);
                                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                                }
                                break;
                        }
                    }
                    //captureLog.Debug("Raw Save completed Raw Images Count = {0}", IVLCamVariables.RawImageList.Count);
                    Args logArg4 = new Args();

                    logArg4["TimeStamp"] = DateTime.Now;
                    logArg4["Msg"] = "Raw Save completed  Raw Images Count = " + RawByteList.Count;

                    //logArg4["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg4);
                    RawByteList.Clear();
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                    //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                }
                #endregion

                #region Save Raw Bitmaps
                else
                {
                    if (IVLCamVariables._Settings.ImageSaveSettings.isSaveRawImage)
                    {
                        int i = 0;
                        List<Bitmap> captureList = new List<Bitmap>();
                        captureList.AddRange(IVLCamVariables.ivl_Camera.CaptureImageList);
                        if (captureList.Count != 0)
                        {

                            int startIndex = 0;
                            int endIndex = captureList.Count;
                            if (!IVLCamVariables.isCaptureFailure)
                            {
                                //if(!IVLCamVariables.intucamHelper.IsAssemblySoftware)
                                //if (endIndex> 1)
                                //    startIndex = captureList.Count - 2;
                            }
                            //int startIndex = IVLCamVariables.CaptureImageIndx-1;
                            //int endIndex = IVLCamVariables.CaptureImageIndx + 1;

                            //if (IVLCamVariables.CaptureImageIndx == 0)
                            //{
                            //    startIndex = 0;
                            //    endIndex = IVLCamVariables.CaptureImageList.Count;
                            //}

                            //else
                            //{
                            //    startIndex = IVLCamVariables.CaptureImageIndx - 1;
                            //    endIndex = startIndex + 2;
                            //}
                            for (i = startIndex; i < endIndex; i++)
                            {
                                if (IVLCamVariables._Settings.CameraSettings.isFFA_mode)
                                {
                                    Graphics g = Graphics.FromImage(captureList[i]);
                                    g.DrawString(IVLCamVariables.ffaTimeStamp, new Font(FontFamily.GenericSansSerif, 100f, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.LimeGreen, new PointF(2672, 1948));
                                }
                                captureList[i].Save(IVLCamVariables.RawImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + (i).ToString() + ".png");
                            }
                        }
                        captureList.Clear();
                    }
                    
                #endregion
                }
                #region // this is for the desktop application to save the image in required format with required compression ratio//
                string str1 = "";
                if (IVLCamVariables._Settings.ImageSaveSettings.isSaveProcessedImage && !IVLCamVariables.isCaptureFailure)
                {
                    //captureLog.Debug("Processed Save Started");
                    Args logArg5 = new Args();

                    logArg5["TimeStamp"] = DateTime.Now;
                    logArg5["Msg"] = "Processed Save Started ";
                    str1 = string.Format("Time = {0},Processed Save Started  ", DateTime.Now.ToString("HH-mm-ss-fff") );
                    //logArg5["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg5);
                    //IVLCamVariables.logClass.CaptureLogList.Add(str1);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str1);

                    IVLCamVariables.ImageName = string.Empty;
                    SaveProcessedImage(IVLCamVariables.ivl_Camera.CaptureBm, IVLCamVariables.ProcessedImageSaveDirPath, IVLCamVariables._Settings.ImageSaveSettings._ImageSaveFormat, IVLCamVariables._Settings.ImageSaveSettings.jpegCompression);// this is to save the image post capture
                    //captureLog.Debug("Processed Save Completed");
                    Args logArg6 = new Args();

                    logArg6["TimeStamp"] = DateTime.Now;
                    logArg6["Msg"] = "Processed Save Completed  ";
                    str1 = string.Format("Time = {0},Processed Save Completed  ", DateTime.Now.ToString("HH-mm-ss-fff") );
                    //logArg6["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg6);
                    //IVLCamVariables.logClass.CaptureLogList.Add(str1);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str1);
                    

                }
                #endregion
            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void SaveProcessedImage(Bitmap srcBm, string path, ImageSaveFormat format, int compressionRatio,bool isSaveAsOrExport = false)
        {
            try
            {
                //if the bool isSaveAsOrExport is true it will compute the image path directly and save the image else it will compute for exporting of images or save images
                 if (isSaveAsOrExport)
                        imagePath = path + "."+ format.ToString();
                 else
                 {
                     IVLCamVariables.ImageName = string.Empty;
                if (IVLCamVariables._Settings.ImageNameSettings.containsEyeSide)
                {
                    if(IVLCamVariables.leftRightPos == LeftRightPosition.Left)
                    IVLCamVariables.ImageName = "OS";
                    else
                        IVLCamVariables.ImageName = "OD";

                }
                if (!string.IsNullOrEmpty(IVLCamVariables.ImageName))
                    IVLCamVariables.ImageName += "_" + DateTime.Now.ToString("yyyyMMddHHmmss")+"."+format.ToString();
                else
                    IVLCamVariables.ImageName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + format.ToString();
                if (IVLCamVariables.isCapturing)
                {
                    if (!(IVLCamVariables._Settings.ImageSaveSettings.isMRNFolderSave))
                        imagePath = IVLCamVariables.ProcessedImageSaveDirPath + Path.DirectorySeparatorChar + IVLCamVariables.ImageName;
                    else
                        imagePath = IVLCamVariables.ProcessedImageSaveDirPath + System.IO.Path.DirectorySeparatorChar + IVLCamVariables.intucamHelper.MRNValue + Path.DirectorySeparatorChar + IVLCamVariables.intucamHelper.VisitDate + Path.DirectorySeparatorChar + IVLCamVariables.ImageName;
                    //path = IVLCamVariables.CaptureArg["ImageName"] as string;
                }
                else
                         imagePath = path + Path.DirectorySeparatorChar + IVLCamVariables.ImageName;
                 }
                Args arg = new Args();
                arg["TimeStamp"] = DateTime.Now;
                arg["Msg"] = imagePath;
                //arg["callstack"] = Environment.StackTrace;
                capture_log.Add(arg);
                switch (format)
                {
                    case ImageSaveFormat.png:
                        {
                            
                            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters jpegParams = new EncoderParameters(1);
                            EncoderParameter encodingParamater = new EncoderParameter(encoder, compressionRatio);
                            jpegParams.Param[0] = encodingParamater;
                            //IVLCamVariables.CaptureBm.Save(IVLCamVariables.CaptureArg["ImageName"] as string, getCodecInfo(ImageFormat.Png), jpegParams);
                            srcBm.Save(imagePath, getCodecInfo(ImageFormat.Png), jpegParams);
                        }
                        break;
                    case ImageSaveFormat.jpg:
                        {
                            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters jpegParams = new EncoderParameters(1);
                            EncoderParameter encodingParamater = new EncoderParameter(encoder, compressionRatio);
                            jpegParams.Param[0] = encodingParamater;
                            //IVLCamVariables.CaptureBm.Save(IVLCamVariables.CaptureArg["ImageName"] as string, getCodecInfo(ImageFormat.Jpeg), jpegParams);
                            srcBm.Save(imagePath, getCodecInfo(ImageFormat.Jpeg), jpegParams);
                        }
                        break;
                    case ImageSaveFormat.bmp:
                        {
                            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters jpegParams = new EncoderParameters(1);
                            EncoderParameter encodingParamater = new EncoderParameter(encoder, compressionRatio);
                            jpegParams.Param[0] = encodingParamater;
                            // IVLCamVariables.CaptureBm.Save(IVLCamVariables.CaptureArg["ImageName"] as string, getCodecInfo(ImageFormat.Bmp), jpegParams);
                            srcBm.Save(imagePath, getCodecInfo(ImageFormat.Bmp), jpegParams);
                        }
                        break;
                    case ImageSaveFormat.tiff:
                        {
                            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters jpegParams = new EncoderParameters(1);
                            EncoderParameter encodingParamater = new EncoderParameter(encoder, 100);
                            jpegParams.Param[0] = encodingParamater;
                            //IVLCamVariables.CaptureBm.Save(IVLCamVariables.CaptureArg["ImageName"] as string, getCodecInfo(ImageFormat.Tiff), jpegParams);
                            srcBm.Save(imagePath, getCodecInfo(ImageFormat.Tiff), jpegParams);
                        }
                        break;
                }
            }

            catch (Exception ex)
            {
                //IVLCamVariables.intucamHelper.CompleteCaptureSequence();
                CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
            IVLCamVariables.ImagePath= imagePath;
        }

        private ImageCodecInfo getCodecInfo(ImageFormat format)
        {
            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == format.Guid)
                        return codec;
                }
            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
            return null;
        }
        void bg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                IVLCamVariables.ImageSavingInProgress = true;
                SaveImagesToDisk();
            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }
        void bg_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //IVLCamVariables.CaptureArg["side"] = IVLCamVariables.eyeSide;
            //IVLCamVariables.CaptureArg["isCaptureFailed"] = IVLCamVariables.isCaptureFailure;
            //IVLCamVariables.CaptureArg["FailureCode"] = IVLCamVariables.captureFailureCode;
            if (IVLCamVariables._Settings.CameraSettings.isRawMode)
            {
                if (IVLCamVariables.ivl_Camera.RawImageList == null)
                {
                    IVLCamVariables.ivl_Camera.RawImageList = new List<byte[]>(); // create new list of raw image list if it is null
                }
                else if (IVLCamVariables.ivl_Camera.RawImageList.Count > 0)
                {
                    IVLCamVariables.ivl_Camera.RawImageList.Clear(); // clear if existing list is present
                    int identificador = GC.GetGeneration(IVLCamVariables.ivl_Camera.RawImageList);
                    GC.Collect(identificador, GCCollectionMode.Forced);
                }
                Graphics g = Graphics.FromImage( IVLCamVariables.ivl_Camera.RawImage);
                g.Clear(Color.Black);
                g.Dispose();
            }
            else
            {
                if (IVLCamVariables.ivl_Camera.CaptureImageList == null) // create new list of image list if it is null
                    IVLCamVariables.ivl_Camera.CaptureImageList = new List<Bitmap>();
                else if (IVLCamVariables.ivl_Camera.CaptureImageList.Count > 0)
                {
                    IVLCamVariables.ivl_Camera.CaptureImageList.Clear();// clear if existing list is present
                    int identificador = GC.GetGeneration(IVLCamVariables.ivl_Camera.CaptureImageList);
                    GC.Collect(identificador, GCCollectionMode.Forced);
                }
            }
            Args logArg = new Args();

            logArg["TimeStamp"] = DateTime.Now;
            logArg["Msg"] = "Capture Failure = " + IVLCamVariables.isCaptureFailure.ToString();
            //logArg["callstack"] = Environment.StackTrace;
            capture_log.Add(logArg);
            //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0},Capture Failure = {1}  ", DateTime.Now.ToString("HH-mm-ss-fff"), IVLCamVariables.CaptureArg["isCaptureFailed"]));
             //IVLCamVariables.CameraLogList.Add(logArg);
            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Capture Failure = {1}  ", DateTime.Now.ToString("HH-mm-ss-fff"), IVLCamVariables.CaptureArg["isCaptureFailed"]));

            //captureLog.Info(string.Format("Capture Failure = {0}", IVLCamVariables.CaptureArg["isCaptureFailed"]));

            //try
            //{
            //    Write2Log();

            //}
            //catch (Exception ex)
            //{

            //   CameraLogger.WriteException(ex, Exception_Log);
            //    // CameraLogger.WriteException(ex, Exception_Log);
            //    //CustomMessageBox.Show(ex.StackTrace);

            //    //log.Debug(ex.StackTrace);
            //    //CustomMessageBox.Show(ex.StackTrace);
            //}
            SaveCaptureLog();
                IVLCamVariables.intucamHelper.CompleteCaptureSequence();
           // IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.FrameCaptured, IVLCamVariables.CaptureArg);
        }

        public void SaveCaptureLog()
        {
            try
            {
                //using (var sw = new StringWriter())
                //{
                //    XmlWriterSettings Settings = new XmlWriterSettings();
                //    Settings.Encoding = Encoding.UTF8;
                //    using (var xmlWriter = XmlWriter.Create(sw, Settings))
                //    {
                //        CaptureLog captureSettings = new CaptureLog();
                //        captureSettings.currentLiveGain = IVLCamVariables._Settings.CameraSettings.LiveGain;
                //        captureSettings.currentLiveExposure = IVLCamVariables._Settings.CameraSettings.LiveExposure;
                //        captureSettings.currentFlashGain = IVLCamVariables._Settings.CameraSettings.CaptureGain;
                //        captureSettings.currentFlashExposure = IVLCamVariables._Settings.CameraSettings.CaptureExposure;
                //        captureSettings.currentCameraType = IVLCamVariables.ImagingMode;
                //        captureSettings.ImageTime = IVLCamVariables.ffaTimeStamp;
                //        captureSettingsSerializer.Serialize(xmlWriter, captureSettings);
                //        xmlWriter.Close();
                //    }
                //    //arg["cameraSettings"] = sw.ToString(); ;
                //    IVLCamVariables.captureCameraSettings = sw.ToString(); ;
                //}

                //using (var sw = new StringWriter())
                //{
                //    XmlWriterSettings Settings = new XmlWriterSettings();
                //    Settings.Encoding = Encoding.UTF8;
                //    using (var xmlWriter = XmlWriter.Create(sw, Settings))
                //    {
                //        maskSettingsSerializer.Serialize(xmlWriter, IVLCamVariables._Settings.PostProcessingSettings.maskSettings);
                //        xmlWriter.Close();
                //    }
                //    //arg["maskSettings"] = sw.ToString(); ;
                //    IVLCamVariables.captureMaskSettings = sw.ToString(); ;
                //}
                        CaptureLog captureSettings = new CaptureLog();
                        captureSettings.currentLiveGain = IVLCamVariables._Settings.CameraSettings.LiveGain;
                        captureSettings.currentLiveExposure = IVLCamVariables._Settings.CameraSettings.LiveExposure;
                        captureSettings.currentFlashGain = IVLCamVariables._Settings.CameraSettings.CaptureGain;
                        captureSettings.currentFlashExposure = IVLCamVariables._Settings.CameraSettings.CaptureExposure;
                        captureSettings.currentCameraType = IVLCamVariables.ImagingMode;
                        captureSettings.ImageTime = IVLCamVariables.FFATime;

                    IVLCamVariables.captureCameraSettings =    JsonConvert.SerializeObject(captureSettings);
                    //arg["cameraSettings"] = sw.ToString(); ;
                   // IVLCamVariables.captureCameraSettings = sw.ToString(); ;

                       // maskSettingsSerializer.Serialize(xmlWriter, IVLCamVariables._Settings.PostProcessingSettings.maskSettings);
                    //arg["maskSettings"] = sw.ToString(); ;
                    IVLCamVariables.captureMaskSettings = JsonConvert.SerializeObject(IVLCamVariables._Settings.PostProcessingSettings.maskSettings);

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
            
        }
        private void Write2Log()
        {

            try
            {
                   XmlSerializer serializer1, serializer2, serializer3;
                   serializer3 = new XmlSerializer(IVLCamVariables._Settings.BoardSettings.GetType());
                   serializer2 = new XmlSerializer(IVLCamVariables._Settings.PostProcessingSettings.GetType());
                   serializer1 = new XmlSerializer(IVLCamVariables._Settings.CameraSettings.GetType());
                string cameraSettings = "";
                StringWriter stringWriter = null;
                stringWriter = new StringWriter();
                serializer1.Serialize(stringWriter, IVLCamVariables._Settings.CameraSettings);
                cameraSettings = stringWriter.ToString();
                stringWriter.Close();
                stringWriter.Dispose();


                stringWriter = new StringWriter();
                serializer2.Serialize(stringWriter, IVLCamVariables._Settings.PostProcessingSettings);
                cameraSettings += Environment.NewLine + stringWriter.ToString();
                stringWriter.Close();
                stringWriter.Dispose();


                stringWriter = new StringWriter();
                serializer3.Serialize(stringWriter, IVLCamVariables._Settings.BoardSettings);
                cameraSettings += Environment.NewLine + stringWriter.ToString();

                CaptureSettingsLog.Debug("Capture Settings = {0}", cameraSettings);

                //captureLog.Debug("Capture Completed");
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = "Capture Completed ";
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg);
                //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0},Capture Completed  ", DateTime.Now.ToString("HH-mm-ss-fff")));
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Capture Completed  ", DateTime.Now.ToString("HH-mm-ss-fff")));
            }
            catch (Exception ex )
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                
            }
            /* Code added by dheeraj */
            //IntucamBoardCommHelper.GetRealTime(0);
            //IntucamBoardCommHelper.GetRealTime(1);
            //IntucamBoardCommHelper.GetRealTime(2);
        }
    }
    public class CaptureLog
    {
        public ushort currentLiveGain;
        public uint currentLiveExposure;
        public ushort currentFlashGain;
        public uint currentFlashExposure;
        public Imaging.ImagingMode currentCameraType;
        public string ImageTime;
        public CaptureLog()
        {

        }
    }
}
