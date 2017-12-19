using System;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    public class WebPageViewModel : BaseContentViewModel
    {
        #region 字段

        private bool _isLastFragmentView = false;

        private string _targetUrl;

        #endregion 字段

        #region 属性

        public override bool CanGoBack { get => WebView?.CanGoBack == true; }

        /// <summary>
        /// 检测导航，在片段后退时，使用导航后退
        /// </summary>
        public bool IsReCheckNavi { get; set; } = false;

        public string TargetUrl
        {
            get { return _targetUrl; }
            set
            {
                _targetUrl = value;
                NotifyPropertyChanged("TargetUrl");
            }
        }

        public WebView WebView { get; set; }

        #endregion 属性

        #region 构造函数

        public WebPageViewModel(WebView webView)
        {
            WebView = webView;
            WebView.PermissionRequested += WebViewPermissionRequested;
            WebView.NavigationCompleted += WebViewNavigationCompleted;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 资源释放
        /// </summary>
        public override void Dispose()
        {
            WebView.PermissionRequested -= WebViewPermissionRequested;
            WebView.NavigationCompleted -= WebViewNavigationCompleted;
            WebView.NavigateToString(string.Empty);
            WebView = null;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            base.Dispose();
        }

        public override async void GoBack()
        {
            await WebView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                WebView.GoBack();
            });
        }

        private async void WebViewNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (_isLastFragmentView && IsReCheckNavi)
            {
                lock (this)
                {
                    _isLastFragmentView = false;
                }
                await WebView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (WebView.CanGoBack)
                    {
                        WebView.GoBack();
                    }
                    WebView.Navigate(args.Uri);
                });
                return;
            }

            if (!string.IsNullOrEmpty(args.Uri.Fragment))
            {
                lock (this)
                {
                    _isLastFragmentView = true;
                }
            }
        }

        private void WebViewPermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            if (args.PermissionRequest.PermissionType == WebViewPermissionType.Geolocation)
                args.PermissionRequest.Allow();
        }

        #endregion 方法
    }
}