using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 附近的车结果
    /// </summary>
    public class NearByOfoCarsResult : BaseResult
    {
        /// <summary>
        /// 附近的车
        /// </summary>
        public NearByOfoCars Data { get => Value?.info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        public class Values
        {
            /// <summary>
            /// 
            /// </summary>
            public NearByOfoCars info { get; set; }
        }
    }

    public class CarsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string carno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ordernum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userIdLast { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lng { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
    }

    public class NearByOfoCars
    {
        /// <summary>
        /// 
        /// </summary>
        public int zoomLevel { get; set; }
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
    }
}
