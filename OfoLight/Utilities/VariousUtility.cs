using System;
using System.Diagnostics;
using System.Text;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 各种各样
    /// </summary>
    public static class VariousUtility
    {
        #region 字段

        /// <summary>
        /// 格林威治标准时间
        /// </summary>
        private static DateTime _standardTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 各种各样工具
        /// </summary>
        static VariousUtility()
        {
            //计算当前时区的标准时间
            _standardTime = _standardTime.Add(TimeZoneInfo.Local.BaseUtcOffset);
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 字节大小转换为描述字符串
        /// </summary>
        /// <param name="size">字节大小</param>
        /// <param name="accuracy">小数保留精确度</param>
        /// <param name="descriptionType">描述类型</param>
        /// <returns></returns>
        public static string ByteSizeToString(ulong size, int accuracy = 2, ByteSizeDescriptionType descriptionType = ByteSizeDescriptionType.Default)
        {
            string accuracyStr = $"f{accuracy}";
            switch (descriptionType)
            {
                case ByteSizeDescriptionType.KB:
                    return $"{(size / 1024.0).ToString(accuracyStr)}KB";

                case ByteSizeDescriptionType.MB:
                    return $"{(size / 1048576.0).ToString(accuracyStr)}MB";

                case ByteSizeDescriptionType.GB:
                    return $"{(size / 1073741824.0).ToString(accuracyStr)}GB";

                case ByteSizeDescriptionType.Default:
                default:
                    {
                        if (size > 1073741824)    //大于1GB
                        {
                            return $"{(size / 1073741824.0).ToString(accuracyStr)}GB";
                        }
                        else if (size > 1048576)  //大于1MB
                        {
                            return $"{(size / 1048576.0).ToString(accuracyStr)}MB";
                        }
                        else if (size > 1024)  //大于1KB
                        {
                            return $"{(size / 1024.0).ToString(accuracyStr)}KB";
                        }
                        else
                        {
                            return $"{size}B";
                        }
                    }
            }
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <returns></returns>
        public static string GetSystemDetail()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                EasClientDeviceInformation eas = new EasClientDeviceInformation();
                sb.Append(eas.SystemManufacturer);
                sb.Append(' ');
                sb.Append(eas.SystemProductName);
                sb.Append(' ');

                AnalyticsVersionInfo analyticsVersion = AnalyticsInfo.VersionInfo;
                sb.Append(analyticsVersion.DeviceFamily);
                sb.Append(' ');

                ulong v = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
                sb.Append((v & 0xFFFF000000000000L) >> 48);
                sb.Append('.');
                sb.Append((v & 0x0000FFFF00000000L) >> 32);
                sb.Append('.');
                sb.Append((v & 0x00000000FFFF0000L) >> 16);
                sb.Append('.');
                sb.Append(v & 0x000000000000FFFFL);
                sb.Append(' ');

                Package package = Package.Current;
                sb.Append(package.Id.Architecture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return Convert.ToInt64((DateTime.Now.ToUniversalTime() - _standardTime).TotalMilliseconds);
        }

        /// <summary>
        /// 时间戳转换为当前时间
        /// </summary>
        /// <param name="timestampStr"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDateTime(string timestampStr)
        {
            if (timestampStr?.Length > 0)
            {
                long.TryParse(timestampStr, out var timestamp);
                return TimeStampToDateTime(timestamp);
            }
            return _standardTime;
        }

        /// <summary>
        /// 时间戳转换为当前时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDateTime(long timestamp)
        {
            TimeSpan timeSpan = new TimeSpan(timestamp);
            return _standardTime.Add(timeSpan);
        }

        #endregion 方法

        #region 枚举

        /// <summary>
        /// 字节大小描述类型
        /// </summary>
        public enum ByteSizeDescriptionType
        {
            /// <summary>
            /// 自动描述
            /// </summary>
            Default,

            /// <summary>
            /// 以KB描述
            /// </summary>
            KB,

            /// <summary>
            /// 以MB描述
            /// </summary>
            MB,

            /// <summary>
            /// 以GB描述
            /// </summary>
            GB,
        }

        #endregion 枚举
    }
}