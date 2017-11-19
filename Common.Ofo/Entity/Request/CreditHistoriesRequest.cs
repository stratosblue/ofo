namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 信用记录请求
    /// </summary>
    public class GetCreditHistoriesRequest : BasePositionRequest
    {
        private int _page = 1;

        /// <summary>
        /// 获取的页面
        /// </summary>
        public int Page
        {
            get { return _page; }
            set
            {
                if (value < 0)
                {
                    _page = 1;
                }
                else
                {
                    _page = value;
                }
            }
        }

        /// <summary>
        /// 信用记录请求
        /// </summary>
        public GetCreditHistoriesRequest()
        {
            ApiUrl = ApiUrls.GetCreditHistories;
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&curPage={Page}";
        }
    }
}
