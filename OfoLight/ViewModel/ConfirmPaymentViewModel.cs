using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.View;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 确认支付页面VM
    /// </summary>
    public class ConfirmPaymentViewModel : BaseViewModel
    {
        #region 字段

        private string _couponUseInfo = "未使用优惠";

        private float _price;

        private float _realPrice;

        private UnLockCarInfo _unLockCarInfo;

        /// <summary>
        /// 使用的红包ID
        /// </summary>
        private long _usePacketId = 0;

        private WalletInfo _walletInfo;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 选择抵扣类型命令
        /// </summary>
        public ICommand ChoseDiscountTypeCommand { get; set; }

        /// <summary>
        /// 优惠券使用信息
        /// </summary>
        public string CouponUseInfo
        {
            get { return _couponUseInfo; }
            set
            {
                _couponUseInfo = value;
                NotifyPropertyChanged("CouponUseInfo");
            }
        }

        /// <summary>
        /// 支付命令
        /// </summary>
        public ICommand PayCommand { get; set; }

        /// <summary>
        /// 骑行价格
        /// </summary>
        public float Price
        {
            get { return _price; }
            set
            {
                _price = value;
                NotifyPropertyChanged("Price");
            }
        }

        /// <summary>
        /// 真正支付价格
        /// </summary>
        public float RealPrice
        {
            get { return _realPrice; }
            set
            {
                _realPrice = value < 0 ? 0 : value;
                NotifyPropertyChanged("RealPrice");
            }
        }

        public UnLockCarInfo UnLockCarInfo
        {
            get { return _unLockCarInfo; }
            set
            {
                _unLockCarInfo = value;
                Price = _unLockCarInfo.Price;
                RealPrice = Price;

                if (!string.IsNullOrEmpty(_unLockCarInfo.Card?.CardId))
                {
                    CouponUseInfo = $"骑行卡：{_unLockCarInfo.Card.Content}";
                }
                else
                {
                    UsePacket(_unLockCarInfo.Packet);
                }
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

        /// <summary>
        /// 确认支付页面VM
        /// </summary>
        public ConfirmPaymentViewModel()
        {
            PayCommand = new RelayCommand(async state =>
            {
                await Payment();
            });

            ChoseDiscountTypeCommand = new RelayCommand(async state =>
            {
                if (!string.IsNullOrEmpty(UnLockCarInfo.Card?.CardId))  //CardId不为空则为月卡，直接返回
                {
                    return;
                }

                //获取红包列表
                var redPacketList = await OfoApi.GetRedPacketListAsync();
                if (await CheckOfoApiResult(redPacketList))
                {
                    var list = redPacketList.Data;

                    if (list?.Length > 0)
                    {
                        await ShowContentNotifyAsync(new PacketChosePupopContentView(), new PacketChosePupopContentViewModel(args =>
                        {
                            UsePacket(args as RedPacketInfo);
                        }, list));
                    }
                    else
                    {
                        await ShowNotifyAsync("没有找到可用的优惠券");
                    }
                }
            });
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
            }
        }

        /// <summary>
        /// 进行支付
        /// </summary>
        /// <returns></returns>
        private async Task Payment()
        {
            var payResult = await OfoApi.ConfirmToPayAsync(UnLockCarInfo.OrderNumber, _usePacketId, UnLockCarInfo.Card?.CardId);
            if (await CheckOfoApiResult(payResult))
            {
                await ShowNotifyAsync(payResult?.Message);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    base.TryGoBack();
                });
                if (!string.IsNullOrWhiteSpace(payResult?.Data?.url))   //检查是否有红包并弹出
                {
                    RedPacketPupopContentView redPacketPupopContentView = new RedPacketPupopContentView();
                    var redPacketPupopContentViewModel = new RedPacketPupopContentViewModel()
                    {
                        PaymentInfo = payResult.Data,
                    };

                    await ShowContentNotifyAsync(redPacketPupopContentView, redPacketPupopContentViewModel);
                }
            }
            else if (!string.IsNullOrEmpty(payResult.Message) && (payResult.Message.Contains("已支付") || payResult.ErrorCode == 40006))   //已支付，返回
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    base.TryGoBack();
                });
            }
        }

        /// <summary>
        /// 使用红包
        /// </summary>
        /// <param name="packet"></param>
        private void UsePacket(RedPacketInfo packet)
        {
            if (packet != null && packet.PacketId > 0)
            {
                _usePacketId = packet.PacketId;

                if (!string.IsNullOrEmpty(packet.CouponId) || packet.opp < 0)  //包天券有CouponId或者opp<0？？
                {
                    RealPrice = 0;
                    CouponUseInfo = $"包天券：免费";
                }
                else    //红包？
                {
                    var amount = packet.Amounts;
                    RealPrice = Price - amount;
                    CouponUseInfo = $"优惠券：{amount} 元";
                }
            }
            else
            {
                RealPrice = Price;
                _usePacketId = 0;
                CouponUseInfo = "未使用优惠";
            }
        }

        #endregion 方法
    }
}