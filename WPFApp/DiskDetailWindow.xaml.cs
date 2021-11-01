using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFApp.Models;
using WPFApp.Services;
using WPFApp.Services.Interfaces;

namespace WPFApp
{
    public partial class DiskDetailsWindow : Window
    {
        private readonly IDirectoryService _service;
        private readonly DriveInfo _drive;
        public ObservableCollection<Dir> Tree { get; set; }
        public DiskDetailsWindow(string diskName)
        {
            InitializeComponent();
            _service = new DirectoryService();
            _drive = DriveInfo.GetDrives().FirstOrDefault(d => d.Name == diskName);

            var disk = new Dir();
            disk.DiskSize = _drive.TotalSize;
            disk.Name = _drive.Name;
            disk.Path = _drive.Name;
            disk.Children = new ObservableCollection<Dir>(_service.GetFolderChildren(disk, _drive));
            disk.IsSizeCalculationDone = false;

            Task.Run(() => 
            {
                disk.Size = _service.GetFolderSize(disk);
                disk.IsSizeCalculationDone = true;
            });

            Tree = new ObservableCollection<Dir> { disk };
            trvStructure.ItemsSource = Tree;
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var origSource = e.OriginalSource as TreeViewItem;
            var dir = origSource.Header as Dir;

            if (dir.IsFile) return;

            foreach (var child in dir.Children)
            {
                child.IsSizeCalculationDone = false;

                if (child.IsFolder)
                {
                    child.Children = new ObservableCollection<Dir>(_service.GetFolderChildren(child, _drive));

                    Task.Run(() => 
                    {
                        child.Size = _service.GetFolderSize(child);
                        child.IsSizeCalculationDone = true;
                    });
                }
                if (child.IsFile)
                {
                    child.Size = new FileInfo(child.Path).Length;
                    child.IsSizeCalculationDone = true;
                }
            }
        }
    }
}


