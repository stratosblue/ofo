using OfoLight.Entity;
using System;
using System.Windows.Input;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 内容页面的 内容基础VM
    /// </summary>
    public class BaseContentViewModel : BaseViewModel
    {
        /// <summary>
        /// 内容导航事件
        /// </summary>
        public event Action<ContentPageArgs> NavigationToContentEvent;

        /// <summary>
        /// 是否可以回退
        /// </summary>
        public virtual bool CanGoBack { get; } = false;

        /// <summary>
        /// 内容导航命令 VM需要重写ContentNavigation
        /// </summary>
        public ICommand ContentNaviCommand { get; set; }

        /// <summary>
        /// 内容页面的 内容基础VM
        /// </summary>
        public BaseContentViewModel() : this(true)
        {
        }

        /// <summary>
        /// 内容页面的 内容基础VM
        /// <paramref name="isInitAsync">是否执行初始化</paramref>
        /// </summary>
        public BaseContentViewModel(bool isInitAsync) : base(isInitAsync)
        {
            ContentNaviCommand = new RelayCommand((state) =>
            {
                ContentNavigation(state);
            });
        }

        /// <summary>
        /// ContentNaviCommand触发的事件
        /// </summary>
        /// <param name="state"></param>
        protected virtual void ContentNavigation(object state)
        { }

        /// <summary>
        /// 内容页导航
        /// </summary>
        /// <param name="args"></param>
        protected void ContentNavigation(ContentPageArgs args)
        {
            NavigationToContentEvent?.Invoke(args);
        }

        public virtual void GoBack()
        {
        }
    }
}
