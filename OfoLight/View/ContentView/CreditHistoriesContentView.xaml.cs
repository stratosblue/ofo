using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class CreditHistoriesContentView : UserControl
    {
        #region 构造函数

        public CreditHistoriesContentView()
        {
            this.InitializeComponent();

            DataContext = new CreditHistoriesContentViewModel();
        }

        #endregion 构造函数
    }
}