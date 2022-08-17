using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DiskUsage
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // 这里选择语言
            var lang = System.Globalization.CultureInfo.CurrentCulture.Name;
            switch (lang)
            {
                case "zh-CN":
                    DiskUsage.Properties.Resources.Culture = new System.Globalization.CultureInfo("zh-CN");
                    break;
                default:
                    DiskUsage.Properties.Resources.Culture = new System.Globalization.CultureInfo("");
                    break;
            }
            
        }
    }
}
