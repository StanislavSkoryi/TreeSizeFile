using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp
{
    public partial class ChooseDiskWindow : Window
    {
        public ChooseDiskWindow()
        {
            InitializeComponent();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
            {
                if (driveInfo.IsReady)
                {
                    directories.Items.Add(new DiskDetails(driveInfo));
                }
            }
        }
        protected void SelectCurrentItem(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }

        private void viewDisk_Click(object sender, RoutedEventArgs e)
        {
            string diskName = ((DiskDetails)directories.SelectedItem)?.Name ?? "Error";
            var mainWindow = new DiskDetailsWindow(diskName);
            mainWindow.Show();
        }
    }
    public class DiskDetails
    {
        public string Name { get; set; }
        public double FullSize { get; set; }
        public double FreeSize { get; set; }
        public double AllowedSize { get; set; }
        public double AllowedPTG { get; set; }

        public DiskDetails(DriveInfo driveInfo)
        {
            Name = driveInfo.Name;
            FullSize = Math.Round((double)driveInfo.TotalSize / 1024 / 1024 / 1024, 2);
            FreeSize = Math.Round((double)driveInfo.AvailableFreeSpace / 1024 / 1024 / 1024, 2);
            AllowedSize = Math.Round(FullSize - FreeSize, 2);
            AllowedPTG = AllowedSize * 100 / FullSize;
        }
    }
}
