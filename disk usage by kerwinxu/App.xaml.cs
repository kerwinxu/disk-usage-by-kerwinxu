using io.github.kerwinxu.tools.du.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace io.github.kerwinxu.tools.du
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // 这里判断语言
            //var lang = System.Globalization.CultureInfo.CurrentCulture.Name;
            //Trace.WriteLine("语言:" + lang);
            //switch (lang)
            //{
            //    case "zh-CN":
            //        disk_usage_by_kerwinxu.Properties.Resources.Culture = new System.Globalization.CultureInfo("zh-CN");
            //        Trace.WriteLine("正在设置成中文");
            //        break;
            //    default:
            //        disk_usage_by_kerwinxu.Properties.Resources.Culture = new System.Globalization.CultureInfo("");
            //        break;
            //}

            io.github.kerwinxu.tools.du.Properties.Resources.Culture = new System.Globalization.CultureInfo("zh-CN");
            var tmp = io.github.kerwinxu.tools.du.Properties.Resources.title;
            Trace.WriteLine("结果:" + tmp + "*************");

           
        }
     
    }
}
