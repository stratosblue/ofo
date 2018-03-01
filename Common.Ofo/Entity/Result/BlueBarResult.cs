using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 验证码结果
    /// </summary>
    public class BlueBarInfo
    {
        #region 属性

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("maintype")]
        public int MainType { get; set; }

        /// <summary>
        /// 骑行集齐5种小黄人，赢77.77元大奖
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 未认证时type=1
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 蓝贴请求结果
    /// </summary>
    public class BlueBarResult : BaseResult
    {
        #region 属性

        [JsonProperty("values")]
        public BlueBarInfo Data { get; set; }

        #endregion 属性
    }
}