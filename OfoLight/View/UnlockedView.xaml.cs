using Common.Ofo.Entity.Result;
using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    /// <summary>
    /// 已解锁车界面
    /// </summary>
    public sealed partial class UnlockedView : Page
    {
        public UnlockedViewModel ViewModel { get; set; }

        /// <summary>
        /// 已解锁车界面
        /// </summary>
        public UnlockedView()
        {
            this.InitializeComponent();
            ViewModel = new UnlockedViewModel();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is UnLockCarInfo unlockInfo)
            {
                ViewModel.UnLockCarInfo = unlockInfo;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel?.StopTimer();
        }
    }
}
