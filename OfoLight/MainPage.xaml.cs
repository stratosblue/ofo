using OfoLight.Utilities;
using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight
{
    /// <summary>
    /// 主页面
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region 属性

        private MainPageViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 主页面
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            ViewModel = new MainPageViewModel(Map);
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await StatusBarUtility.ShowAsync();

            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.CheckCurrentActivitiesAsync();
            }
            base.OnNavigatedTo(e);
        }

        #endregion 方法
    }
}