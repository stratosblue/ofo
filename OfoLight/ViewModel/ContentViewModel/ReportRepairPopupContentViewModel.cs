using Common.Ofo.Entity.Result;
using OfoLight.Controls;
using OfoLight.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    public class ReportRepairPopupContentViewModel : BasePopupContentViewModel
    {
        public ObservableCollection<RepairReasonInfo> RepairReasonList { get; set; } = new ObservableCollection<RepairReasonInfo>();

        /// <summary>
        /// 确认报修命令
        /// </summary>
        public ICommand RepairCommand { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; private set; }

        /// <summary>
        /// 是否Gsm 1是，0否
        /// </summary>
        public int IsGsm { get; private set; } = 0;

        /// <summary>
        /// 报修弹出内容框VM
        /// </summary>
        /// <param name="completeCallBack">关闭时的回调</param>
        /// <param name="orderNum"></param>
        /// <param name="isGsm"></param>
        public ReportRepairPopupContentViewModel(Action completeCallBack, string orderNum, bool isGsm) : base(completeCallBack)
        {
            OrderNum = orderNum;
            IsGsm = isGsm ? 1 : 0;
            RepairCommand = new RelayCommand(async (state) =>
            {
                if (state is ListView listView) //检查是否正确传递参数
                {
                    if (listView.SelectedItems.Count > 0)   //如果有选择项则报修
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (RepairReasonInfo item in listView.SelectedItems)
                        {
                            sb.Append(item.Code);
                            sb.Append(",");
                        }
                        sb.Remove(sb.Length - 1, 1);

                        var geopoint = await PositionUtility.GetUnFixGeopointAsync();
                        var locationInfos = await MapLocationFinder.FindLocationsAtAsync(geopoint);
                        string address = "位置获取失败";
                        if (locationInfos.Status == MapLocationFinderStatus.Success)
                        {
                            var locationInfo = locationInfos.Locations.FirstOrDefault();
                            address = locationInfo?.Address?.FormattedAddress?.Replace(" ", string.Empty) ?? "位置获取失败";
                        }

                        var reportResult = await OfoApi.ReportRepairCarAsync(geopoint.Position, OrderNum, IsGsm, address, sb.ToString());
                        if (await CheckOfoApiResult(reportResult))
                        {
                            await ShowNotifyAsync(reportResult.Message);
                            CloseAction();
                        }
                    }
                    else    //没有选择项，关闭
                    {
                        CloseAction();
                    }
                }
            });
        }

        protected override async Task InitializationAsync()
        {
            var repairReasonListResult = await OfoApi.GetRepairReasonListAsync(OrderNum);
            if (await CheckOfoApiResult(repairReasonListResult))
            {
                foreach (var item in repairReasonListResult.Data)
                {
                    RepairReasonList.Add(item);
                }
            }
        }
    }
}
