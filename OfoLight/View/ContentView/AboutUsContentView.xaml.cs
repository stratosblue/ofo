using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class AboutUsContentView : UserControl
    {
        public string Version { get; set; }

        public AboutUsContentView()
        {
            this.InitializeComponent();

            Version = $"UWP V {Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build} beta";
        }

        private void DownClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ChangeView(null, scrollViewer.ScrollableHeight, null);
        }
    }
}
