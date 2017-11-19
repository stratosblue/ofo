using Common.Ofo.Entity.Result;
using OfoLight.Entity;
using OfoLight.Utilities;
using OfoLight.View;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 活动弹出界面VM
    /// </summary>
    public class ActivityPopupContentViewModel : BasePopupContentViewModel
    {
        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }

        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                NotifyPropertyChanged("Url");
            }
        }

        private AdvertisementItem _activityInfo;

        /// <summary>
        /// 活动信息
        /// </summary>
        public AdvertisementItem Activity
        {
            get { return _activityInfo; }
            set
            {
                _activityInfo = value;
                Url = value.link;

                Task.Run(async () =>
                {
                    IRandomAccessStream imageStream = null;
                    try
                    {
                        if (!await LocalCacheUtility.ExistsCacheFile(value.ImgName))
                        {
                            if (await LocalCacheUtility.CacheHttpFileAsync(value.ImgName, value.ImgUrl))
                            {
                                imageStream = await LocalCacheUtility.GetCacheAsync(value.ImgName);
                            }
                        }
                        else
                        {
                            imageStream = await LocalCacheUtility.GetCacheAsync(value.ImgName);
                        }
                        if (imageStream.Size > 0)
                        {
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
                            {
                                try
                                {
                                    var tempImage = new BitmapImage();
                                    await tempImage.SetSourceAsync(imageStream);
                                    Image = tempImage;
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);
                                    CloseAction();
                                }
                            });
                            Global.AppConfig.LastShowActivityId = value.id;
                            Global.AppConfig.LastShowActivityTime = DateTime.Now;
                            Global.SaveAppConfig();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        CloseAction();
                    }
                    finally
                    {
                        imageStream?.Dispose();
                    }
                });
            }
        }

        /// <summary>
        /// 查看活动命令
        /// </summary>
        public ICommand ViewActivityCommand { get; set; }

        /// <summary>
        /// 活动弹出界面VM
        /// </summary>
        public ActivityPopupContentViewModel() : base(null)
        {
            ViewActivityCommand = new RelayCommand((state) =>
            {
                ContentPageArgs args = new ContentPageArgs()
                {
                    Name = "OFO活动"
                };
                args.ContentElement = new WebPageContentView(Url);

                TryNavigate(typeof(ContentPageView), args);

                CloseAction();
            });
        }
    }
}
