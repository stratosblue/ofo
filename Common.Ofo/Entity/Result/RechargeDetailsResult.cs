using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 获取充值明细
    /// </summary>
    public class RechargeDetailsResult : BaseResult
    {
        /// <summary>
        /// 充值明细
        /// </summary>
        public List<RechargeDetail> Data { get => Value?.info; }

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
            public List<RechargeDetail> info { get; set; }
        }
    }

    public class RechargeDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 签到打卡补偿用户3元
        /// </summary>
        public string descr { get; set; }
    }
}
