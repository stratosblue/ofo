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
        #region 属性

        public Action<string> ModifyUserNick { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 昵称修改控件
        /// </summary>
        public UserNickModifyControl()
        {
            this.InitializeComponent();
            Unloaded += UserNickModifyControl_Unloaded;
        }

        #endregion 构造函数

        #region 方法

        private void ModifyNickButtonClick(object sender, RoutedEventArgs e)
        {
            ModifyUserNick?.Invoke(nickTextBox.Text.Trim());
        }

        private void UserNickModifyControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= UserNickModifyControl_Unloaded;
            ModifyUserNick = null;
        }

        #endregion 方法
    }
}