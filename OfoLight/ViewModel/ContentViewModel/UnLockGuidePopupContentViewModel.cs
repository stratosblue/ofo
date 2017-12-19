using OfoLight.Utilities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 解锁引导内容页VM
    /// </summary>
    public class UnLockGuidePopupContentViewModel : BasePopupContentViewModel
    {
        #region 属性

        public ObservableCollection<BitmapImage> GuideImages { get; set; } = new ObservableCollection<BitmapImage>();

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 解锁引导内容页VM
        /// </summary>
        public UnLockGuidePopupContentViewModel() : base(null)
        { }

        #endregion 构造函数

        #region 方法

        protected override async Task InitializationAsync()
        {
            for (int i = 1; i < 5; i++)
            {
                BitmapImage image = new BitmapImage();
                using (var imageStream = await ResourceUtility.GetApplicationResourceStreamAsync($"Assets/new_user_guide_{i}.jpg"))
                {
                    image.SetSource(imageStream);
                }
                GuideImages.Add(image);
            }
        }

        #endregion 方法
    }
}