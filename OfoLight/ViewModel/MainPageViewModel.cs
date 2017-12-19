using Amap.Web;
using Amap.Web.Entity.Result;
using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 主页面的ViewModel
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        #region 属性

        /// <summary>
        /// BlueBar显示命令
        /// </summary>
        public ICommand BlueBarVisibilityCommand { get; set; }

        /// <summary>
        /// 使用网页版命令
        /// </summary>
        public ICommand OpenWithEdgeCommand { get; set; }

        /// <summary>
        /// 重定位命令
        /// </summary>
        public ICommand ReLocationCommand { get; set; }

        /// <summary>
        /// 最后一次获取活动的时间
        /// </summary>
        private static DateTime LastGetActivityTime { get; set; }

        #endregion 属性



        #region 属性
        private Visibility _blueBarButtonVisibility = Visibility.Collapsed;

        private BlueBarInfo _blueBarInfo;

        private Visibility _blueBarVisibility = Visibility.Collapsed;

        /// <summary>
        /// 车辆图标-大
        /// </summary>
        private IRandomAccessStreamReference _carBigImageStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/icons/img_bike_nearby_big.png"));

        /// <summary>
        /// 车辆图标
        /// </summary>
        private IRandomAccessStreamReference _carImageStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/icons/icon_bike_nearby.png"));

        private Geopoint _destinationGeoPosition;

        /// <summary>
        /// 地理位置
        /// </summary>
        private Geopoint _geoPosition;

        private bool _isPositioning = false;

        private bool _isShowDestinationInfo;

        private bool _isShowMapCenterFlag = true;

        /// <summary>
        /// 最后更改了图片的MapIcon
        /// </summary>
        private MapIcon _lastChangeImgMapIcon = null;

        /// <summary>
        /// 最后一次刷新附近车辆的地点
        /// </summary>
        private BasicGeoposition _lastRefreshGeoposition = new BasicGeoposition();

        /// <summary>
        /// 目标位置移动timer
        /// </summary>
        private Timer _locationMoveTimer = null;

        private Geopoint _originGeoPosition;

        private string _walkingDistance;

        private string _walkingTime;

        /// <summary>
        /// 缩放等级
        /// </summary>
        private double _zoomLevel;

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

        public BlueBarInfo BlueBarInfo
        {
            get { return _blueBarInfo; }
            set
            {
                _blueBarInfo = value;
                NotifyPropertyChanged("BlueBarInfo");
            }
        }

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

        /// <summary>
        /// 目的地位置
        /// </summary>
        public Geopoint DestinationGeoPosition
        {
            get { return _destinationGeoPosition; }
            set
            {
                _destinationGeoPosition = value;
                NotifyPropertyChanged("DestinationGeoPosition");
            }
        }

        /// <summary>
        /// 地理位置
        /// </summary>
        public Geopoint GeoPosition
        {
            get { return _geoPosition; }
            set
            {
                if (_geoPosition != value)
                {
                    _geoPosition = value;
                    NotifyPropertyChanged("GeoPosition");
                }
            }
        }

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
        /// 是否显示目的地信息
        /// </summary>
        public bool IsShowDestinationInfo
        {
            get { return _isShowDestinationInfo; }
            set
            {
                _isShowDestinationInfo = value;
                NotifyPropertyChanged("IsShowDestinationInfo");
            }
        }

        /// <summary>
        /// 是否显示地图中心标志
        /// </summary>
        public bool IsShowMapCenterFlag
        {
            get { return _isShowMapCenterFlag; }
            set
            {
                _isShowMapCenterFlag = value;
                NotifyPropertyChanged("IsShowMapCenterFlag");
            }
        }

        /// <summary>
        /// 地图控件
        /// </summary>
        public MapControl Map { get; set; }

        /// <summary>
        /// 行走起始地点
        /// </summary>
        public Geopoint OriginGeoPosition
        {
            get { return _originGeoPosition; }
            set
            {
                _originGeoPosition = value;
                NotifyPropertyChanged("OriginGeoPosition");
            }
        }

        /// <summary>
        /// 行走距离
        /// </summary>
        public string WalkingDistance
        {
            get { return _walkingDistance; }
            set
            {
                _walkingDistance = value;
                NotifyPropertyChanged("WalkingDistance");
            }
        }

        /// <summary>
        /// 行走路径
        /// </summary>
        public MapPolyline WalkingPolyLine { get; private set; }

        /// <summary>
        /// 目的地行走时间
        /// </summary>
        public string WalkingTime
        {
            get { return _walkingTime; }
            set
            {
                _walkingTime = value;
                NotifyPropertyChanged("WalkingTime");
            }
        }

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
        /// 地图API
        /// </summary>
        private AmapWebAPIs AmapWebApi { get; set; }

        #endregion 属性

        /// <summary>
        /// 主页面的ViewModel
        /// </summary>
        public MainPageViewModel(MapControl map) : base(false)
        {
            Map = map;
            Map.MapTapped += MapTappedAsync;
            Map.CenterChanged += MapCenterChanged;
            GeoPosition = Map.Center;

            CanExitApplication = true;
            var amapConfig = new AmapConfig()
            {
                AppName = "common.ofo.so",
                Key = "0afcd8a0b0fe5b9b89469e3531dc23ea",
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

            _locationMoveTimer = new Timer(LocationMoveTimerCallBack, null, Timeout.Infinite, Timeout.Infinite);

            var loadTask = InitializationAsync();
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

        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// 从挂起恢复时
        /// </summary>
        public override async Task OnResumingAsync()
        {
            await CheckUnfinishedOrder();
        }

        /// <summary>
        /// 初始化ViewModel
        /// </summary>
        /// <returns></returns>
        protected override async Task InitializationAsync()
        {
            ZoomLevel = 17.5;

            var freshBlueBarTask = FreshBlueBar();

            await CheckUnfinishedOrder();
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

        protected override void TryGoBack()
        {
            if (!IsShowMapCenterFlag)   //没有显示地图中心标志时，显示地图中心标志
            {
                IsShowMapCenterFlag = true;
                ClearMapWalkingAddOnInfo();
            }
            else
            {
                base.TryGoBack();
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
        /// 清除地图的附加显示信息
        /// </summary>
        private void ClearMapAddOnInfo()
        {
            //清除显示
            Map?.MapElements?.Clear();

            ClearMapWalkingAddOnInfo();
        }

        /// <summary>
        /// 清除地图的行走附加信息
        /// </summary>
        private void ClearMapWalkingAddOnInfo()
        {
            try
            {
                //清空行走信息
                WalkingTime = string.Empty;
                WalkingDistance = string.Empty;
                IsShowDestinationInfo = false;

                Map.MapElements.Remove(WalkingPolyLine);

                if (_lastChangeImgMapIcon != null)  //恢复之前改变为大图标的MapIcon
                {
                    _lastChangeImgMapIcon.Image = _carImageStreamReference;
                    _lastChangeImgMapIcon = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 定位以及 刷新Bluebar
        /// </summary>
        /// <returns></returns>
        private async Task FreshBlueBar()
        {
            //先定位以便获取位置信息
            //await LocationNowAsync();
            GeoPosition = await PositionUtility.GetFixedGeopointAsync();

            Map.Center = GeoPosition;

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

        /// <summary>
        /// 获取目标地点附近的自行车
        /// </summary>
        /// <param name="geoposition">目标地点</param>
        /// <returns></returns>
        private async Task GetPositionBicycles(BasicGeoposition geoposition)
        {
            _lastRefreshGeoposition = geoposition;
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
            {
                IsPositioning = true;

                try
                {
                    //清除显示
                    ClearMapAddOnInfo();

                    var nearByOfoCarsResult = await OfoApi.GetNearByOfoCarAsync(geoposition);
                    if (await CheckOfoApiResult(nearByOfoCarsResult))
                    {
                        Point normalizedAnchorPoint = new Point(0.5, 0.8);

                        for (int i = 0; i < nearByOfoCarsResult.Data?.cars?.Count; i++)
                        {
                            var bicycle = nearByOfoCarsResult.Data.cars[i];
                            MapIcon bicycleIcon = new MapIcon
                            {
                                Image = _carImageStreamReference,
                                Location = new Geopoint(new BasicGeoposition() { Latitude = bicycle.lat, Longitude = bicycle.lng }),
                                NormalizedAnchorPoint = normalizedAnchorPoint,
                                ZIndex = 50,
                            };
                            Map.MapElements.Add(bicycleIcon);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await ShowNotifyAsync($"刷新车辆信息失败：{ex}");
                }
                finally
                {
                    IsPositioning = false;
                }
            });
        }

        /// <summary>
        /// 位置移动的Timer回调函数
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private async void LocationMoveTimerCallBack(object state)
        {
            if (IsPositioning || !IsShowMapCenterFlag)    //在定位，或者没有显示地图中心标志
            {
                return;
            }

            BasicGeoposition nowGeoposition = new BasicGeoposition();
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                nowGeoposition = Map.Center.Position;
            });

            //移动量超过一定值时，进行重新获取附近车辆
            if (Math.Abs(nowGeoposition.Latitude - _lastRefreshGeoposition.Latitude) > 0.0026 || Math.Abs(nowGeoposition.Longitude - _lastRefreshGeoposition.Longitude) > 0.0026)
            {
                await GetPositionBicycles(nowGeoposition);
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
            IsShowMapCenterFlag = true;

            try
            {
                ClearMapAddOnInfo();

                GeoPosition = await PositionUtility.GetFixedGeopointAsync();

                Map.Center = GeoPosition;

                await GetPositionBicycles(GeoPosition.Position);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 地图中心移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MapCenterChanged(MapControl sender, object args)
        {
            _locationMoveTimer.Change(1200, Timeout.Infinite);
        }

        /// <summary>
        /// 点击地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void MapTappedAsync(MapControl sender, MapInputEventArgs args)
        {
            if (!IsShowMapCenterFlag)
            {
                return;
            }
            var mapElements = sender.FindMapElementsAtOffset(args.Position);
            var clickItem = mapElements.FirstOrDefault();

            if (clickItem is MapIcon mapIcon)   //如果点击的是MapIcon
            {
                Map.MapElements.Remove(WalkingPolyLine);

                IsShowDestinationInfo = false;

                if (_lastChangeImgMapIcon != null)  //恢复之前改变为大图标的MapIcon
                {
                    _lastChangeImgMapIcon.Image = _carImageStreamReference;
                    _lastChangeImgMapIcon = null;
                }

                OriginGeoPosition = Map.Center;
                var origin = OriginGeoPosition.Position;
                var destination = mapIcon.Location.Position;

                //请求路线
                var walkingRouteResult = await AmapWebApi.GetGetWalkingRouteAsync(origin, destination);

                if (walkingRouteResult?.route?.paths?.FirstOrDefault() is PathsItem walkingPath)    //有行走路径
                {
                    IsShowMapCenterFlag = false;
                    mapIcon.Image = _carBigImageStreamReference;
                    _lastChangeImgMapIcon = mapIcon;

                    //创建路径列表，并加入起始点
                    var paths = new List<BasicGeoposition> { origin };

                    foreach (var step in walkingPath.steps) //遍历行走步骤
                    {
                        if (step.PolyLineList != null)
                        {
                            paths.AddRange(step.PolyLineList);
                        }
                    }
                    paths.Add(destination);

                    WalkingPolyLine = new MapPolyline
                    {
                        Path = new Geopath(paths),
                        StrokeColor = Color.FromArgb(255, 107, 182, 82),
                        StrokeThickness = 4.5,
                        ZIndex = 1
                    };

                    Map.MapElements.Add(WalkingPolyLine);

                    //显示行走信息
                    DestinationGeoPosition = mapIcon.Location;
                    WalkingTime = walkingPath.duration;
                    WalkingDistance = walkingPath.distance;
                    IsShowDestinationInfo = true;
                }
            }
        }
    }
}