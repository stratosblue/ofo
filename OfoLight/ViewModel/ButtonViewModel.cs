using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 按键VM
    /// </summary>
    public class ButtonViewModel : NotifyViewModel
    {
        #region 字段

        private string _contentText;
        private string _description;
        private ImageSource _icon;

        private string _iconUri;

        private bool _isTip;

        private string _name;

        private Visibility _tipStatus = Visibility.Collapsed;

        #endregion 字段

        #region 属性

        /// <summary>
        /// button点击的command
        /// </summary>
        public ICommand ButtonClickCommand { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ContentText
        {
            get { return _contentText; }
            set
            {
                _contentText = value;
                NotifyPropertyChanged("ContentText");
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                NotifyPropertyChanged("Icon");
            }
        }

        /// <summary>
        /// iconfont图标
        /// </summary>
        public string IconFont { get; set; }

        /// <summary>
        /// 图标的资源路径
        /// </summary>
        public string IconUri
        {
            get { return _iconUri; }
            set
            {
                _iconUri = value;
                try
                {
                    Icon = new BitmapImage(new Uri(value));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// 是否提示
        /// </summary>
        public bool IsTip
        {
            get { return _isTip; }
            set
            {
                _isTip = value;
                NotifyPropertyChanged("IsTip");
                TipStatus = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 提示的显示状态
        /// </summary>
        public Visibility TipStatus
        {
            get { return _tipStatus; }
            private set
            {
                _tipStatus = value;
                NotifyPropertyChanged("TipStatus");
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 按键VM
        /// </summary>
        public ButtonViewModel()
        { }

        #endregion 构造函数
    }
}