using Common.Ofo.Entity.Result;
using OfoLight.Controls;
using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.ViewModel
{
    public class UserProfileContentViewModel : BaseContentViewModel
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
                NotifyPropertyChanged("UserInfo");
            }
        }

        private UserProfile _userProfile;

        /// <summary>
        /// 用户简介？
        /// </summary>
        public UserProfile UserProfile
        {
            get { return _userProfile; }
            set
            {
                _userProfile = value;
                NotifyPropertyChanged("UserProfile");
            }
        }

        private string _schoolInfo;

        /// <summary>
        /// 学校信息
        /// </summary>
        public string SchoolInfo
        {
            get { return _schoolInfo; }
            set
            {
                _schoolInfo = value;
                NotifyPropertyChanged("SchoolInfo");
            }
        }

        private string _telPhone;

        /// <summary>
        /// 电话号码
        /// </summary>
        public string TelPhone
        {
            get { return _telPhone; }
            set
            {
                _telPhone = value;
                NotifyPropertyChanged("TelPhone");
            }
        }

        private string _nick;

        /// <summary>
        /// 昵称
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

        private string _creditScoreInfo;

        /// <summary>
        /// 信用分信息
        /// </summary>
        public string CreditScoreInfo
        {
            get { return _creditScoreInfo; }
            set
            {
                _creditScoreInfo = value;
                NotifyPropertyChanged("CreditScoreInfo");
            }
        }

        /// <summary>
        /// 信息按钮
        /// 0昵称 1性别 2生日 3ofo身份
        /// </summary>
        public ObservableCollection<ButtonViewModel> InfoButtons { get; set; } = new ObservableCollection<ButtonViewModel>()
        {
            new ButtonViewModel() { Name = "我的昵称",IsTip = true,},
            new ButtonViewModel() { Name = "性别" },
            new ButtonViewModel() { Name = "生日" },
            new ButtonViewModel() { Name = "ofo身份" }
        };

        /// <summary>
        /// 更换头像命令
        /// </summary>
        public ICommand ChangeAvatarCommand { get; set; }

        #endregion

        /// <summary>
        /// 个人信息内容页VM
        /// </summary>
        /// <param name="userInfo"></param>
        public UserProfileContentViewModel(UserInfo userInfo) : base(false)
        {
            UserInfo = userInfo;

            ChangeAvatarCommand = new RelayCommand(async (state) =>
            {
                try
                {
                    FileOpenPicker fileOpenPicker = new FileOpenPicker
                    {
                        ViewMode = PickerViewMode.Thumbnail,
                    };

                    fileOpenPicker.FileTypeFilter.Add(".png");
                    fileOpenPicker.FileTypeFilter.Add(".jpg");
                    fileOpenPicker.FileTypeFilter.Add(".bmp");

                    StorageFile file = await fileOpenPicker.PickSingleFileAsync();
                    if (file != null)
                    {
                        byte[] avatarData = null;
                        using (var orgStream = await file.OpenReadAsync())
                        {
                            IRandomAccessStream stream = orgStream;
                            using (var compressStream = await ImageUtility.ImageCompressAsync(orgStream, file.FileType, 1024, 1024))
                            {
                                if (compressStream != null)
                                {
                                    stream = compressStream;
                                }
                                avatarData = await AccessStreamUtility.AccessStreamToBytesAsync(stream);
                            }
                        }
                        var modifyUserAvatarResult = await OfoApi.ModifyUserAvatarAsync(avatarData);
                        if (await CheckOfoApiResult(modifyUserAvatarResult))
                        {
                            await ShowNotifyAsync(modifyUserAvatarResult.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

            var initTask = InitializationAsync();
        }

        protected override async Task InitializationAsync()
        {
            Nick = UserInfo.Name;
            TelPhone = OfoUtility.GetMaskTelPhoneNum(UserInfo.TelPhone);
            if (string.IsNullOrEmpty(Nick))
            {
                Nick = TelPhone;
            }
            InfoButtons[0].ContentText = Nick;

            CreditScoreInfo = $"信用分： {UserInfo.CreditTotal} >";

            //获取头像图片
            OfoUtility.GetAvatarImageByUrlAsync(UserInfo.AvatarUrl, avatar => Avatar = avatar);

            InfoButtons[3].ContentText = UserInfo?.IsBond == 1 ? "认证用户" : "未认证用户";

            var userProfileResult = await OfoApi.GetUserProfileAsync();
            if (await CheckOfoApiResult(userProfileResult))
            {
                UserProfile = userProfileResult.Data;
                InfoButtons[1].ContentText = UserProfile.SexType == 1 ? "男" : UserProfile.SexType == 2 ? "女" : "-";
                if (UserProfile.BirthYear > 0)
                {
                    InfoButtons[2].ContentText = $"{UserProfile.BirthYear} 年 {UserProfile.BirthMonth} 月 {UserProfile.BirthDay} 日";
                }
                else
                {
                    InfoButtons[2].ContentText = "-";
                }
                SchoolInfo = string.IsNullOrEmpty(UserProfile.School) ? "非校园用户" : UserProfile.School;
            }
        }


        protected override void NavigationActionAsync(object state)
        {
            if (state is string param)
            {
                if (param.Equals("CreditScoreInfo"))
                {
                    ContentPageArgs args = new ContentPageArgs()
                    {
                        Name = "信用分",
                        ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?CreditPoints"),
                    };
                    ContentNavigation(args);
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
            try
            {
                if (e.ClickedItem is ButtonViewModel bvm)
                {
                    if (bvm?.Name == "我的昵称")
                    {
                        var userNickModifyControl = new UserNickModifyControl();
                        Flyout flyout = new Flyout
                        {
                            Content = userNickModifyControl
                        };
                        userNickModifyControl.ModifyUserNick = async usernick =>
                        {
                            if (usernick.Length < 4)
                            {
                                await ShowNotifyAsync("昵称过短");
                            }
                            else if (usernick.Length > 16)
                            {
                                await ShowNotifyAsync("昵称过长");
                            }
                            else
                            {
                                var modifyUserNickResult = await OfoApi.ModifyUserNickAsync(usernick);
                                if (await CheckOfoApiResult(modifyUserNickResult))
                                {
                                    await ShowNotifyAsync(modifyUserNickResult.Message);
                                    UserInfo.Name = usernick;
                                    Nick = usernick;
                                    InfoButtons[0].ContentText = usernick;
                                }
                            }
                            flyout.Hide();
                        };
                        flyout.ShowAt(sender as FrameworkElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
