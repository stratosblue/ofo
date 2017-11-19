using HttpUtility;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 本地缓存工具
    /// </summary>
    public class LocalCacheUtility
    {
        /// <summary>
        /// 本地缓存文件夹
        /// </summary>
        public static StorageFolder LocalCacheFolder { get; private set; } = ApplicationData.Current.LocalCacheFolder;

        /// <summary>
        /// 缓存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public static async Task<bool> CacheAsync(string fileName, byte[] bytes)
        {
            var ibuffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);
            return await CacheAsync(fileName, ibuffer);
        }

        /// <summary>
        /// 缓存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ibuffer"></param>
        /// <returns></returns>
        public static async Task<bool> CacheAsync(string fileName, IBuffer ibuffer)
        {
            try
            {
                var fileInfo = await LocalCacheFolder.CreateFileAsync(fileName);
                using (var irandomAccessStream = await fileInfo.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await irandomAccessStream.WriteAsync(ibuffer);
                    await irandomAccessStream.FlushAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 缓存网络文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<bool> CacheHttpFileAsync(string fileName, string url)
        {
            var result = await new HttpHelper().GetResultAsync(new HttpItem()
            {
                ResultType = ResultType.DATA,
                URL = url,
            });
            if (result.HttpOk)
            {
                return await CacheAsync(fileName, result.Data);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否存在缓存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<bool> ExistsCacheFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            var storageItem = await LocalCacheFolder.TryGetItemAsync(fileName);
            return storageItem != null;
        }

        /// <summary>
        /// 删除缓存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteCacheFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return true;
            }

            var storageItem = await LocalCacheFolder.TryGetItemAsync(fileName);
            if (storageItem == null)
            {
                return true;
            }
            else
            {
                try
                {
                    await storageItem.DeleteAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 清空本地缓存
        /// </summary>
        /// <returns></returns>
        public static async Task ClearLocalCacheFile()
        {
            var files = await LocalCacheFolder.GetFilesAsync();

            foreach (var file in files)
            {
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }

            var folders = await LocalCacheFolder.GetFoldersAsync();

            foreach (var folder in folders)
            {
                await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }

        /// <summary>
        /// 缓存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public static async Task<IRandomAccessStream> GetCacheAsync(string fileName)
        {
            IRandomAccessStream result = null;
            try
            {
                var fileInfo = await LocalCacheFolder.GetFileAsync(fileName);
                result = await fileInfo.OpenReadAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return result;
        }

        /// <summary>
        /// 获取本地缓存文件的总大小
        /// </summary>
        /// <param name="containsSubFolder">是否包含子目录</param>
        /// <returns></returns>
        public static async Task<ulong> GetLocalCacheSizeAsync(bool containsSubFolder)
        {
            return await CountStorageFolderSize(LocalCacheFolder, containsSubFolder);
        }

        /// <summary>
        /// 计算目录文件大小
        /// </summary>
        /// <param name="storageFolder"></param>
        /// <param name="containsSubFolder">是否包含子目录</param>
        /// <returns></returns>
        public static async Task<ulong> CountStorageFolderSize(StorageFolder storageFolder, bool containsSubFolder)
        {
            ulong result = 0;
            var files = await storageFolder.GetFilesAsync();
            foreach (var file in files)
            {
                var fileProperties = await file.GetBasicPropertiesAsync();
                result += fileProperties.Size;
            }

            if (containsSubFolder)
            {
                var folders = await storageFolder.GetFoldersAsync();

                foreach (var folder in folders)
                {
                    result += await CountStorageFolderSize(folder, containsSubFolder);
                }
            }

            return result;
        }
    }
}
