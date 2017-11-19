namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 确认支付
    /// </summary>
    public class ConfirmToPayRequest : BaseRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderNum { get; set; }

        /// <summary>
        /// 钱包Id?默认为0
        /// </summary>
        public int Packetid { get; set; } = 0;

        /// <summary>
        /// 确认支付
        /// </summary>
        public ConfirmToPayRequest()
        {
            ApiUrl = ApiUrls.ConfirmToPay;
        }

        public override string GetFormString()
        {
            return base.GetFormString() + $"&ordernum={OrderNum}&packetid={Packetid}";
        }
    }
}
