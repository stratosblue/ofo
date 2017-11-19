using System;
using Windows.UI.Xaml;

namespace OfoLight.Entity
{
    /// <summary>
    /// 内容页面参数
    /// </summary>
    public class ContentPageArgs : EventArgs
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头可见状态
        /// </summary>
        public Visibility HeaderVisibility { get; set; } = Visibility.Visible;

        /// <summary>
        /// 内容元素
        /// </summary>
        public UIElement ContentElement { get; set; }

        /// <summary>
        /// 可以进入内容栈
        /// </summary>
        public bool CanStack { get; set; } = true;

        /// <summary>
        /// 是否后退
        /// 为true时内容后退
        /// </summary>
        public bool IsGoBack { get; set; } = false;

        /// <summary>
        /// 内容页面参数
        /// </summary>
        public ContentPageArgs()
        { }
    }
}
