using System.Text;

namespace Common.Ofo.Entity.Request
{
    /// <summary>
    /// 验证码验证
    /// </summary>
    public class VerifyCodeRequest : BaseRequest
    {
        #region 属性

        /// <summary>
        /// 最后一次的验证码
        /// </summary>
        public static string LastCaptchaCode { get; set; }

        /// <summary>
        /// 最后一次的验证ID
        /// </summary>
        public static string LastVerifyId { get; set; }

        /// <summary>
        /// 验证码
        /// 表单参数captcha
        /// </summary>
        public string CaptchaCode { get; set; }

        /// <summary>
        /// 是否是重新获取
        /// 表单参数repeatGetSms=1
        /// </summary>
        public bool IsRepeatGetSms { get; set; } = false;

        /// <summary>
        /// 电话
        /// 表单参数tel
        /// </summary>
        public string TelPhone { get; set; }

        /// <summary>
        /// 验证ID
        /// 表单参数verifyId
        /// </summary>
        public string VerifyId { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 验证码验证
        /// </summary>
        public VerifyCodeRequest()
        {
            ApiUrl = ApiUrls.GetVerifyCode;
        }

        #endregion 构造函数

        #region 方法

        public override string GetFormString()
        {
            StringBuilder stringBuilder = new StringBuilder(base.GetFormString());
            if (IsRepeatGetSms)
            {
                stringBuilder.Append($"&repeatGetSms=1");
                VerifyId = LastVerifyId;
                CaptchaCode = LastCaptchaCode;
            }
            else
            {
                LastVerifyId = VerifyId;
                LastCaptchaCode = CaptchaCode;
            }

            stringBuilder.Append($"&tel={TelPhone}&captcha={CaptchaCode}&verifyId={VerifyId}");
            return stringBuilder.ToString();
        }

        #endregion 方法
    }
}