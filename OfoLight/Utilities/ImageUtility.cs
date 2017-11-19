using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public static class ImageUtility
    {
        /// <summary>
        /// 图片压缩
        /// （目前好像还有些问题）
        /// </summary>
        /// <param name="imageStream">图片流</param>
        /// <param name="fileType">文件后缀</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <returns></returns>
        public static async Task<IRandomAccessStream> ImageCompressAsync(IRandomAccessStream imageStream, string fileType, uint maxWidth, uint maxHeight)
        {
            IRandomAccessStream result = null;
            try
            {
                result = new InMemoryRandomAccessStream();

                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);

                //判断是否需要压缩图片
                if (decoder.PixelWidth > maxWidth || decoder.PixelHeight > maxHeight || imageStream.Size > 2097152)
                {
                    BitmapTransform transform = new BitmapTransform();
                    Guid encoderId;
                    switch (fileType)
                    {
                        case ".png":
                            encoderId = BitmapEncoder.PngEncoderId;
                            break;
                        case ".bmp":
                            encoderId = BitmapEncoder.BmpEncoderId;
                            break;
                        case ".jpg":
                        default:
                            encoderId = BitmapEncoder.JpegEncoderId;
                            break;
                    }

                    //设置缩放大小
                    if (decoder.OrientedPixelWidth / maxWidth > decoder.OrientedPixelHeight / maxHeight)    //按宽度缩放
                    {
                        double scale = maxWidth * 1.0 / decoder.OrientedPixelWidth;
                        transform.ScaledHeight = (uint)(decoder.OrientedPixelHeight * scale);
                        transform.ScaledWidth = (uint)(decoder.OrientedPixelWidth * scale);
                    }
                    else    //按高度缩放
                    {
                        double scale = maxHeight * 1.0 / decoder.OrientedPixelHeight;
                        transform.ScaledHeight = (uint)(decoder.OrientedPixelHeight * scale);
                        transform.ScaledWidth = (uint)(decoder.OrientedPixelWidth * scale);
                    }

                    #region 如果不这样做，可能会缩放出错？
                    var width = transform.ScaledWidth;
                    var height = transform.ScaledHeight;

                    if (decoder.OrientedPixelWidth != decoder.PixelWidth)
                    {
                        width = transform.ScaledHeight;
                        height = transform.ScaledWidth;
                    }
                    #endregion

                    // Fant是相对高质量的插值模式。  
                    transform.InterpolationMode = BitmapInterpolationMode.Fant;

                    // BitmapDecoder指示最佳匹配本地存储的图像数据的像素格式和alpha模式。 这可以提供高性能的与或质量增益。  
                    BitmapPixelFormat format = decoder.BitmapPixelFormat;
                    BitmapAlphaMode alpha = decoder.BitmapAlphaMode;

                    // PixelDataProvider提供对位图帧中像素数据的访问  
                    PixelDataProvider pixelProvider = await decoder.GetPixelDataAsync(
                        format,
                        alpha,
                        transform,
                        ExifOrientationMode.RespectExifOrientation,
                        ColorManagementMode.ColorManageToSRgb
                        );

                    byte[] pixels = pixelProvider.DetachPixelData();

                    //将像素数据写入编码器。  
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderId, result);
                    //设置像素数据  
                    encoder.SetPixelData(
                        format,
                        alpha,
                        width,
                        height,
                        decoder.DpiX,
                        decoder.DpiY,
                        pixels
                        );

                    await encoder.FlushAsync(); //异步提交和刷新所有图像数据（这一步保存图片到文件） 
                }
                else    //不需要缩放
                {
                    result = imageStream;
                }
            }
            catch (Exception ex)
            {
                result = null;
                Debug.WriteLine(ex);
            }

            return result;
        }
    }
}
