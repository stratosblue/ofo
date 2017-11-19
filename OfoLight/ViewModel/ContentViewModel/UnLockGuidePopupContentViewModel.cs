using OfoLight.Controls;
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
        public ObservableCollection<BitmapImage> GuideImages { get; set; } = new ObservableCollection<BitmapImage>();

        /// <summary>
        /// 解锁引导内容页VM
        /// </summary>
        public UnLockGuidePopupContentViewModel() : base(null)
        { }

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
    }
}
