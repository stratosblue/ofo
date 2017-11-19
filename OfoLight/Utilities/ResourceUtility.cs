using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 资源工具类
    /// </summary>
    public static class ResourceUtility
    {
        /// <summary>
        /// 获取程序资源文件访问流
        /// 路径eg. Assets/new_splash_content.png
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<IRandomAccessStream> GetApplicationResourceStreamAsync(string url)
        {
            return await GetApplicationResourceStreamAsync(new Uri($"ms-appx:///{url}"));
        }

        /// <summary>
        /// 获取程序资源文件访问流
        /// 路径eg. ms-appx:///Assets/new_splash_content.png
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<IRandomAccessStream> GetApplicationResourceStreamAsync(Uri uri)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);

                return await file?.OpenAsync(FileAccessMode.Read);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
