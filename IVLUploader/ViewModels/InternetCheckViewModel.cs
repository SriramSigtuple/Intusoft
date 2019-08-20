using BaseViewModel;
using NLog;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Input;

namespace IVLUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class InternetCheckViewModel : ViewBaseModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        Timer PingDNSTimer;
        Ping myPing;
        PingOptions pingOptions;
        PingReply reply;
        string host = "8.8.8.8"; // TODO : to be configured
        byte[] buffer = new byte[32];
        int timeout = 10000;// TODO : to be configured
        int timerTick = 20000;// TODO : to be configured
        Boolean internetPresent;
        const int MaxRetryCount = 60;// TODO : to be configured

        int retryCount = 0;
        private static InternetCheckViewModel _internetCheckViewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        private InternetCheckViewModel()
        {
            logger.Info("");

            myPing = new Ping();
            pingOptions = new PingOptions();
            PingDNSTimer = new Timer(new TimerCallback(PingDNS), null, 0, timerTick);
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");


        }
        /// <summary>
        /// Implementing singleton pattern in order to handle to values across the module
        /// </summary>
        /// <returns></returns>
        public static InternetCheckViewModel GetInstance()
        {
            logger.Info("");

            if (_internetCheckViewModel == null)
                _internetCheckViewModel = new InternetCheckViewModel();
            logger.Info("");

            return _internetCheckViewModel;
        }

        /// <summary>
        /// Method to check internet connection
        /// </summary>
        /// <param name="state"></param>
        private void PingDNS(object state)
        {
            logger.Info("");

            IPStatus status = IPStatus.Unknown;
            try
            {
                reply = myPing.Send(host, timeout, buffer, pingOptions);
                status = reply.Status;
            }
            catch (Exception)
            {

                status = IPStatus.Unknown;
            }
            finally
            {
                if (status == IPStatus.Success)
                {
                    RetryCount = 0;
                    InternetPresent = true;
                }
                else
                {
                    InternetPresent = false;
                    if (RetryCount == MaxRetryCount)
                    {
                        //showMessageBox();
                        RetryCount = 0;
                    }
                    else
                        RetryCount++;

                }
            }
            logger.Info("");

        }

        public ICommand SetValue
        {
            get;
            set;
        }


        /// <summary>
        /// Property for to Display Internet connection
        /// </summary>
        public bool InternetPresent
        {
            get => internetPresent;
            set
            {
                internetPresent = value;
                OnPropertyChanged("InternetPresent");
            }
        }
        /// <summary>
        /// Retry Count used to Pop up for user internet
        /// </summary>
        public int RetryCount
        {
            get => retryCount;
            set
            {
                retryCount = value;
                OnPropertyChanged("RetryCount");
            }
        }

        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
