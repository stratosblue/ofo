using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    public class RechargeDetail
    {
        #region 属性

        /// <summary>
        /// 签到打卡补偿用户3元
        /// </summary>
        public string descr { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string money { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string time { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 获取充值明细
    /// </summary>
    public class RechargeDetailsResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 充值明细
        /// </summary>
        public List<RechargeDetail> Data { get => Value?.info; }

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
            public List<RechargeDetail> info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}