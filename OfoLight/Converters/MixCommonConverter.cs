using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OfoLight.Converters
{
    /// <summary>
    /// 混合的通用转换类
    /// </summary>
    public class MixCommonConverter : IValueConverter
    {
        #region 方法

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string para)
            {
                switch (para)
                {
                    case "ReapirButtonText":
                        if (value == null)
                        {
                            return "取消报修";
                        }
                        else
                        {
                            return "确认报修";
                        }
                    case "GuideButtonVisibility":
                        if (value is int guideImgSelectIndex)
                        {
                            if (guideImgSelectIndex == 3)
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                        break;

                    case "BondStatus":
                        if (value is float bond)
                        {
                            if (bond > 0)
                            {
                                return Visibility.Visible;
                            }
                            else
                            {
                                return Visibility.Collapsed;
                            }
                        }
                        break;

                    case "LoginButtonEnable":
                        if (value is string inputVerifyCode)
                        {
                            if (inputVerifyCode.Length >= 4)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        break;

                    case "BlueBarType":
                        if (value is int inputBlueBarType)
                        {
                            ImageSource blubarImage = null;

                            switch (inputBlueBarType)
                            {
                                case 1:
                                    blubarImage = new BitmapImage(new Uri("ms-appx:///Assets/icons/blue_bar_alert.png"));
                                    break;

                                case 0:
                                default:
                                    blubarImage = new BitmapImage(new Uri("ms-appx:///Assets/icons/blue_bar_message.png"));
                                    break;
                            }
                            return blubarImage;
                        }
                        break;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

        #endregion 方法
    }
}