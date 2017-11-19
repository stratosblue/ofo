using Common.Ofo.Entity.Result;
using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class UserProfileContentView : UserControl
    {
        UserProfileContentViewModel ViewModel { get; set; }
        public UserProfileContentView(UserInfo UserInfo)
        {
            this.InitializeComponent();
            ViewModel = new UserProfileContentViewModel(UserInfo);
            DataContext = ViewModel;
        }
    }
}
