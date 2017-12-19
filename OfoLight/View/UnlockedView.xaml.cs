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
        #region 属性

        public UnlockedViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 已解锁车界面
        /// </summary>
        public UnlockedView()
        {
            this.InitializeComponent();
            ViewModel = new UnlockedViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel?.StopTimer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is UnLockCarInfo unlockInfo)
            {
                ViewModel.UnLockCarInfo = unlockInfo;
            }
        }

        #endregion 方法
    }
}