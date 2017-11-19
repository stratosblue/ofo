using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 验证码请求结果
    /// </summary>
    public class LoginResult : BaseResult
    {
        [JsonProperty("values")]
        public LoginInfo Data { get; set; }
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// refreshToken
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 是否新用户
        /// </summary>
        [JsonProperty("isNewuser")]
        public bool IsNewUser { get; set; }
    }
}
