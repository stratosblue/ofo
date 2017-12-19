using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Common.Ofo.Entity.Result
{
    public class Advertisement
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public List<AdvertisementItem> activity { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<object> drawer { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<SplashItem> splash { get; set; }

        #endregion 属性
    }

    public class AdvertisementItem
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public long expire { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("img")]
        public string ImgUrl
        {
            get => _imgUrl;
            set
            {
                _imgUrl = value;

                ImgName = Path.GetFileName(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string link { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long starts { get; set; }

        private string _imgUrl { get; set; }

        #endregion 属性
    }

    public class AdvertisementResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 配置信息
        /// </summary>
        public Advertisement Data { get => Value?.info; }

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
            public Advertisement info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }

    public class SplashItem
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public int count { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int duration { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long expire { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("img")]
        public string ImgUrl
        {
            get => _imgUrl;
            set
            {
                _imgUrl = value;

                ImgName = Path.GetFileName(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string link { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long starts { get; set; }

        private string _imgUrl { get; set; }

        #endregion 属性
    }
}