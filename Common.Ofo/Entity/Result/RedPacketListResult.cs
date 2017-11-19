using Newtonsoft.Json;
using System;

namespace Common.Ofo.Entity.Result
{
    public class RedPacketListResult : BaseResult
    {
        /// <summary>
        /// 附近的车
        /// </summary>
        public RedPacketInfo[] Data { get => Value?.info; }

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
            public RedPacketInfo[] info { get; set; }
        }
    }

    /// <summary>
    /// 红包信息
    /// </summary>
    public class RedPacketInfo
    {
        public long packetid { get; set; }
        public int amounts { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        [JsonProperty("gettime")]
        public DateTime? GetTime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [JsonProperty("deadtime")]
        public DateTime? DeadTime { get; set; }
        public long expired { get; set; }
        public int used { get; set; }
        public int source { get; set; }
        public int opp { get; set; }
    }
}
