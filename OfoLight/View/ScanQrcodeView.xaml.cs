using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    public sealed partial class ScanQrcodeView : Page
    {
        #region 属性

        private ScanQrcodeViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        public ScanQrcodeView()
        {
            this.InitializeComponent();
            ViewModel = new ScanQrcodeViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.InitVideoCapture();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await ViewModel.CleanupCameraAsync();
        }

        #endregion 方法
    }
}