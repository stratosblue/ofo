using System;
using System.Collections.Generic;
using System.Text;
using Windows.Web.Http.Headers;

namespace HttpUtility
{
    public class HttpItem
    {
        /// <summary>
        /// 默认是否使用app容器内URI的Cookies
        /// </summary>
        public static bool DefaultUseCookies = true;

        /// <summary>
        /// 默认是否将请求结果写入缓存
        /// </summary>
        public static bool DefaultCacheWrite = true;

        /// <summary>
        /// 默认是否抛出异常
        /// </summary>
        public static bool DefaultThrowException = false;

        /// <summary>
        /// Http请求对象
        /// </summary>
        public HttpItem()
        { }

        /// <summary>
        /// 访问地址URL
        /// </summary>
        private string _url;

        /// <summary>
        /// 请求地址
        /// </summary>
        private Uri _uri;

        /// <summary>
        /// Cookie字符串
        /// </summary>
        private string _cookie;

        /// <summary>
        /// Cookie集合
        /// </summary>
        private List<HttpCookiePairHeaderValue> _cookieCollection;

        /// <summary>
        /// 超时时间
        /// </summary>
        private int _timeOut = 25_000;

        /// <summary>
        /// 请求类型
        /// </summary>
        public MethodType Method { get; set; } = MethodType.GET;

        /// <summary>
        /// 请求地址
        /// </summary>
        public Uri URI
        {
            get
            {
                return _uri;
            }

            set
            {
                _uri = value;
                _url = value.ToString();
            }
        }

        /// <summary>
        /// 访问地址URL
        /// </summary>
        public string URL
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
                _uri = new Uri(value);
            }
        }

        /// <summary>
        /// post数据
        /// 不需要进行Urlencode处理
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// post的文件列表描述
        /// </summary>
        public List<PostFile> PostFiles { get; set; } = new List<PostFile>();

        /// <summary>
        /// post数据键值对集合
        /// </summary>
        public KeyValuePairCollection<string, string> PostDataCollection { get; set; } = new KeyValuePairCollection<string, string>();

        /// <summary>
        /// Cookie字符串
        /// </summary>
        public string Cookie
        {
            get
            {
                return _cookie;
            }

            set
            {
                _cookie = value;
            }
        }

        /// <summary>
        /// Cookie集合
        /// </summary>
        public List<HttpCookiePairHeaderValue> CookieCollection
        {
            get
            {
                return _cookieCollection;
            }

            set
            {
                _cookieCollection = value;
            }
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        public ResultType ResultType { get; set; } = ResultType.STRING;

        /// <summary>
        /// 请求的编码 默认UTF-8
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// post数据类型
        /// </summary>
        public PostType PostType { get; set; } = PostType.STRING;

        /// <summary>
        /// 请求UA
        /// 默认为 UserAgents.Edge 的值
        /// </summary>
        public string UserAgent { get; set; } = UserAgents.Edge;

        /// <summary>
        /// HTTP Accept 标头
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// HTTP Accept-Encoding 标头
        /// </summary>
        public string AcceptEncoding { get; set; }

        /// <summary>
        /// HTTP Accept-Language 标头
        /// </summary>
        public string AcceptLanguage { get; set; }

        /// <summary>
        /// HTTP ContentType 标头
        /// </summary>
        [Obsolete("目前没有用")]
        public string ContentType { get; set; }

        /// <summary>
        /// HTTP Host 标头
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// HTTP Referer 标头
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 其它请求头
        /// </summary>
        public KeyValuePairCollection<string, string> Header = new KeyValuePairCollection<string, string>();

        /// <summary>
        /// 指示是否应跟随重定向响应的值。
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = true;

        /// <summary>
        /// 指示是否能够在服务器发出请求时提示用户输入凭据的值。
        /// </summary>
        public bool AllowUI { get; set; } = true;

        /// <summary>
        /// 指示是否可自动解压缩 HTTP 内容响应的值。
        /// </summary>
        public bool AutomaticDecompression { get; set; } = true;

        /// <summary>
        /// 允许每个 HTTP 服务器建立的连接的最大数目。
        /// </summary>
        public uint MaxConnectionsPerServer { get; set; } = 6;

        /// <summary>
        /// 指示是否可使用代理发送 HTTP 请求的值。
        /// </summary>
        public bool UseProxy { get; set; } = true;

        /// <summary>
        /// 是否使用app容器内URI的Cookies
        /// </summary>
        public bool UseCookies { get; set; } = DefaultUseCookies;

        /// <summary>
        /// 是否将请求写入缓存
        /// </summary>
        public bool IsWriteCache { get; set; } = DefaultCacheWrite;

        /// <summary>
        /// 是否抛出异常
        /// </summary>
        public bool ThrowException { get; set; } = DefaultThrowException;

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut
        {
            get { return _timeOut; }
            set
            {
                _timeOut = value;
            }
        }

        /// <summary>
        /// 将postData设置到postDataCollection
        /// </summary>
        public void SetPostDataCollection()
        {
            if (!string.IsNullOrWhiteSpace(PostData))
            {
                string[] formdatas = PostData.Split('&');
                if (formdatas != null && formdatas.Length > 0)
                {
                    foreach (var item in formdatas)
                    {
                        string[] formdata = item.Split('=');
                        if (formdata.Length == 2)
                        {
                            PostDataCollection.Add(formdata[0], formdata[1]);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 键值对集合
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class KeyValuePairCollection<T1, T2> : List<KeyValuePair<T1, T2>>
    {
        /// <summary>
        /// 增加一个键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(T1 key, T2 value)
        {
            base.Add(new KeyValuePair<T1, T2>(key, value));
        }
    }

    /// <summary>
    /// 部分浏览器参考UA
    /// </summary>
    public class UserAgents
    {
        public const string Edge = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
        public const string IE = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
        public const string FireFox = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:54.0) Gecko/20100101 Firefox/54.0";
        public const string Chrome = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
    }

    /// <summary>
    /// 请求类型
    /// </summary>
    public enum MethodType
    {
        GET,
        POST
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        STRING,
        /// <summary>
        /// 数据
        /// </summary>
        DATA
    }

    /// <summary>
    /// Post数据类型
    /// </summary>
    public enum PostType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        STRING,
        /// <summary>
        /// 数据
        /// </summary>
        DATA
    }

    /// <summary>
    /// Post的文件描述
    /// </summary>
    public class PostFile
    {
        /// <summary>
        /// 键名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 内容类型
        /// 默认：application/octet-stream
        /// </summary>
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
