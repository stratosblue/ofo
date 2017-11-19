using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OfoLight.View
{
    public sealed partial class ActivityPopupContentView : UserControl
    {
        public ActivityPopupContentView()
        {
            this.InitializeComponent();
            mainRoot.Background = new SolidColorBrush(Color.FromArgb(180, 133, 133, 133));
        }
    }
}
