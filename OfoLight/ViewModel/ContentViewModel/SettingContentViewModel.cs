using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    public class SettingContentViewModel : BaseContentViewModel
    {
        #region 字段

        private string _cacheSize;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 免责声明命令
        /// </summary>
        public ICommand AboutSoftWareCommand { get; set; }

        /// <summary>
        /// 软件授权信息命令
        /// </summary>
        public ICommand AboutSoftWareLicenseCommand { get; set; }

        /// <summary>
        /// 关于我们命令
        /// </summary>
        public ICommand AboutUsCommand { get; set; }

        public string CacheSize
        {
            get { return _cacheSize; }
            set
            {
                _cacheSize = value;
                NotifyPropertyChanged("CacheSize");
            }
        }

        /// <summary>
        /// 清理缓存命令
        /// </summary>
        public ICommand ClearCacheCommand { get; set; }

        /// <summary>
        /// 退出登录命令
        /// </summary>
        public ICommand LoginOutCommand { get; set; }

        /// <summary>
        /// 是否使用了透明图标
        /// </summary>
        public bool UseTransparentIcon
        {
            get { return Global.AppConfig.UseTransparentIcon; }
            set
            {
                Global.AppConfig.UseTransparentIcon = value;
                NotifyPropertyChanged("UseTransparentIcon");
            }
        }

        #endregion 属性

        #region 构造函数

        public SettingContentViewModel()
        {
            LoginOutCommand = new RelayCommand(async (state) =>
            {
                var dialogResult = await MessageDialogUtility.ShowMessageAsync("您确认要退出登录吗？", "退出登录");
                if (dialogResult == MessageDialogResult.OK)
                {
                    Global.ClearUserStatus();

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        if (Window.Current?.Content is Frame rootFrame)
                        {
                            rootFrame.BackStack.Clear();
                        }
                    });

                    await TryNavigateAsync(typeof(NoLoginView));
                }
            });

            ClearCacheCommand = new RelayCommand(async (state) =>
            {
                var dialogResult = await MessageDialogUtility.ShowMessageAsync("您确认要清除所有缓存吗？", "缓存清理");
                if (dialogResult == MessageDialogResult.OK)
                {
                    Global.ClearCacheSetting();

                    await LocalCacheUtility.ClearLocalCacheFile();

                    await MessageDialogUtility.ShowMessageAsync("缓存清理完成", "缓存清理", MessageDialogType.OK);

                    var cacheSize = await LocalCacheUtility.GetLocalCacheSizeAsync(true);
                    CacheSize = VariousUtility.ByteSizeToString(cacheSize);
                }
            });

            AboutSoftWareLicenseCommand = new RelayCommand((state) =>
            {
                var args = new ContentPageArgs()
                {
                    Name = "软件授权",
                    ContentElement = new SoftWareLicenseContentView(),
                };
                ContentNavigation(args);
            });

            AboutUsCommand = new RelayCommand((state) =>
            {
                var args = new ContentPageArgs()
                {
                    Name = "关于我们",
                    ContentElement = new AboutUsContentView(),
                };
                ContentNavigation(args);
            });

            AboutSoftWareCommand = new RelayCommand((state) =>
            {
                var args = new ContentPageArgs()
                {
                    Name = "免责声明",
                    ContentElement = new AboutSoftWareContentView(),
                };
                ContentNavigation(args);
            });
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 透明图标状态改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TransparentIconSwitch(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UseTransparentIcon)
                {
                    var xmlContent =
    @"<tile>
    <visual version='4'>
        <binding template='TileSquare71x71Image'>
            <image id='1' src='ms-appx:///Assets\appicon_transport\SmallTile.png'/>
        </binding>
        <binding template='TileSquare150x150Image' fallback='TileSquareImage'>
            <image id='1' src='ms-appx:///Assets\appicon_transport\Square150x150Logo.png'/>
        </binding>
        <binding template='TileWide310x150Image'>
            <image id='1' src='ms-appx:///Assets\appicon_transport\Wide310x150Logo.png'/>
        </binding>
        <binding template='TileSquare310x310Image'>
            <image id='1' src='ms-appx:///Assets\appicon_transport\LargeTile.png'/>
        </binding>
    </visual>
</tile>";
                    var xml = new XmlDocument();
                    xml.LoadXml(xmlContent);

                    var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
                    tileUpdater.Clear();
                    tileUpdater.Update(new TileNotification(xml));
                }
                else
                {
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                }

                Global.SaveAppConfig();
            }
            catch (Exception ex)
            {
                ShowNotifyAsync("设置失败").NoWarning();
                Debug.WriteLine(ex);
            }
        }

        protected override async Task InitializationAsync()
        {
            var cacheSize = await LocalCacheUtility.GetLocalCacheSizeAsync(true);
            CacheSize = VariousUtility.ByteSizeToString(cacheSize);
        }

        #endregion 方法
    }
}