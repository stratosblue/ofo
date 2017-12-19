using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    public class Packet
    {
        #region 属性

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

        #endregion 属性
    }

    /// <summary>
    /// 未完成订单
    /// </summary>
    public class UnfinishedOrderResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 配置信息
        /// </summary>
        public UnLockCarInfo Data { get => Value?.info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        #endregion 属性

        #region 类

        public class Values
        {
            #region 属性

            /// <summary>
            ///
            /// </summary>
            public UnLockCarInfo info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}