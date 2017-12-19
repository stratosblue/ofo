namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 解锁车请求
    /// </summary>
    public class UnLockCarRequest : BasePositionRequest
    {
        #region 属性

        /// <summary>
        /// 车编号
        /// </summary>
        public string CarNumber { get; set; }

        /// <summary>
        /// continue?默认为空
        /// </summary>
        public string Continue { get; set; }

        /// <summary>
        /// Smart?默认值1
        /// </summary>
        public int Smart { get; set; } = 1;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 解锁车请求
        /// </summary>
        public UnLockCarRequest()
        {
            ApiUrl = ApiUrls.UnLockCar;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            return base.GetFormString() + $"&smart={Smart}&carno={CarNumber}&timestamp{GetTimeStamp()}&continue={Continue}";
        }

        #endregion 方法
    }
}