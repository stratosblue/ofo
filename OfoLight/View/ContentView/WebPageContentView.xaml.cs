using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class WebPageContentView : UserControl
    {
        WebPageViewModel ViewModel { get; set; }

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

        private void PageUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Dispose();
            (webView.Parent as Grid)?.Children?.Remove(webView);
            ViewModel = null;
            Unloaded -= PageUnloaded;
        }
    }
}
