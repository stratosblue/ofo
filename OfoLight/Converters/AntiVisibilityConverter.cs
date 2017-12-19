using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OfoLight.Converters
{
    /// <summary>
    /// 互斥可见性转换器
    /// </summary>
    public class AntiVisibilityConverter : IValueConverter
    {
        #region 方法

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility inputValue)
            {
                if (inputValue == Visibility.Visible)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Visibility.Visible;
        }

        #endregion 方法
    }
}