using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class SettingContentView : UserControl
    {
        #region 构造函数

        public SettingContentView()
        {
            this.InitializeComponent();
            DataContext = new SettingContentViewModel();
        }

        #endregion 构造函数
    }
}