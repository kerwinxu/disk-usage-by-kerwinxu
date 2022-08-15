using disk_usage_by_kerwinxu.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using System;


namespace disk_usage_by_kerwinxu.ViewModel
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
        }

        private List<FolderItem> _folder;
        /// <summary>
        /// 由这个提供项目
        /// </summary>
        public List<FolderItem> Folder
        {
            get { return _folder; }
            set { _folder = value; RaisePropertyChanged(() => Folder); }
        }

        private string _state;
        /// <summary>
        /// 当前的状态
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged(() => State); }
        }


        private RelayCommand _all;
        /// <summary>
        /// 全部按钮的处理。
        /// </summary>
        public RelayCommand All
        {
            get
            {
                if (_all == null) _all = new RelayCommand(all);
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
                if (_select_folder == null) _select_folder = new RelayCommand(select_folder);
                return _select_folder;
            }

        }



        /// <summary>
        /// 根据多个目录遍历。
        /// </summary>
        /// <param name="dirs"></param>
        private List<FolderItem> all2(string[] dirs)
        {
            // 首先清空
            List<FolderItem> result = new List<FolderItem>();
            // 我这里用广度优先
            Queue<FolderItem> folderItems = new Queue<FolderItem>();

            // 这里有一个定时器，定时检查还有多少个
            System.Timers.Timer timer = new System.Timers.Timer(50);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => {
                // 更改界面
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    State = string.Format(Properties.Resources.remainder + ":" + folderItems.Count.ToString());
                });

                //Trace.WriteLine(State);
                if (folderItems.Count == 0)
                {
                    timer.Stop();
                }
            });

            timer.Start();

            foreach (var item in dirs)
            {
                FolderItem folderItem = new FolderItem()
                {
                    IsFolder = true,
                    Info = new DirectoryInfo(item)
                };
                // 添加
                result.Add(folderItem);
                folderItems.Enqueue(folderItem);

            }
            // 循环
            while (folderItems.Count > 0)
            {
                var item = folderItems.Dequeue();
                var info = item.Info as DirectoryInfo; // 这个是目录
                // 不管是遍历文件夹还是文件，都要有异常处理。
                // 然后下边遍历文件夹
                try
                {
                    foreach (var item2 in info.GetDirectories())
                    {
                        var tmp = new FolderItem();
                        tmp.Info = item2;
                        tmp.IsFolder = true;
                        folderItems.Enqueue(tmp);
                        item.Children.Add(tmp);

                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }

                try
                {
                    // 处理文件
                    foreach (var item2 in info.GetFiles())
                    {

                        var tmp = new FolderItem();
                        tmp.Info = item2;
                        tmp.IsFolder = false;

                        tmp.Size = item2.Length;
                        item.Children.Add(tmp);
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }

            Trace.WriteLine("finish");
            return result;


        }


        /// <summary>
        /// all按钮的处理
        /// </summary>
        private void all()
        {
            Thread t = new Thread(delegate () {

                List<FolderItem> tmp = all2(DriveInfo.GetDrives().Select(x => x.Name).ToArray());
                DispatcherHelper.CheckBeginInvokeOnUI(() => {
                    Folder = tmp;
                    State = "ok";
                    Trace.WriteLine("完结");
                });
            });
            t.Start();

        }


        private void select_folder()
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Thread t = new Thread(delegate ()
                {

                    List<FolderItem> tmp = all2(new string[] { dlg.SelectedPath });
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Folder = tmp;
                        State = "ok";
                        Trace.WriteLine("完结");
                    });
                });
                t.Start();
            }

        }
    }
}