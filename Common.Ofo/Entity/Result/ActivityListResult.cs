using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 获取活动列表
    /// </summary>
    public class ActivityListResult : BaseResult
    {
        /// <summary>
        /// 活动列表
        /// </summary>
        public List<ActivityInfo> Data { get => Value?.info; }

        /// <summary>
        /// 返回的值
        /// </summary>
        [JsonProperty("values")]
        public Values Value { get; set; }

        public class Values
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ActivityInfo> info { get; set; }
        }
    }

    public class ActivityInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string NO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activityUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expired { get; set; }

        private string _picUrl { get; set; }

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

        /// <summary>
        /// 图片名称
        /// </summary>
        public string PicName { get; set; }
    }

}
