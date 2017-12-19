using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace OfoLight.Controls
{
    /// <summary>
    /// 内容弹出框
    /// </summary>
    public sealed partial class ContentPopup : UserControl, IDisposable
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
        public UIElement NotifyContent { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 内容弹出框
        /// </summary>
        public ContentPopup()
        {
            InitializeComponent();
            _popup = new Popup()
            {
                Child = this,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                ChildTransitions = new TransitionCollection() { new PopupThemeTransition() { FromHorizontalOffset = Window.Current.Bounds.Width, FromVerticalOffset = Window.Current.Bounds.Height } }
            };
            Unloaded += NotifyPopup_Unloaded;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 销毁资源
        /// </summary>
        public void Dispose()
        {
            Hide();
            Window.Current.SizeChanged -= Current_SizeChanged;
            _popup = null;
        }

        /// <summary>
        /// 隐藏显示
        /// </summary>
        /// <returns></returns>
        public void Hide()
        {
            IsShowing = false;
            _popup.IsOpen = false;
            NotifyContent = null;
        }

        public async Task ShowAsync()
        {
            await ShowAsync(null);
        }

        public async Task ShowAsync(UIElement content)
        {
            IsShowing = true;
            NotifyContent = content ?? NotifyContent;
            await _popup.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                Content = NotifyContent;
                Width = Window.Current.Bounds.Width;
                Height = Window.Current.Bounds.Height;
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
        }

        #endregion 方法
    }
}