using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    public class CarsItem
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public string carno { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double lng { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ordernum { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string userIdLast { get; set; }

        #endregion 属性
    }

    public class NearByOfoCars
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public List<CarsItem> cars { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<string> redPacketAreas { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int time { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int zoomLevel { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 附近的车结果
    /// </summary>
    public class NearByOfoCarsResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 附近的车
        /// </summary>
        public NearByOfoCars Data { get => Value?.info; }

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
            public NearByOfoCars info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}