using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NLog;
using NLog.Config;
using NLog.Targets;
namespace INTUSOFT.Imaging
{
   internal static class CameraLogger
    {

       public static void WriteException(Exception ex, Logger logger)
       {

           if (IVLCamVariables.isCapturing)
               IVLCamVariables.intucamHelper.WriteCaptureLog();
           IntucamBoardCommHelper.BulkLogList.RemoveAll(x => ( x.Count == 0));
           IVLCamVariables.logClass.BulkTransferLogList.AddRange(IntucamBoardCommHelper.BulkLogList);
           IntucamBoardCommHelper.InterruptLogList.RemoveAll(x => x.Count == 0);
           IVLCamVariables.logClass.BulkTransferLogList.AddRange(IntucamBoardCommHelper.InterruptLogList);
           IVLCamVariables.logClass.BulkTransferLogList = IVLCamVariables.logClass.BulkTransferLogList.OrderBy(x => x["TimeStamp"]).ToList(); ;
           IntucamBoardCommHelper.BulkLogList.Clear();
           IVLCamVariables.logClass.InterruptTransferLogList.AddRange(IntucamBoardCommHelper.InterruptLogList);

           IntucamBoardCommHelper.InterruptLogList.Clear();
           IVLCamVariables.logClass.CameraLogList.AddRange(IVLCamVariables.CameraLogList);
           IVLCamVariables.CameraLogList.Clear();
           IVLCamVariables.logClass.FrameLogList.AddRange(Camera.frameLogList);
           IVLCamVariables.logClass.WriteLogs2File();
           ExceptionLogWriter.WriteLog(ex, logger);

       }

    }
}
