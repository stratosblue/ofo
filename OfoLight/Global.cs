using Common.Ofo;
using Newtonsoft.Json;
using OfoLight.Entity;
using OfoLight.Utilities;
using System;
using System.Diagnostics;

namespace OfoLight
{
    /// <summary>
    /// 全局设置
    /// </summary>
    public static class Global
    {
        #region 字段

        /// <summary>
        /// Cookie的域名称
        /// </summary>
        public const string COOKIE_DOMAIN = "ofo.so";

        /// <summary>
        /// 应用程序配置 键名
        /// </summary>
        public const string KEY_APP_CONFIG = "application_config";

        /// <summary>
        /// 主页地址
        /// </summary>
        public const string MAIN_WEBPAGE_URL = "https://common.ofo.so/newdist/?Journey";

        #endregion 字段

        #region 属性

        /// <summary>
        /// 程序配置
        /// </summary>
        public static AppConfiguration AppConfig { get; set; }

        /// <summary>
        /// Ofo接口
        /// </summary>
        public static OfoWebAPIs OfoApi { get; set; } = new OfoWebAPIs();

        #endregion 属性

        #region 方法

        /// <summary>
        /// 清理缓存相关设置
        /// </summary>
        public static void ClearCacheSetting()
        {
            try
            {
                AppConfig.ClearCacheSetting();
                SaveAppConfig();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 清理用户相关设置
        /// </summary>
        public static void ClearUserStatus()
        {
            try
            {
                ClientCookieManager.ClearCookie($"http://{COOKIE_DOMAIN}");
                AppConfig.ClearUserStatus();
                SaveAppConfig();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 加载应用程序配置
        /// </summary>
        public static void LoadAppConfig()
        {
            var serializedConfig = SettingUtility.GetSetting<string>(KEY_APP_CONFIG);
            if (string.IsNullOrWhiteSpace(serializedConfig))
            {
                AppConfig = AppConfig ?? new AppConfiguration();
            }
            else
            {
                AppConfiguration configuration = null;
                try
                {
                    configuration = JsonConvert.DeserializeObject<AppConfiguration>(serializedConfig);
                }
                finally
                {
                    AppConfig = configuration ?? AppConfig ?? new AppConfiguration();
                }
            }
        }

        /// <summary>
        /// 序保存应用程序配置
        /// </summary>
        public static void SaveAppConfig()
        {
            var serializedConfig = JsonConvert.SerializeObject(AppConfig);
            SettingUtility.SaveSetting(KEY_APP_CONFIG, serializedConfig);
        }

        #endregion 方法
    }
}