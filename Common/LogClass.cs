using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Common
{
    public class LogClass
    {

        private static readonly Logger log = LogManager.GetLogger("INTUSOFT.Imaging");
        private static readonly Logger captureLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureLog");
        private static readonly Logger FrameLog = LogManager.GetLogger("INTUSOFT.Imaging.FrameLog");
        private static readonly Logger InterruptLog = LogManager.GetLogger("INTUSOFT.Imaging.InterruptTransferLog");
        private static readonly Logger BulkTransferLog = LogManager.GetLogger("INTUSOFT.Imaging.BulkTransferLog");// overall log for camera and board
        private readonly Logger ExceptionLog = LogManager.GetLogger("INTUSOFT.Desktop.ExceptionLog");
        public enum LogTypeEnum {Log , captureLog, FrameLog, BulkLog, ExceptionLog,InterruptLog}
        public List<INTUSOFT.EventHandler.Args> CameraLogList;
        public List<INTUSOFT.EventHandler.Args> FrameLogList;
        public List<INTUSOFT.EventHandler.Args> CaptureLogList;
        public List<INTUSOFT.EventHandler.Args> BulkTransferLogList;
        public List<INTUSOFT.EventHandler.Args> InterruptTransferLogList;
        public bool isLogWritingCompleted = false;
        //public List<string> CameraLogList;
        //public List<string> FrameLogList;
        //public List<string> CaptureLogList;
        //public List<string> BulkTransferLogList;
        private static LogClass logClass;
        public static LogClass GetInstance()
        {
            if (logClass == null)
                logClass = new LogClass();
            return logClass;
        }
        private LogClass()
        {
            CameraLogList = new List<INTUSOFT.EventHandler.Args>();
            FrameLogList = new List<INTUSOFT.EventHandler.Args>();
            CaptureLogList = new List<INTUSOFT.EventHandler.Args>();
            BulkTransferLogList = new List<INTUSOFT.EventHandler.Args>();
            InterruptTransferLogList = new List<INTUSOFT.EventHandler.Args>();
            //CameraLogList = new List<string> ();
            //FrameLogList = new List<string> ();
            //CaptureLogList = new List<string>();
            //BulkTransferLogList = new List<string>();
        }
        public void WriteCaptureLog2File(List<INTUSOFT.EventHandler.Args> capLog)
        {
            try
            {
                write2Log(capLog, captureLog);

                //for (int i = 0; i < CaptureLogList.Count; i++)
                //{
                //    captureLog.Info(CaptureLogList[i] + Environment.NewLine);
                //}
                //CaptureLogList.Clear();
            }
            catch (Exception ex)
            {
                ExceptionLog.Info(Exception2StringConverter.GetInstance().ConvertException2String(ex));
            }

        }
        private void write2Log(List<INTUSOFT.EventHandler.Args> LogList, Logger log)
        {
            foreach (INTUSOFT.EventHandler.Args item in LogList)
            {
                if (item != null )
                {
                    string timeStamp = string.Empty;
                    string msg = string.Empty;
                    string stackTrace = string.Empty;
                    if(item.ContainsKey("TimeStamp"))
                    {
                      DateTime dt = (DateTime)item["TimeStamp"];
                      timeStamp =  dt.ToString("HH-mm-ss-fff");
                    }
                    if(item.ContainsKey("Msg"))
                    {
                        msg = item["Msg"] as string;
                    }
                    if(item.ContainsKey("callstack"))
                    {
                    string[] lines = item["callstack"].ToString().Split(
                                                                             new[] { "\r\n", "\r", "\n" },
                                                                             StringSplitOptions.None
                                                                         );
                        stackTrace = lines[2] + Environment.NewLine + lines[3] + Environment.NewLine + lines[4] ;
                    }
                    string logValue = string.Format("Time = {0}, {1}, Call Stack = {2} ",timeStamp , msg,stackTrace );
                    log.Info(logValue + Environment.NewLine);
                }
            }
            LogList.Clear();
        }
        public void WriteLogs2File()
        {
            try
            {
                List<INTUSOFT.EventHandler.Args>[] LogListArr = new List<INTUSOFT.EventHandler.Args>[] { FrameLogList,BulkTransferLogList,CameraLogList,InterruptTransferLogList };
                LogTypeEnum[] logType = new LogTypeEnum[] {LogTypeEnum.FrameLog,LogTypeEnum.BulkLog,LogTypeEnum.Log,LogTypeEnum.InterruptLog };
                int count = 0;
                foreach (List<INTUSOFT.EventHandler.Args> item in LogListArr)
                {
                    if (item != null)
                    {
                        LogTypeEnum logT = logType[count];
                        Logger L = null;
                        switch (logT)
                        {
                            case LogTypeEnum.BulkLog:
                                {
                                    L = BulkTransferLog;
                                    break;
                                }
                            case LogTypeEnum.captureLog:
                                {
                                    L = captureLog;
                                    break;
                                }
                            case LogTypeEnum.Log:
                                {
                                    L = log;
                                    break;
                                }
                            case LogTypeEnum.FrameLog:
                                {
                                    L = FrameLog;
                                    break;
                                }
                            case LogTypeEnum.InterruptLog:
                                {
                                    L = InterruptLog;
                                    break;
                                }
                        }
                        write2Log(item, L);
                    }
                    count++;
                   
                }
                isLogWritingCompleted = true;
                //if (CameraLogList != null)
                //{
                //    for (int i = 0; i < CameraLogList.Count; i++)
                //    {
                //        log.Info(CameraLogList[i] + Environment.NewLine);
                //    }
                //    CameraLogList.Clear();
                //}
                //if (FrameLogList != null)
                //{
                //    for (int i = 0; i < FrameLogList.Count; i++)
                //    {
                //        FrameLog.Info(FrameLogList[i] + Environment.NewLine);
                //    }
                //    FrameLogList.Clear();
                //}
                //if (CaptureLogList != null)
                //{
                //    for (int i = 0; i < CaptureLogList.Count; i++)
                //    {
                //        captureLog.Info(CaptureLogList[i] + Environment.NewLine);
                //    }
                //    CaptureLogList.Clear();
                //}
                //if (BulkTransferLogList != null)
                //{
                //    for (int i = 0; i < BulkTransferLogList.Count; i++)
                //    {
                //        BulkTransferLog.Info(BulkTransferLogList[i] + Environment.NewLine);
                //    }
                //    BulkTransferLogList.Clear();
                //}
            }
            catch (Exception ex)
            {
                ExceptionLog.Info(Exception2StringConverter.GetInstance().ConvertException2String(ex));
            }

        }
    }
}
