using OfoLight.Controls;
using System;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 内容弹出页面VM
    /// </summary>
    public class BasePopupContentViewModel : BaseViewModel
    {
        #region 字段

        /// <summary>
        /// 关闭回调
        /// </summary>
        public Action _closeCallBack = null;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 内容弹出消息
        /// </summary>
        public ContentPopup ContentPopup { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 内容弹出页面VM
        /// </summary>
        /// <param name="closeCallBack">关闭时的回调委托</param>
        public BasePopupContentViewModel(Action closeCallBack)
        {
            _closeCallBack = closeCallBack;
        }

        #endregion 构造函数

        #region 方法

        protected override async void CloseAction()
        {
            await ContentPopup?.Dispatcher?.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                ContentPopup?.Hide();
            });
            _closeCallBack?.Invoke();
        }

        #endregion 方法
    }
}