using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 未完成订单
    /// </summary>
    public class UnfinishedOrderResult : BaseResult
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public UnLockCarInfo Data { get => Value?.info; }

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
            public UnLockCarInfo info { get; set; }
        }
    }


    public class Packet
    {
        /// <summary>
        /// 
        /// </summary>
        public int amounts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int opp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int packetid { get; set; }
    }
}
