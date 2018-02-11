using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class SettingContentView : UserControl
    {
        private SettingContentViewModel ViewModel { get; set; }

        #region 构造函数

        public SettingContentView()
        {
            this.InitializeComponent();
            ViewModel = new SettingContentViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数
    }
}