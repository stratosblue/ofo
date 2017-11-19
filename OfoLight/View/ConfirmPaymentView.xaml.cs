using Common.Ofo.Entity.Result;
using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    /// <summary>
    /// 确认支付页面
    /// </summary>
    public sealed partial class ConfirmPaymentView : Page
    {
        public ConfirmPaymentViewModel ViewModel { get; set; }

        /// <summary>
        /// 确认支付页面
        /// </summary>
        public ConfirmPaymentView()
        {
            this.InitializeComponent();
            ViewModel = new ConfirmPaymentViewModel();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is UnLockCarInfo unlockinfo)
            {
                ViewModel.UnLockCarInfo = unlockinfo;
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.OnResumingAsync();
            }
        }
    }
}
