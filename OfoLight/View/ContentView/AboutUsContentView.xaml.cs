using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class AboutUsContentView : UserControl
    {
        #region 属性

        public string Version { get; set; }

        #endregion 属性

        #region 构造函数

        public AboutUsContentView()
        {
            this.InitializeComponent();

            Version = $"UWP V {Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build} beta";
        }

        #endregion 构造函数

        #region 方法

        private void DownClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ChangeView(null, scrollViewer.ScrollableHeight, null);
        }

        #endregion 方法
    }
}