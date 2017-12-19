namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 登陆请求
    /// </summary>
    public class LoginRequest : BaseRequest
    {
        #region 属性

        /// <summary>
        /// 授权码？？？
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 电话
        /// 表单参数tel
        /// </summary>
        public string TelPhone { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 登陆请求
        /// </summary>
        public LoginRequest()
        {
            ApiUrl = ApiUrls.Login;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&tel={TelPhone}&code={Code}&authCode={AuthCode}";
        }

        #endregion 方法
    }
}