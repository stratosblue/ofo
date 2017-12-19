using Common.Ofo.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 程序启动加载页VM
    /// </summary>
    public class LoadingViewModel : BaseViewModel
    {
        #region 属性

        private string _buttonTip = "跳过(3)";

        /// <summary>
        /// 显示Splash的TokenSource
        /// </summary>
        private CancellationTokenSource _showSplashTokenSource = new CancellationTokenSource();

        private Visibility _skipSplashButtonVisibility = Visibility.Visible;

        private BitmapImage _splashImage;

        /// <summary>
        /// 跳过启动画面按钮的字符串
        /// </summary>
        public string ButtonTip
        {
            get { return _buttonTip; }
            set
            {
                _buttonTip = value;
                NotifyPropertyChanged("ButtonTip");
            }
        }

        /// <summary>
        /// 跳过SplashButton可视状态
        /// </summary>
        public Visibility SkipSplashButtonVisibility
        {
            get { return _skipSplashButtonVisibility; }
            set
            {
                _skipSplashButtonVisibility = value;
                NotifyPropertyChanged("SkipSplashButtonVisibility");
            }
        }

        /// <summary>
        /// 跳过SkipSplashCommand命令
        /// </summary>
        public ICommand SkipSplashCommand { get; set; }

        /// <summary>
        /// 启动画面图片
        /// </summary>
        public BitmapImage SplashImage
        {
            get { return _splashImage; }
            set
            {
                _splashImage = value;
                NotifyPropertyChanged("SplashImage");
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 程序启动加载页VM
        /// </summary>
        public LoadingViewModel()
        {
            SkipSplashCommand = new RelayCommand((state) =>
            {
                _showSplashTokenSource.Cancel();
            });
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        protected override async Task InitializationAsync()
        {
            await ConfirmGeolocationAccessStatus();

            //检查登录状态
            var checkTokenTask = CheckSavedUserTokenAsync();

            //检查开始界面图片
            var splashStream = await GetSplashImageAsync();

            try
            {
                if (!_showSplashTokenSource.Token.IsCancellationRequested && SkipSplashButtonVisibility == Visibility.Visible)
                {
                    if (splashStream?.Size > 0)
                    {
                        SplashImage = new BitmapImage();
                        SplashImage.SetSource(splashStream);
                        await Task.Delay(600, _showSplashTokenSource.Token);
                        for (int i = 2; i >= 0; i--)
                        {
                            ButtonTip = $"跳过({i})";
                            await Task.Delay(1000, _showSplashTokenSource.Token);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                splashStream?.Dispose();
            }

            //不再显示跳过按钮
            SkipSplashButtonVisibility = Visibility.Collapsed;

            await checkTokenTask.ContinueWith(async tokenTask =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
                {
                    LoginStatus loginStatus = await tokenTask;

                    switch (loginStatus)
                    {
                        case LoginStatus.NetWorkFailed:
                            if (await MessageDialogUtility.ShowMessageAsync("网络访问异常，请检查您的网络状态，并重新尝试。", "出现了一些问题...", MessageDialogType.RetryCancel) == MessageDialogResult.Retry)
                            {
                                await InitializationAsync();
                            }
                            else
                            {
                                Application.Current.Exit();
                            }
                            break;

                        case LoginStatus.NoToken:   //没有Token
                        case LoginStatus.TokenExpire:   //Token过期
                            await TryReplaceNavigateAsync(typeof(NoLoginView));
                            break;

                        case LoginStatus.Logined:   //已登录
                            await TryReplaceNavigateAsync(typeof(MainPage));
                            break;

                        case LoginStatus.CheckFailed:
                        case LoginStatus.Default:
                        default:
                            if (await MessageDialogUtility.ShowMessageAsync("出现了未知的问题，要重试吗？", "出现了一些问题...", MessageDialogType.RetryCancel) == MessageDialogResult.Retry)
                            {
                                await InitializationAsync();
                            }
                            else
                            {
                                Application.Current.Exit();
                            }
                            break;
                    }
                });
            });
        }

        /// <summary>
        /// 检查已保存的用户Token
        /// </summary>
        /// <returns></returns>
        private async Task<LoginStatus> CheckSavedUserTokenAsync()
        {
            LoginStatus result = LoginStatus.Default;

            if (!NetworkStatusUtility.IsNetworkAvailable)
            {
                await ShowNotifyAsync("无法正常访问网络，请检查网络状态", new TimeSpan(0, 0, 5));
                result = LoginStatus.NetWorkFailed;
            }
            else
            {
                var token = Global.AppConfig.Token;
                OfoApi.CurUser.Token = token;
                ClientCookieManager.AddCookies(Global.COOKIE_DOMAIN, $"ofo-tokened={token}");

                //验证登录状态
                result = await OfoApi.CheckLoginStatus();
            }

            return result;
        }

        /// <summary>
        /// 确认位置访问权限
        /// </summary>
        /// <returns></returns>
        private async Task ConfirmGeolocationAccessStatus()
        {
            try
            {
                var geolocationAccessStatus = await Geolocator.RequestAccessAsync();

                switch (geolocationAccessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        break;

                    case GeolocationAccessStatus.Unspecified:
                    case GeolocationAccessStatus.Denied:
                        await MessageDialogUtility.ShowMessageAsync("软件必须获取位置权限才能够正常运行，请在“设置”-“隐私”-“位置”中允许软件访问位置信息后，再次重新运行应用。", "注意", MessageDialogType.OK);
                        Application.Current.Exit();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if ((ex.Message?.ToLower().Contains("cancel")).GetValueOrDefault(true))    //判定是否用户取消了步骤
                {
                    if ((await MessageDialogUtility.ShowMessageAsync("软件必须获取位置权限才能够正常运行，确认不允许软件访问位置信息吗？", "注意", MessageDialogType.OKCancel) == MessageDialogResult.OK))
                    {
                        Application.Current.Exit();
                    }
                    else
                    {
                        await ConfirmGeolocationAccessStatus();
                    }
                }
            }
        }

        /// <summary>
        /// 获取Splash图片
        /// </summary>
        /// <returns></returns>
        private async Task<IRandomAccessStream> GetSplashImageAsync()
        {
            try
            {
                var lastCacheSplashFileName = Global.AppConfig.LastCacheSplashFileName; //缓存的Splash文件名称
                bool isSplashExpired = false;   //Splash是否已过期

                if (Global.AppConfig.CacheSplashExpire.HasValue)
                {
                    if (Global.AppConfig.CacheSplashExpire < DateTime.Now)
                    {
                        isSplashExpired = true;
                        Global.AppConfig.CacheSplashExpire = null;  //清空过期时间
                        Global.SaveAppConfig();
                    }
                }

                IRandomAccessStream splashStream = null;
                if (isSplashExpired || string.IsNullOrWhiteSpace(lastCacheSplashFileName) || !(await LocalCacheUtility.ExistsCacheFile(lastCacheSplashFileName))) //没有缓存Splash,或者缓存已过期,或者缓存图片获取失败
                {
                    await LocalCacheUtility.DeleteCacheFile(lastCacheSplashFileName);

                    splashStream = await ResourceUtility.GetApplicationResourceStreamAsync("Assets/new_splash_content.png");
                }
                else
                {
                    splashStream = await LocalCacheUtility.GetCacheAsync(lastCacheSplashFileName);
                }

                return splashStream;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        #endregion 方法
    }
}