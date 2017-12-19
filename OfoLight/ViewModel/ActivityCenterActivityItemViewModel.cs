using System;
using System.Windows.Input;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 活动中心，活动项VM
    /// </summary>
    public class ActivityCenterActivityItemViewModel : NotifyViewModel, IDisposable
    {
        #region 字段

        private string _activityId;

        private ICommand _clickCommand;

        private string _imgUrl;

        private string _jumpUrl;

        #endregion 字段

        #region 属性

        public string ActivityId
        {
            get { return _activityId; }
            set
            {
                _activityId = value;
            }
        }

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
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }

        public string ImgUrl
        {
            get { return _imgUrl; }
            set { _imgUrl = value; }
        }

        public string JumpUrl
        {
            get { return _jumpUrl; }
            set
            {
                _jumpUrl = value;
                NotifyPropertyChanged("JumpUrl");
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 活动中心，活动项VM
        /// </summary>
        public ActivityCenterActivityItemViewModel()
        {
        }

        #endregion 构造函数

        #region 方法

        public void Dispose()
        {
            ClickCommand = null;
        }

        #endregion 方法
    }
}