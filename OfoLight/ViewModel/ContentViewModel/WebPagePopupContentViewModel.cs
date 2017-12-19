using System;

namespace OfoLight.ViewModel
{
    public class WebPagePopupContentViewModel : BasePopupContentViewModel
    {
        #region 字段

        private string _targetUrl;

        #endregion 字段

        #region 属性

        public string TargetUrl
        {
            get { return _targetUrl; }
            set
            {
                _targetUrl = value;
                NotifyPropertyChanged("TargetUrl");
            }
        }

        #endregion 属性

        #region 构造函数

        public WebPagePopupContentViewModel(Action completeCallBack) : base(completeCallBack)
        {
        }

        #endregion 构造函数
    }
}