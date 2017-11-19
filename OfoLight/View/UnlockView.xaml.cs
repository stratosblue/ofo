using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    /// <summary>
    /// 解锁页。
    /// </summary>
    public sealed partial class UnlockView : Page
    {
        private UnlockViewModel ViewModel { get; set; }

        /// <summary>
        /// 解锁页。
        /// </summary>
        public UnlockView()
        {
            this.InitializeComponent();
            ViewModel = new UnlockViewModel();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string carNumber) //参数为true时跳向扫描页
            {
                ViewModel.CarNumber = carNumber;
                ViewModel.UnlockCarCommand.Execute(null);
            }
        }
    }
}
