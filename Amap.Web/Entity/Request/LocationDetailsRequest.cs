using Windows.Devices.Geolocation;

namespace Amap.Web.Entity.Request
{
    /// <summary>
    /// 地点详情请求
    /// </summary>
    public class LocationDetailsRequest : BaseRequest
    {
        #region 字段

        private AmapConfig _config = null;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 地点
        /// </summary>
        public BasicGeoposition Location { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 地点详情请求
        /// </summary>
        public LocationDetailsRequest(AmapConfig config)
        {
            _config = config;
            ApiUrl = ApiUrls.GetLocationDetails;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 获取查询字符串
        /// </summary>
        /// <returns></returns>
        public override string GetQueryString()
        {
            return $"location={Location.Longitude},{Location.Latitude}&s=rsv3&{_config.ToString()}&{base.GetQueryString()}";
        }

        #endregion 方法
    }
}