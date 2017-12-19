namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 信用记录请求
    /// </summary>
    public class GetCreditHistoriesRequest : BasePositionRequest
    {
        #region 字段

        private int _page = 1;

        #endregion 字段

        #region 属性

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

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 信用记录请求
        /// </summary>
        public GetCreditHistoriesRequest()
        {
            ApiUrl = ApiUrls.GetCreditHistories;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&curPage={Page}";
        }

        #endregion 方法
    }
}