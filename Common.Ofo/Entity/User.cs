namespace Common.Ofo.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        #region 属性

        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelPhone { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 用户
        /// </summary>
        public User()
        {
        }

        #endregion 构造函数
    }
}