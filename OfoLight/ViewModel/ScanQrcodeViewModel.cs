using OfoLight.View;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;
using ZXing.Common;

namespace OfoLight.ViewModel
{
    /// <summary>
    /// 二维码扫描VM
    /// </summary>
    public class ScanQrcodeViewModel : BaseViewModel
    {
        private Result _result;
        private MediaCapture _mediaCapture;
        private DispatcherTimer _scan_Timer;
        private bool _isBusy = false;
        private bool _isPreviewing = false;
        BarcodeReader _barcodeReader;
        CaptureElement _captureElement;

        /// <summary>
        /// 手电筒命令
        /// </summary>
        public ICommand LightOnFlashCommand { get; set; }

        /// <summary>
        /// 手动输入命令
        /// </summary>
        public ICommand ManualInputCommand { get; set; }

        private bool _isLightOn;

        /// <summary>
        /// 手电筒是否打开
        /// </summary>
        public bool IsLightOn
        {
            get { return _isLightOn; }
            set
            {
                _isLightOn = value;
                NotifyPropertyChanged("IsLightOn");
            }
        }

        /// <summary>
        /// 最后一次扫描的结果
        /// </summary>
        public string LastScanResult { get; private set; } = string.Empty;

        /// <summary>
        /// 预览控件
        /// </summary>
        public CaptureElement CaptureElement
        {
            get => _captureElement;
            set
            {
                _captureElement = value;
                NotifyPropertyChanged("CaptureElement");
            }
        }

        /// <summary>
        /// 二维码扫描VM
        /// </summary>
        public ScanQrcodeViewModel()
        {
            _barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    CharacterSet = "utf-8",
                    PossibleFormats = new BarcodeFormat[] { BarcodeFormat.QR_CODE },
                },
            };

            LightOnFlashCommand = new RelayCommand(async (state) =>
            {
                if (IsLightOn)
                {
                    await LightOffAsync();
                }
                else
                {
                    await LightOnAsync();
                }
            });

            ManualInputCommand = new RelayCommand(async (state) =>
            {
                if (IsLightOn)
                {
                    await LightOffAsync();
                }
                TryReplaceNavigate(typeof(UnlockView), null);
            });
        }

        private static readonly Guid RotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");

        /// <summary>
        /// 清理相机
        /// </summary>
        /// <returns></returns>
        public async Task CleanupCameraAsync()
        {
            try
            {
                if (_scan_Timer != null)
                {
                    _scan_Timer.Stop();
                    _scan_Timer.Tick -= ScanTimerTickAsync;
                    _scan_Timer = null;
                }

                if (_isPreviewing)
                {
                    await StopPreviewAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 初始化图像检查Timer
        /// </summary>
        private void InitVideoTimer()
        {
            _scan_Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(1100)
            };
            _scan_Timer.Tick += ScanTimerTickAsync;
            _scan_Timer.Start();
        }

        /// <summary>
        /// 图像检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ScanTimerTickAsync(object sender, object e)
        {
            try
            {
                if (!_isBusy)
                {
                    _isBusy = true;

                    //var previewProperties = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;
                    VideoFrame videoFrame = new VideoFrame(BitmapPixelFormat.Rgba8, 640, 360);
                    VideoFrame previewFrame = await _mediaCapture.GetPreviewFrameAsync(videoFrame);

                    WriteableBitmap bitmap = new WriteableBitmap(previewFrame.SoftwareBitmap.PixelWidth, previewFrame.SoftwareBitmap.PixelHeight);

                    previewFrame.SoftwareBitmap.CopyToBuffer(bitmap.PixelBuffer);

                    await Task.Run(async () => { await ScanBitmap(bitmap); });
                }
                await Task.Delay(50);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                _isBusy = false;
            }
        }

        /// <summary>
        /// 停止预览
        /// </summary>
        /// <returns></returns>
        private async Task StopPreviewAsync()
        {
            _isPreviewing = false;
            await _mediaCapture?.StopPreviewAsync();
            _mediaCapture?.Dispose();
            _mediaCapture = null;
        }

        /// <summary>
        /// 解析二维码图片
        /// </summary>
        /// <param name="writeableBmp">图片</param>
        /// <returns></returns>
        private async Task ScanBitmap(WriteableBitmap writeableBmp)
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    _result = _barcodeReader.Decode(writeableBmp.PixelBuffer.ToArray(), writeableBmp.PixelWidth, writeableBmp.PixelHeight, RGBLuminanceSource.BitmapFormat.RGB32);

                    if (_result != null)
                    {
                        var scanText = _result.Text.Trim();
                        if (scanText.Contains("http://ofo.so/plate") || scanText.Contains("http://ofo.com/oneplate"))
                        {
                            LastScanResult = scanText.Split('/').Last();
                        }

                        TryReplaceNavigate(typeof(UnlockView), LastScanResult);
                        LastScanResult = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 初始化摄像头
        /// <paramref name="changeFocusSetting">更改对焦设置</paramref>
        /// </summary>
        public async Task InitVideoCapture(bool changeFocusSetting = true)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
             {
                 try
                 {
                     ///摄像头的检测  
                     var cameraDevice = await FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel.Back);

                     if (cameraDevice == null)
                     {
                         Debug.WriteLine("No camera device found!");
                         return;
                     }
                     var settings = new MediaCaptureInitializationSettings
                     {
                         StreamingCaptureMode = StreamingCaptureMode.Video,
                         MediaCategory = MediaCategory.Other,
                         AudioProcessing = AudioProcessing.Default,
                         VideoDeviceId = cameraDevice.Id,
                     };

                     _mediaCapture = new MediaCapture();

                     await _mediaCapture.InitializeAsync(settings);
                     CaptureElement = new CaptureElement()
                     {
                         Stretch = Stretch.UniformToFill,
                         Source = _mediaCapture
                     };

                     await _mediaCapture.StartPreviewAsync();

                     var props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
                     props.Properties.Add(RotationKey, 90);

                     await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null);

                     var focusControl = _mediaCapture.VideoDeviceController.FocusControl;

                     if (changeFocusSetting && focusControl.Supported)
                     {
                         try
                         {
                             await focusControl.UnlockAsync();
                             var setting = new FocusSettings { Mode = FocusMode.Continuous, AutoFocusRange = AutoFocusRange.Normal, };
                             focusControl.Configure(setting);
                         }
                         catch (Exception ex)   //出现异常时重启预览并不设置对焦
                         {
                             Debug.WriteLine(ex);
                             await StopPreviewAsync();
                             await InitVideoCapture(false);
                             return;
                         }
                     }

                     if (focusControl.Supported)
                     {
                         try
                         {
                             await focusControl.FocusAsync();
                         }
                         catch (Exception ex)
                         {
                             Debug.WriteLine(ex);
                         }
                     }

                     _isPreviewing = true;

                     InitVideoTimer();
                 }
                 catch (Exception ex)
                 {
                     Debug.WriteLine(ex);
                 }
             });
        }

        /// <summary>
        /// 查找摄像头
        /// </summary>
        /// <param name="desiredPanel"></param>
        /// <returns></returns>
        private async Task<DeviceInformation> FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        {
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            DeviceInformation desiredDevice = allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredPanel);

            return desiredDevice ?? allVideoDevices.FirstOrDefault();
        }

        /// <summary>
        /// 关闭手电筒
        /// </summary>
        private async Task LightOffAsync()
        {
            try
            {
                var torchControl = _mediaCapture.VideoDeviceController.TorchControl;

                if (torchControl.Supported)
                {
                    torchControl.Enabled = false;
                    IsLightOn = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await ShowNotifyAsync("关闭手电筒失败");
            }
        }

        /// <summary>
        /// 开启手电筒
        /// </summary>
        private async Task LightOnAsync()
        {
            try
            {
                if (_mediaCapture?.VideoDeviceController?.TorchControl != null)
                {
                    var torchControl = _mediaCapture.VideoDeviceController.TorchControl;

                    if (torchControl.Supported)
                    {
                        torchControl.Enabled = true;
                        IsLightOn = true;
                        if (torchControl.PowerSupported)
                        {
                            torchControl.PowerPercent = 70;
                        }
                    }
                }
                else
                {
                    await ShowNotifyAsync("开启手电筒失败");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await ShowNotifyAsync("开启手电筒失败");
            }
        }

        /// <summary>
        /// 从挂起恢复时
        /// </summary>
        public override async Task OnResumingAsync()
        {
            await base.OnResumingAsync();

            await InitVideoCapture();
        }

        /// <summary>
        /// 挂起
        /// </summary>
        /// <returns></returns>
        public override async Task OnSuspendingAsync()
        {
            await base.OnSuspendingAsync();

            await CleanupCameraAsync();
        }

    }
}
