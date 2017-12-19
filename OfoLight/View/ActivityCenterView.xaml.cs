using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    /// <summary>
    /// 活动中心View
    /// </summary>
    public sealed partial class ActivityCenterView : Page
    {
        #region 属性

        public ActivityCenterViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 活动中心View
        /// </summary>
        public ActivityCenterView()
        {
            this.InitializeComponent();
            ViewModel = new ActivityCenterViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数
    }
}