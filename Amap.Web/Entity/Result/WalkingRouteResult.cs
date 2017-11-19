using Newtonsoft.Json;
using System.Collections.Generic;

namespace Amap.Web.Entity.Result
{
    /// <summary>
    /// 行走路线请求结果
    /// </summary>
    public class WalkingRouteResult : BaseResult
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Route route { get; set; }
    }
    public class StepsItem
    {
        /// <summary>
        /// 沿吉泰四路向西步行196米右转
        /// </summary>
        public string instruction { get; set; }
        /// <summary>
        /// 西
        /// </summary>
        public string orientation { get; set; }
        /// <summary>
        /// 吉泰四路
        /// </summary>
        public string road { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string duration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string polyline { get; set; }
        /// <summary>
        /// 右转
        /// </summary>
        public string action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string assistant_action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string walk_type { get; set; }
    }

    public class PathsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string duration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<StepsItem> steps { get; set; }
    }

    public class Route
    {
        /// <summary>
        /// 
        /// </summary>
        public string origin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string destination { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PathsItem> paths { get; set; }
    }
}
