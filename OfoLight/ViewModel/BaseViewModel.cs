using Common.Ofo;
using Common.Ofo.Entity.Result;
using OfoLight.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 基础ViewModel
    /// </summary>
    public class BaseViewModel : NotifyViewModel, IDisposable
    {
        #region 静态字段、属性

        /// <summary>
        /// 主窗体大小更改
        /// </summary>
        protected static event Action<double, double> SizeChanged;

        /// <summary>
        /// 应用程序
        /// </summary>
        private static Application _application;

        /// <summary>
        /// Ofo接口实例
        /// </summary>
        private static OfoWebAPIs _ofoApi;

        /// <summary>
        /// Ofo接口实例
        /// </summary>
        protected static OfoWebAPIs OfoApi { get => _ofoApi; }

        /// <summary>
        /// 消息提示
        /// </summary>
        protected static NotifyPopup NotifyPopupInstance { get; private set; }

        /// <summary>
        /// 内容弹出消息
        /// </summary>
        protected static ContentPopup ContentPopupInstance { get; set; }

        /// <summary>
        /// 消息提示内容
        /// </summary>
        protected static string NotifyContent
        {
            get => NotifyPopupInstance?.NotifyContent ?? string.Empty;
            set
            {
                if (NotifyPopupInstance != null)
                {
                    NotifyPopupInstance.NotifyContent = value;
                }
            }
        }

        /// <summary>
        /// 消息提示是否正在显示
        /// </summary>
        protected static bool IsNotifyShowing
        {
            get
            {
                return NotifyPopupInstance == null ? false : NotifyPopupInstance.IsShowing;
            }
        }

        #endregion

        /// <summary>
        /// 名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 当前VM的UI线程
        /// </summary>
        protected CoreDispatcher Dispatcher { get; set; }

        private static double _windowHeight;

        /// <summary>
        /// 主窗体高度
        /// </summary>
        public double WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                _windowHeight = value;
                NotifyPropertyChanged("WindowHeight");
            }
        }

        private static double _windowWidth;

        /// <summary>
        /// 主窗体宽度
        /// </summary>
        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                NotifyPropertyChanged("WindowWidth");
            }
        }

        /// <summary>
        /// 当前VM是否可以退出程序
        /// </summary>
        public virtual bool CanExitApplication { get; set; } = false;

        /// <summary>
        /// 设置初始化application
        /// </summary>
        /// <param name="application"></param>
        public static void InitApplication(Application application)
        {
            if (_application != null)
            {
                _application.Suspending -= ApplicationSuspending;
                _application.Resuming -= ApplicationResuming;
            }
            _application = application;
            _application.Suspending += ApplicationSuspending;
            _application.Resuming += ApplicationResuming;
        }

        /// <summary>
        /// 应用由挂起转为运行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ApplicationResuming(object sender, object e)
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                if (rootFrame.Content is Page currentPage)
                {
                    if (currentPage.DataContext is BaseViewModel currentViewModel)
                    {
                        currentViewModel.OnResumingAsync();
                    }
                }
            }
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private static void ApplicationSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            //TODO: 保存应用程序状态并停止任何后台活动
            if (Window.Current.Content is Frame rootFrame)
            {
                if (rootFrame.Content is Page currentPage)
                {
                    if (currentPage.DataContext is BaseViewModel currentViewModel)
                    {
                        currentViewModel.OnSuspendingAsync();
                    }
                }
            }
            deferral.Complete();
        }

        /// <summary>
        /// 设置当前窗体为主窗体
        /// </summary>
        public static void SetMainWindow()
        {
            Window.Current.CoreWindow.SizeChanged += (s, e) =>
            {
                SizeChanged?.Invoke(s.Bounds.Width, s.Bounds.Height);
            };

            _windowHeight = Window.Current.CoreWindow.Bounds.Height;
            _windowWidth = Window.Current.CoreWindow.Bounds.Width;

            SizeChanged?.Invoke(Window.Current.CoreWindow.Bounds.Width, Window.Current.CoreWindow.Bounds.Height);

            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += (s, e) =>
                {
                    if ((ContentPopupInstance?.IsShowing).GetValueOrDefault(false))
                    {
                        ContentPopupInstance.Hide();
                        e.Handled = true;
                    }
                    else if (Window.Current.Content is Frame rootFrame)
                    {
                        if (rootFrame.Content is Page currentPage)
                        {
                            if (currentPage.DataContext is BaseViewModel currentViewModel)
                            {
                                currentViewModel.InternalViewBackPressed(s, e);
                            }
                        }
                    }
                };
            }

            NotifyPopupInstance = new NotifyPopup();

            ContentPopupInstance = new ContentPopup();
        }

        /// <summary>
        /// 设置Ofo接口实例
        /// </summary>
        /// <param name="ofoApi"></param>
        public static void SetOfoWebApiInstance(OfoWebAPIs ofoApi)
        {
            if (_ofoApi == null)
            {
                _ofoApi = ofoApi;
            }
            else
            {
                throw new InvalidOperationException("已设置Ofo接口实例");
            }
        }

        /// <summary>
        /// 是否已监听视图大小改变
        /// </summary>
        public bool IsWatchedViewSizeChange { get; private set; } = false;

        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// 导航命令
        /// </summary>
        public ICommand NaviCommand { get; set; }

        /// <summary>
        /// 基础ViewModel
        /// </summary>
        public BaseViewModel() : this(true)
        {
        }

        /// <summary>
        /// 基础ViewModel
        /// <paramref name="isInitAsync">是否执行初始化</paramref>
        /// </summary>
        public BaseViewModel(bool isInitAsync)
        {
            Dispatcher = Window.Current.Dispatcher;

            if (isInitAsync)
            {
                InitializationAsync();
            }

            CloseCommand = new RelayCommand((state) =>
            {
                CloseAction();
            });
            NaviCommand = new RelayCommand((state) =>
            {
                try
                {
                    NavigationActionAsync(state);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        /// <summary>
        /// 监听视图大小改变
        /// </summary>
        protected void WatchViewSizeChanged()
        {
            if (!IsWatchedViewSizeChange)
            {
                SizeChanged += ViewSizeChanged;
                IsWatchedViewSizeChange = true;
            }
        }

        /// <summary>
        /// 内部调用的后退按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InternalViewBackPressed(object sender, BackPressedEventArgs e)
        {
            if (e.Handled == false && ViewBackPressed(sender, e))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 视图点击后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual bool ViewBackPressed(object sender, BackPressedEventArgs e)
        {
            if (CanExitApplication)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                    return true;

                if (e.Handled == false)
                {
                    if (IsNotifyShowing && NotifyContent.Equals("再次点击退出"))
                    {
                        Global.SaveAppConfig();
                        Application.Current.Exit();
                    }
                    else
                    {
                        ShowNotifyAsync("再次点击退出");
                    }
                }
                return true;
            }
            else
            {
                TryGoBack();
                return true;
            }
        }

        /// <summary>
        /// 窗体大小更改
        /// </summary>
        protected virtual void WindowSizeChanged()
        { }

        /// <summary>
        /// 窗体大小更改
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void ViewSizeChanged(double width, double height)
        {
            WindowHeight = height;
            WindowWidth = width;

            WindowSizeChanged();
        }

        /// <summary>
        /// 初始化ViewModel
        /// </summary>
        /// <returns></returns>
        protected virtual Task InitializationAsync()
        {
            return null;
        }

        /// <summary>
        /// 页面回退
        /// </summary>
        protected virtual void TryGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame?.CanGoBack == true)
            {
                rootFrame.GoBack();
            }
        }

        /// <summary>
        /// 异步页面回退
        /// </summary>
        protected virtual async Task TryGoBackAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TryGoBack();
            });
        }

        /// <summary>
        /// 关闭命令的回调函数
        /// </summary>
        protected virtual void CloseAction()
        {
            TryGoBack();
        }

        /// <summary>
        /// NaviCommand命令的回调函数
        /// </summary>
        /// <param name="state"></param>
        protected virtual void NavigationActionAsync(object state)
        { }

        /// <summary>
        /// 应用程序从挂起恢复运行时
        /// </summary>
        public virtual async Task OnResumingAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 应用程序挂起
        /// </summary>
        public virtual async Task OnSuspendingAsync()
        {
            await Task.CompletedTask;
        }

        #region 导航

        #region 同步
        /// <summary>
        /// 尝试页面导航
        /// </summary>
        /// <param name="pageType">页面type</param>
        protected void TryNavigate(Type pageType)
        {
            TryNavigate(pageType, null, false);
        }

        /// <summary>
        /// 尝试页面导航
        /// </summary>
        /// <param name="pageType">页面type</param>
        protected void TryNavigate(Type pageType, object parameter)
        {
            TryNavigate(pageType, parameter, false);
        }

        /// <summary>
        /// 尝试页面导航(页面替换)
        /// </summary>
        /// <param name="pageType">页面type</param>
        protected void TryReplaceNavigate(Type pageType)
        {
            TryNavigate(pageType, null, true);
        }

        /// <summary>
        /// 尝试页面导航(页面替换)
        /// </summary>
        /// <param name="pageType">页面type</param>
        /// <param name="parameter">参数</param>
        protected void TryReplaceNavigate(Type pageType, object parameter)
        {
            TryNavigate(pageType, parameter, true);
        }

        /// <summary>
        /// 尝试页面导航
        /// </summary>
        /// <param name="pageType">页面type</param>
        /// <param name="parameter">参数</param>
        /// <param name="isReplaceCurrent">是否替换当前页</param>
        protected virtual void TryNavigate(Type pageType, object parameter, bool isReplaceCurrent)
        {
            if (Window.Current?.Content is Frame rootFrame)
            {
                if (rootFrame.Navigate(pageType, parameter) && isReplaceCurrent)
                {
                    rootFrame.BackStack.RemoveAt(rootFrame.BackStack.Count - 1);
                }
            }
        }

        #endregion

        #region 异步

        /// <summary>
        /// 尝试页面导航
        /// </summary>
        /// <param name="pageType">页面type</param>
        protected async Task TryNavigateAsync(Type pageType)
        {
            await TryNavigateAsync(pageType, null, false);
        }

        /// <summary>
        /// 尝试页面导航
        /// </summary>
        /// <param name="pageType">页面type</param>
        protected async Task TryNavigateAsync(Type pageType, object parameter)
        {
            await TryNavigateAsync(pageType, parameter, false);
        }

        /// <summary>
        /// 尝试页面导航(页面替换)
        /// </summary>
        /// <param name="pageType">页面type</param>
        protected async Task TryReplaceNavigateAsync(Type pageType)
        {
            await TryNavigateAsync(pageType, null, true);
        }

        /// <summary>
        /// 尝试页面导航(页面替换)
        /// </summary>
        /// <param name="pageType">页面type</param>
        /// <param name="parameter">参数</param>
        protected async Task TryReplaceNavigateAsync(Type pageType, object parameter)
        {
            await TryNavigateAsync(pageType, parameter, true);
        }

        /// <summary>
        /// 尝试页面导航
        /// </summary>
        /// <param name="pageType">页面type</param>
        /// <param name="parameter">参数</param>
        /// <param name="isReplaceCurrent">是否替换当前页</param>
        protected virtual async Task TryNavigateAsync(Type pageType, object parameter, bool isReplaceCurrent)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (Window.Current?.Content is Frame rootFrame)
                {
                    if (rootFrame.Navigate(pageType, parameter) && isReplaceCurrent)
                    {
                        rootFrame.BackStack.RemoveAt(rootFrame.BackStack.Count - 1);
                    }
                }
            });
        }

        #endregion
        #endregion

        #region 消息提示

        #region 文字提示

        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <returns></returns>
        protected async Task ShowNotifyAsync()
        {
            await NotifyPopupInstance.ShowAsync();
        }

        /// <summary>
        /// 显示提示消息
        /// <paramref name="notifyContent">提示内容</paramref>
        /// </summary>
        public async Task ShowNotifyAsync(string notifyContent)
        {
            await NotifyPopupInstance.ShowAsync(notifyContent);
        }

        /// <summary>
        /// 显示提示消息
        /// <paramref name="notifyContent">提示内容</paramref>
        /// <paramref name="showTime">提示显示时长</paramref>
        /// </summary>
        public async Task ShowNotifyAsync(string notifyContent, TimeSpan showTime)
        {
            await NotifyPopupInstance.ShowAsync(notifyContent, showTime);
        }

        #endregion

        #region 内容提示

        /// <summary>
        /// 显示提示消息
        /// <paramref name="content">提示内容</paramref>
        /// <paramref name="contentViewModel">提示显示时长</paramref>
        /// </summary>
        public async Task ShowContentNotifyAsync(Control content, BasePopupContentViewModel contentViewModel)
        {
            contentViewModel.ContentPopup = ContentPopupInstance;
            content.DataContext = contentViewModel;

            ContentPopupInstance.NotifyContent = content;

            await ContentPopupInstance.ShowAsync();
        }

        #endregion

        #endregion

        /// <summary>
        /// 检查Ofo接口返回数据是否正常
        /// </summary>
        /// <param name="ofoApiResult"></param>
        /// <returns></returns>
        public async Task<bool> CheckOfoApiResult(BaseResult ofoApiResult)
        {
            if (ofoApiResult.OK)
            {
                if (ofoApiResult.IsSuccess)
                {
                    return true;
                }
                else
                {
                    await ShowNotifyAsync(ofoApiResult?.Message);
                    return false;
                }
            }
            else
            {
                await ShowNotifyAsync("网络异常");
                return false;
            }
        }

        /// <summary>
        /// 检查Ofo接口返回数据Http状态
        /// </summary>
        /// <param name="ofoApiResult"></param>
        /// <returns></returns>
        public async Task<bool> CheckOfoApiResultHttpStatus(BaseResult ofoApiResult)
        {
            if (ofoApiResult.OK)
            {
                return true;
            }
            else
            {
                await ShowNotifyAsync("网络异常");
                return false;
            }
        }


        /// <summary>
        /// 销毁VM相关资源
        /// </summary>
        public virtual void Dispose()
        {
            if (IsWatchedViewSizeChange)
            {
                SizeChanged -= ViewSizeChanged;
                IsWatchedViewSizeChange = false;
            }
        }
    }
}
