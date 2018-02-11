using Common.Ofo.Entity.Result;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 使用红包选择弹出内容VM
    /// </summary>
    public class PacketChosePupopContentViewModel : BasePopupContentViewModel
    {
        #region 属性

        /// <summary>
        /// 不使用红包命令
        /// </summary>
        public ICommand NoUseCommand { get; set; }

        /// <summary>
        /// 红包列表
        /// </summary>
        public ObservableCollection<RedPacketInfo> RedPacketList { get; set; } = new ObservableCollection<RedPacketInfo>();

        /// <summary>
        /// 已选择的红包
        /// </summary>
        public RedPacketInfo SelectedPacket { get; set; }

        /// <summary>
        /// 红包选择命令
        /// </summary>
        public ICommand SelectPacketCommand { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 使用红包选择弹出内容VM
        /// </summary>
        /// <param name="closeCallBack">关闭时的回调</param>
        /// <param name="redPacketList">红包列表</param>
        public PacketChosePupopContentViewModel(Action<object> closeCallBack, IEnumerable<RedPacketInfo> redPacketList) : base(closeCallBack)
        {
            if (redPacketList != null)
            {
                var orderedList = redPacketList.OrderBy(item => item.DeadTime);

                foreach (var item in orderedList)
                {
                    RedPacketList.Add(item);
                }
            }

            NoUseCommand = new RelayCommand(state =>
            {
                SelectedPacket = null;
                CloseAction(SelectedPacket);
            });

            SelectPacketCommand = new RelayCommand(state =>
            {
                if (state is ListView listView)
                {
                    SelectedPacket = listView.SelectedItem as RedPacketInfo;
                }
                CloseAction(SelectedPacket);
            });
        }

        #endregion 构造函数
    }
}