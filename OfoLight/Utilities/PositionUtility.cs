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
        #region 字段

        /// <summary>
        /// 纬度修正值
        /// </summary>
        public const double LatitudeFixValue = -0.002474;

        /// <summary>
        /// 经度修正值
        /// </summary>
        public const double LongitudeFixValue = 0.002502;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 位置访问实例
        /// </summary>
        public static Geolocator GeolocatorInstance { get; private set; } = new Geolocator();

        #endregion 属性

        #region 方法

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
        /// <param name="basicGeoposition"></param>
        /// <returns></returns>
        public static BasicGeoposition ToFixBasicGeoposition(BasicGeoposition basicGeoposition)
        {
            var result = new BasicGeoposition()
            {
                Altitude = basicGeoposition.Altitude,
                Latitude = basicGeoposition.Latitude + LatitudeFixValue,
                Longitude = basicGeoposition.Longitude + LatitudeFixValue,
            };
            return result;
        }

        /// <summary>
        /// 转换为未修正的位置信息
        /// </summary>
        /// <param name="geopoint"></param>
        /// <returns></returns>
        public static Geopoint ToFixGeopoint(Geopoint geopoint)
        {
            var basicPosition = ToFixBasicGeoposition(geopoint.Position);
            return new Geopoint(basicPosition);
        }

        /// <summary>
        /// 转换为未修正的位置信息
        /// </summary>
        /// <param name="basicGeoposition"></param>
        /// <returns></returns>
        public static BasicGeoposition ToUnFixBasicGeoposition(BasicGeoposition basicGeoposition)
        {
            var result = new BasicGeoposition()
            {
                Altitude = basicGeoposition.Altitude,
                Latitude = basicGeoposition.Latitude - LatitudeFixValue,
                Longitude = basicGeoposition.Longitude - LatitudeFixValue,
            };
            return result;
        }

        /// <summary>
        /// 转换为未修正的位置信息
        /// </summary>
        /// <param name="geopoint"></param>
        /// <returns></returns>
        public static Geopoint ToUnFixGeopoint(Geopoint geopoint)
        {
            var basicPosition = ToUnFixBasicGeoposition(geopoint.Position);
            return new Geopoint(basicPosition);
        }

        #endregion 方法
    }
}