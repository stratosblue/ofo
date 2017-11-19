using OfoLight.ViewModel;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    public sealed partial class ScanQrcodeView : Page
    {
        private ScanQrcodeViewModel ViewModel { get; set; }

        public ScanQrcodeView()
        {
            this.InitializeComponent();
            ViewModel = new ScanQrcodeViewModel();
            DataContext = ViewModel;
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await ViewModel.CleanupCameraAsync();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.InitVideoCapture();
        }
    }
}
