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
        public int alpha { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int baseDistance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int baseDistanceCost { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double baseTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int baseTimeCost { get; set; }

        /// <summary>
        /// 骑行卡信息
        /// </summary>
        [JsonProperty("card")]
        public CardInfo Card { get; set; }

        [JsonProperty("carno")]
        public string CarNumber { get; set; }

        public DateTime createTime { get; set; }

        /// <summary>
        /// 未结算？=1
        /// </summary>
        public int egt { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string endTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int isDiscount { get; set; }

        public int isGsmLock { get; set; }

        public int isLast { get; set; }

        public int isRedPacketArea { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lock")]
        public LockedInfo Lockinfo { get; set; }

        public int lockRefreshTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int lockType { get; set; }

        public int model { get; set; }

        public string notice { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int opp { get; set; }

        public string ordernum { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("orderno")]
        public long OrderNumber { get; set; }

        public int orderStatus { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int overDistance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int overDistanceCost { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int overTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int overTimeCost { get; set; }

        /// <summary>
        ///
        /// </summary>
        public RedPacketInfo packet { get; set; }

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

        public string redPacketNotice { get; set; }

        /// <summary>
        ///
        /// </summary>
        public float refreshTime { get; set; }

        public int repairTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int s { get; set; }

        public string sanMianRemainCount { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [JsonProperty("second")]
        public float Second { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int t { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int timeout { get; set; }

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