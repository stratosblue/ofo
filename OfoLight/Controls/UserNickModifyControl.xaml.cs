using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace OfoLight.Controls
{
    /// <summary>
    /// 昵称修改控件
    /// </summary>
    public sealed partial class UserNickModifyControl : UserControl
    {
        public Action<string> ModifyUserNick { get; set; }

        /// <summary>
        /// 昵称修改控件
        /// </summary>
        public UserNickModifyControl()
        {
            this.InitializeComponent();
            Unloaded += UserNickModifyControl_Unloaded;
        }

        private void UserNickModifyControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= UserNickModifyControl_Unloaded;
            ModifyUserNick = null;
        }

        private void ModifyNickButtonClick(object sender, RoutedEventArgs e)
        {
            ModifyUserNick?.Invoke(nickTextBox.Text.Trim());
        }
    }
}
