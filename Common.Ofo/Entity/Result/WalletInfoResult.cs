using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    public class WalletInfo
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public int activityBalance { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [JsonProperty("balance")]
        public float Balance { get; set; }

        /// <summary>
        /// 押金金额
        /// </summary>
        [JsonProperty("bond")]
        public float Bond { get; set; }

        public string bondDesc { get; set; }

        public int cardPackageShow { get; set; }

        public int depositStatus { get; set; }

        public string freeBondExpireTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string freeBondType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool isBondWithdrawing { get; set; }

        public int monthCardCount { get; set; }

        /// <summary>
        /// 月卡剩余天数
        /// </summary>
        [JsonProperty("monthCardEndTime")]
        public int MonthCardEndTime { get; set; }

        /// <summary>
        /// 红包数量
        /// </summary>
        [JsonProperty("packetnum")]
        public int PacketNum { get; set; }

        /// <summary>
        /// 充值余额、真实余额
        /// </summary>
        [JsonProperty("realBalance")]
        public float RealBalance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int rechargeBalance { get; set; }

        /// <summary>
        /// 红包收入余额
        /// </summary>
        [JsonProperty("redPacketBalance")]
        public float RedPacketBalance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int refundingBalance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int todayWithdrawCount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string zhiMaMsg { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int zhiMaState { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 钱包信息结果
    /// </summary>
    public class WalletInfoResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 钱包信息
        /// </summary>
        public WalletInfo Data { get => Value?.info; }

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
            public WalletInfo info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}