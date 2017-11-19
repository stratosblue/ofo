using System;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace HttpUtility
{
    /// <summary>
    /// Http请求结果
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// 返回页面内容
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public IBuffer Data { get; set; }

        /// <summary>
        /// 请求状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Http请求成功
        /// </summary>
        public bool HttpOk { get => StatusCode == HttpStatusCode.Ok; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        public Uri Location { get; set; }

        /// <summary>
        /// 请求的地址，重定向时会与最初的请求地址不同
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        /// 原返回结果
        /// </summary>
        public HttpResponseMessage OriginResponse { get; set; }
    }
}
