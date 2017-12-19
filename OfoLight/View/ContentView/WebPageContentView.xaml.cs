using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class WebPageContentView : UserControl
    {
        #region 属性

        private WebPageViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        public WebPageContentView(string startUrl)
        {
            this.InitializeComponent();
            ViewModel = new WebPageViewModel(webView)
            {
                TargetUrl = startUrl,
                IsReCheckNavi = true,
            };
            DataContext = ViewModel;
            Unloaded += PageUnloaded;
        }

        #endregion 构造函数

        #region 方法

        private void PageUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Dispose();
            (webView.Parent as Grid)?.Children?.Remove(webView);
            ViewModel = null;
            Unloaded -= PageUnloaded;
        }

        #endregion 方法
    }
}