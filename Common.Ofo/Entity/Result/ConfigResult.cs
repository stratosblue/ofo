using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ConfigResult : BaseResult
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public ConfigInfo Data { get => Value?.info; }

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
            public ConfigInfo info { get; set; }
        }
    }

    public class Abtest
    {
        /// <summary>
        /// 
        /// </summary>
        public int purchase1702 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int spot1702 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int repair1702 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int freeWalk0222 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int refund1702 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int report189 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int refund170422 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rideNavigation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int repairNeedPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int zhongxin0721 { get; set; }
    }

    public class Resource
    {
    }

    public class RedPacketArea
    {
        /// <summary>
        /// 
        /// </summary>
        public int isOpened { get; set; }
    }

    public class Adopt
    {
        /// <summary>
        /// 
        /// </summary>
        public int isOpened { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string homepage { get; set; }
    }

    public class TypesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string img { get; set; }
        /// <summary>
        /// 上私锁
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class Report
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TypesItem> types { get; set; }
    }
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ConfigInfo
    {
        /// <summary>
        /// 计费说明：1元/小时
        /// </summary>
        public string valuation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int inputType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int voiceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Abtest abtest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Resource resource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RedPacketArea redPacketArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Adopt adopt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Report report { get; set; }
    }
}

