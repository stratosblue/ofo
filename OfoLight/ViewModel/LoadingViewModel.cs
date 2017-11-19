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

        private BitmapImage _splashImage;

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

        private Visibility _skipSplashButtonVisibility = Visibility.Visible;

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
        /// 显示Splash的TokenSource
        /// </summary>
        private CancellationTokenSource _showSplashTokenSource = new CancellationTokenSource();

        #endregion

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
                    TokenStatus tokenStatus = await tokenTask;

                    switch (tokenStatus)
                    {
                        case TokenStatus.NetWorkFail:
                            if (await MessageDialogUtility.ShowMessageAsync("网络访问异常，请检查您的网络状态，并重新尝试。", "出现了一些问题...", MessageDialogType.RetryCancel) == MessageDialogResult.Retry)
                            {
                                await InitializationAsync();
                            }
                            else
                            {
                                Application.Current.Exit();
                            }
                            break;
                        case TokenStatus.NoToken:   //没有Token
                        case TokenStatus.TokenExpire:   //Token过期
                            await TryReplaceNavigateAsync(typeof(NoLoginView));
                            break;
                        case TokenStatus.Logined:   //已登录
                            await TryReplaceNavigateAsync(typeof(MainPage));
                            break;
                        case TokenStatus.Default:
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
        /// 检查本地存储用户Token状态
        /// </summary>
        /// <returns></returns>
        private async Task<TokenStatus> CheckSavedUserTokenAsync()
        {
            TokenStatus result = TokenStatus.Default;

            if (!NetworkStatusUtility.IsNetworkAvailable)
            {
                await ShowNotifyAsync("无法正常访问网络，请检查网络状态", new TimeSpan(0, 0, 5));
                result = TokenStatus.NetWorkFail;
            }
            else
            {
                string token = Global.AppConfig.Token;

                //无Token直接登陆页面
                if (string.IsNullOrWhiteSpace(token))
                {
                    result = TokenStatus.NoToken;
                }
                else    //有Token，判断一下Token
                {
                    //验证登录状态
                    async Task<bool> ValidateLoginStatus()
                    {
                        Global.OfoApi.CurUser.Token = token;
                        var userInfo = await OfoApi.GetUserInfoAsync();
                        if (await CheckOfoApiResultHttpStatus(userInfo))
                        {
                            if (userInfo.IsSuccess)
                            {
                                OfoApi.CurUser.TelPhone = userInfo.Data.TelPhone;
                                ClientCookieManager.AddCookies(Global.COOKIE_DOMAIN, $"ofo-tokened={token}");
                                result = TokenStatus.Logined;
                            }
                            else
                            {
                                result = TokenStatus.TokenExpire;
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (!await ValidateLoginStatus())
                    {
                        await ValidateLoginStatus();
                    }
                }
            }

            return result;
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

        /// <summary>
        /// Token状态
        /// </summary>
        private enum TokenStatus
        {
            /// <summary>
            /// 默认值，没有正确检查
            /// </summary>
            Default,
            /// <summary>
            /// 网络失败
            /// </summary>
            NetWorkFail,
            /// <summary>
            ///没有Token
            /// </summary>
            NoToken,
            /// <summary>
            /// Token过期
            /// </summary>
            TokenExpire,
            /// <summary>
            /// 正确登录的Token
            /// </summary>
            Logined,
        }
    }
}
