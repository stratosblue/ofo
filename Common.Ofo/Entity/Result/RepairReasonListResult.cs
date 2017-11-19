using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 保修原因列表请求结果
    /// </summary>
    public class RepairReasonListResult : BaseResult
    {
        /// <summary>
        /// 充值明细
        /// </summary>
        public List<RepairReasonInfo> Data { get => Value?.info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        public class Values
        {
            public List<RepairReasonInfo> info { get; set; }
        }
    }

    /// <summary>
    /// 报修原因信息
    /// </summary>
    public class RepairReasonInfo
    {
        /// <summary>
        /// 报修代码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 报修原因名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
