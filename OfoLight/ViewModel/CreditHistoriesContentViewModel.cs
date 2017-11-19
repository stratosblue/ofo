using Common.Ofo.Entity.Result;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 信用日志内容页VM
    /// </summary>
    public class CreditHistoriesContentViewModel : BaseContentViewModel
    {
        public int Page { get; set; }

        private bool _isLoading = false;

        private bool _canLoadMore = true;

        private object _loadingLock = new object();

        /// <summary>
        /// 信用历史
        /// </summary>
        public ObservableCollection<CreditItem> CreditHistories = new ObservableCollection<CreditItem>();

        protected override async Task InitializationAsync()
        {
            await LoadHistoriesAsync();
        }

        private async Task LoadHistoriesAsync()
        {
            if (_isLoading)
            {
                return;
            }
            else if (_canLoadMore)
            {
                _isLoading = true;
                var getCreditHistoriesResult = await OfoApi.GetCreditHistoriesAsync(Page);
                if (await CheckOfoApiResult(getCreditHistoriesResult))
                {
                    if (getCreditHistoriesResult.Data.CurPage > getCreditHistoriesResult.Data.TotalPage)
                    {
                        _canLoadMore = false;
                        await ShowNotifyAsync("没有更多记录可以加载");
                    }
                    if (getCreditHistoriesResult.Data.CreditItemList.Count > 0)
                    {
                        foreach (var item in getCreditHistoriesResult.Data.CreditItemList)
                        {
                            CreditHistories.Add(item);
                        }
                    }
                }
                _isLoading = false;
            }
            else
            {
                await ShowNotifyAsync("没有更多记录可以加载");
            }
        }
    }
}
