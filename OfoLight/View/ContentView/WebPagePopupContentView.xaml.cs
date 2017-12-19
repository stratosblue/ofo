using System;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class WebPagePopupContentView : UserControl
    {
        #region 构造函数

        public WebPagePopupContentView()
        {
            this.InitializeComponent();
            Unloaded += ContentViewUnloaded;
        }

        #endregion 构造函数

        #region 方法

        private void ContentViewUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (webView.Parent as Grid)?.Children?.Remove(webView);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            Unloaded -= ContentViewUnloaded;
        }

        #endregion 方法
    }
}