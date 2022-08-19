using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DiskUsage.Converter
{
    /// <summary>
    /// 字符串和bool的转换
    /// </summary>
    public class StrToBoolConverter : IValueConverter
    {
        public static string SizeMode = DiskUsage.Properties.Resources.sizeMode_Auto; // 默认是自动模式

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = value as string; // 转换类型，这个是依赖属性
            string par = parameter as string; // 这个是参数
            return par == data;

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = value as string; // 转换类型，这个是依赖属性
            string par = parameter as string; // 这个是参数

            return par;

            // throw new NotImplementedException();
        }
    }
}
