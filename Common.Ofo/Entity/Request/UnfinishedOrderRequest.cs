namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 未完成订单查询请求
    /// </summary>
    public class UnfinishedOrderRequest : BaseRequest
    {
        /// <summary>
        /// 未完成订单查询请求
        /// </summary>
        public UnfinishedOrderRequest()
        {
            ApiUrl = ApiUrls.GetUnfinishedOrder;
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&timestamp={GetTimeStamp()}";
        }
    }
}
