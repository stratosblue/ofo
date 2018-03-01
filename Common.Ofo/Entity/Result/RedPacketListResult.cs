using Newtonsoft.Json;
using System;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 红包信息
    /// </summary>
    public class RedPacketInfo
    {
        #region 属性

        /// <summary>
        /// 金额
        /// </summary>
        [JsonProperty("amounts")]
        public float Amounts { get; set; }

        /// <summary>
        /// 优惠券ID
        /// </summary>
        [JsonProperty("couponId")]
        public string CouponId { get; set; }

        /// <summary>
        /// 是否是包天券
        /// （CouponId不为空则是包天券？）
        /// </summary>
        public bool IsCoupon { get => !string.IsNullOrWhiteSpace(CouponId); }

        /// <summary>
        /// 失效时间
        /// </summary>
        [JsonProperty("deadtime")]
        public DateTime? DeadTime { get; set; }

        public long expired { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        [JsonProperty("gettime")]
        public DateTime? GetTime { get; set; }

        /// <summary>
        /// opp?    红包为0    七天券-10  ？？
        /// </summary>
        public int opp { get; set; }

        /// <summary>
        /// 红包ID
        /// </summary>
        [JsonProperty("packetid")]
        public long PacketId { get; set; }

        public string remark { get; set; }

        public int source { get; set; }

        public int used { get; set; }

        #endregion 属性
    }

    public class RedPacketListResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 附近的车
        /// </summary>
        public RedPacketInfo[] Data { get => Value?.info; }

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
            public RedPacketInfo[] info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}