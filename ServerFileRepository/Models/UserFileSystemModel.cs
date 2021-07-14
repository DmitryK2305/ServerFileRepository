using Microsoft.AspNetCore.Http;
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
            if (!System.IO.Directory.Exists(curUserPath))
            {
                System.IO.Directory.CreateDirectory(curUserPath);
            }
        }

        public bool OpenDirectory(string folderName)
        {
            var folderPath = Path.Combine(CurrentDirGlobalPath, folderName);
            if (System.IO.Directory.Exists(folderPath))
            {
                currentDir = Path.Combine(currentDir, folderName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BackDirectory()
        {
            if (string.IsNullOrEmpty(currentDir))
            {
                return false;
            }
            else
            {
                currentDir = Path.GetDirectoryName(currentDir);
                return true;
            }
        }

        public IEnumerable<IFileSystemItem> Items {
            get {
                IEnumerable<IFileSystemItem> items = System.IO.Directory.GetDirectories(CurrentDirGlobalPath).Select(t => new Directory() { Name = Path.GetFileName(t) });
                items = items.Concat(System.IO.Directory.GetFiles(CurrentDirGlobalPath).Select(t => new File() { Name = Path.GetFileName(t) }));

                if (!string.IsNullOrEmpty(currentDir))
                    items = new IFileSystemItem[] { new BackFolder()}.Concat(items);
                return items;
            }
        }

        public void Reset()
        {
            currentDir = "";
        }

        public bool DeleteFile(string name)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(CurrentDirGlobalPath, name));
                return true;
            }
            catch { }
            return false;
        }

        public bool DeleteDirectory(string name)
        {
            try
            {
                System.IO.Directory.Delete(Path.Combine(CurrentDirGlobalPath, name), true);
                return true;
            }
            catch { }
            return false;
        }

        public void AddDirectory(string name)
        {
            System.IO.Directory.CreateDirectory(Path.Combine(CurrentDirGlobalPath, name));
        }

        //По возможности заменить IFormFile
        public async Task UploadFileAsync(IFormFile file)
        {
            using (var fileStream = new FileStream(Path.Combine(CurrentDirGlobalPath, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public string GetFilePath(string name)
        {
            return Path.Combine(CurrentDirGlobalPath, name);
        }
    }
}
