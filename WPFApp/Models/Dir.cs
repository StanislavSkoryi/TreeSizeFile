using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFApp.Extensions;

namespace WPFApp.Models
{
    public enum DirType
    {
        Folder = 1,
        File
    }
    public class Dir : INotifyPropertyChanged
    {
        public long DiskSize { get; set; } = 0;
        public string Name { get; set; }
        public string Path { get; set; }
        public DirType Type { get; set; } = DirType.Folder;
        public bool IsFolder => this.Type == DirType.Folder;
        public bool IsFile => this.Type == DirType.File;

        public bool IsSizeCalculationDone = false;
        private long _size;
        public long Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                RaisePropertyChange("Size");
                RaisePropertyChange("ConvertedSize");
                RaisePropertyChange("AllowedPTG");
                RaisePropertyChange("ProgressMessage");
            }
        }

        public string ConvertedSize
        {
            get
            {
                if (Size >= 1073741824) return Convert.ToString(Size.RoundToGb(2) + " GB");
                else if (Size >= 1048576) return Convert.ToString(Size.RoundToMb(2) + " MB");
                else return Convert.ToString(Size.RoundToKb(2) + " KB");
            }
        }
        public double AllowedPTG => Math.Round((double)Size * 100 / DiskSize, 2);
        public string ProgressMessage => IsSizeCalculationDone ? "Done" : "Calculation...";
        public int Level { get; set; } = 0;
        public ObservableCollection<Dir> Children { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

