using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class UserCenterView : Page
    {
        public UserCenterViewModel ViewModel { get; set; }
        public UserCenterView()
        {
            this.InitializeComponent();
            ViewModel = new UserCenterViewModel();
            DataContext = ViewModel;
        }
    }
}
