using Common.Ofo.Entity.Result;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 活动中心VM
    /// </summary>
    public class ActivityCenterViewModel : BaseViewModel
    {
        #region 字段

        private ActivityHomePageDetail _activityHomePageDetail;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 活动点击命令
        /// </summary>
        public ICommand ActivityClickCommand { get; set; }

        public ActivityHomePageDetail ActivityHomePageDetail
        {
            get { return _activityHomePageDetail; }
            set
            {
                _activityHomePageDetail = value;
                NotifyPropertyChanged("ActivityHomePageDetail");
            }
        }

        /// <summary>
        /// 活动列表
        /// </summary>
        public ObservableCollection<ActivityCenterActivityItemViewModel> ActivityList { get; set; } = new ObservableCollection<ActivityCenterActivityItemViewModel>();

        /// <summary>
        /// 关注列表？
        /// </summary>
        public ObservableCollection<FocusItem> FocusList { get; set; } = new ObservableCollection<FocusItem>();

        /// <summary>
        /// 操作列表？
        /// </summary>
        public ObservableCollection<OperationItem> OperationList { get; set; } = new ObservableCollection<OperationItem>();

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 活动中心VM
        /// </summary>
        public ActivityCenterViewModel()
        {
            ActivityClickCommand = new RelayCommand(state =>
            {
            });
        }

        #endregion 构造函数

        #region 方法

        protected override async Task InitializationAsync()
        {
            var activityHomePageDetailResult = await OfoApi.GetActivityHomePageDetailAsync();
            if (await CheckOfoApiResult(activityHomePageDetailResult))
            {
                var activityHomePageDetail = activityHomePageDetailResult.Data;
                if (activityHomePageDetail?.AdList?.Count > 0)
                {
                    activityHomePageDetail.AdList.ForEach(item =>
                    {
                        ActivityList.Add(new ActivityCenterActivityItemViewModel()
                        {
                            ActivityId = item.activityId,
                            ClickCommand = ActivityClickCommand,
                            ImgName = item.ImgName,
                            ImgUrl = item.ImgUrl,
                            JumpUrl = item.JumpUrl,
                        });
                    });
                }
                if (activityHomePageDetail?.OperationList?.Count > 0)
                {
                    activityHomePageDetail.OperationList.ForEach(item =>
                    {
                        OperationList.Add(item);
                    });
                }
                if (activityHomePageDetail?.FocusList?.Count > 0)
                {
                    activityHomePageDetail.FocusList.ForEach(item =>
                    {
                        FocusList.Add(item);
                    });
                }
            }
        }

        #endregion 方法
    }
}