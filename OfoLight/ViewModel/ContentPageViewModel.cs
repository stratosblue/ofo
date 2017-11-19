using OfoLight.Entity;
using System.Collections.Concurrent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 内容页面VM
    /// </summary>
    public class ContentPageViewModel : BaseViewModel
    {
        public override bool CanExitApplication
        {
            get
            {
                if (ContentElement is Control control)
                {
                    if (control.DataContext is BaseViewModel viewModel)
                    {
                        if (viewModel != this)  //避免死循环
                        {
                            return viewModel.CanExitApplication;
                        }
                    }
                }
                return false;
            }
        }

        private Visibility _headerVisibility = Visibility.Visible;

        public Visibility HeaderVisibility
        {
            get { return _headerVisibility; }
            set
            {
                _headerVisibility = value;
                NotifyPropertyChanged("HeaderVisibility");
            }
        }

        private UIElement _contentElement;

        /// <summary>
        /// 内容元素
        /// </summary>
        public UIElement ContentElement
        {
            get { return _contentElement; }
            set
            {
                _contentElement = value;
                NotifyPropertyChanged("ContentElement");
            }
        }

        private ContentPageArgs _contentPageArgs;

        /// <summary>
        /// 内容页面参数
        /// </summary>
        public ContentPageArgs ContentPageArgs
        {
            get { return _contentPageArgs; }
            set
            {
                if (value == null)
                {
                    ContentElement = null;
                    HeaderVisibility = Visibility.Collapsed;
                    Name = string.Empty;
                }
                else
                {
                    if (_contentPageArgs != null && _contentPageArgs != value && value.CanStack)    //检查入栈
                    {
                        ContentStack.Push(_contentPageArgs);
                    }
                    ContentElement = value.ContentElement;
                    HeaderVisibility = value.HeaderVisibility;
                    Name = value.Name;
                }
                _contentPageArgs = value;
            }
        }

        /// <summary>
        /// 内容栈
        /// </summary>
        protected ConcurrentStack<ContentPageArgs> ContentStack { get; set; } = new ConcurrentStack<ContentPageArgs>();

        /// <summary>
        /// 内容导航
        /// </summary>
        /// <param name="args"></param>
        public void ContentNavication(ContentPageArgs args)
        {
            if (args.IsGoBack)  //后退
            {
                if (ContentStack.TryPop(out var contentArgs))
                {
                    ContentPageArgs = null;
                    ContentPageArgs = contentArgs;
                }
            }
            else
            {
                if (args.ContentElement is Control control)
                {
                    if (control.DataContext is BaseContentViewModel baseContentViewModel)
                    {
                        baseContentViewModel.NavigationToContentEvent += NavigationToContent;
                    }
                }
                ContentPageArgs = args;
            }
        }

        /// <summary>
        /// 内容事件触发
        /// </summary>
        /// <param name="args"></param>
        private void NavigationToContent(ContentPageArgs args)
        {
            ContentNavication(args);
        }

        /// <summary>
        /// 内容页面返回
        /// </summary>
        protected override void TryGoBack()
        {
            bool isHandle = false;
            if (ContentElement is UserControl content)
            {
                if (content.DataContext is BaseContentViewModel viewModel)  //是否是内容页面
                {
                    if (viewModel.CanGoBack)    //检查ContentViewModel是否可以后退
                    {
                        viewModel.GoBack();
                        isHandle = true;
                    }
                    else
                    {
                        viewModel.NavigationToContentEvent -= NavigationToContent;
                    }
                }
                //else if (content.DataContext is ContentPageViewModel contentviewModel)  //是否是嵌套页面
                //{ }

                if (!isHandle && ContentStack.Count > 0) //未处理后退请求，且Content可以后退
                {
                    if (ContentStack.TryPop(out var args))
                    {
                        args.CanStack = false;
                        ContentPageArgs = args;
                        isHandle = true;
                    }
                }
            }
            if (!isHandle)
            {
                base.TryGoBack();
            }
        }

        public override void Dispose()
        {
            _contentElement = null;
            _contentPageArgs = null;
            while (ContentStack.TryPop(out var contentArgs))
            {
                if (contentArgs.ContentElement is UserControl content)
                {
                    if (content.DataContext is BaseContentViewModel viewModel)  //是否是内容页面
                    {
                        viewModel.NavigationToContentEvent -= NavigationToContent;
                    }
                }
            }
            base.Dispose();
        }
    }
}
