using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class SettingContentView : UserControl
    {
        public SettingContentView()
        {
            this.InitializeComponent();
            DataContext = new SettingContentViewModel();
        }
    }
}
