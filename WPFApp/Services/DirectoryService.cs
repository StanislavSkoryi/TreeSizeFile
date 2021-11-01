using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPFApp.Models;
using WPFApp.Services.Interfaces;

namespace WPFApp.Services
{
    public class DirectoryService : IDirectoryService
    {
        public IEnumerable<Dir> GetFolderChildren(Dir dir, DriveInfo drive)
        {
            var directoryInfo = new DirectoryInfo(dir.Path);

            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
            };

            var children = directoryInfo.GetDirectories("*", options).Select(d => new Dir { Name = d.Name, Path = d.FullName, DiskSize = drive.TotalSize, Level = dir.Level + 1, Type = DirType.Folder }).ToList();
            var files = directoryInfo.GetFiles("*", options).Select(d => new Dir { Name = d.Name, Path = d.FullName, DiskSize = drive.TotalSize, Level = dir.Level + 1, Type = DirType.File });

            foreach (var item in files)
            {
                children.Add(item);
            }
            return new ObservableCollection<Dir>(children);

        }
        public long GetFolderSize(Dir dir)
        {
            var directoryInfo = new DirectoryInfo(dir.Path);
            return GetFolderSize(directoryInfo);
        }

        private long GetFolderSize(DirectoryInfo directoryInfo)
        {
            long startDirectorySize = default(long);

            if (directoryInfo == null || !directoryInfo.Exists)
            {
                return startDirectorySize;
            }

            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
            };

            var summ = directoryInfo
                .GetFiles("*", options)
                .Sum(f => f.Length);

            Interlocked.Add(ref startDirectorySize, summ);

            Parallel.ForEach(
                directoryInfo.GetDirectories("*", options),
                (subDirectory) => Interlocked.Add(ref startDirectorySize, GetFolderSize(subDirectory))
            );

            return startDirectorySize;
        }
    }
}

