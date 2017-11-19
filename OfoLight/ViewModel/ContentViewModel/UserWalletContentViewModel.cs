using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.View;
using System.Threading.Tasks;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 用户钱包VM
    /// </summary>
    public class UserWalletContentViewModel : BaseContentViewModel
    {
        private WalletInfo _walletInfo;

        public WalletInfo WalletInfo
        {
            get { return _walletInfo; }
            set
            {
                _walletInfo = value;
                Balance = value.Balance;
                Bond = value.Bond;
                MonthCardEndTime = value.MonthCardEndTime;
                PacketNum = value.PacketNum;
                RedPacketBalance = value.RedPacketBalance;
                NotifyPropertyChanged("WalletInfo");
            }
        }

        private float _balance;

        public float Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                NotifyPropertyChanged("Balance");
            }
        }

        private float _bond;

        public float Bond
        {
            get { return _bond; }
            set
            {
                _bond = value;
                NotifyPropertyChanged("Bond");
            }
        }

        private int _monthCardEndTime;

        public int MonthCardEndTime
        {
            get { return _monthCardEndTime; }
            set
            {
                _monthCardEndTime = value;
                NotifyPropertyChanged("MonthCardEndTime");
            }
        }
        private float _packetNum;

        public float PacketNum
        {
            get { return _packetNum; }
            set
            {
                _packetNum = value;
                NotifyPropertyChanged("PacketNum");
            }
        }
        private float _redPacketBalance;

        public float RedPacketBalance
        {
            get { return _redPacketBalance; }
            set
            {
                _redPacketBalance = value;
                NotifyPropertyChanged("RedPacketBalance");
            }
        }


        public UserWalletContentViewModel()
        {
        }

        protected override async Task InitializationAsync()
        {
            var walletInfoResult = await OfoApi.GetWalletInfoAsync();
            if (await CheckOfoApiResult(walletInfoResult))
            {
                WalletInfo = walletInfoResult.Data;
            }
        }

        protected override void ContentNavigation(object state)
        {
            if (state is string param)
            {
                var args = new ContentPageArgs()
                {
                    Name = param,
                };
                switch (param)
                {
                    case "卡包":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?CardPackagePage");
                        break;
                    case "余额":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?NewBalancePage");
                        break;
                    case "红包收入":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?MyPacketPage");
                        break;
                    case "优惠券":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?Packets");
                        break;
                    case "押金":
                        args.ContentElement = new WebPageContentView("https://common.ofo.so/newdist/?ForegiftWithdrawSuccess");
                        break;
                }
                if (args.ContentElement != null)
                {
                    ContentNavigation(args);
                }
            }
        }
    }
}
