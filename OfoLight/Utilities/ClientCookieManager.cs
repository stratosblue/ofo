using System;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 客户端Cookie管理
    /// </summary>
    public static class ClientCookieManager
    {
        /// <summary>
        /// 基本协议筛选器
        /// </summary>
        private static HttpBaseProtocolFilter ProtocolFilter { get; set; } = new HttpBaseProtocolFilter();

        /// <summary>
        /// 清理指定url的cookie
        /// </summary>
        /// <param name="url"></param>
        public static void ClearCookie(string url)
        {
            ClearCookie(new Uri(url));
        }

        /// <summary>
        /// 清理指定URI的Cookie
        /// </summary>
        /// <param name="uri"></param>
        public static void ClearCookie(Uri uri)
        {
            foreach (var cookie in ProtocolFilter.CookieManager.GetCookies(uri))
            {
                ProtocolFilter.CookieManager.DeleteCookie(cookie);
            }
        }

        /// <summary>
        /// 附加Cookie
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="cookiesStr"></param>
        public static void AddCookies(string domain, string cookiesStr)
        {
            AddCookies(domain, cookiesStr, "/", null, false);
        }

        /// <summary>
        /// 附加Cookie
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="cookiesStr"></param>
        /// <param name="path"></param>
        /// <param name="expires"></param>
        /// <param name="httpOnly"></param>
        public static void AddCookies(string domain, string cookiesStr, string path, DateTime? expires, bool httpOnly)
        {
            if (!string.IsNullOrWhiteSpace(cookiesStr))
            {
                string[] cookies = cookiesStr.Split(';');
                foreach (var item in cookies)
                {
                    var kv = item.Split('=');
                    if (kv.Length == 2)
                    {
                        AddCookie(domain, kv[0], kv[1], path, expires, httpOnly);
                    }
                }
            }
        }

        /// <summary>
        /// 附加Cookie
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="expires"></param>
        /// <param name="httpOnly"></param>
        public static void AddCookie(string domain, string name, string value, string path, DateTime? expires, bool httpOnly)
        {
            var cookie = new HttpCookie(name, domain, path);
            cookie.Value = value;
            if (expires.HasValue)
            {
                cookie.Expires = new DateTimeOffset(expires.Value);
            }
            cookie.HttpOnly = httpOnly;

            ProtocolFilter.CookieManager.SetCookie(cookie);
        }
    }
}
