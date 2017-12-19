using System;

namespace Amap.Web
{
    /// <summary>
    /// amap配置
    /// 参考配置https://restapi.amap.com/v3/direction/walking?origin=104.056902,30.532741&destination=104.056593,30.53216&multipath=0&s=rsv3&key=0afcd8a0b0fe5b9b89469e3531dc23ea&callback=jsonp_178029_&platform=JS&logversion=2.0&sdkversion=1.3&appname=https%3A%2F%2Fcommon.ofo.so%2Fnewdist%2F%3FJourney&csid=71C11230-EBD6-4E28-A77C-85A6F1970A70
    /// </summary>
    public class AmapConfig
    {
        #region 属性

        /// <summary>
        /// App名称？ https://common.ofo.so/newdist/?Login
        /// </summary>
        public string AppName { get; set; } = string.Empty;

        /// <summary>
        /// Csid？
        /// </summary>
        public string Csid { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 日志版本？ 2.0
        /// </summary>
        public float LogVersion { get; set; } = 2.0F;

        /// <summary>
        /// 平台？ JS
        /// </summary>
        public string Platform { get; set; } = "JS";

        /// <summary>
        /// SDK版本？ 1.3
        /// </summary>
        public float SdkVersion { get; set; } = 1.3F;

        #endregion 属性

        #region 方法

        /// <summary>
        /// 获取配置的Url参数字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"key={Key}&platform={Platform}&logversion={LogVersion}&sdkversion={SdkVersion}&appname={AppName}&csid={Csid}";
        }

        #endregion 方法
    }
}