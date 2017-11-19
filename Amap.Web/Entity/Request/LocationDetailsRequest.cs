using Windows.Devices.Geolocation;

namespace Amap.Web.Entity.Request
{
    /// <summary>
    /// 地点详情请求
    /// </summary>
    public class LocationDetailsRequest : BaseRequest
    {
        private AmapConfig _config = null;

        /// <summary>
        /// 地点
        /// </summary>
        public BasicGeoposition Location { get; set; }

        /// <summary>
        /// 地点详情请求
        /// </summary>
        public LocationDetailsRequest(AmapConfig config)
        {
            _config = config;
            ApiUrl = ApiUrls.GetLocationDetails;
        }

        /// <summary>
        /// 获取查询字符串
        /// </summary>
        /// <returns></returns>
        public override string GetQueryString()
        {
            return $"location={Location.Longitude},{Location.Latitude}&s=rsv3&{_config.ToString()}&{base.GetQueryString()}";
        }
    }
}
