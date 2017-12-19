namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 结束骑行请求
    /// </summary>
    public class EndRideRequest : BasePositionRequest
    {
        #region 属性

        /// <summary>
        /// 订单号
        /// </summary>
        public string Ordernum { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 结束骑行请求
        /// </summary>
        public EndRideRequest()
        {
            ApiUrl = ApiUrls.EndRide;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&ordernum={Ordernum}";
        }

        #endregion 方法
    }
}