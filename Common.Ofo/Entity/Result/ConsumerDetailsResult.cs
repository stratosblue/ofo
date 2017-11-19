using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 获取消费明细
    /// </summary>
    public class ConsumerDetailsResult : BaseResult
    {
        /// <summary>
        /// 消费明细
        /// </summary>
        public List<ConsumerDetail> Data { get => Value?.info; }

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
            public List<ConsumerDetail> info { get; set; }
        }
    }

    public class ConsumerDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public long orderno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string carno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int money { get; set; }
        /// <summary>
        /// 行程消费
        /// </summary>
        public string descr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isTrueRepair { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ordernum { get; set; }
    }

}
