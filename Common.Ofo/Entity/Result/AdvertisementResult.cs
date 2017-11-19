using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Ofo.Entity.Result
{
    public class AdvertisementResult : BaseResult
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public Advertisement Data { get => Value?.info; }

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
            public Advertisement info { get; set; }
        }
    }

    public class AdvertisementItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long expire { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long starts { get; set; }

        private string _imgUrl { get; set; }

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
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }
    }

    public class SplashItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        private string _imgUrl { get; set; }

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
        /// 图片名称
        /// </summary>
        public string ImgName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string link { get; set; }
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
        public long starts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int count { get; set; }
    }

    public class Advertisement
    {
        /// <summary>
        /// 
        /// </summary>
        public List<AdvertisementItem> activity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SplashItem> splash { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<object> drawer { get; set; }
    }

}
