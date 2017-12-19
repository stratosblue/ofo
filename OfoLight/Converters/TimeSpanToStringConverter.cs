using System;
using Windows.UI.Xaml.Data;

namespace OfoLight.Converters
{
    /// <summary>
    /// TimeSpan对象格式化字符串获取转换器
    /// </summary>
    public class TimeSpanToStringConverter : IValueConverter
    {
        #region 方法

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan inputValue)
            {
                if (parameter is string format)
                {
                    return inputValue.ToString(format);
                }
                else
                {
                    return inputValue.ToString("hh\\:mm\\:ss");
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.MinValue;
        }

        #endregion 方法
    }
}