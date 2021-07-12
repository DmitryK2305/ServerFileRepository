using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServerFileRepository.Models
{
    public class UserFileSystemModel
    {
        private readonly FileSystemModel owner;
        private string userName;
        private string currentDir;
        public string CurrentDirGlobalPath => Path.Combine(owner.RepositoryPath, userName, currentDir);

        public UserFileSystemModel(FileSystemModel mainModel, string userName)
        {
            owner = mainModel;
            this.userName = userName;
            currentDir = "";

            var curUserPath = Path.Combine(owner.RepositoryPath, userName);
            if (!Directory.Exists(curUserPath))
            {
                Directory.CreateDirectory(curUserPath);
            }
        }

        public bool OpenFolder(string folderName)
        {
            var folderPath = Path.Combine(CurrentDirGlobalPath, folderName);
            if (Directory.Exists(folderPath))
            {
                currentDir = Path.Combine(currentDir, folderName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BackFolder()
        {
            if (string.IsNullOrEmpty(currentDir))
            {
                return false;
            }
            else
            {
                currentDir = Path.Combine(currentDir, "..");
                return true;
            }
        }

        public IEnumerable<IFileSystemItem> Items {
            get {
                IEnumerable<IFileSystemItem> items = Directory.GetDirectories(CurrentDirGlobalPath).Select(t => new Folder() { Name = Path.GetFileName(t) });
                items = items.Concat(Directory.GetFiles(CurrentDirGlobalPath).Select(t => new File() { Name = Path.GetFileName(t) }));
                return items;
            }
        }

        public void Reset()
        {
            currentDir = "";
        }
    }
}
