using OfoLight.Entity;
using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    /// <summary>
    /// 内容展示页
    /// </summary>
    public sealed partial class ContentPageView : Page
    {
        ContentPageViewModel ViewModel { get; set; }

        public ContentPageView()
        {
            this.InitializeComponent();
            ViewModel = new ContentPageViewModel();
            DataContext = ViewModel;
            Unloaded += PageUnloaded;
        }

        private void PageUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Dispose();
            Unloaded -= PageUnloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ContentPageArgs args)
            {
                ViewModel.ContentNavication(args);
            }
        }
    }
}
