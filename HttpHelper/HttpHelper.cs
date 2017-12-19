using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace HttpUtility
{
    /// <summary>
    /// Http请求帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 获取指定Url在当前APP容器内的Cookie集合
        /// </summary>
        public static HttpCookieCollection GetCookie(string url)
        {
            return GetCookie(new Uri(url));
        }

        /// <summary>
        /// 获取指定Uri在当前APP容器内的Cookie集合
        /// </summary>
        public static HttpCookieCollection GetCookie(Uri uri)
        {
            var protocolFilter = new HttpBaseProtocolFilter();
            return protocolFilter.CookieManager.GetCookies(uri);
        }

        private HttpClient _client = null;

        /// <summary>
        /// 基础协议筛选器
        /// </summary>
        public HttpBaseProtocolFilter ProtocolFilter { get; set; } = new HttpBaseProtocolFilter();

        /// <summary>
        /// Http客户端
        /// </summary>
        public HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new HttpClient(ProtocolFilter);
                }
                return _client;
            }

            set
            {
                _client = value;
            }
        }

        /// <summary>
        /// 获取请求返回结果
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        public async Task<HttpResult> GetResultAsync(HttpItem requestItem)
        {
            HttpResult result = new HttpResult();

            try
            {
                ProtocolFilter.AllowAutoRedirect = requestItem.AllowAutoRedirect;
                //10586没有这个属性？
                //ProtocolFilter.AllowUI = requestItem.AllowUI;
                ProtocolFilter.AutomaticDecompression = requestItem.AutomaticDecompression;
                ProtocolFilter.MaxConnectionsPerServer = requestItem.MaxConnectionsPerServer;
                ProtocolFilter.UseProxy = requestItem.UseProxy;
                ProtocolFilter.CookieUsageBehavior = requestItem.UseCookies ? HttpCookieUsageBehavior.Default : HttpCookieUsageBehavior.NoCookies;
                ProtocolFilter.CacheControl.WriteBehavior = requestItem.IsWriteCache ? HttpCacheWriteBehavior.Default : HttpCacheWriteBehavior.NoCache;

                //请求结果
                HttpResponseMessage responseMessage = null;
                //请求的异常
                Exception requestException = null;

                //获取请求体
                using (HttpRequestMessage requestMessage = await GetRequestMessage(requestItem))
                {
                    AutoResetEvent waitEvent = new AutoResetEvent(false);

                    //进行请求并获取结果
                    var requestTask = Task.Run(async () =>
                    {
                        try
                        {
                            responseMessage = await Client.SendRequestAsync(requestMessage);
                        }
                        catch (Exception ex)
                        {
                            requestException = ex;
                        }
                        finally
                        {
                            waitEvent.Set();
                        }
                    });

                    waitEvent.WaitOne(requestItem.TimeOut);
                }

                if (requestException != null)
                {
                    throw requestException;
                }

                if (responseMessage == null)
                {
                    result.Html = "请求超时";
                    result.StatusCode = HttpStatusCode.RequestTimeout;
                }
                else
                {
                    result.OriginResponse = responseMessage;

                    //设置状态
                    result.StatusCode = responseMessage.StatusCode;

                    //设置重定向地址
                    result.Location = responseMessage?.Headers?.Location;

                    //设置请求地址
                    result.RequestUri = responseMessage?.RequestMessage?.RequestUri;

                    //读取内容buffer
                    IBuffer buffer = await responseMessage.Content.ReadAsBufferAsync();

                    #region 根据返回类型处理请求的返回值

                    switch (requestItem.ResultType)
                    {
                        case ResultType.STRING: //返回字符串
                            result.Html = requestItem.Encoding.GetString(WindowsRuntimeBufferExtensions.ToArray(buffer), 0, (int)buffer.Length);
                            break;
                        case ResultType.DATA:   //返回数据
                            result.Data = buffer;
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (requestItem.ThrowException)
                {
                    throw;
                }
                else
                {
                    result.Html = ex.Message;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取Post请求正文内容
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        private async Task<IHttpContent> GetHttpContent(HttpItem requestItem)
        {
            IHttpContent result = null;
            if (!string.IsNullOrWhiteSpace(requestItem.PostData) || requestItem.PostDataCollection != null)
            {
                //根据发送类型获取请求内容
                switch (requestItem.PostType)
                {
                    case PostType.DATA:
                        result = await GetHttpStreamContent(requestItem);
                        break;
                    case PostType.STRING:
                    default:
                        result = GetHttpFormUrlEncodedContent(requestItem);
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取Post请求正文内容    流
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        private async Task<IHttpContent> GetHttpStreamContent(HttpItem requestItem)
        {
            IHttpContent result = null;
            if (requestItem.PostFiles?.Count > 0)
            {
                HttpMultipartFormDataContent httpMultipartFormDataContent = new HttpMultipartFormDataContent();

                if (!string.IsNullOrWhiteSpace(requestItem.PostData))
                {
                    requestItem.SetPostDataCollection();

                    foreach (var postdata in requestItem.PostDataCollection)
                    {
                        HttpStringContent kvContent = new HttpStringContent(postdata.Value);
                        kvContent.Headers.ContentType = null;
                        kvContent.Headers.ContentLength = null;

                        //将表单添加进MultipartForm
                        httpMultipartFormDataContent.Add(kvContent, postdata.Key);
                    }
                }

                foreach (var file in requestItem.PostFiles)
                {
                    //将byte转换为流
                    var stream = await BytesToStream(file.Data);

                    HttpStreamContent streamContent = new HttpStreamContent(stream);

                    streamContent.Headers.ContentType = new HttpMediaTypeHeaderValue(file.ContentType);
                    //修改了ContentDisposition没用？Why？？？
                    //streamContent.Headers.ContentDisposition = new HttpContentDispositionHeaderValue("form-data") { FileName = file.FileName, Name = file.Name, FileNameStar = file.FileName };

                    //将表单添加进MultipartForm
                    httpMultipartFormDataContent.Add(streamContent, file.Name, file.FileName);
                }

                //返回MultipartForm
                result = httpMultipartFormDataContent;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(requestItem.PostData))
                {
                    var stream = await BytesToStream(requestItem.Encoding.GetBytes(requestItem.PostData));

                    result = new HttpStreamContent(stream);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取Post请求正文内容    表单
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        private IHttpContent GetHttpFormUrlEncodedContent(HttpItem requestItem)
        {
            IHttpContent result = null;

            KeyValuePairCollection<string, string> keyValueList = new KeyValuePairCollection<string, string>(); ;

            if (requestItem.PostDataCollection?.Count > 0)
            {
                keyValueList.AddRange(requestItem.PostDataCollection);
            }
            if (!string.IsNullOrWhiteSpace(requestItem.PostData))
            {
                string[] formdatas = requestItem.PostData.Split('&');
                if (formdatas != null && formdatas.Length > 0)
                {
                    foreach (var item in formdatas)
                    {
                        string[] formdata = item.Split('=');
                        if (formdata.Length == 2)
                        {
                            keyValueList.Add(formdata[0], formdata[1]);
                            //keyValueList.Add(WebUtility.UrlDecode(formdata[0]), WebUtility.UrlDecode(formdata[1]));
                        }
                    }
                }
            }

            if (keyValueList.Count > 0)
            {
                result = new HttpFormUrlEncodedContent(keyValueList);
            }

            return result;
        }

        /// <summary>
        /// 获取请求消息
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        private async Task<HttpRequestMessage> GetRequestMessage(HttpItem requestItem)
        {
            HttpRequestMessage result = new HttpRequestMessage();

            //设置请求内容
            result.Content = await GetHttpContent(requestItem);

            #region 设置Method
            switch (requestItem.Method)
            {
                case MethodType.POST:
                    result.Method = new HttpMethod("POST");
                    break;
                case MethodType.GET:
                default:
                    result.Method = new HttpMethod("GET");
                    break;
            }
            #endregion

            //请求URI
            result.RequestUri = requestItem.URI;

            SetHeaders(result.Headers, requestItem);

            //从字符串设置cookie
            SetCookies(result.Headers.Cookie, requestItem.Cookie);

            //从集合设置cookie
            SetCookies(result.Headers.Cookie, requestItem.CookieCollection);

            return result;
        }

        /// <summary>
        /// 将参数httpItem除请求URL之外的设置配置为此对象的默认设置
        /// </summary>
        /// <param name="httpItem"></param>
        public void SetAsDefaultHeader(HttpItem httpItem)
        {
            SetCookies(Client.DefaultRequestHeaders.Cookie, httpItem.Cookie);

            SetCookies(Client.DefaultRequestHeaders.Cookie, httpItem.CookieCollection);

            SetHeaders(Client.DefaultRequestHeaders, httpItem);
        }

        /// <summary>
        /// 将cookie字符串添加到指定cookie集合中
        /// </summary>
        /// <param name="cookieCollection"></param>
        /// <param name="cookieStr"></param>
        private void SetCookies(HttpCookiePairHeaderValueCollection cookieCollection, string cookieStr)
        {
            if (!string.IsNullOrWhiteSpace(cookieStr) && cookieCollection != null)
            {
                string[] cookies = cookieStr.Split(';');
                foreach (var item in cookies)
                {
                    cookieCollection.TryParseAdd(item);
                }
            }
        }

        /// <summary>
        /// 将cookie集合添加到指定的cookie集合
        /// </summary>
        /// <param name="cookieCollection"></param>
        /// <param name="srcCookieCollection"></param>
        private void SetCookies(HttpCookiePairHeaderValueCollection cookieCollection, IEnumerable<HttpCookiePairHeaderValue> srcCookieCollection)
        {
            if (cookieCollection != null && srcCookieCollection != null)
            {
                foreach (var item in srcCookieCollection)
                {
                    cookieCollection.Add(item);
                }
            }
        }

        /// <summary>
        /// 将HttpItem的请求头信息设置到指定Http请求头
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="httpItem"></param>
        private void SetHeaders(HttpRequestHeaderCollection headers, HttpItem httpItem)
        {
            if (headers != null && httpItem != null)
            {
                if (!string.IsNullOrEmpty(httpItem.Accept))
                {
                    headers.Accept.TryParseAdd(httpItem.Accept);
                }

                if (!string.IsNullOrEmpty(httpItem.AcceptEncoding))
                {
                    headers.AcceptEncoding.TryParseAdd(httpItem.AcceptEncoding);
                }

                if (!string.IsNullOrEmpty(httpItem.AcceptLanguage))
                {
                    headers.AcceptLanguage.TryParseAdd(httpItem.AcceptLanguage);
                }

                //if (!string.IsNullOrEmpty(httpItem.ContentType))
                //{
                //    headers.Add("Content-Type", httpItem.ContentType);
                //}

                if (!string.IsNullOrEmpty(httpItem.Host))
                {
                    headers.Host = new HostName(httpItem.Host);
                }

                if (!string.IsNullOrEmpty(httpItem.Referer))
                {
                    headers.Referer = new Uri(httpItem.Referer);
                }

                if (!string.IsNullOrEmpty(httpItem.UserAgent))
                {
                    headers.UserAgent.TryParseAdd(httpItem.UserAgent);
                }

                if (httpItem.Header?.Count > 0)
                {
                    foreach (var item in httpItem.Header)
                    {
                        headers.Add(item.Key, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// byte数组转换为InMemoryRandomAccessStream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static async Task<InMemoryRandomAccessStream> BytesToStream(byte[] bytes)
        {
            InMemoryRandomAccessStream inputStream = new InMemoryRandomAccessStream();
            IBuffer postDataBuffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes);

            DataWriter datawriter = new DataWriter(inputStream.GetOutputStreamAt(0));
            datawriter.WriteBuffer(postDataBuffer, 0, postDataBuffer.Length);
            await datawriter.StoreAsync();
            return inputStream;
        }
    }
}
