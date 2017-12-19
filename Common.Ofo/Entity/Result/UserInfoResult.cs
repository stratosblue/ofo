using Newtonsoft.Json;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// Cls信息
    /// </summary>
    public class ClsInfo
    {
        #region 属性

        [JsonProperty("cls")]
        public int Cls { get; set; }

        [JsonProperty("clsDes")]
        public string ClsDes { get; set; }

        #endregion 属性
    }

    public class UserInfo
    {
        #region 属性

        /// <summary>
        /// 头像
        /// </summary>
        [JsonProperty("img")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [JsonProperty("balance")]
        public float Balance { get; set; }

        /// <summary>
        /// bond？不知道是啥
        /// </summary>
        [JsonProperty("bond")]
        public int Bond { get; set; }

        /// <summary>
        /// 唯一编码？
        /// </summary>
        [JsonProperty("cid")]
        public string Cid { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        [JsonProperty("cityIndex")]
        public int CityIndex { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        [JsonProperty("cityName")]
        public string CityName { get; set; }

        /// <summary>
        /// cls？不知道是啥
        /// </summary>
        [JsonProperty("cls")]
        public int Cls { get; set; }

        /// <summary>
        /// clsList？cls列表？
        /// </summary>
        [JsonProperty("clsList")]
        public ClsInfo[] ClsList { get; set; }

        /// <summary>
        /// 信用日期
        /// </summary>
        [JsonProperty("creditDate")]
        public string CreditDate { get; set; }

        /// <summary>
        /// 信用总分
        /// </summary>
        [JsonProperty("creditTotal")]
        public int CreditTotal { get; set; }

        /// <summary>
        /// 认证类型？1认证 0未认证
        /// </summary>
        [JsonProperty("identityType")]
        public int IdentityType { get; set; }

        /// <summary>
        /// isBond？不知道是啥
        /// </summary>
        [JsonProperty("isBond")]
        public int IsBond { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 姓名检查字符串？
        /// </summary>
        [JsonProperty("nameCheckStr")]
        public string NameCheckStr { get; set; }

        /// <summary>
        /// oauth？不知道是啥
        /// </summary>
        [JsonProperty("oauth")]
        public int Oauth { get; set; }

        /// <summary>
        /// 不知道是啥
        /// </summary>
        [JsonProperty("pricode")]
        public string Pricode { get; set; }

        /// <summary>
        /// 学校代码？
        /// </summary>
        [JsonProperty("schoolCode")]
        public string SchoolCode { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [JsonProperty("tel")]
        public string TelPhone { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 用户信息结果
    /// </summary>
    public class UserInfoResult : BaseResult
    {
        #region 属性

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo Data { get => Value?.info; }

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
            public UserInfo info { get; set; }

            #endregion 属性
        }

        #endregion 类
    }
}