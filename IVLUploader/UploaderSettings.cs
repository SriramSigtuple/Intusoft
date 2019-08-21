using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuUploader
{
    [Serializable]
    public class UploaderSettings
    {
        public double OutboxTimerInterval = 10;
        public double SentItemsTimerInterval = 10;
        public double InternetCheckTimerInterval = 10;

        public UploaderSettings()
        {

        }

    }
}
