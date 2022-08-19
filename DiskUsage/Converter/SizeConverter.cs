using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DiskUsage.Converter
{
    /// <summary>
    /// 怎么显示大小的
    /// </summary>
    class SizeConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (long)values[0]; // 转换类型，这个是依赖属性
            string par = values[1] as string; // 这个是参数
            if (par == DiskUsage.Properties.Resources.sizeMode_Kb)
            {
                return string.Format("{0:f2} Kb", data / 1024.0);
            }
            else if (par == DiskUsage.Properties.Resources.sizeMode_Mb)
            {
                return string.Format("{0:f2} Mb", data / (1024.0 * 1024));
            }
            else if (par == DiskUsage.Properties.Resources.sizeMode_Auto)
            {
                // 这里先判断是否可以以kb显示
                var tmp = data / 1024.0;
                if (tmp < 1024)
                {
                    return string.Format("{0:f2} Kb", tmp);
                }
                // 其他情况按照Mb显示
                return string.Format("{0:f2} Mb", data / (1024.0 * 1024));
            }

            throw new NotImplementedException();
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
