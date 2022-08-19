using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace DiskUsage.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            if (IsInDesignMode)
            {
                List<FolderItem> folderItems = new List<FolderItem>();
                folderItems.Add(new FolderItem() { Info = new DirectoryInfo(@"E:\onedrive\project\c#\disk usage by kerwinxu") });
                folderItems.Add(new FolderItem() { Info = new DirectoryInfo(@"E:\onedrive\project\c#\disk usage by kerwinxu") });
                Folder = folderItems;
                //
                State = "测试";
                
            }
            // 尺寸模式是自动。
            SizeMode = DiskUsage.Properties.Resources.sizeMode_Auto;

        }


        #region 依赖属性

        private List<FolderItem> _folder;
        /// <summary>
        /// 由这个提供项目
        /// </summary>
        public List<FolderItem> Folder
        {
            get { return _folder; }
            set { _folder = value; RaisePropertyChanged(() => Folder); }
        }

        /// <summary>
        /// 已经统计的个数
        /// </summary>
        private  long count = 0;

        private string _state;
        /// <summary>
        /// 当前的状态
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged(() => State); }
        }

        private string _select_path;
        /// <summary>
        /// 当前选择的目录
        /// </summary>
        public string SelectPath
        {
            get { return _select_path; }
            set { _select_path = value; RaisePropertyChanged(() => SelectPath); }
        }


        private string _sizeMode;
        /// <summary>
        /// 当前的状态
        /// </summary>
        public string SizeMode
        {
            get { return _sizeMode; }
            set { _sizeMode = value; RaisePropertyChanged(() => SizeMode); }
        }


        #endregion

        #region 命令


        private RelayCommand _all;
        /// <summary>
        /// 全部按钮的处理。
        /// </summary>
        public RelayCommand All
        {
            get
            {
                if (_all == null) _all = new RelayCommand(()=> {
                    // 这个实际上是将分区搜索
                    GetFolderItems(DriveInfo.GetDrives().Select(x => x.Name).ToArray());
                });
                return _all;
            }

        }

        private RelayCommand _select_folder;
        /// <summary>
        /// 全部按钮的处理。
        /// </summary>
        public RelayCommand SelectFolder
        {
            get
            {
                if (_select_folder == null) _select_folder = new RelayCommand(()=> {
                    // 这里选择文件夹
                    var dlg = new System.Windows.Forms.FolderBrowserDialog();
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectPath = dlg.SelectedPath;
                        GetFolderItems(new string[] { SelectPath });
                    }
                });
                return _select_folder;
            }

        }
        private RelayCommand _start_scan;
        /// <summary>
        /// 全部按钮的处理。
        /// </summary>
        public RelayCommand StartScan
        {
            get
            {
                if (_start_scan == null) _start_scan = new RelayCommand(() =>
                {
                    GetFolderItems(new string[] {SelectPath });
                });
                return _start_scan;
            }
        }


        private RelayCommand <FolderItem>_open_path;
        /// <summary>
        /// 打开一个目录或者文件
        /// </summary>
        public RelayCommand<FolderItem> OpenPath
        {
            get
            {
                if (_open_path == null) _open_path = new RelayCommand<FolderItem>((s) =>
                {
                    // 这里判断是否是文件
                    if (s.Info is FileInfo)
                    {
                        System.Diagnostics.Process.Start(s.Info.FullName);
                    }
                    else
                    {
                        System.Diagnostics.Process.Start("Explorer.exe", s.Info.FullName);
                    }
                    
                });
                return _open_path;
            }
        }

        private RelayCommand _save;
        /// <summary>
        /// 保存结果的
        /// </summary>
        public RelayCommand Save
        {
            get
            {
                if (_save == null) _save = new RelayCommand(() =>
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(Folder);
                        var dlg =new  SaveFileDialog();
                        dlg.AddExtension = true;
                        dlg.DefaultExt = ".json";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllText(dlg.FileName, json,System.Text.Encoding.UTF8);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        //throw;
                    }
                    

                     
                });
                return _save;
            }
        }

        private RelayCommand _load;
        /// <summary>
        /// 保存结果的
        /// </summary>
        public RelayCommand Load
        {
            get
            {
                if (_load == null) _load = new RelayCommand(() =>
                {
                    try
                    {
                        var dlg = new OpenFileDialog();
                        dlg.Filter = "json文件|*.json";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            var json = File.ReadAllText(dlg.FileName, System.Text.Encoding.UTF8);
                            Folder = JsonConvert.DeserializeObject<List<FolderItem>>(json);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        //throw;
                    }
                });
                return _load;
            }
        }


        #endregion

        #region 函数

        private bool is_running;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        private void GetFolderItems(string [] paths)
        {
            // 如果有启动，就退出。
            if (is_running)
            {
                return;
            }
            is_running = true;
            if (Folder == null)
            {
                Folder = new List<FolderItem>();
            }
            if(Folder.Count > 0) Folder.Clear();// 先清空原先的
            count = 0;     // 数量重置
            // 下边是以线程的方式来处理
            Thread t = new Thread(delegate () {
                // 一个新的。
                List<FolderItem> folderItems = paths.Select(x => GetFolderItem(new DirectoryInfo(x))).ToList();
                DispatcherHelper.CheckBeginInvokeOnUI(new Action(() => { Folder = folderItems; }));
                is_running = false;
            });
            t.Start();

            // 下边是是定时器的方式来处理。
            System.Timers.Timer timer = new System.Timers.Timer(50);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((s,e) => {
                DispatcherHelper.CheckBeginInvokeOnUI(new Action(() => {
                    // 这里要判断是以什么形式显示
                    State = Properties.Resources.count + ":" + count.ToString();
                    if (!is_running)
                    {
                        timer.Stop();
                    }
                } 
                )); 
            });
            timer.Start();


        }

        /// <summary>
        /// 根据文件夹返回所有的节点。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private FolderItem GetFolderItem(DirectoryInfo info)
        {
            // 取得一个文件夹的大小
            FolderItem result = new FolderItem();
            result.Children = new List<FolderItem>();
            result.Info = info;

            // 这里递归子文件夹的大小
            try
            {
                foreach (var item in info.GetDirectories())
                {
                    try
                    {
                        FolderItem folderItem2 = GetFolderItem(item);
                        result.Children.Add(folderItem2);
                        count++;
                        result.Size += folderItem2.Size;
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }

                }

            }
            catch (Exception ex)
            {

                //throw;
            }

            // 这里递归文件的大小
            try
            {
                foreach (var item in info.GetFiles())
                {
                    try
                    {
                        FolderItem folderItem3 = new FolderItem();
                        folderItem3.Size = item.Length;
                        folderItem3.Info = item;
                        result.Children.Add(folderItem3);
                        count++;
                        result.Size += folderItem3.Size;
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }
                }

            }
            catch (Exception)
            {

                //throw;
            }

            return result;

        }

        #endregion
    }
}