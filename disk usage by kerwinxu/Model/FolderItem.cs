using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace disk_usage_by_kerwinxu.Model
{
    public  class FolderItem: ObservableObject
    {


        private FileSystemInfo _info;
        /// <summary>
        /// 目录和文件夹都用这个。
        /// </summary>
        public FileSystemInfo Info { 
            get { return _info; }
            set { _info = value; RaisePropertyChanged(() => Info); }
        }


        private bool _is_folder;
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool IsFolder
        {
            get { return _is_folder; }
            set { _is_folder = value; RaisePropertyChanged(() => IsFolder); }
        }

        private List<FolderItem> _children = new List<FolderItem>();
        /// <summary>
        /// 这个代表子项目
        /// </summary>
        public List<FolderItem> Children
        {
            get { return _children; }

        }

        private long _size;
        /// <summary>
        /// 返回尺寸
        /// </summary>
        public long Size
        {
            get
            {
                if (IsFolder)
                {
                    return Children.Select(x => x.Size).Sum();

                }
                else
                {
                    return _size;
                }
            }
            set
            {
                _size = value; RaisePropertyChanged(() => Size);
            }
        }
        

    }
}
