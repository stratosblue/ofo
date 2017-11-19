using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 结束骑行结果
    /// </summary>
    public class EndRideResult : BaseResult
    {
        /// <summary>
        /// 结束骑行结果
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
}
