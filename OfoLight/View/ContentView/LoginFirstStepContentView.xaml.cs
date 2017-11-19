using OfoLight.ViewModel;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace OfoLight.View
{
    /// <summary>
    /// 登录第一步view
    /// </summary>
    public sealed partial class LoginFirstStepContentView : UserControl
    {
        public LoginFirstStepContentViewModel ViewModel { get; set; }

        /// <summary>
        /// 登录第一步view
        /// </summary>
        public LoginFirstStepContentView()
        {
            this.InitializeComponent();
            ViewModel = new LoginFirstStepContentViewModel();
            DataContext = ViewModel;
        }


        public void InputPhoneNumTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (Regex.IsMatch(tb.Text, "^1[34578][0-9]{9}$"))
                {
                    tbCaptchaCodeInput.Focus(FocusState.Pointer);
                    return;
                }
            }
        }
    }
}
