namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 认证信息请求
    /// </summary>
    public class IdentificationRequest : BasePositionRequest
    {
        #region 属性

        /// <summary>
        /// 登陆？默认值为1
        /// </summary>
        public int Login { get; set; } = 1;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 认证信息请求
        /// </summary>
        public IdentificationRequest()
        {
            ApiUrl = ApiUrls.GetIdentificationInfo;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&login={Login}";
        }

        #endregion 方法
    }
}