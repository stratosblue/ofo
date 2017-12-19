using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace OfoLight.Converters
{
    /// <summary>
    /// 运行状态颜色转换器
    /// </summary>
    public class RunningStateColorConverter : IValueConverter
    {
        #region 方法

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (System.Convert.ToBoolean(value))
            {
                return (SolidColorBrush)Application.Current.Resources["MainColorBrush"];
            }
            else
            {
                return (SolidColorBrush)Application.Current.Resources["WhiteSolidBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}