using Windows.Devices.Geolocation;

namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 需要地址的请求
    /// </summary>
    public class BasePositionRequest : BaseRequest
    {
        public BasicGeoposition Location { get; set; }

        /// <summary>
        /// 需要地址的请求
        /// </summary>
        public BasePositionRequest()
        {
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&lat={Location.Latitude}&lng={Location.Longitude}";
        }
    }
}
