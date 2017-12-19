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
        #region 属性

        private UnlockViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 解锁页。
        /// </summary>
        public UnlockView()
        {
            this.InitializeComponent();
            ViewModel = new UnlockViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string carNumber) //参数为true时跳向扫描页
            {
                ViewModel.CarNumber = carNumber;
                ViewModel.UnlockCarCommand.Execute(null);
            }
        }

        #endregion 方法
    }
}