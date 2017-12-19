using System.ComponentModel;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 属性改变通知的VM
    /// </summary>
    public class NotifyViewModel : INotifyPropertyChanged
    {
        #region 事件

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion 事件

        #region 方法

        /// <summary>
        /// 通知属性改变
        /// </summary>
        /// <param name="name"></param>
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion 方法
    }
}