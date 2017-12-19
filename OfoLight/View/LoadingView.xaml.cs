using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    /// <summary>
    /// 程序启动加载页
    /// </summary>
    public sealed partial class LoadingView : Page
    {
        #region 构造函数

        /// <summary>
        /// 程序启动加载页
        /// </summary>
        public LoadingView()
        {
            this.InitializeComponent();
            DataContext = new LoadingViewModel();
        }

        #endregion 构造函数
    }
}