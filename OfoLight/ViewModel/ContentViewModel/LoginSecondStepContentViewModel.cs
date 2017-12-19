using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 登录第二步VM
    /// </summary>
    public class LoginSecondStepContentViewModel : BaseContentViewModel
    {
        #region 字段

        private string _phoneVerifyCode;
        private string _telPhone;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 登陆命令
        /// </summary>
        public ICommand LoginCommand { get; set; }

        public string PhoneVerifyCode
        {
            get { return _phoneVerifyCode; }
            set
            {
                _phoneVerifyCode = value;
                NotifyPropertyChanged("PhoneVerifyCode");
            }
        }

        /// <summary>
        /// 重新获取验证码命令
        /// </summary>
        public ICommand ReGetVerifyCodeCommand { get; set; }

        /// <summary>
        /// 用户注册协议
        /// </summary>
        public ICommand RegistrationProtocolCommand { get; set; }

        public string TelPhone
        {
            get { return _telPhone; }
            set
            {
                _telPhone = value;
                NotifyPropertyChanged("TelPhone");
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 登录第二步VM
        /// </summary>
        public LoginSecondStepContentViewModel()
        {
            TelPhone = OfoApi.CurUser.TelPhone;
            ReGetVerifyCodeCommand = new RelayCommand(async (state) =>
            {
                var getVerifyCodeResult = await OfoApi.ReGetVerifyCodeAsync();
                if (await CheckOfoApiResult(getVerifyCodeResult))
                {
                    await ShowNotifyAsync(getVerifyCodeResult.Message);
                }
            });

            LoginCommand = new RelayCommand(LoginAsync);

            RegistrationProtocolCommand = new RelayCommand(async (state) =>
            {
                WebPagePopupContentView webPagePopupContentView = new WebPagePopupContentView();

                WebPagePopupContentViewModel webPagePopupContentViewModel = new WebPagePopupContentViewModel(null)
                {
                    TargetUrl = "https://common.ofo.so/about/legal.html?11",
                };

                await ShowContentNotifyAsync(webPagePopupContentView, webPagePopupContentViewModel);
            });
        }

        #endregion 构造函数

        #region 方法

        protected override void CloseAction()
        {
            ContentNavigation(new ContentPageArgs() { IsGoBack = true, });
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="state"></param>
        private async void LoginAsync(object state)
        {
            if (string.IsNullOrWhiteSpace(PhoneVerifyCode))
            {
                await ShowNotifyAsync("请输入正确的验证码");
            }
            else
            {
                try
                {
                    var loginResult = await OfoApi.LoginAsync(PhoneVerifyCode.Trim());
                    if (await CheckOfoApiResultHttpStatus(loginResult))
                    {
                        if (loginResult.IsSuccess)
                        {
                            Global.AppConfig.Token = loginResult.Data.Token;
                            Global.SaveAppConfig();
                            ClientCookieManager.AddCookies(Global.COOKIE_DOMAIN, $"ofo-tokened={loginResult.Data.Token}");
                            Global.OfoApi.CurUser.Token = loginResult.Data.Token;
                            await TryNavigateAsync(typeof(MainPage));
                        }
                        else
                        {
                            await ShowNotifyAsync(loginResult.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await ShowNotifyAsync($"登陆出现异常：{ex}");
                }
            }
        }

        #endregion 方法
    }
}