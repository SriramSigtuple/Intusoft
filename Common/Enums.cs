using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    public enum CloudReportStatus { Initialized = 1, Uploading, Processing, View, Failed };
    public enum GainLevels { Low, Medium, High };
    public enum PageDisplayed { Login, Emr, Image };

    public enum LiveView {Live, View};
    public enum QIStatus { NotAnalysed = 0, Initialised = 1, Uploading, Processing, Gradable, NonGradable, Failed };

    public enum AnalysisType { QI, Fundus }
    public enum DirectoryEnum { OutboxDir, ActiveDir, LoginDir, CreateAnalysis, UploadDir, StartAnalysisDir, SentItemsDir, ProcessedDir, InboxDir, ReadDir,CloudImagesDir };
}

