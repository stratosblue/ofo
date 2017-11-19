using System;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class SoftWareLicenseContentView : UserControl
    {
        public SoftWareLicenseContentView()
        {
            this.InitializeComponent();
        }

        private async void OpenUrlClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri((sender as Button).Tag as string));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
