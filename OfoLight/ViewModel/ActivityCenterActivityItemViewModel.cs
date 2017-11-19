using System;
using System.Windows.Input;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 活动中心，活动项VM
    /// </summary>
    public class ActivityCenterActivityItemViewModel : NotifyViewModel, IDisposable
    {
        private string _activityId;

        public string ActivityId
        {
            get { return _activityId; }
            set
            {
                _activityId = value;
            }
        }
        private string _jumpUrl;

        public string JumpUrl
        {
            get { return _jumpUrl; }
            set
            {
                _jumpUrl = value;
                NotifyPropertyChanged("JumpUrl");
            }
        }

        private string _imgUrl;

        public string ImgUrl
        {
            get { return _imgUrl; }
            set { _imgUrl = value; }
        }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }

        private ICommand _clickCommand;

        /// <summary>
        /// 活动点击命令
        /// </summary>
        public ICommand ClickCommand
        {
            get { return _clickCommand; }
            set
            {
                _clickCommand = value;
                NotifyPropertyChanged("ClickCommand");
            }
        }


        /// <summary>
        /// 活动中心，活动项VM
        /// </summary>
        public ActivityCenterActivityItemViewModel()
        {

        }

        public void Dispose()
        {
            ClickCommand = null;
        }
    }
}
