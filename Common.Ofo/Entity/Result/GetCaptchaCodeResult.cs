using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 验证码请求结果
    /// </summary>
    public class GetCaptchaCodeResult : BaseResult
    {
        [JsonProperty("values")]
        public CaptchaCodeInfo Data { get; set; }
    }

    /// <summary>
    /// 验证码结果
    /// </summary>
    public class CaptchaCodeInfo
    {
        /// <summary>
        /// 验证字符串
        /// </summary>
        [JsonProperty("captcha")]
        public string CaptchaStr { get; set; }

        /// <summary>
        /// 验证ID
        /// </summary>
        [JsonProperty("verifyId")]
        public string VerifyId { get; set; }
    }
}
