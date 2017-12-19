namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 确认支付
    /// </summary>
    public class ConfirmToPayRequest : BaseRequest
    {
        #region 属性

        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderNum { get; set; }

        /// <summary>
        /// 钱包Id?默认为0
        /// </summary>
        public int Packetid { get; set; } = 0;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 确认支付
        /// </summary>
        public ConfirmToPayRequest()
        {
            ApiUrl = ApiUrls.ConfirmToPay;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&ordernum={OrderNum}&packetid={Packetid}";
        }

        #endregion 方法
    }
}