using OfoLight.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    public sealed partial class WebPageView : Page
    {
        WebPageViewModel ViewModel { get; set; }

        public WebPageView()
        {
            this.InitializeComponent();
            ViewModel = new WebPageViewModel(webView);
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string url)
            {
                ViewModel.TargetUrl = url;
            }
        }
    }
}
