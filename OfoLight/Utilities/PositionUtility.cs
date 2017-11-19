using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 位置相关工具
    /// </summary>
    public class PositionUtility
    {
        /// <summary>
        /// 经度修正值
        /// </summary>
        public const double LongitudeFixValue = 0.002816;

        /// <summary>
        /// 纬度修正值
        /// </summary>
        public const double LatitudeFixValue = -0.002543;

        /// <summary>
        /// 位置访问实例
        /// </summary>
        public static Geolocator GeolocatorInstance { get; private set; } = new Geolocator();

        /// <summary>
        /// 获取未修正的基础位置
        /// </summary>
        /// <returns></returns>
        public static async Task<BasicGeoposition> GetUnFixBasicPositionAsync()
        {
            try
            {
                var geoPoint = await GetUnFixGeopointAsync();
                return geoPoint.Position;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new BasicGeoposition();
            }
        }

        /// <summary>
        /// 获取修正的基础位置
        /// </summary>
        /// <returns></returns>
        public static async Task<BasicGeoposition> GetFixedBasicPositionAsync()
        {
            try
            {
                var geoPoint = await GetFixedGeopointAsync();
                return geoPoint.Position;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new BasicGeoposition();
            }
        }

        /// <summary>
        /// 获取修正的位置信息
        /// </summary>
        /// <returns></returns>
        public static async Task<Geopoint> GetFixedGeopointAsync()
        {
            BasicGeoposition basicPosition = new BasicGeoposition();
            var geopoint = await GetUnFixGeopointAsync();
            if (geopoint != null)
            {
                var basicGeoposition = geopoint.Position;

                basicPosition.Longitude = basicGeoposition.Longitude + LongitudeFixValue;
                basicPosition.Latitude = basicGeoposition.Latitude + LatitudeFixValue;
            }

            return new Geopoint(basicPosition);
        }

        /// <summary>
        /// 获取位置
        /// </summary>
        /// <returns></returns>
        public static async Task<Geopoint> GetUnFixGeopointAsync()
        {
            try
            {
                return (await GeolocatorInstance.GetGeopositionAsync()).Coordinate.Point;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new Geopoint(new BasicGeoposition());
            }
        }

        /// <summary>
        /// 转换为未修正的位置信息
        /// </summary>
        /// <param name="geopoint"></param>
        /// <returns></returns>
        public static Geopoint ToUnFixGeopoint(Geopoint geopoint)
        {
            var basicPosition = new BasicGeoposition()
            {
                Altitude = geopoint.Position.Altitude,
                Latitude = geopoint.Position.Latitude - LatitudeFixValue,
                Longitude = geopoint.Position.Longitude - LatitudeFixValue,
            };
            return new Geopoint(basicPosition);
        }
    }
}
