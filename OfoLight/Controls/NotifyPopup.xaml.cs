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
        private Popup _popup;

        /// <summary>
        /// 提示内容
        /// </summary>
        public string NotifyContent { get; set; }

        /// <summary>
        /// 是否正在显示
        /// </summary>
        public bool IsShowing { get; set; }

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

        private void NotifyPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            Unloaded -= NotifyPopup_Unloaded;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            Width = e.Size.Width;
            Height = e.Size.Height;
        }

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
            IsShowing = true;
            NotifyContent = notifyContent ?? NotifyContent;
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

        private void Show_Completed(object sender, object e)
        {
            hideStoryboard.Stop();
            IsShowing = false;
            _popup.IsOpen = false;
            Window.Current.SizeChanged -= Current_SizeChanged;
            hideStoryboard.Completed -= Show_Completed;

            NotifyContent = string.Empty;
        }
    }
}
