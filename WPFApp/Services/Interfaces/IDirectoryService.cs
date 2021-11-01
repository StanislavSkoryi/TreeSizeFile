using System.Collections.Generic;
using System.IO;
using WPFApp.Models;

namespace WPFApp.Services.Interfaces
{
    public interface IDirectoryService
    {
        public IEnumerable<Dir> GetFolderChildren(Dir dir, DriveInfo drive);
        public long GetFolderSize(Dir dir);
    }
}
