using Newtonsoft.Json;

namespace Amap.Web.Entity.Result
{
    public class Addresscomponent
    {
        #region 属性

        public string adcode { get; set; }
        public Building building { get; set; }
        public Businessarea[] businessAreas { get; set; }
        public string city { get; set; }
        public string citycode { get; set; }
        public string country { get; set; }
        public string district { get; set; }
        public Neighborhood neighborhood { get; set; }
        public string province { get; set; }
        public Streetnumber streetNumber { get; set; }
        public object[] towncode { get; set; }
        public object[] township { get; set; }

        #endregion 属性
    }

    public class Building
    {
        #region 属性

        public object[] name { get; set; }
        public object[] type { get; set; }

        #endregion 属性
    }

    public class Businessarea
    {
        #region 属性

        public string id { get; set; }
        public string location { get; set; }
        public string name { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 地点详情请求结果
    /// </summary>
    public class LocationDetailsResult : BaseResult
    {
        #region 属性

        [JsonProperty("regeocode")]
        public Regeocode Regeocode { get; set; }

        #endregion 属性
    }

    public class Neighborhood
    {
        #region 属性

        public string name { get; set; }
        public string type { get; set; }

        #endregion 属性
    }

    public class Regeocode
    {
        #region 属性

        public Addresscomponent addressComponent { get; set; }
        public string formatted_address { get; set; }

        #endregion 属性
    }

    public class Streetnumber
    {
        #region 属性

        public string direction { get; set; }
        public string distance { get; set; }
        public string location { get; set; }
        public string number { get; set; }
        public string street { get; set; }

        #endregion 属性
    }
}