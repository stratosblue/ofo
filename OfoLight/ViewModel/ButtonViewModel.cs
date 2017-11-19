using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.Storage.Streams;
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
        private ImageSource _icon;

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

        private string _iconUri;

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
        /// iconfont图标
        /// </summary>
        public string IconFont { get; set; }

        private string _contentText;

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

        private string _name;

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

        private string _description;

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

        private bool _isTip;

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

        private Visibility _tipStatus = Visibility.Collapsed;

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

        /// <summary>
        /// button点击的command
        /// </summary>
        public ICommand ButtonClickCommand { get; set; }

        /// <summary>
        /// 按键VM
        /// </summary>
        public ButtonViewModel()
        { }
    }
}
