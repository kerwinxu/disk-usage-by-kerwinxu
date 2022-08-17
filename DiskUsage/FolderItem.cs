using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiskUsage
{
    /// <summary>
    /// 
    /// </summary>
    public  class FolderItem : ObservableObject
    {
        /// <summary>
        /// 目录和文件夹都用这个。
        /// </summary>
        public FileSystemInfo Info { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<FolderItem> Children { get; set; }


    }
}
