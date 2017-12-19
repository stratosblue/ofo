using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class UserCenterView : Page
    {
        #region 属性

        public UserCenterViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        public UserCenterView()
        {
            this.InitializeComponent();
            ViewModel = new UserCenterViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数
    }
}