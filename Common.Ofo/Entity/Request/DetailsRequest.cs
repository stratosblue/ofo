namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 消费明细请求
    /// </summary>
    public class DetailsRequest : BaseRequest
    {
        #region 属性

        /// <summary>
        /// 类别 0消费 1充值
        /// </summary>
        public int Classify { get; set; } = 0;

        /// <summary>
        /// 页
        /// </summary>
        public int Page { get; set; } = 1;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 消费明细请求
        /// </summary>
        public DetailsRequest()
        {
            ApiUrl = ApiUrls.GetDetails;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&classify={Classify}&page={Page}";
        }

        #endregion 方法
    }
}