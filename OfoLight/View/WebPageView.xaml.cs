using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    public sealed partial class WebPageView : Page
    {
        #region 属性

        private WebPageViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        public WebPageView()
        {
            this.InitializeComponent();
            ViewModel = new WebPageViewModel(webView);
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string url)
            {
                ViewModel.TargetUrl = url;
            }
        }

        #endregion 方法
    }
}