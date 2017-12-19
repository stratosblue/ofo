using Common.Ofo.Entity.Result;
using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class UserProfileContentView : UserControl
    {
        #region 属性

        private UserProfileContentViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        public UserProfileContentView(UserInfo UserInfo)
        {
            this.InitializeComponent();
            ViewModel = new UserProfileContentViewModel(UserInfo);
            DataContext = ViewModel;
        }

        #endregion 构造函数
    }
}