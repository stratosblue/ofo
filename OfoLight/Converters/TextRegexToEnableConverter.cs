using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace OfoLight.Converters
{
    /// <summary>
    /// 文本长度可见性转换器
    /// </summary>
    public class TextRegexToEnableConverter : IValueConverter
    {
        #region 方法

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            var regex = parameter as string;
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(regex))
            {
                return false;
            }

            return Regex.IsMatch(str, regex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return string.Empty;
        }

        #endregion 方法
    }
}