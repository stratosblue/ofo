using Newtonsoft.Json;

namespace Amap.Web.Entity.Result
{
    /// <summary>
    /// 地点详情请求结果
    /// </summary>
    public class LocationDetailsResult : BaseResult
    {
        [JsonProperty("regeocode")]
        public Regeocode Regeocode { get; set; }
    }

    public class Regeocode
    {
        public string formatted_address { get; set; }
        public Addresscomponent addressComponent { get; set; }
    }

    public class Addresscomponent
    {
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string citycode { get; set; }
        public string district { get; set; }
        public string adcode { get; set; }
        public object[] township { get; set; }
        public object[] towncode { get; set; }
        public Neighborhood neighborhood { get; set; }
        public Building building { get; set; }
        public Streetnumber streetNumber { get; set; }
        public Businessarea[] businessAreas { get; set; }
    }

    public class Neighborhood
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Building
    {
        public object[] name { get; set; }
        public object[] type { get; set; }
    }

    public class Streetnumber
    {
        public string street { get; set; }
        public string number { get; set; }
        public string location { get; set; }
        public string direction { get; set; }
        public string distance { get; set; }
    }

    public class Businessarea
    {
        public string location { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }
}
