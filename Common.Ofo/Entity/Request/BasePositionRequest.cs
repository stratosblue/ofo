using Windows.Devices.Geolocation;

namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 需要地址的请求
    /// </summary>
    public class BasePositionRequest : BaseRequest
    {
        #region 属性

        public BasicGeoposition Location { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 需要地址的请求
        /// </summary>
        public BasePositionRequest()
        {
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&lat={Location.Latitude}&lng={Location.Longitude}";
        }

        #endregion 方法
    }
}