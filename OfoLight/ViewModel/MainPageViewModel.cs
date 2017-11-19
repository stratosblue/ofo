using Amap.Web;
using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 主页面的ViewModel
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        /// <summary>
        /// 最后一次获取活动的时间
        /// </summary>
        static DateTime LastGetActivityTime { get; set; }

        #region 属性
        /// <summary>
        /// 缩放等级
        /// </summary>
        private double _zoomLevel;

        /// <summary>
        /// 缩放等级
        /// </summary>
        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                if (_zoomLevel != value)
                {
                    _zoomLevel = value;
                    NotifyPropertyChanged("ZoomLevel");
                }
            }
        }

        /// <summary>
        /// 地理位置
        /// </summary>
        private Geopoint _geoPosition;

        /// <summary>
        /// 地理位置
        /// </summary>
        public Geopoint GeoPosition
        {
            get { return _geoPosition; }
            set
            {
                {
                    if (_geoPosition != value)
                    {
                        _geoPosition = value;
                        NotifyPropertyChanged("GeoPosition");
                    }
                }
            }
        }

        private bool _isPositioning = false;

        /// <summary>
        /// 是否正在定位
        /// </summary>
        public bool IsPositioning
        {
            get { return _isPositioning; }
            set
            {
                _isPositioning = value;
                NotifyPropertyChanged("IsPositioning");
            }
        }


        /// <summary>
        /// 地图控件
        /// </summary>
        public MapControl Map { get; set; }

        /// <summary>
        /// 所在位置的标记
        /// </summary>
        private MapIcon LocationMarker { get; set; } = new MapIcon() { ZIndex = 9999, NormalizedAnchorPoint = new Point(1, 1) };

        /// <summary>
        /// 地图API
        /// </summary>
        private AmapWebAPIs AmapWebApi { get; set; }

        private Visibility _blueBarButtonVisibility = Visibility.Collapsed;

        /// <summary>
        /// BlueBar按钮显示状态
        /// </summary>
        public Visibility BlueBarButtonVisibility
        {
            get { return _blueBarButtonVisibility; }
            set
            {
                _blueBarButtonVisibility = value;
                NotifyPropertyChanged("BlueBarButtonVisibility");
            }
        }

        private Visibility _blueBarVisibility = Visibility.Collapsed;

        /// <summary>
        /// BlueBar内容显示状态
        /// </summary>
        public Visibility BlueBarVisibility
        {
            get { return _blueBarVisibility; }
            set
            {
                _blueBarVisibility = value;
                NotifyPropertyChanged("BlueBarVisibility");
            }
        }

        private BlueBarInfo _blueBarInfo;

        public BlueBarInfo BlueBarInfo
        {
            get { return _blueBarInfo; }
            set
            {
                _blueBarInfo = value;
                NotifyPropertyChanged("BlueBarInfo");
            }
        }

        #endregion

        /// <summary>
        /// 使用网页版命令
        /// </summary>
        public ICommand OpenWithEdgeCommand { get; set; }

        /// <summary>
        /// 重定位命令
        /// </summary>
        public ICommand ReLocationCommand { get; set; }

        /// <summary>
        /// BlueBar显示命令
        /// </summary>
        public ICommand BlueBarVisibilityCommand { get; set; }

        /// <summary>
        /// 主页面的ViewModel
        /// </summary>
        public MainPageViewModel(MapControl map) : base(false)
        {
            Map = map;
            CanExitApplication = true;
            var amapConfig = new AmapConfig()
            {
                AppName = "",
                Key = "0afcd8a0b0fe5b9b89469e3531dc23ea",
                Csid = "71C11230-EBD6-4E28-A77C-85A6F1970A70",
                LogVersion = 2.0F,
                Platform = "JS",
                SdkVersion = 1.3F
            };

            AmapWebApi = new AmapWebAPIs(amapConfig);

            OpenWithEdgeCommand = new RelayCommand(async (state) =>
            {
                if ((await MessageDialogUtility.ShowMessageAsync("确定要使用Web版本吗？", "确认")) == MessageDialogResult.OK)
                {
                    ContentPageArgs args = new ContentPageArgs()
                    {
                        Name = "ofo共享单车",
                        ContentElement = new WebPageContentView(Global.MAIN_WEBPAGE_URL)
                    };

                    TryNavigate(typeof(ContentPageView), args);
                }
            });

            ReLocationCommand = new RelayCommand(async (state) =>
            {
                await LocationNowAsync();
            });

            BlueBarVisibilityCommand = new RelayCommand((state) =>
            {
                if (BlueBarButtonVisibility == Visibility.Collapsed)
                {
                    BlueBarButtonVisibility = Visibility.Visible;
                    BlueBarVisibility = Visibility.Collapsed;
                }
                else
                {
                    BlueBarButtonVisibility = Visibility.Collapsed;
                    BlueBarVisibility = Visibility.Visible;
                }
            });

            var loadTask = InitializationAsync();
        }

        /// <summary>
        /// 初始化ViewModel
        /// </summary>
        /// <returns></returns>
        protected override async Task InitializationAsync()
        {
            ZoomLevel = 17.5;

            LocationMarker.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/icons/map_marker.png"));

            var freshBlueBarTask = FreshBlueBar();

            await CheckUnfinishedOrder();
        }

        /// <summary>
        /// 定位以及 刷新Bluebar
        /// </summary>
        /// <returns></returns>
        private async Task FreshBlueBar()
        {
            //先定位以便获取位置信息
            await LocationNowAsync();

            var blueBarResult = await OfoApi.GetBlueBarAsync(GeoPosition.Position);
            if (await CheckOfoApiResult(blueBarResult))
            {
                BlueBarInfo = blueBarResult.Data;
                if (!string.IsNullOrEmpty(blueBarResult.Data.Text))
                {
                    BlueBarButtonVisibility = Visibility.Visible;
                    if (BlueBarInfo.MainType == 1)
                    {
                        BlueBarButtonVisibility = Visibility.Collapsed;
                        BlueBarVisibility = Visibility.Visible;
                    }
                }
            }
        }

        protected override async void NavigationActionAsync(object state)
        {
            if (state is string param)
            {
                switch (param)
                {
                    case "UsingCar":
                        TryNavigate(typeof(ScanQrcodeView));
                        break;
                    case "UserCenter":
                        TryNavigate(typeof(UserCenterView));
                        break;
                    case "ActivityCenter":
                        ContentPageArgs args = new ContentPageArgs()
                        {
                            Name = "我的消息",
                            ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?DiscoverPage")
                        };
                        TryNavigate(typeof(ContentPageView), args);
                        //TryNavigate(typeof(ActivityCenterView));
                        break;
                    case "BlueBar":
                        if (BlueBarInfo.MainType == 1 && BlueBarInfo.Text.Contains("认证"))
                        {
                            var identificationResult = await OfoApi.GetIdentificationInfoAsync(GeoPosition.Position);
                            if (await CheckOfoApiResult(identificationResult))
                            {
                                ContentPageArgs blueBarArgs = new ContentPageArgs()
                                {
                                    Name = "ofo小黄车",
                                    ContentElement = new WebPageContentView($"https://common.ofo.so/newdist/?Identification&~result={identificationResult.SourceHtml}&~lat={GeoPosition.Position.Latitude}&~lng={GeoPosition.Position.Longitude}"),
                                };
                                TryNavigate(typeof(ContentPageView), blueBarArgs);
                            }
                        }
                        else
                        {
                            ContentPageArgs blueBarArgs = new ContentPageArgs()
                            {
                                Name = "ofo小黄车",
                                ContentElement = new WebPageContentView(BlueBarInfo.Action),
                            };
                            TryNavigate(typeof(ContentPageView), blueBarArgs);
                        }
                        break;
                    case "MainPageReport":
                        ContentPageArgs args1 = new ContentPageArgs()
                        {
                            Name = "我的客服",
                            ContentElement = new WebPageContentView("https://common.ofo.so/about/customer/")
                        };
                        TryNavigate(typeof(ContentPageView), args1);
                        break;
                }
            }
        }

        /// <summary>
        /// 定位
        /// </summary>
        /// <returns></returns>
        private async Task LocationNowAsync()
        {
            if (IsPositioning)
            {
                return;
            }
            IsPositioning = true;
            try
            {
                //清除显示
                Map?.MapElements?.Clear();
                LocationMarker.Location = GeoPosition;
                Map.MapElements.Add(LocationMarker);

                GeoPosition = await PositionUtility.GetFixedGeopointAsync();

                var nearByOfoCarsResult = await OfoApi.GetNearByOfoCarAsync(GeoPosition.Position);
                if (await CheckOfoApiResult(nearByOfoCarsResult))
                {
                    var carImageStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/icons/icon_bike_nearby.png"));

                    LocationMarker.Location = GeoPosition;

                    for (int i = 0; i < nearByOfoCarsResult.Data?.cars?.Count; i++)
                    {
                        var bicycle = nearByOfoCarsResult.Data.cars[i];
                        MapIcon bicycleIcon = new MapIcon
                        {
                            Image = carImageStreamReference,
                            Location = new Geopoint(new BasicGeoposition() { Latitude = bicycle.lat, Longitude = bicycle.lng })
                        };
                        Map.MapElements.Add(bicycleIcon);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsPositioning = false;
            }
        }

        /// <summary>
        /// 从挂起恢复时
        /// </summary>
        public override async Task OnResumingAsync()
        {
            await CheckUnfinishedOrder();
        }

        /// <summary>
        /// 检查当前活动是否需要显示
        /// </summary>
        public async void CheckCurrentActivitiesAsync()
        {
            if (Global.AppConfig.LastShowActivityTime < DateTime.Now.AddHours(-3))
            {
                await GetCurrentActivities();
            }
        }

        /// <summary>
        /// 检查未完成订单
        /// </summary>
        /// <returns></returns>
        private async Task CheckUnfinishedOrder(int tryTimes = 2)
        {
            var unfinishedOrder = await OfoApi.GetUnfinishedOrderAsync();
            if (await CheckOfoApiResultHttpStatus(unfinishedOrder))
            {
                if (unfinishedOrder.ErrorCode == 30005)
                {
                    if (unfinishedOrder.Data.egt == 0)  //还在骑行，获取信息
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
                        if (isSavedLastOrder || unfinishedOrder.Data.Second >= 120)    //保存了订单信息或者现在已经不需要密码
                        {
                            unfinishedOrder.Data.Second += 1;   //+1S
                            await TryNavigateAsync(typeof(UnlockedView), unfinishedOrder.Data);
                        }
                        else
                        {
                            //TODO 没有存储，这个要怎么取才好
                            await TryNavigateAsync(typeof(WebPageView), Global.MAIN_WEBPAGE_URL);
                        }
                    }
                    else if (unfinishedOrder.Data.egt == 1)  //等待确认付款
                    {
                        await TryNavigateAsync(typeof(ConfirmPaymentView), unfinishedOrder.Data);
                    }
                }
                else    //没有未完成的订单，获取活动
                {
                    await GetCurrentActivities();
                }
            }
            else
            {
                if (tryTimes > 0)
                {
                    await CheckUnfinishedOrder(tryTimes--);
                }
            }
        }

        /// <summary>
        /// 获取当前活动
        /// </summary>
        /// <param name="tryTimes"></param>
        /// <returns></returns>
        private async Task GetCurrentActivities(int tryTimes = 2)
        {
            if (LastGetActivityTime > DateTime.Now.AddMinutes(-5))  //三分钟以内不重复获取
            {
                return;
            }

            var advertisementResult = await OfoApi.GetAdvertisementsAsync(await PositionUtility.GetUnFixBasicPositionAsync());
            if (await CheckOfoApiResult(advertisementResult))
            {
                LastGetActivityTime = DateTime.Now;
                //缓存起始屏幕splash
                var cacheSplashTask = Task.Run(async () =>
                {
                    try
                    {
                        if (advertisementResult?.Data?.splash?.Count > 0)
                        {
                            var splashInfo = advertisementResult.Data.splash[0];

                            if (!splashInfo.ImgName.Equals(Global.AppConfig.LastCacheSplashFileName))  //缓存的不是当前Splash
                            {
                                if (await LocalCacheUtility.CacheHttpFileAsync(splashInfo.ImgName, splashInfo.ImgUrl))
                                {
                                    Global.AppConfig.LastCacheSplashFileName = splashInfo.ImgName;
                                    Global.AppConfig.CacheSplashExpire = VariousUtility.TimeStampToDateTime(splashInfo.expire * 10000L);
                                    Global.SaveAppConfig();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                });

                var activity = advertisementResult.Data.activity?.Count > 0 ? advertisementResult.Data.activity[0] : null;

                if (activity != null)
                {
                    if (Global.AppConfig.LastShowActivityId == activity.id && Global.AppConfig.LastShowActivityTime > DateTime.Now.AddHours(-3))    //今天已经显示过当前活动，则不再显示
                    {
                        return;
                    }
                    else    //显示活动
                    {
                        ActivityPopupContentView activityPopupContentView = new ActivityPopupContentView();

                        ActivityPopupContentViewModel popupContentViewModel = new ActivityPopupContentViewModel()
                        {
                            Activity = activity,
                        };

                        await ShowContentNotifyAsync(activityPopupContentView, popupContentViewModel);
                    }
                }
            }
            else
            {
                if (tryTimes-- > 0)
                {
                    await GetCurrentActivities(tryTimes);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
