using OfoLight.Controls;
using System;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 内容弹出页面VM
    /// </summary>
    public class BasePopupContentViewModel : BaseViewModel
    {
        /// <summary>
        /// 内容弹出消息
        /// </summary>
        public ContentPopup ContentPopup { get; set; }

        /// <summary>
        /// 关闭回调
        /// </summary>
        public Action _closeCallBack = null;

        /// <summary>
        /// 内容弹出页面VM
        /// </summary>
        /// <param name="closeCallBack">关闭时的回调委托</param>
        public BasePopupContentViewModel(Action closeCallBack)
        {
            _closeCallBack = closeCallBack;
        }

        protected override async void CloseAction()
        {
            await ContentPopup?.Dispatcher?.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                ContentPopup?.Hide();
            });
            _closeCallBack?.Invoke();
        }
    }
}
