using OfoLight.ViewModel;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OfoLight.View
{
    /// <summary>
    /// 登录第一步view
    /// </summary>
    public sealed partial class LoginFirstStepContentView : UserControl
    {
        #region 属性

        public LoginFirstStepContentViewModel ViewModel { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 登录第一步view
        /// </summary>
        public LoginFirstStepContentView()
        {
            this.InitializeComponent();
            ViewModel = new LoginFirstStepContentViewModel();
            DataContext = ViewModel;
        }

        #endregion 构造函数

        #region 方法

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

        #endregion 方法
    }
}