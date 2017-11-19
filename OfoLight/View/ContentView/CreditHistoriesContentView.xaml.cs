using OfoLight.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    public sealed partial class CreditHistoriesContentView : UserControl
    {
        public CreditHistoriesContentView()
        {
            this.InitializeComponent();

            DataContext = new CreditHistoriesContentViewModel();
        }
    }
}
