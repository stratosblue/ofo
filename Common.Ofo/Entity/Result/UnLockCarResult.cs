using Newtonsoft.Json;
using System;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 骑行卡信息
    /// </summary>
    public class CardInfo
    {
        #region 属性

        /// <summary>
        /// 骑行卡ID
        /// </summary>
        [JsonProperty("cardId")]
        public string CardId { get; set; }

        /// <summary>
        /// 显示内容？
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        #endregion 属性
    }

    public class LockedInfo
    {
        #region 属性

        public LockedUserInfo info { get; set; }
        public int type { get; set; }

        #endregion 属性

        #region 类

        public class LockedUserInfo
        {
            #region 属性

            public string name { get; set; }

            #endregion 属性
        }

        #endregion 类
    }

    public class UnLockCarInfo
    {
        #region 属性

        /// <summary>
        /// 未结算？=1
        /// </summary>
        [JsonProperty("alpha")]
        public int Alpha { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("baseDistance")]
        public double BaseDistance { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("baseDistanceCost")]
        public double BaseDistanceCost { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("baseTime")]
        public double BaseTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("baseTimeCost")]
        public double BaseTimeCost { get; set; }

        /// <summary>
        /// 骑行卡信息
        /// </summary>
        [JsonProperty("card")]
        public CardInfo Card { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("carno")]
        public string CarNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 未结算？=1
        /// </summary>
        [JsonProperty("egt")]
        public int Egt { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string endTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("isDiscount")]
        public int IsDiscount { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("isGsmLock")]
        public int IsGsmLock { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int isLast { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("isRedPacketArea")]
        public int IsRedPacketArea { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lock")]
        public LockedInfo Lockinfo { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lockRefreshTime")]
        public int LockRefreshTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lockType")]
        public int LockType { get; set; }

        [JsonProperty("model")]
        public int Model { get; set; }

        public string notice { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int opp { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("ordernum")]
        public string Ordernum { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("orderno")]
        public long OrderNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("orderStatus")]
        public int OrderStatus { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("overDistance")]
        public int OverDistance { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("overDistanceCost")]
        public int OverDistanceCost { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("overTime")]
        public int OverTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("overTimeCost")]
        public int OverTimeCost { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("packet")]
        public RedPacketInfo Packet { get; set; }

        /// <summary>
        /// 红包ID？
        /// </summary>
        [JsonProperty("packetid")]
        public int PacketId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int pamounts { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("pwd")]
        public string Password { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("redPacketNotice")]
        public string RedPacketNotice { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("refreshTime")]
        public float RefreshTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("repairTime")]
        public int RepairTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("s")]
        public double S { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("sanMianRemainCount")]
        public string SanMianRemainCount { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [JsonProperty("second")]
        public float Second { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("t")]
        public int T { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int total { get; set; }

        public int unlouckStatus { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 解锁结果
    /// </summary>
    public class UnLockCarResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 解锁信息
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

            public string notice { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}