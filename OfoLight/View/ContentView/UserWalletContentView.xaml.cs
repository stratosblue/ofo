using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    /// <summary>
    /// 用户钱包视图
    /// </summary>
    public sealed partial class UserWalletContentView : UserControl
    {
        /// <summary>
        /// 用户钱包视图
        /// </summary>
        public UserWalletContentView()
        {
            this.InitializeComponent();
            DataContext = new UserWalletContentViewModel();
        }
    }
}
