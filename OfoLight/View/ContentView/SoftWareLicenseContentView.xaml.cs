using System;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class SoftWareLicenseContentView : UserControl
    {
        #region 构造函数

        public SoftWareLicenseContentView()
        {
            this.InitializeComponent();
        }

        #endregion 构造函数

        #region 方法

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

        #endregion 方法
    }
}