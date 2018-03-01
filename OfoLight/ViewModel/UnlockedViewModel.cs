using Common.Ofo.Entity.Result;
using OfoLight.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 已解锁的VM
    /// </summary>
    public class UnlockedViewModel : BaseViewModel
    {
        #region 字段

        private TimeSpan _addMinusTimeSpan = new TimeSpan(0, 0, -1);
        private TimeSpan _addTimeSpan = new TimeSpan(0, 0, 1);
        private bool _isEnableFinish = true;
        private List<char> _password;
        private int _repairTimeOut;
        private TimeSpan _ridingTime;
        private Timer _ridingTimer = null;
        private Visibility _timerPanelVisibility;
        private UnLockCarInfo _unLockCarInfo;

        /// <summary>
        /// 结束用车命令
        /// </summary>
        public RelayCommand FinishUsingCarCommand { get; set; }

        /// <summary>
        /// 是否可以结束
        /// </summary>
        public bool IsEnableFinish
        {
            get { return _isEnableFinish; }
            set
            {
                _isEnableFinish = value;
                NotifyPropertyChanged("IsEnableFinish");
            }
        }

        public List<char> Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }

        /// <summary>
        /// 报修命令
        /// </summary>
        public RelayCommand RepairCommand { get; set; }

        public int RepairTimeOut
        {
            get { return _repairTimeOut; }
            set
            {
                _repairTimeOut = value;
                NotifyPropertyChanged("RepairTimeOut");
            }
        }

        public TimeSpan RidingTime
        {
            get { return _ridingTime; }
            set
            {
                _ridingTime = value;
                NotifyPropertyChanged("RidingTime");
            }
        }

        /// <summary>
        /// 计时面板的显示
        /// </summary>
        public Visibility TimerPanelVisibility
        {
            get { return _timerPanelVisibility; }
            set
            {
                _timerPanelVisibility = value;
                NotifyPropertyChanged("TimerPanelVisibility");
            }
        }

        /// <summary>
        /// 解锁车信息
        /// </summary>
        public UnLockCarInfo UnLockCarInfo
        {
            get { return _unLockCarInfo; }
            set
            {
                _unLockCarInfo = value;

                if (value.Second >= 120)    //开始计时
                {
                    TimerPanelVisibility = Visibility.Visible;

                    RidingTime = new TimeSpan(0, 0, (int)value.Second - 120);
                }
                else    //还没有开始计时
                {
                    TimerPanelVisibility = Visibility.Collapsed;
                    Password = value.Password.ToList();
                    RepairTimeOut = UnLockCarInfo.RepairTime - (int)value.Second;
                }

                StartTimer();

                NotifyPropertyChanged("UnLockCarInfo");
            }
        }

        /// <summary>
        /// 解锁帮助命令
        /// </summary>
        public RelayCommand UnlockHelpCommand { get; set; }

        #endregion 字段

        #region 构造函数

        public UnlockedViewModel()
        {
            FinishUsingCarCommand = new RelayCommand(FinishUsingCarAsync);
            RepairCommand = new RelayCommand(RepairAsync);
            UnlockHelpCommand = new RelayCommand(UnlockHelpAsync);
        }

        #endregion 构造函数

        #region 析构函数

        ~UnlockedViewModel()
        {
            StopTimer();
        }

        #endregion 析构函数

        #region 方法

        /// <summary>
        /// 页面从挂起恢复
        /// </summary>
        public override async Task OnResumingAsync()
        {
            var unfinishedOrder = await OfoApi.GetUnfinishedOrderAsync();
            if (await CheckOfoApiResultHttpStatus(unfinishedOrder))
            {
                if (unfinishedOrder.ErrorCode == 30005)
                {
                    if (unfinishedOrder.Data.Egt == 0)  //还在骑行，获取信息
                    {
                        //有未完成订单
                        var isSavedLastOrder = unfinishedOrder.Data.OrderNumber.Equals(Global.AppConfig.LastOrderNum);
                        if (isSavedLastOrder)//储存了最后一次的订单信息
                        {
                            if (!string.IsNullOrWhiteSpace(Global.AppConfig.LastOrderPwd))
                            {
                                unfinishedOrder.Data.Password = Global.AppConfig.LastOrderPwd;
                            }
                        }
                        if (isSavedLastOrder && unfinishedOrder.Data.Second >= 120)    //或者现在已经不需要密码
                        {
                            unfinishedOrder.Data.Second += 1;   //+1S
                        }
                        UnLockCarInfo = unfinishedOrder.Data;
                    }
                    else if (unfinishedOrder.Data.Egt == 1)  //等待确认付款
                    {
                        await TryReplaceNavigateAsync(typeof(ConfirmPaymentView), unfinishedOrder.Data);
                    }
                }
                else    //没有未完成订单
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        //返回主页
                        base.TryGoBack();
                    });
                }
            }
        }

        /// <summary>
        /// 开始计时器
        /// </summary>
        public void StartTimer()
        {
            _ridingTimer?.Dispose();
            _ridingTimer = new Timer(RidingTimerCallBackAsync, null, 0, 1000);
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        public void StopTimer()
        {
            _ridingTimer?.Dispose();
        }

        protected override void TryGoBack()
        {
            //此页面不允许返回
        }

        /// <summary>
        /// 结束用车
        /// </summary>
        /// <param name="state"></param>
        private async void FinishUsingCarAsync(object state)
        {
            for (int i = 0; i < 2; i++)
            {
                var endRideResult = await OfoApi.EndRideAsync(UnLockCarInfo.OrderNumber.ToString());
                if (await CheckOfoApiResultHttpStatus(endRideResult))
                {
                    if (endRideResult.IsSuccess || endRideResult.ErrorCode == 40002)    //40002订单已结束
                    {
                        await TryReplaceNavigateAsync(typeof(ConfirmPaymentView), endRideResult.Data);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 报修
        /// </summary>
        /// <param name="state"></param>
        private async void RepairAsync(object state)
        {
            ReportRepairPopupContentView reportRepairPopupContentView = new ReportRepairPopupContentView();

            var repairPopupContentViewModel = new ReportRepairPopupContentViewModel(async args =>
            {
                await Task.Delay(1500);
                OnResumingAsync().NoWarning();
            }, UnLockCarInfo.Ordernum, UnLockCarInfo.IsGsmLock == 1);

            await ShowContentNotifyAsync(reportRepairPopupContentView, repairPopupContentViewModel);
        }

        /// <summary>
        /// Timer回调函数
        /// </summary>
        /// <param name="state"></param>
        private async void RidingTimerCallBackAsync(object state)
        {
            bool isRepairTimeOut = false;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (RepairTimeOut > 0)
                {
                    if (--RepairTimeOut == 0)  //报修计时结束
                    {
                        isRepairTimeOut = true;
                        StopTimer();
                    }
                }
                else
                {
                    RidingTime = RidingTime.Add(_addTimeSpan);
                }
            });

            if (isRepairTimeOut)
            {
                UnfinishedOrderResult unfinishedOrderResult = null;
                for (int i = 0; i < 2; i++)
                {
                    unfinishedOrderResult = await OfoApi.GetUnfinishedOrderAsync();
                    if (await CheckOfoApiResultHttpStatus(unfinishedOrderResult))
                    {
                        if (unfinishedOrderResult.ErrorCode == 30005)
                        {
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                            {
                                UnLockCarInfo = unfinishedOrderResult.Data;
                            });
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解锁帮助
        /// </summary>
        /// <param name="state"></param>
        private async void UnlockHelpAsync(object state)
        {
            UnLockGuideContentView unLockGuideContentView = new UnLockGuideContentView();
            var unLockGuidePopupContentViewModel = new UnLockGuidePopupContentViewModel();

            await ShowContentNotifyAsync(unLockGuideContentView, unLockGuidePopupContentViewModel);
        }

        #endregion 方法
    }
}