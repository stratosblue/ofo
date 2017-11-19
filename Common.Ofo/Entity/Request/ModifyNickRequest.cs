namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 昵称修改请求
    /// </summary>
    public class ModifyNickRequest : BaseRequest
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// 昵称修改请求
        /// </summary>
        public ModifyNickRequest()
        {
            ApiUrl = ApiUrls.ModifyUserNick;
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&nickname={Nick}";
        }
    }
}
