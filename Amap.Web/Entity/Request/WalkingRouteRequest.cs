using Windows.Devices.Geolocation;

namespace Amap.Web.Entity.Request
{
    /// <summary>
    /// 行走路径请求
    /// </summary>
    public class WalkingTouteRequest : BaseRequest
    {
        #region 字段

        private AmapConfig _config = null;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 目的地址
        /// </summary>
        public BasicGeoposition Destination { get; set; }

        /// <summary>
        /// 多路径？默认0
        /// </summary>
        public int Multipath { get; set; } = 0;

        /// <summary>
        /// 起始地址
        /// </summary>
        public BasicGeoposition Origin { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 行走路径请求
        /// </summary>
        /// <param name="config">高德地图配置</param>
        public WalkingTouteRequest(AmapConfig config)
        {
            _config = config;
            ApiUrl = ApiUrls.GetWalkingRoute;
        }

        #endregion 构造函数

        #region 方法

        public override string GetQueryString()
        {
            return $"origin={Origin.Longitude},{Origin.Latitude}&destination={Destination.Longitude},{Destination.Latitude}&multipath={Multipath}&s=rsv3&{_config.ToString()}&{base.GetQueryString()}";
        }

        #endregion 方法
    }
}