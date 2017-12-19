namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 保修原因列表获取请求
    /// </summary>
    public class RepairReasonListRequest : BaseRequest
    {
        #region 属性

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 保修原因列表获取请求
        /// </summary>
        public RepairReasonListRequest()
        {
            ApiUrl = ApiUrls.GetRepairReason;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&ordernum={OrderNum}";
        }

        #endregion 方法
    }
}