using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 手机验证码请求结果
    /// </summary>
    public class GetVerifyCodeResult : BaseResult
    {
        #region 属性

        [JsonProperty("values")]
        public CaptchaCodeInfo Data { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 手机验证码信息
    /// </summary>
    public class VerifyCodeInfo
    {
        #region 属性

        /// <summary>
        /// 验证剩余时间
        /// </summary>
        [JsonProperty("leftTime")]
        public string LeftTime { get; set; }

        #endregion 属性
    }
}