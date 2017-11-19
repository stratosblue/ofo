using HttpUtility;

namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 头像修改请求
    /// </summary>
    public class ModifyUserAvatarRequest : BaseRequest
    {
        /// <summary>
        /// 头像数据
        /// </summary>
        public byte[] AvatarData { get; set; }

        /// <summary>
        /// 头像修改请求
        /// </summary>
        public ModifyUserAvatarRequest()
        {
            ApiUrl = ApiUrls.ModifyUserAvatar;
        }

        public override HttpItem GetHttpItem()
        {
            HttpItem result = new HttpItem()
            {
                Method = MethodType.POST,
                PostData = GetFormString(),
                URL = ApiUrl,
                PostType = PostType.DATA,
            };

            result.PostFiles.Add(
                        new PostFile()
                        {
                            ContentType = "image/jpeg",
                            FileName = $"file_{BaseRequest.GetTimeStamp().ToString()}",
                            Data = AvatarData,
                            Name = "file",
                        }
                    );

            return result;
        }
    }
}
