using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace OfoLight.Utilities
{
    /// <summary>
    /// 访问流工具类
    /// </summary>
    public class AccessStreamUtility
    {
        /// <summary>
        /// 从base64字符串获取内存随机访问流
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static async Task<InMemoryRandomAccessStream> GetRandomAccessStreamFormBase64String(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            return await GetRandomAccessStreamFormBytes(bytes);
        }

        /// <summary>
        /// 从bytes数组获取内存随机访问流
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static async Task<InMemoryRandomAccessStream> GetRandomAccessStreamFormBytes(byte[] bytes)
        {
            var buffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);

            return await GetRandomAccessStreamFormIBuffer(buffer);
        }

        /// <summary>
        /// 从IBuffer获取内存随机访问流
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static async Task<InMemoryRandomAccessStream> GetRandomAccessStreamFormIBuffer(IBuffer buffer)
        {
            InMemoryRandomAccessStream result = new InMemoryRandomAccessStream();

            DataWriter datawriter = new DataWriter(result.GetOutputStreamAt(0));
            datawriter.WriteBuffer(buffer, 0, buffer.Length);
            await datawriter.StoreAsync();

            return result;
        }

        /// <summary>
        /// Buffer转换为byte[]
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] BufferToBytes(IBuffer buffer)
        {
            using (var dataReader = DataReader.FromBuffer(buffer))
            {
                var bytes = new byte[buffer.Length];
                dataReader.ReadBytes(bytes);
                return bytes;
            }
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            if (stream.CanSeek) // stream.Length 已确定
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return bytes;
            }
            else // stream.Length 不确定
            {
                int initialLength = 32768; // 32k

                byte[] buffer = new byte[initialLength];
                int read = 0;

                int chunk;
                while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                {
                    read += chunk;

                    if (read == buffer.Length)
                    {
                        int nextByte = stream.ReadByte();

                        if (nextByte == -1)
                        {
                            return buffer;
                        }

                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }

                byte[] ret = new byte[read];
                Array.Copy(buffer, ret, read);
                return ret;
            }
        }

        public static IBuffer RandomAccessStream2Buffer(IRandomAccessStream randomStream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(randomStream.GetInputStreamAt(0));
            MemoryStream memoryStream = new MemoryStream();
            if (stream != null)
            {
                byte[] bytes = StreamToBytes(stream);
                if (bytes != null)
                {
                    var binaryWriter = new BinaryWriter(memoryStream);
                    binaryWriter.Write(bytes);
                }
            }
            IBuffer buffer = WindowsRuntimeBufferExtensions.GetWindowsRuntimeBuffer(memoryStream, 0, (int)memoryStream.Length);
            return buffer;
        }

        /// <summary>
        /// IRandomAccessStream转换为bytes
        /// </summary>
        /// <param name="randomAccessStream"></param>
        /// <returns></returns>
        public static async Task<byte[]> AccessStreamToBytesAsync(IRandomAccessStream randomAccessStream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(randomAccessStream.GetInputStreamAt(0));
            MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
