using System;
using Windows.UI.Xaml.Data;

namespace OfoLight.Converters
{
    /// <summary>
    /// 整型转换为描述字符串通用转换器
    /// </summary>
    public class IntToDescribeCommonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int inputValue)
            {
                if (parameter is string para)
                {
                    switch (para)
                    {
                        case "IdentityType":    //认证类型
                            switch (inputValue)
                            {
                                case 0:
                                    return "未认证";
                                case 1:
                                    return "已认证";
                            }
                            break;
                        case "reapirButton":
                            if (inputValue > 0)
                            {
                                return "确认报修";
                            }
                            else
                            {
                                return "取消报修";
                            }
                    }
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
