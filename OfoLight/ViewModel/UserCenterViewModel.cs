using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 用户中心VM
    /// </summary>
    public class UserCenterViewModel : BaseViewModel
    {
        #region 字段、属性
        private UserInfo _userInfo;

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get { return _userInfo; }
            set
            {
                _userInfo = value;
                if (string.IsNullOrEmpty(value.Name))   //没有昵称时显示手机号
                {
                    Nick = OfoUtility.GetMaskTelPhoneNum(value.TelPhone);
                }
                else
                {
                    Nick = value.Name;
                }
                IdentityType = value.IdentityType;
                CreditTotal = value.CreditTotal;
            }
        }

        private BitmapImage _avatar;

        /// <summary>
        /// 头像
        /// </summary>
        public BitmapImage Avatar
        {
            get { return _avatar; }
            set
            {
                _avatar = value;
                NotifyPropertyChanged("Avatar");
            }
        }

        private string _nick;

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nick
        {
            get { return _nick; }
            set
            {
                _nick = value;
                NotifyPropertyChanged("Nick");
            }
        }

        private int _identityType;

        /// <summary>
        /// 认证类型
        /// </summary>
        public int IdentityType
        {
            get { return _identityType; }
            set
            {
                _identityType = value;
                NotifyPropertyChanged("IdentityType");
            }
        }

        private int _creditTotal;

        /// <summary>
        /// 信用总分
        /// </summary>
        public int CreditTotal
        {
            get { return _creditTotal; }
            set
            {
                _creditTotal = value;
                NotifyPropertyChanged("CreditTotal");
            }
        }

        /// <summary>
        /// 菜单按键列表
        /// </summary>
        public ObservableCollection<ButtonViewModel> UserCenterButtons { get; set; } = new ObservableCollection<ButtonViewModel>();

        #endregion

        /// <summary>
        /// 用户中心VM
        /// </summary>
        public UserCenterViewModel()
        { }

        protected override async Task InitializationAsync()
        {
            UserCenterButtons.Add(new ButtonViewModel() { ContentText = "我的行程", IconUri = "ms-appx:///Assets/icons/journey_img.png" });
            UserCenterButtons.Add(new ButtonViewModel() { ContentText = "我的钱包", IconUri = "ms-appx:///Assets/icons/wallet_img.png" });
            UserCenterButtons.Add(new ButtonViewModel() { ContentText = "邀请好友", IconUri = "ms-appx:///Assets/icons/invite_img.png" });
            UserCenterButtons.Add(new ButtonViewModel() { ContentText = "我的客服", IconUri = "ms-appx:///Assets/icons/icon_my_service.png" });

            var userInfoResult = await OfoApi.GetUserInfoAsync();

            if (await CheckOfoApiResult(userInfoResult))
            {
                UserInfo = userInfoResult.Data;

                //获取头像图片
                await OfoUtility.GetAvatarImageByUrlAsync(UserInfo.AvatarUrl, avatar => Avatar = avatar);
            }
        }

        /// <summary>
        /// 导航事件
        /// </summary>
        /// <param name="state"></param>
        protected override void NavigationActionAsync(object state)
        {
            if (state is string param)
            {
                ContentPageArgs args = new ContentPageArgs()
                {
                    Name = param
                };

                switch (param)
                {
                    case "个人信息":
                        args.ContentElement = new UserProfileContentView(UserInfo);
                        break;
                    case "我的行程":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?MeineReise");
                        break;
                    case "我的钱包":
                        args.ContentElement = new UserWalletContentView();
                        break;
                    case "邀请好友":
                        args.ContentElement = new WebPageContentView($"https://common.ofo.so/about/new_invitation/invite.html?update&refcode={OfoApi.CurUser.TelPhone}");
                        break;
                    case "我的客服":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/about/customer/");
                        break;
                    case "设置":
                        args.ContentElement = new SettingContentView();
                        break;
                }
                if (args.ContentElement != null)
                {
                    TryNavigate(typeof(ContentPageView), args);
                }
            }
        }

        /// <summary>
        /// xbind的listview点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonListItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is ButtonViewModel ubvm)
            {
                NavigationActionAsync(ubvm.ContentText);
            }
        }
    }
}
