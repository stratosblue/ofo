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
        MainPageViewModel ViewModel { get; set; }

        /// <summary>
        /// 主页面
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            ViewModel = new MainPageViewModel(Map);
            DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await StatusBarUtility.ShowAsync();

            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.CheckCurrentActivitiesAsync();
            }
            base.OnNavigatedTo(e);
        }
    }
}
