using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 认证信息结果
    /// </summary>
    public class IdentificationResult : BaseResult
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public IdentificationInfo Data { get => Value?.info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        public class Values
        {
            /// <summary>
            /// 
            /// </summary>
            public IdentificationInfo info { get; set; }
        }
    }

    /// <summary>
    /// 认证信息
    /// </summary>
    public class IdentificationInfo
    {
        [JsonProperty("jump")]
        public int Jump { get; set; }
        [JsonProperty("rule")]
        public int Rule { get; set; }
        [JsonProperty("bond")]
        public int Bond { get; set; }
        [JsonProperty("auth")]
        public int Auth { get; set; }
        [JsonProperty("zhiMaEnable")]
        public int ZhiMaEnable { get; set; }
        [JsonProperty("cityIndex")]
        public int CityIndex { get; set; }
        [JsonProperty("bondStandard")]
        public int BondStandard { get; set; }
        [JsonProperty("freeBondType")]
        public string FreeBondType { get; set; }
        [JsonProperty("sanMianShow")]
        public int SanMianShow { get; set; }
        [JsonProperty("sanMianUser")]
        public int SanMianUser { get; set; }
        [JsonProperty("authInfo")]
        public AuthInfo AuthInfo { get; set; }
        [JsonProperty("bondWithdrawAlert")]
        public string BondWithdrawAlert { get; set; }
        [JsonProperty("bondGroup")]
        public string BondGroup { get; set; }
        [JsonProperty("bondGroupMsgType")]
        public string BondGroupMsgType { get; set; }
    }

    public class AuthInfo
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("info")]
        public SchoolIdentityInfo SchoolInfo { get; set; }
    }

    public class SchoolIdentityInfo
    {
        [JsonProperty("identity")]
        public int Identity { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("school")]
        public string School { get; set; }
        [JsonProperty("no")]
        public string No { get; set; }
        [JsonProperty("imgList")]
        public List<object> ImgList { get; set; }
    }
}
