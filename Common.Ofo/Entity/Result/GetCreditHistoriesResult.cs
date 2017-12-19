using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 信用记录信息
    /// </summary>
    public class CreditHistoriesInfo
    {
        #region 属性

        [JsonProperty("data")]
        public List<CreditItem> CreditItemList { get; set; }

        [JsonProperty("curPage")]
        public int CurPage { get; set; }

        [JsonProperty("totalPage")]
        public int TotalPage { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 信用记录项
    /// </summary>
    public class CreditItem
    {
        #region 属性

        [JsonProperty("create_time")]
        public string CreateTime { get; set; }

        [JsonProperty("credit")]
        public int Credit { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 信用记录请求结果
    /// </summary>
    public class GetCreditHistoriesResult : BaseResult
    {
        #region 属性

        [JsonProperty("values")]
        public CreditHistoriesInfo Data { get => Value?.Info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        #endregion 属性

        #region 类

        /// <summary>
        ///
        /// </summary>
        public class Values
        {
            #region 属性

            [JsonProperty("info")]
            public CreditHistoriesInfo Info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}