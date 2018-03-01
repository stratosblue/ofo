using Common.Ofo.Entity.Result;
using OfoLight.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OfoLight.View
{
    /// <summary>
    /// 确认支付页面
    /// </summary>
    public sealed partial class ConfirmPaymentView : Page
    {
        #region 属性

        public ConfirmPaymentViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 确认支付页面
        /// </summary>
        public ConfirmPaymentView()
        {
            this.InitializeComponent();
            ViewModel = new ConfirmPaymentViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is UnLockCarInfo unlockinfo)
            {
                ViewModel.UnLockCarInfo = unlockinfo;
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.OnResumingAsync().NoWarning();
            }
        }

        #endregion 方法
    }
}