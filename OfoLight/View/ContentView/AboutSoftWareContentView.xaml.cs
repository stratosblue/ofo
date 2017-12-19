using OfoLight.Utilities;
using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class AboutSoftWareContentView : UserControl
    {
        #region 构造函数

        public AboutSoftWareContentView()
        {
            this.InitializeComponent();
        }

        #endregion 构造函数

        #region 方法

        private async void SendEmailButtonClick(object sender, RoutedEventArgs e)
        {
            var version = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";
            var mailto = new Uri($"mailto:stratosblue@outlook.com?subject=ofo共享单车反馈&body=%0A%0A版本信息：{version}%0A系统信息：{VariousUtility.GetSystemDetail()}%0A");
            await Launcher.LaunchUriAsync(mailto);
        }

        #endregion 方法
    }
}