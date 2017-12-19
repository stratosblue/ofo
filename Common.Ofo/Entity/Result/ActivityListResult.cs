using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Common.Ofo.Entity.Result
{
    public class ActivityInfo
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public string activityUrl { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string expired { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string NO { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string PicName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("picUrl")]
        public string PicUrl
        {
            get => _picUrl;
            set
            {
                _picUrl = value;

                PicName = Path.GetFileName(value);
            }
        }

        private string _picUrl { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 获取活动列表
    /// </summary>
    public class ActivityListResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 活动列表
        /// </summary>
        public List<ActivityInfo> Data { get => Value?.info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        #endregion 属性

        #region 类

        public class Values
        {
            #region 属性

            /// <summary>
            ///
            /// </summary>
            public List<ActivityInfo> info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}