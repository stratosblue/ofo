using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    public class ConsumerDetail
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public string carno { get; set; }

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
        public int money { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long orderno { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ordernum { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int status { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string time { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 获取消费明细
    /// </summary>
    public class ConsumerDetailsResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 消费明细
        /// </summary>
        public List<ConsumerDetail> Data { get => Value?.info; }

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
            public List<ConsumerDetail> info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}