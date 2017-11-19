using System;

namespace OfoLight.Entity
{
    /// <summary>
    /// 程序配置
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// ofoToken
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 最后一次的订单号
        /// </summary>
        public long LastOrderNum { get; set; }

        /// <summary>
        /// 最后一次订单的密码
        /// </summary>
        public string LastOrderPwd { get; set; }

        /// <summary>
        /// 最后一次显示的活动ID
        /// </summary>
        public long LastShowActivityId { get; set; }

        /// <summary>
        /// 最后一次显示活动时间
        /// </summary>
        public DateTime LastShowActivityTime { get; set; }

        /// <summary>
        /// 最后一次缓存的splash文件名称
        /// </summary>
        public string LastCacheSplashFileName { get; set; }

        /// <summary>
        ///  缓存的splash过期时间
        /// </summary>
        public DateTime? CacheSplashExpire { get; set; }

        /// <summary>
        /// 程序配置
        /// </summary>
        public AppConfiguration()
        { }

        /// <summary>
        /// 清理用户相关设置
        /// </summary>
        public void ClearUserStatus()
        {
            Token = string.Empty;
            LastOrderNum = 0;
            LastOrderPwd = string.Empty;
            LastShowActivityId = 0;
            LastShowActivityTime = new DateTime();
        }

        /// <summary>
        /// 清理缓存相关设置
        /// </summary>
        public void ClearCacheSetting()
        {
            CacheSplashExpire = null;
            LastCacheSplashFileName = string.Empty;
            LastShowActivityId = 0;
            LastShowActivityTime = new DateTime();
        }
    }
}
