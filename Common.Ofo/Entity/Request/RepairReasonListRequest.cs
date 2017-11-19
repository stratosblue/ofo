namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 保修原因列表获取请求
    /// </summary>
    public class RepairReasonListRequest : BaseRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 保修原因列表获取请求
        /// </summary>
        public RepairReasonListRequest()
        {
            ApiUrl = ApiUrls.GetRepairReason;
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&ordernum={OrderNum}";
        }
    }
}
