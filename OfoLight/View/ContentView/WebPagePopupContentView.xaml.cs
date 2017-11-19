using System;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class WebPagePopupContentView : UserControl
    {
        public WebPagePopupContentView()
        {
            this.InitializeComponent();
            Unloaded += ContentViewUnloaded;
        }

        private void ContentViewUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            (webView.Parent as Grid)?.Children?.Remove(webView);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            Unloaded -= ContentViewUnloaded;
        }
    }
}
