namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 昵称修改请求
    /// </summary>
    public class ModifyNickRequest : BaseRequest
    {
        #region 属性

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 昵称修改请求
        /// </summary>
        public ModifyNickRequest()
        {
            ApiUrl = ApiUrls.ModifyUserNick;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&nickname={Nick}";
        }

        #endregion 方法
    }
}