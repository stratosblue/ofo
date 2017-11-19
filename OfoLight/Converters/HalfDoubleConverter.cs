using System;
using Windows.UI.Xaml.Data;

namespace OfoLight.Converters
{
    /// <summary>
    /// 取一半值转换器
    /// </summary>
    public class HalfDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var srcvalue = System.Convert.ToDouble(value);
            return srcvalue / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var srcvalue = System.Convert.ToDouble(value);
            return srcvalue * 2;
        }
    }
}
