using Common.Ofo.Entity;
using Common.Ofo.Entity.Request;
using Common.Ofo.Entity.Result;
using HttpUtility;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Common.Ofo
{
    /// <summary>
    /// OFOWeb接口实现
    /// </summary>
    public class OfoWebAPIs
    {
        #region 字段

        /// <summary>
        /// 当前用户
        /// </summary>
        private User _curUser = new User();

        #endregion 字段

        #region 属性

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurUser
        {
            get => _curUser;
            set
            {
                _curUser = value ?? throw new NullReferenceException("用户不可为空");
            }
        }

        /// <summary>
        /// 最后的地址
        /// </summary>
        public BasicGeoposition LastLocation { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// OFOWeb接口实现
        /// </summary>
        public OfoWebAPIs()
        {
        }

        /// <summary>
        /// OFOWeb接口实现
        /// </summary>
        /// <param name="telPhone">电话号码</param>
        public OfoWebAPIs(string telPhone) : this()
        {
            CurUser.TelPhone = telPhone;
        }

        /// <summary>
        /// OFOWeb接口实现
        /// </summary>
        /// <param name="user">用户</param>
        public OfoWebAPIs(User user) : this()
        {
            CurUser = user;
        }

        #endregion 构造函数

        #region 功能实现

        /// <summary>
        /// 检查登陆状态
        /// </summary>
        /// <returns></returns>
        public async Task<LoginStatus> CheckLoginStatus()
        {
            LoginStatus result = LoginStatus.Default;

            //无Token直接登陆页面
            if (string.IsNullOrWhiteSpace(CurUser.Token))
            {
                result = LoginStatus.NoToken;
            }
            else
            {
                var userInfo = await GetUserInfoAsync();

                if (userInfo.OK)
                {
                    if (userInfo.IsSuccess)
                    {
                        CurUser.TelPhone = userInfo.Data.TelPhone;
                        return LoginStatus.Logined;
                    }
                    else
                    {
                        result = LoginStatus.TokenExpire;
                    }
                }
                else
                {
                    result = LoginStatus.NetWorkFailed;
                }
            }
            return result;
        }

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderNum">订单编号</param>
        /// <param name="packetid">钱包id</param>
        /// <param name="packetid">骑行卡id</param>
        /// <returns></returns>
        public async Task<ConfirmToPayResult> ConfirmToPayAsync(long orderNum, long packetid, string cardId)
        {
            ConfirmToPayRequest request = new ConfirmToPayRequest()
            {
                OrderNum = orderNum,
                CardId = cardId,
                Packetid = packetid,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<ConfirmToPayResult>(request);
        }

        /// <summary>
        /// 结束骑行
        /// </summary>
        /// <param name="ordernum">订单号</param>
        /// <returns></returns>
        public async Task<EndRideResult> EndRideAsync(string ordernum)
        {
            return await EndRideAsync(ordernum, LastLocation);
        }

        /// <summary>
        /// 结束骑行
        /// </summary>
        /// <param name="ordernum">订单号</param>
        /// <param name="location">位置</param>
        /// <returns></returns>
        public async Task<EndRideResult> EndRideAsync(string ordernum, BasicGeoposition location)
        {
            if (string.IsNullOrWhiteSpace(ordernum))
            {
                return new EndRideResult() { Message = "订单号不正确" };
            }
            SetLastLocation(location);

            EndRideRequest request = new EndRideRequest()
            {
                Ordernum = ordernum,
                Location = location,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<EndRideResult>(request);
        }

        /// <summary>
        /// 获取活动中心首页详情
        /// </summary>
        /// <returns></returns>
        public async Task<ActivityHomePageDetailResult> GetActivityHomePageDetailAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetActivityHomePageDetail,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<ActivityHomePageDetailResult>(request);
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActivityListResult> GetActivityListAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetActivityList,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<ActivityListResult>(request);
        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <returns></returns>
        public async Task<AdvertisementResult> GetAdvertisementsAsync()
        {
            return await GetAdvertisementsAsync(LastLocation);
        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns></returns>
        public async Task<AdvertisementResult> GetAdvertisementsAsync(BasicGeoposition location)
        {
            SetLastLocation(location);

            BasePositionRequest request = new BasePositionRequest()
            {
                ApiUrl = ApiUrls.GetAdvertisements,
                Location = location,
                Token = CurUser.Token,
            };
            return await GetHttpResultAsync<AdvertisementResult>(request);
        }

        /// <summary>
        /// 获取蓝贴？
        /// </summary>
        /// <returns></returns>
        public async Task<BlueBarResult> GetBlueBarAsync()
        {
            return await GetBlueBarAsync(LastLocation);
        }

        /// <summary>
        /// 获取蓝贴？
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns></returns>
        public async Task<BlueBarResult> GetBlueBarAsync(BasicGeoposition location)
        {
            SetLastLocation(location);

            BasePositionRequest request = new BasePositionRequest()
            {
                ApiUrl = ApiUrls.GetBlueBar,
                Location = location,
                Token = CurUser.Token,
            };
            return await GetHttpResultAsync<BlueBarResult>(request);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public async Task<GetCaptchaCodeResult> GetCaptchaCodeAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetCaptchaCode,
            };
            return await GetHttpResultAsync<GetCaptchaCodeResult>(request);
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<ConfigResult> GetConfigAsync()
        {
            return await GetConfigAsync(LastLocation);
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns></returns>
        public async Task<ConfigResult> GetConfigAsync(BasicGeoposition location)
        {
            SetLastLocation(location);

            BasePositionRequest request = new BasePositionRequest()
            {
                ApiUrl = ApiUrls.GetConfig,
                Location = location,
                Token = CurUser.Token,
            };
            return await GetHttpResultAsync<ConfigResult>(request);
        }

        /// <summary>
        /// 获取消费明细
        /// </summary>
        /// <param name="page">页面</param>
        /// <returns></returns>
        public async Task<ConsumerDetailsResult> GetConsumerDetailsAsync(int page = 1)
        {
            DetailsRequest request = new DetailsRequest()
            {
                Classify = 0,
                Page = page,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<ConsumerDetailsResult>(request);
        }

        /// <summary>
        /// 获取信用历史
        /// </summary>
        /// <param name="page">获取页数</param>
        /// <returns></returns>
        public async Task<GetCreditHistoriesResult> GetCreditHistoriesAsync(int page)
        {
            GetCreditHistoriesRequest request = new GetCreditHistoriesRequest()
            {
                Token = CurUser.Token,
                Page = page,
            };

            return await GetHttpResultAsync<GetCreditHistoriesResult>(request);
        }

        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <returns></returns>
        public async Task<IdentificationResult> GetIdentificationInfoAsync()
        {
            return await GetIdentificationInfoAsync(LastLocation);
        }

        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns></returns>
        public async Task<IdentificationResult> GetIdentificationInfoAsync(BasicGeoposition location)
        {
            SetLastLocation(location);

            IdentificationRequest request = new IdentificationRequest()
            {
                Location = location,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<IdentificationResult>(request);
        }

        /// <summary>
        /// 获取附近的车辆
        /// 以上一次的位置获取
        /// </summary>
        /// <returns></returns>
        public async Task<NearByOfoCarsResult> GetNearByOfoCarAsync()
        {
            return await GetNearByOfoCarAsync(LastLocation);
        }

        /// <summary>
        /// 获取附近的车辆
        /// </summary>
        /// <param name="location">位置</param>
        /// <returns></returns>
        public async Task<NearByOfoCarsResult> GetNearByOfoCarAsync(BasicGeoposition location)
        {
            SetLastLocation(location);

            BasePositionRequest request = new BasePositionRequest()
            {
                ApiUrl = ApiUrls.GetNearByOfoCar,
                Location = location,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<NearByOfoCarsResult>(request);
        }

        /// <summary>
        /// 获取充值明细
        /// </summary>
        /// <param name="page">页面</param>
        /// <returns></returns>
        public async Task<RechargeDetailsResult> GetRechargeDetailsAsync(int page = 1)
        {
            DetailsRequest request = new DetailsRequest()
            {
                Classify = 1,
                Page = page,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<RechargeDetailsResult>(request);
        }

        /// <summary>
        /// 获取红包列表
        /// </summary>
        /// <returns></returns>
        public async Task<RedPacketListResult> GetRedPacketListAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetRedPacketList,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<RedPacketListResult>(request);
        }

        /// <summary>
        /// 获取报修原因列表
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <returns></returns>
        public async Task<RepairReasonListResult> GetRepairReasonListAsync(string orderNumber)
        {
            RepairReasonListRequest request = new RepairReasonListRequest()
            {
                Token = CurUser.Token,
                OrderNum = orderNumber,
            };
            return await GetHttpResultAsync<RepairReasonListResult>(request);
        }

        /// <summary>
        /// 获取未完成订单
        /// </summary>
        /// <returns></returns>
        public async Task<UnfinishedOrderResult> GetUnfinishedOrderAsync()
        {
            UnfinishedOrderRequest request = new UnfinishedOrderRequest()
            {
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<UnfinishedOrderResult>(request);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfoResult> GetUserInfoAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetUserInfo,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<UserInfoResult>(request);
        }

        /// <summary>
        /// 获取用户简介
        /// </summary>
        /// <returns></returns>
        public async Task<UserProfileResult> GetUserProfileAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetUserProfile,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<UserProfileResult>(request);
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="captchaCode">图片验证码</param>
        /// <param name="verifyId">验证ID</param>
        /// <returns></returns>
        public async Task<GetVerifyCodeResult> GetVerifyCodeAsync(string captchaCode, string verifyId)
        {
            if (string.IsNullOrWhiteSpace(CurUser?.TelPhone))
            {
                throw new ArgumentException("没有设置TelPhone，执行该请求必须先设置此属性");
            }
            else if (string.IsNullOrWhiteSpace(captchaCode) || string.IsNullOrWhiteSpace(verifyId))
            {
                throw new ArgumentException("captchaCode或verifyId值为空");
            }

            VerifyCodeRequest request = new VerifyCodeRequest()
            {
                CaptchaCode = captchaCode,
                VerifyId = verifyId,
                TelPhone = CurUser.TelPhone,
            };
            return await GetHttpResultAsync<GetVerifyCodeResult>(request);
        }

        /// <summary>
        /// 获取钱包信息
        /// </summary>
        /// <returns></returns>
        public async Task<WalletInfoResult> GetWalletInfoAsync()
        {
            BaseRequest request = new BaseRequest()
            {
                ApiUrl = ApiUrls.GetWalletInfo,
                Token = CurUser.Token
            };
            return await GetHttpResultAsync<WalletInfoResult>(request);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="authCode">授权码？？</param>
        /// <returns></returns>
        public async Task<LoginResult> LoginAsync(string code, string authCode = null)
        {
            LoginRequest request = new LoginRequest()
            {
                TelPhone = CurUser.TelPhone,
                Code = code,
                AuthCode = authCode,
            };
            return await GetHttpResultAsync<LoginResult>(request);
        }

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResult> ModifyUserAvatarAsync(byte[] avatarData)
        {
            ModifyUserAvatarRequest request = new ModifyUserAvatarRequest()
            {
                AvatarData = avatarData,
                Token = CurUser.Token,
            };

            return await GetHttpResultAsync<BaseResult>(request);
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="nick">新的昵称</param>
        /// <returns></returns>
        public async Task<BaseResult> ModifyUserNickAsync(string nick)
        {
            if (nick?.Length < 4)
            {
                throw new ArgumentException("昵称过短");
            }
            else if (nick?.Length > 16)
            {
                throw new ArgumentException("昵称过长");
            }

            ModifyNickRequest request = new ModifyNickRequest()
            {
                Token = CurUser.Token,
                Nick = nick,
            };
            return await GetHttpResultAsync<BaseResult>(request);
        }

        /// <summary>
        /// 再次获取手机验证码
        /// </summary>
        /// <returns></returns>
        public async Task<GetVerifyCodeResult> ReGetVerifyCodeAsync()
        {
            if (string.IsNullOrWhiteSpace(CurUser?.TelPhone))
            {
                throw new ArgumentException("没有设置TelPhone，执行该请求必须先设置此属性");
            }
            else if (string.IsNullOrWhiteSpace(VerifyCodeRequest.LastCaptchaCode) || string.IsNullOrWhiteSpace(VerifyCodeRequest.LastVerifyId))
            {
                throw new ArgumentException("最后一次请求的captchaCode或verifyId值为空");
            }

            VerifyCodeRequest request = new VerifyCodeRequest()
            {
                TelPhone = CurUser.TelPhone,
                IsRepeatGetSms = true,
            };

            return await GetHttpResultAsync<GetVerifyCodeResult>(request);
        }

        /// <summary>
        /// 车辆报修
        /// </summary>
        /// <param name="location">位置</param>
        /// <param name="orderNumber">订单编号</param>
        /// <param name="isGsm">是否gsm 0,1</param>
        /// <param name="address">地址字符串</param>
        /// <param name="reason">保修原因，由接口获得</param>
        /// <returns></returns>
        public async Task<BaseResult> ReportRepairCarAsync(BasicGeoposition location, string orderNumber, int isGsm, string address, string reason)
        {
            SetLastLocation(location);

            ReportRepairRequest request = new ReportRepairRequest()
            {
                Location = location,
                Token = CurUser.Token,
                OrderNumber = orderNumber,
                Address = address,
                IsGsm = isGsm,
                Reason = reason,
            };
            return await GetHttpResultAsync<BaseResult>(request);
        }

        /// <summary>
        /// 解锁车
        /// </summary>
        /// <param name="carNumber">车辆编号</param>
        /// <returns></returns>
        public async Task<UnLockCarResult> UnlockCarAsync(string carNumber)
        {
            UnLockCarRequest request = new UnLockCarRequest()
            {
                Location = LastLocation,
                Token = CurUser.Token,
                CarNumber = carNumber,
            };
            return await GetHttpResultAsync<UnLockCarResult>(request);
        }

        #endregion 功能实现

        #region 基础实现

        /// <summary>
        /// 将json字符串转换为指定对象
        /// </summary>
        /// <typeparam name="T">目标对象</typeparam>
        /// <param name="str">源json字符串</param>
        /// <returns></returns>
        private static T ConvertToEntity<T>(string str)
        {
            T result = default(T);

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<T>(str);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取HttpResult
        /// </summary>
        /// <param name="httpItem">httpItem类型</param>
        /// <param name="itemAction">请求前对HttpItem的操作</param>
        /// <returns></returns>
        private static async Task<HttpResult> GetHttpResultAsync(HttpItem httpItem)
        {
            HttpResult result = null;

            result = await new HttpHelper().GetResultAsync(httpItem);

            return result;
        }

        /// <summary>
        /// 获取请求返回值的Json实体对象 (BaseResult)
        /// </summary>
        /// <typeparam name="T">返回的对象类型 (BaseResult)</typeparam>
        /// <param name="request">请求后对HttpResult的操作</param>
        /// <returns></returns>
        private static async Task<T> GetHttpResultAsync<T>(BaseRequest request) where T : BaseResult, new()
        {
            T result = default(T);

            var httpItem = request.GetHttpItem();

            //请求数据
            HttpResult httpResult = await GetHttpResultAsync(httpItem);

            //转换对象
            result = ConvertToEntity<T>(httpResult.Html);

            if (result == null) //请求失败或者转换对象失败
            {
                result = new T();
            }

            if (result is BaseResult baseResult)
            {
                baseResult.StatusCode = httpResult.StatusCode;
                baseResult.SourceHtml = httpResult.Html;
            }

            return result;
        }

        /// <summary>
        /// 设置最后访问地址
        /// </summary>
        /// <param name="location"></param>
        private void SetLastLocation(BasicGeoposition location)
        {
            LastLocation = location;
        }

        #endregion 基础实现
    }
}