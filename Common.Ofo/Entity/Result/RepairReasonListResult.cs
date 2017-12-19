using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 报修原因信息
    /// </summary>
    public class RepairReasonInfo
    {
        #region 属性

        /// <summary>
        /// 报修代码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 报修原因名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 保修原因列表请求结果
    /// </summary>
    public class RepairReasonListResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 充值明细
        /// </summary>
        public List<RepairReasonInfo> Data { get => Value?.info; }

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

            public List<RepairReasonInfo> info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}