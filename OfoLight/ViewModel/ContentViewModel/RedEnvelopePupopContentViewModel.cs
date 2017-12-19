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
    /// 红包弹出内容VM
    /// </summary>
    public class RedPacketPupopContentViewModel : BasePopupContentViewModel
    {
        #region 字段

        private BitmapImage _image;

        private PaymentInfo _paymentInfo;

        #endregion 字段

        #region 属性

        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }

        /// <summary>
        /// 打开红包命令
        /// </summary>
        public ICommand OpenRedPacketCommand { get; set; }

        /// <summary>
        /// 支付信息
        /// </summary>
        public PaymentInfo PaymentInfo
        {
            get { return _paymentInfo; }
            set
            {
                _paymentInfo = value;

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
                NotifyPropertyChanged("PaymentInfo");
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 红包弹出内容VM
        /// </summary>
        public RedPacketPupopContentViewModel() : base(null)
        {
            OpenRedPacketCommand = new RelayCommand((state) =>
            {
                CloseAction();
                ContentPageArgs args = new ContentPageArgs()
                {
                    Name = PaymentInfo.title,
                    ContentElement = new WebPageContentView(PaymentInfo.url)
                };

                TryNavigate(typeof(ContentPageView), args);
            });
        }

        #endregion 构造函数
    }
}