using Newtonsoft.Json;
using Windows.Web.Http;

namespace Common.Ofo.Entity.Result
{
    /// <summary>
    /// 基础返回结果
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 40012订单已报修
        /// </summary>
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; } = -1;

        /// <summary>
        /// 是否成功（判断erroCode是否为200）
        /// </summary>
        public bool IsSuccess { get => ErrorCode == 200; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        /// <summary>
        /// 网页源文本
        /// </summary>
        public string SourceHtml { get; set; }

        /// <summary>
        /// Http状态
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Http状态是否为200
        /// </summary>
        public bool OK { get => StatusCode == HttpStatusCode.Ok; }
    }
}
