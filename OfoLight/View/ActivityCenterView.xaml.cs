using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    /// <summary>
    /// 活动中心View
    /// </summary>
    public sealed partial class ActivityCenterView : Page
    {
        public ActivityCenterViewModel ViewModel { get; set; }

        /// <summary>
        /// 活动中心View
        /// </summary>
        public ActivityCenterView()
        {
            this.InitializeComponent();
            ViewModel = new ActivityCenterViewModel();
            DataContext = ViewModel;
        }
    }
}
