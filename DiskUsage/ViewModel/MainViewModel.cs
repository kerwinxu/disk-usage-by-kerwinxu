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
                State = "����";
                
            }
            // �ߴ�ģʽ���Զ���
            SizeMode = DiskUsage.Properties.Resources.sizeMode_Auto;

        }


        #region ��������

        private List<FolderItem> _folder;
        /// <summary>
        /// ������ṩ��Ŀ
        /// </summary>
        public List<FolderItem> Folder
        {
            get { return _folder; }
            set { _folder = value; RaisePropertyChanged(() => Folder); }
        }

        /// <summary>
        /// �Ѿ�ͳ�Ƶĸ���
        /// </summary>
        private  long count = 0;

        private string _state;
        /// <summary>
        /// ��ǰ��״̬
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged(() => State); }
        }

        private string _select_path;
        /// <summary>
        /// ��ǰѡ���Ŀ¼
        /// </summary>
        public string SelectPath
        {
            get { return _select_path; }
            set { _select_path = value; RaisePropertyChanged(() => SelectPath); }
        }


        private string _sizeMode;
        /// <summary>
        /// ��ǰ��״̬
        /// </summary>
        public string SizeMode
        {
            get { return _sizeMode; }
            set { _sizeMode = value; RaisePropertyChanged(() => SizeMode); }
        }


        #endregion

        #region ����


        private RelayCommand _all;
        /// <summary>
        /// ȫ����ť�Ĵ���
        /// </summary>
        public RelayCommand All
        {
            get
            {
                if (_all == null) _all = new RelayCommand(()=> {
                    // ���ʵ�����ǽ���������
                    GetFolderItems(DriveInfo.GetDrives().Select(x => x.Name).ToArray());
                });
                return _all;
            }

        }

        private RelayCommand _select_folder;
        /// <summary>
        /// ȫ����ť�Ĵ���
        /// </summary>
        public RelayCommand SelectFolder
        {
            get
            {
                if (_select_folder == null) _select_folder = new RelayCommand(()=> {
                    // ����ѡ���ļ���
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
        /// ȫ����ť�Ĵ���
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
        /// ��һ��Ŀ¼�����ļ�
        /// </summary>
        public RelayCommand<FolderItem> OpenPath
        {
            get
            {
                if (_open_path == null) _open_path = new RelayCommand<FolderItem>((s) =>
                {
                    // �����ж��Ƿ����ļ�
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
        /// ��������
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
        /// ��������
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
                        dlg.Filter = "json�ļ�|*.json";
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

        #region ����

        private bool is_running;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        private void GetFolderItems(string [] paths)
        {
            // ��������������˳���
            if (is_running)
            {
                return;
            }
            is_running = true;
            if (Folder == null)
            {
                Folder = new List<FolderItem>();
            }
            if(Folder.Count > 0) Folder.Clear();// �����ԭ�ȵ�
            count = 0;     // ��������
            // �±������̵߳ķ�ʽ������
            Thread t = new Thread(delegate () {
                // һ���µġ�
                List<FolderItem> folderItems = paths.Select(x => GetFolderItem(new DirectoryInfo(x))).ToList();
                DispatcherHelper.CheckBeginInvokeOnUI(new Action(() => { Folder = folderItems; }));
                is_running = false;
            });
            t.Start();

            // �±����Ƕ�ʱ���ķ�ʽ������
            System.Timers.Timer timer = new System.Timers.Timer(50);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((s,e) => {
                DispatcherHelper.CheckBeginInvokeOnUI(new Action(() => {
                    // ����Ҫ�ж�����ʲô��ʽ��ʾ
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
        /// �����ļ��з������еĽڵ㡣
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private FolderItem GetFolderItem(DirectoryInfo info)
        {
            // ȡ��һ���ļ��еĴ�С
            FolderItem result = new FolderItem();
            result.Children = new List<FolderItem>();
            result.Info = info;

            // ����ݹ����ļ��еĴ�С
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

            // ����ݹ��ļ��Ĵ�С
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