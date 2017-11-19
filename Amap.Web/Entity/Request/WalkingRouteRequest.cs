using Windows.Devices.Geolocation;

namespace Amap.Web.Entity.Request
{
    /// <summary>
    /// 行走路径请求
    /// </summary>
    public class WalkingTouteRequest : BaseRequest
    {
        private AmapConfig _config = null;

        /// <summary>
        /// 行走路径请求
        /// </summary>
        /// <param name="config">高德地图配置</param>
        public WalkingTouteRequest(AmapConfig config)
        {
            _config = config;
            ApiUrl = ApiUrls.GetWalkingRoute;
        }

        /// <summary>
        /// 起始地址
        /// </summary>
        public BasicGeoposition Origin { get; set; }

        /// <summary>
        /// 目的地址
        /// </summary>
        public BasicGeoposition Destination { get; set; }

        /// <summary>
        /// 多路径？默认0
        /// </summary>
        public int Multipath { get; set; } = 0;

        public override string GetQueryString()
        {
            return $"origin={Origin.Longitude},{Origin.Latitude}&destination={Destination.Longitude},{Destination.Latitude}&multipath={Multipath}&s=rsv3&{_config.ToString()}&{base.GetQueryString()}";
        }
    }
}
