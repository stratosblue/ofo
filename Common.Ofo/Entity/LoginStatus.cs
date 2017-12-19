namespace Common.Ofo.Entity
{
    /// <summary>
    /// 登陆状态
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// 默认值，没有正确检查
        /// </summary>
        Default,

        /// <summary>
        /// 网络失败
        /// </summary>
        NetWorkFailed,

        /// <summary>
        ///没有Token
        /// </summary>
        NoToken,

        /// <summary>
        /// Token过期
        /// </summary>
        TokenExpire,

        /// <summary>
        /// 正确登录的Token
        /// </summary>
        Logined,

        /// <summary>
        /// 检查失败
        /// </summary>
        CheckFailed,
    }
}
