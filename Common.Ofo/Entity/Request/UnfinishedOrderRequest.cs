namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 未完成订单查询请求
    /// </summary>
    public class UnfinishedOrderRequest : BaseRequest
    {
        #region 构造函数

        /// <summary>
        /// 未完成订单查询请求
        /// </summary>
        public UnfinishedOrderRequest()
        {
            ApiUrl = ApiUrls.GetUnfinishedOrder;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&timestamp={GetTimeStamp()}";
        }

        #endregion 方法
    }
}