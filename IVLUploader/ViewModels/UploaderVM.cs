using BaseViewModel;
namespace IntuUploader.ViewModels
{
    public class UploaderVM:ViewBaseModel
    {

        private SentItemsViewModel sentItemsViewModel;

        public SentItemsViewModel SentItemsViewModel
        {
            get { return sentItemsViewModel; }
            set { sentItemsViewModel = value;
                OnPropertyChanged("SentItemsViewModel");
            }
        }


        private OutboxViewModel outboxViewModel;

        public OutboxViewModel OutboxViewModel
        {
            get { return outboxViewModel; }
            set {
                outboxViewModel = value;
                OnPropertyChanged("OutboxViewModel");
            }
        }


        private bool startStopTimer;

        public bool StartStopTimer
        {
            get { return startStopTimer; }
            set
            {
                if(startStopTimer != value)
                {
                    startStopTimer = value;
                    SentItemsViewModel.StartStopSentItemsTimer(startStopTimer);
                    OutboxViewModel.StartStopSentItemsTimer(startStopTimer);
                    if (!value)
                    {
                        SentItemsViewModel.activeFileCloudVM.IsBusy = value;
                        OutboxViewModel.activeFileCloudVM.IsBusy = value;
                    }
                    OnPropertyChanged("StartStopTimer");
                }
                

            }
        }

        public UploaderVM(AnalysisType analysisType)
        {
            SentItemsViewModel = new SentItemsViewModel(analysisType);
            OutboxViewModel = new OutboxViewModel(analysisType);
        }
    }
}
