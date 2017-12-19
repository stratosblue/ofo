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
        #region 属性

        private ContentPageViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        public ContentPageView()
        {
            this.InitializeComponent();
            ViewModel = new ContentPageViewModel();
            DataContext = ViewModel;
            Unloaded += PageUnloaded;
        }

        #endregion 构造函数

        #region 方法

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ContentPageArgs args)
            {
                ViewModel.ContentNavication(args);
            }
        }

        private void PageUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Dispose();
            Unloaded -= PageUnloaded;
        }

        #endregion 方法
    }
}