using Amap.Web.Entity.Request;
using Amap.Web.Entity.Result;
using HttpUtility;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Amap.Web
{
    /// <summary>
    /// 高德地图网页版API
    /// </summary>
    public class AmapWebAPIs
    {
        /// <summary>
        /// 接口配置
        /// </summary>
        public AmapConfig Config { get; private set; }

        /// <summary>
        /// 高德地图网页版API
        /// </summary>
        public AmapWebAPIs()
        {
            Config = new AmapConfig();
        }

        /// <summary>
        /// 高德地图网页版API
        /// </summary>
        /// <param name="config">高德地图的设置</param>
        public AmapWebAPIs(AmapConfig config)
        {
            Config = config ?? throw new ArgumentNullException("配置不可为空");
        }

        /// <summary>
        /// 获取行走路线
        /// </summary>
        /// <param name="origin">起始地点</param>
        /// <param name="destination">目标地点</param>
        /// <returns></returns>
        public async Task<WalkingRouteResult> GetGetWalkingRouteAsync(BasicGeoposition origin, BasicGeoposition destination)
        {
            WalkingTouteRequest request = new WalkingTouteRequest(Config)
            {
                Origin = origin,
                Destination = destination,
            };

            return await GetHttpResultAsync<WalkingRouteResult>(request);
        }

        /// <summary>
        /// 地点详情
        /// </summary>
        /// <param name="location">地点</param>
        /// <returns></returns>
        public async Task<LocationDetailsResult> GetLocationDetailsAsync(BasicGeoposition location)
        {
            LocationDetailsRequest request = new LocationDetailsRequest(Config)
            {
                Location = location,
            };

            return await GetHttpResultAsync<LocationDetailsResult>(request);
        }

        #region 基础实现

        /// <summary>
        /// 获取HttpResult
        /// </summary>
        /// <param name="httpItem">httpItem类型</param>
        /// <param name="itemAction">请求前对HttpItem的操作</param>
        /// <returns></returns>
        private static async Task<HttpResult> GetHttpResultAsync(HttpItem httpItem)
        {
            HttpResult result = null;

            result = await new HttpHelper().GetResultAsync(httpItem);

            return result;
        }


        /// <summary>
        /// 获取请求返回值的Json实体对象 (BaseResult)
        /// </summary>
        /// <typeparam name="T">返回的对象类型 (BaseResult)</typeparam>
        /// <param name="request">请求后对HttpResult的操作</param>
        /// <returns></returns>
        private static async Task<T> GetHttpResultAsync<T>(BaseRequest request) where T : BaseResult, new()
        {
            T result = default(T);

            var httpItem = request.GetHttpItem();

            //请求数据
            HttpResult httpResult = await GetHttpResultAsync(httpItem);

            //转换对象
            result = ConvertToEntity<T>(httpResult.Html);

            if (result == null) //请求失败或者转换对象失败
            {
                result = new T();
            }

            if (result is BaseResult baseResult)
            {
                baseResult.StatusCode = httpResult.StatusCode;
                baseResult.SourceHtml = httpResult.Html;
            }

            return result;
        }

        /// <summary>
        /// 将json字符串转换为指定对象
        /// </summary>
        /// <typeparam name="T">目标对象</typeparam>
        /// <param name="str">源json字符串</param>
        /// <returns></returns>
        private static T ConvertToEntity<T>(string str)
        {
            T result = default(T);

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    //TODO 高德地图的空返回修正，不知可有更好的办法？
                    var fStr = str.Replace(":[]", ":null");
                    result = JsonConvert.DeserializeObject<T>(fStr);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return result;
        }

        #endregion
    }
}
