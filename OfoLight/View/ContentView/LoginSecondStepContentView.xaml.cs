using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    /// <summary>
    /// 登录第二步View
    /// </summary>
    public sealed partial class LoginSecondStepContentView : UserControl
    {
        /// <summary>
        /// 登录第二步View
        /// </summary>
        public LoginSecondStepContentView()
        {
            this.InitializeComponent();
            DataContext = new LoginSecondStepContentViewModel();
        }
    }
}
