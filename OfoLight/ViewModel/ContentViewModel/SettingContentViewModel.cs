using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    public class SettingContentViewModel : BaseContentViewModel
    {
        private string _cacheSize;

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
        /// 退出登录命令
        /// </summary>
        public ICommand LoginOutCommand { get; set; }

        /// <summary>
        /// 清理缓存命令
        /// </summary>
        public ICommand ClearCacheCommand { get; set; }

        /// <summary>
        /// 软件授权信息命令
        /// </summary>
        public ICommand AboutSoftWareLicenseCommand { get; set; }

        /// <summary>
        /// 关于我们命令
        /// </summary>
        public ICommand AboutUsCommand { get; set; }

        /// <summary>
        /// 免责声明命令
        /// </summary>
        public ICommand AboutSoftWareCommand { get; set; }

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

        protected override async Task InitializationAsync()
        {
            var cacheSize = await LocalCacheUtility.GetLocalCacheSizeAsync(true);
            CacheSize = VariousUtility.ByteSizeToString(cacheSize);
        }
    }
}
