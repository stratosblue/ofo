using System;

namespace OfoLight.ViewModel
{
    public class WebPagePopupContentViewModel : BasePopupContentViewModel
    {
        private string _targetUrl;

        public string TargetUrl
        {
            get { return _targetUrl; }
            set
            {
                _targetUrl = value;
                NotifyPropertyChanged("TargetUrl");
            }
        }

        public WebPagePopupContentViewModel(Action completeCallBack) : base(completeCallBack)
        {
        }
    }
}
