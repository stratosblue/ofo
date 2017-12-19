using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.View;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfoLight.ViewModel
{
    public class ConfirmPaymentViewModel : BaseViewModel
    {
        #region 字段

        private UnLockCarInfo _unLockCarInfo;

        private WalletInfo _walletInfo;

        #endregion 字段

        #region 属性

        public ICommand PayCommand { get; set; }

        public UnLockCarInfo UnLockCarInfo
        {
            get { return _unLockCarInfo; }
            set
            {
                _unLockCarInfo = value;
                NotifyPropertyChanged("UnLockCarInfo");
            }
        }

        public WalletInfo WalletInfo
        {
            get { return _walletInfo; }
            set
            {
                _walletInfo = value;
                NotifyPropertyChanged("WalletInfo");
            }
        }

        #endregion 属性

        #region 构造函数

        public ConfirmPaymentViewModel()
        {
            PayCommand = new RelayCommand(ConfirmPay);
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 从挂起恢复
        /// </summary>
        public override async Task OnResumingAsync()
        {
            await CheckOrderStatusAsync();
        }

        protected override async Task InitializationAsync()
        {
            var walletResult = await OfoApi.GetWalletInfoAsync();
            if (await CheckOfoApiResult(walletResult))
            {
                WalletInfo = walletResult.Data;
            }
        }

        protected override void NavigationActionAsync(object state)
        {
            if (state is string param)
            {
                if (state.Equals("WebPay"))
                {
                    ContentPageArgs args = new ContentPageArgs()
                    {
                        Name = "ofo小黄车",
                        ContentElement = new WebPageContentView(Global.MAIN_WEBPAGE_URL),
                    };
                    TryNavigate(typeof(ContentPageView), args);
                }
            }
        }

        protected override void TryGoBack()
        {
            //此页面不允许返回
        }

        /// <summary>
        /// 检查订单状态
        /// </summary>
        /// <returns></returns>
        private async Task CheckOrderStatusAsync()
        {
            if (UnLockCarInfo == null)
            {
                var unLockCarInfoResult = await OfoApi.GetUnfinishedOrderAsync();
                if (await CheckOfoApiResult(unLockCarInfoResult))
                {
                    UnLockCarInfo = unLockCarInfoResult.Data;
                }

                if (UnLockCarInfo == null)
                {
                    return;
                }
            }
            var payResult = await OfoApi.ConfirmToPayAsync(UnLockCarInfo.OrderNumber, 0);
            if (await CheckOfoApiResult(payResult))
            {
                await ShowNotifyAsync(payResult?.Message);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    base.TryGoBack();
                });
                if (!string.IsNullOrWhiteSpace(payResult?.Data?.url))
                {
                    RedPacketPupopContentView redPacketPupopContentView = new RedPacketPupopContentView();
                    var redPacketPupopContentViewModel = new RedPacketPupopContentViewModel()
                    {
                        PaymentInfo = payResult.Data,
                    };

                    await ShowContentNotifyAsync(redPacketPupopContentView, redPacketPupopContentViewModel);
                }
            }
            else if (!string.IsNullOrEmpty(payResult.Message) && (payResult.Message.Contains("已支付") || payResult.ErrorCode == 40006))
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    base.TryGoBack();
                });
            }
        }

        private async void ConfirmPay(object state)
        {
            await CheckOrderStatusAsync();
        }

        #endregion 方法
    }
}