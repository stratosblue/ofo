using Windows.Storage;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 设置工具类
    /// </summary>
    public static class SettingUtility
    {
        #region 字段

        /// <summary>
        /// 默认容器名称
        /// </summary>
        public const string DEFAULT_LOCAL_CONTAINERS_NAME = "Default";

        #endregion 字段

        #region 属性

        /// <summary>
        /// 当前程序的本地配置
        /// </summary>
        public static ApplicationDataContainer LocalSettings { get; private set; } = ApplicationData.Current.LocalSettings;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 设置工具类
        /// </summary>
        static SettingUtility()
        {
            if (!LocalSettings.Containers.ContainsKey(DEFAULT_LOCAL_CONTAINERS_NAME))
            {
                LocalSettings.CreateContainer(DEFAULT_LOCAL_CONTAINERS_NAME, ApplicationDataCreateDisposition.Always);
            }
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 从默认容器获取一个值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetSetting(string key)
        {
            return GetSetting(DEFAULT_LOCAL_CONTAINERS_NAME, key);
        }

        /// <summary>
        /// 从指定容器获取一个设置值
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetSetting(string containerName, string key)
        {
            return LocalSettings.Containers[containerName].Values[key];
        }

        /// <summary>
        /// 从默认容器获取一个值
        /// 并尝试强制换行为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string key)
        {
            return GetSetting<T>(DEFAULT_LOCAL_CONTAINERS_NAME, key);
        }

        /// <summary>
        /// 从指定容器获取一个设置值
        /// 并尝试强制换行为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string containerName, string key)
        {
            var value = GetSetting(containerName, key);
            return value == null ? default(T) : (T)value;
        }

        /// <summary>
        /// 保存一个值到默认容器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveSetting(string key, object value)
        {
            SaveSetting(DEFAULT_LOCAL_CONTAINERS_NAME, key, value);
        }

        /// <summary>
        /// 保存一个值到指定容器
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveSetting(string containerName, string key, object value)
        {
            LocalSettings.Containers[containerName].Values[key] = value;
        }

        #endregion 方法
    }
}