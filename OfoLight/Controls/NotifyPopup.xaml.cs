using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace OfoLight.Controls
{
    /// <summary>
    /// 弹出提示控件
    /// </summary>
    public sealed partial class NotifyPopup : UserControl
    {
        #region 字段

        private Popup _popup;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 是否正在显示
        /// </summary>
        public bool IsShowing { get; set; }

        /// <summary>
        /// 提示内容
        /// </summary>
        public string NotifyContent { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 弹出提示控件
        /// </summary>
        public NotifyPopup()
        {
            InitializeComponent();
            _popup = new Popup()
            {
                Child = this
            };
            Unloaded += NotifyPopup_Unloaded;
        }

        #endregion 构造函数

        #region 方法

        public async Task ShowAsync()
        {
            await ShowAsync(null, TimeSpan.FromSeconds(2));
        }

        public async Task ShowAsync(string notifyContent)
        {
            await ShowAsync(notifyContent, TimeSpan.FromSeconds(2));
        }

        public async Task ShowAsync(string notifyContent, TimeSpan showTime)
        {
            NotifyContent = notifyContent ?? NotifyContent;
            if (string.IsNullOrEmpty(NotifyContent))
            {
                return;
            }
            IsShowing = true;
            await _popup.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                Width = Window.Current.Bounds.Width;
                Height = Window.Current.Bounds.Height;
                tbNotify.Text = NotifyContent;
                hideStoryboard.BeginTime = showTime;
                hideStoryboard.Begin();
                hideStoryboard.Completed += Show_Completed;
                Window.Current.SizeChanged += Current_SizeChanged;
                _popup.IsOpen = true;
            });
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            Width = e.Size.Width;
            Height = e.Size.Height;
        }

        private void NotifyPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            Unloaded -= NotifyPopup_Unloaded;
        }

        private void Show_Completed(object sender, object e)
        {
            hideStoryboard.Stop();
            IsShowing = false;
            _popup.IsOpen = false;
            Window.Current.SizeChanged -= Current_SizeChanged;
            hideStoryboard.Completed -= Show_Completed;

            NotifyContent = string.Empty;
        }

        #endregion 方法
    }
}