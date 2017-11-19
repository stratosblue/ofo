namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 登陆请求
    /// </summary>
    public class LoginRequest : BaseRequest
    {
        /// <summary>
        /// 电话
        /// 表单参数tel
        /// </summary>
        public string TelPhone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 授权码？？？
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 登陆请求
        /// </summary>
        public LoginRequest()
        {
            ApiUrl = ApiUrls.Login;
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&tel={TelPhone}&code={Code}&authCode={AuthCode}";
        }
    }
}
