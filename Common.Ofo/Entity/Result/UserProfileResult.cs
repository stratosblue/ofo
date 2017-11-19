using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 用户简介结果
    /// </summary>
    public class UserProfileResult : BaseResult
    {
        /// <summary>
        /// 用户简介
        /// </summary>
        public UserProfile Data { get => Value?.info; }

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
            public UserProfile info { get; set; }
        }
    }

    /// <summary>
    /// 用户简介
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// 生日年份
        /// </summary>
        [JsonProperty("byear")]
        public int BirthYear { get; set; }
        /// <summary>
        /// 生日月份
        /// </summary>
        [JsonProperty("bmonth")]
        public int BirthMonth { get; set; }
        /// <summary>
        /// 生日日期
        /// </summary>
        [JsonProperty("bday")]
        public int BirthDay { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        [JsonProperty("school")]
        public string School { get; set; }

        /// <summary>
        /// 性别类型
        /// </summary>
        [JsonProperty("sex")]
        public int SexType { get; set; }

        /// <summary>
        /// 等级？学历？
        /// </summary>
        [JsonProperty("grade")]
        public string Grade { get; set; }
    }
}
