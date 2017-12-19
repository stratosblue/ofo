using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfo
    {
        #region 属性

        /// <summary>
        /// 是否新用户
        /// </summary>
        [JsonProperty("isNewuser")]
        public bool IsNewUser { get; set; }

        /// <summary>
        /// refreshToken
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 验证码请求结果
    /// </summary>
    public class LoginResult : BaseResult
    {
        #region 属性

        [JsonProperty("values")]
        public LoginInfo Data { get; set; }

        #endregion 属性
    }
}