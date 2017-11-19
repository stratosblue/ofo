using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.Utilities
{
    /// <summary>
    /// Ofo相关的工具
    /// </summary>
    public static class OfoUtility
    {
        /// <summary>
        /// 获取掩盖的手机号
        /// </summary>
        /// <returns></returns>
        public static string GetMaskTelPhoneNum(string phoneNum)
        {
            var stringBuilder = new StringBuilder();
            if (phoneNum.Length == 11)
            {
                stringBuilder.Append(phoneNum.Substring(0, 3));
                stringBuilder.Append("****");
                stringBuilder.Append(phoneNum.Substring(7, 4));
            }
            else
            {
                stringBuilder.Append(phoneNum);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 通过URL获取头像图片的访问流
        /// </summary>
        /// <param name="avatarUrl"></param>
        /// <returns></returns>
        public static async Task<BitmapImage> GetAvatarImageByUrlAsync(string avatarUrl, Action<BitmapImage> completeAction)
        {
            BitmapImage result = null;

            IRandomAccessStream avatarStream = null;

            try
            {
                //从网络或缓存获取头像
                if (!string.IsNullOrWhiteSpace(avatarUrl))
                {
                    var fileName = Path.GetFileName(avatarUrl);
                    if (!await LocalCacheUtility.ExistsCacheFile(fileName))
                    {
                        await LocalCacheUtility.CacheHttpFileAsync(fileName, avatarUrl);
                    }
                    avatarStream = await LocalCacheUtility.GetCacheAsync(fileName);

                }

                //头像没有正确获取，则获取默认头像
                if (avatarStream == null || avatarStream.Size <= 0)
                {
                    avatarStream = await ResourceUtility.GetApplicationResourceStreamAsync("Assets/icons/default_avatar_img.png");
                }

                if (avatarStream != null)
                {
                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        result = new BitmapImage();
                        result.SetSource(avatarStream);
                        completeAction?.Invoke(result);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                avatarStream = null;
            }
            finally
            {
                avatarStream?.Dispose();
            }

            return result;
        }
    }
}
